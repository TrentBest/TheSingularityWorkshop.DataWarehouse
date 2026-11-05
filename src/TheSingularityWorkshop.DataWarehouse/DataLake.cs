using System.Runtime.InteropServices;

namespace TheSingularityWorkshop.DataWarehouse;

/// <summary>
/// Owns the aligned unmanaged allocation boundary used by shelves.
/// </summary>
/// <remarks>
/// Allocation and release are O(1) bookkeeping operations. The lake does not index individual
/// records; record addressing is performed directly by each shelf.
/// </remarks>
public sealed unsafe class DataLake : IDisposable
{
    private readonly object _allocationGate = new();
    private long _allocatedBytes;
    private int _disposed;

    public long AllocatedBytes => Interlocked.Read(ref _allocatedBytes);
    public bool IsDisposed => Volatile.Read(ref _disposed) != 0;

    internal void* AllocateAligned(nuint byteCount, nuint alignment)
    {
        lock (_allocationGate)
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            if (byteCount == 0) throw new ArgumentOutOfRangeException(nameof(byteCount));
            if (alignment < (nuint)IntPtr.Size || (alignment & (alignment - 1)) != 0)
                throw new ArgumentOutOfRangeException(nameof(alignment), "Alignment must be a power of two and at least pointer-sized.");

            var remainder = byteCount % alignment;
            var allocationSize = remainder == 0 ? byteCount : checked(byteCount + alignment - remainder);
            // Validate accounting range before acquiring native memory, so a checked conversion
            // cannot fail after allocation and strand the pointer.
            var accountedSize = checked((long)allocationSize);
            var pointer = NativeMemory.AlignedAlloc(allocationSize, alignment);
            if (pointer == null) throw new OutOfMemoryException($"Unable to allocate {allocationSize} aligned bytes.");
            NativeMemory.Clear(pointer, allocationSize);
            Interlocked.Add(ref _allocatedBytes, accountedSize);
            return pointer;
        }
    }

    internal void FreeAligned(void* pointer, nuint allocationSize)
    {
        if (pointer is null) return;
        NativeMemory.AlignedFree(pointer);
        Interlocked.Add(ref _allocatedBytes, -checked((long)allocationSize));
    }

    public void Dispose()
    {
        lock (_allocationGate)
        {
            Interlocked.Exchange(ref _disposed, 1);
        }
        GC.SuppressFinalize(this);
    }
}
