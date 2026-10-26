using System.Numerics;
using System.Runtime.InteropServices;

namespace TheSingularityWorkshop.DataWarehouse;

/// <summary>
/// A page-aligned, fixed-capacity shelf of unmanaged values with external alive and dirty bitmaps.
/// </summary>
/// <typeparam name="T">The unmanaged record stored in every slot.</typeparam>
/// <remarks>
/// Resolving a valid handle to a slot is O(1): validate three integers, then calculate
/// base address + slot * sizeof(T). Allocation uses a free-slot stack; no per-record objects
/// or dictionary lookup are required on the access path.
/// </remarks>
public sealed unsafe class DataShelf<T> : IDisposable where T : unmanaged
{
    private readonly DataLake _lake;
    private readonly void* _baseAddress;
    private readonly nuint _allocationSize;
    private readonly int[] _freeSlots;
    private readonly int[] _generations;
    private readonly ulong[] _alive;
    private readonly ulong[] _dirty;
    private int _freeCount;
    private int _disposed;

    public int Id { get; }
    public int Capacity { get; }
    public int PageSize { get; }
    public int ElementSize => sizeof(T);
    public int AliveCount { get; private set; }
    public int DirtyCount { get; private set; }
    public nint BaseAddress => (nint)_baseAddress;
    public long AllocatedBytes => checked((long)_allocationSize);

    internal DataShelf(DataLake lake, int capacity, int pageSize)
    {
        ArgumentNullException.ThrowIfNull(lake);
        if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
        if (pageSize < 64 || (pageSize & (pageSize - 1)) != 0)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be a power of two and at least 64 bytes.");

        _lake = lake;
        Capacity = capacity;
        PageSize = pageSize;
        Id = ShelfIdentity.Next();

        var requestedBytes = checked((nuint)capacity * (nuint)sizeof(T));
        var remainder = requestedBytes % (nuint)pageSize;
        _allocationSize = remainder == 0 ? requestedBytes : checked(requestedBytes + (nuint)pageSize - remainder);
        _baseAddress = lake.AllocateAligned(requestedBytes, (nuint)pageSize);

        _freeSlots = new int[capacity];
        _generations = new int[capacity];
        _alive = new ulong[(capacity + 63) / 64];
        _dirty = new ulong[(capacity + 63) / 64];
        for (var i = 0; i < capacity; i++)
        {
            _freeSlots[i] = capacity - 1 - i;
            _generations[i] = 1;
        }
        _freeCount = capacity;
    }

    public bool TryAllocate(out DataHandle handle)
    {
        ThrowIfDisposed();
        if (_freeCount == 0)
        {
            handle = default;
            return false;
        }

        var slot = _freeSlots[--_freeCount];
        SetBit(_alive, slot);
        if (!GetBit(_dirty, slot)) DirtyCount++;
        SetBit(_dirty, slot);
        AliveCount++;
        NativeMemory.Clear((byte*)_baseAddress + checked((nuint)slot * (nuint)sizeof(T)), (nuint)sizeof(T));
        handle = new DataHandle(Id, slot, _generations[slot]);
        return true;
    }

    /// <summary>Returns a mutable reference and marks the slot dirty before exposing it.</summary>
    public ref T GetRef(DataHandle handle)
    {
        var slot = Validate(handle);
        if (!GetBit(_dirty, slot)) { SetBit(_dirty, slot); DirtyCount++; }
        return ref ((T*)_baseAddress)[slot];
    }

    /// <summary>Returns a read-only reference without changing the dirty state.</summary>
    public ref readonly T Read(DataHandle handle)
    {
        var slot = Validate(handle);
        return ref ((T*)_baseAddress)[slot];
    }

    public bool IsAlive(DataHandle handle) =>
        !IsDisposed && handle.ShelfId == Id && (uint)handle.Slot < (uint)Capacity &&
        _generations[handle.Slot] == handle.Generation && GetBit(_alive, handle.Slot);

    public bool IsDirty(DataHandle handle)
    {
        var slot = Validate(handle);
        return GetBit(_dirty, slot);
    }

    public bool MarkClean(DataHandle handle)
    {
        var slot = Validate(handle);
        if (!GetBit(_dirty, slot)) return false;
        ClearBit(_dirty, slot);
        DirtyCount--;
        return true;
    }

    public bool TryFree(DataHandle handle)
    {
        ThrowIfDisposed();
        if (!IsAlive(handle)) return false;
        var slot = handle.Slot;
        ClearBit(_alive, slot);
        if (GetBit(_dirty, slot)) { ClearBit(_dirty, slot); DirtyCount--; }
        AliveCount--;
        _generations[slot] = _generations[slot] == int.MaxValue ? 1 : _generations[slot] + 1;
        _freeSlots[_freeCount++] = slot;
        return true;
    }

    /// <summary>Copies dirty slot indexes into the caller-provided buffer; returns the count written.</summary>
    public int CopyDirtySlots(Span<int> destination)
    {
        ThrowIfDisposed();
        var written = 0;
        for (var wordIndex = 0; wordIndex < _dirty.Length && written < destination.Length; wordIndex++)
        {
            var word = _dirty[wordIndex];
            while (word != 0 && written < destination.Length)
            {
                var bit = BitOperations.TrailingZeroCount(word);
                var slot = (wordIndex * 64) + bit;
                if (slot < Capacity) destination[written++] = slot;
                word &= word - 1;
            }
        }
        return written;
    }

    private int Validate(DataHandle handle)
    {
        ThrowIfDisposed();
        if (handle.ShelfId != Id || (uint)handle.Slot >= (uint)Capacity ||
            handle.Generation != _generations[handle.Slot] || !GetBit(_alive, handle.Slot))
            throw new InvalidOperationException("The data handle is stale, invalid, or belongs to another shelf.");
        return handle.Slot;
    }

    private static bool GetBit(ulong[] bits, int index) => (bits[index >> 6] & (1UL << (index & 63))) != 0;
    private static void SetBit(ulong[] bits, int index) => bits[index >> 6] |= 1UL << (index & 63);
    private static void ClearBit(ulong[] bits, int index) => bits[index >> 6] &= ~(1UL << (index & 63));
    private bool IsDisposed => Volatile.Read(ref _disposed) != 0;
    private void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(IsDisposed, this);

    public void Dispose()
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0) return;
        _lake.FreeAligned(_baseAddress, _allocationSize);
        AliveCount = 0;
        DirtyCount = 0;
        GC.SuppressFinalize(this);
    }
}
