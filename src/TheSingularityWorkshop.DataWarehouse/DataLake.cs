using System.Numerics;
using System.Runtime.InteropServices;

namespace TheSingularityWorkshop.DataWarehouse;

/// <summary>
/// Owns shared, page-aligned native backing regions and allocates contiguous page runs to shelves.
/// </summary>
/// <remarks>
/// Allocation and reclamation are serialized at the lake boundary. Record access is not: each
/// shelf owns its slot metadata and can be assigned to an independent worker. Regions remain
/// reserved for reuse until the lake is disposed and its final outstanding allocation is freed.
/// </remarks>
public sealed unsafe class DataLake : IDisposable
{
    private readonly object _allocationGate = new();
    private readonly List<DataLakeRegion> _regions = [];
    private readonly int _regionPageCount;
    private long _allocatedBytes;
    private long _reservedBytes;
    private int _disposed;

    /// <summary>The minimum allocation unit and base alignment, in bytes.</summary>
    public int PageSize { get; }

    /// <summary>The normal native backing-region size, in bytes.</summary>
    public int RegionSize { get; }

    /// <summary>Bytes currently assigned to live shelf allocations, including buddy-block rounding.</summary>
    public long AllocatedBytes => Interlocked.Read(ref _allocatedBytes);

    /// <summary>Total native bytes reserved by backing regions.</summary>
    public long ReservedBytes => Interlocked.Read(ref _reservedBytes);

    /// <summary>Reserved bytes not currently assigned to shelf allocations.</summary>
    public long FreeBytes => checked(ReservedBytes - AllocatedBytes);

    /// <summary>Number of native backing regions currently retained.</summary>
    public int RegionCount
    {
        get { lock (_allocationGate) return _regions.Count; }
    }

    /// <summary>Whether the lake has closed to new page allocations.</summary>
    public bool IsDisposed => Volatile.Read(ref _disposed) != 0;

    public DataLake(int pageSize = 4096, int regionSize = 4 * 1024 * 1024)
    {
        if (pageSize < 64 || (pageSize & (pageSize - 1)) != 0)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be a power of two and at least 64 bytes.");
        if (regionSize < pageSize || (regionSize & (regionSize - 1)) != 0 || regionSize % pageSize != 0)
            throw new ArgumentOutOfRangeException(nameof(regionSize), "Region size must be a power of two, at least one page, and a multiple of page size.");

        PageSize = pageSize;
        RegionSize = regionSize;
        _regionPageCount = regionSize / pageSize;
    }

    internal DataLakeAllocation AllocateAligned(nuint byteCount, nuint alignment)
    {
        lock (_allocationGate)
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            if (byteCount == 0)
                throw new ArgumentOutOfRangeException(nameof(byteCount));
            if (alignment < (nuint)IntPtr.Size || (alignment & (alignment - 1)) != 0)
                throw new ArgumentOutOfRangeException(nameof(alignment), "Alignment must be a power of two and at least pointer-sized.");

            var effectiveAlignment = alignment > (nuint)PageSize ? alignment : (nuint)PageSize;
            var requiredPagesNative = byteCount / (nuint)PageSize;
            if (byteCount % (nuint)PageSize != 0) requiredPagesNative++;
            var alignmentPagesNative = effectiveAlignment / (nuint)PageSize;
            if (requiredPagesNative > int.MaxValue || alignmentPagesNative > int.MaxValue)
                throw new OutOfMemoryException("The requested allocation exceeds the supported page-index range.");

            var requiredPages = (int)requiredPagesNative;
            var alignmentPages = (int)alignmentPagesNative;

            foreach (var region in _regions)
            {
                if (region.TryAllocate(requiredPages, alignmentPages, out var startPage, out var order))
                    return CreateAllocation(region, startPage, order);
            }

            var regionPages = NextPowerOfTwo(Math.Max(_regionPageCount, Math.Max(requiredPages, alignmentPages)));
            var regionBytes = checked((long)PageSize * regionPages);
            var nextReservedBytes = checked(ReservedBytes + regionBytes);
            var newRegion = new DataLakeRegion(PageSize, regionPages);
            try
            {
                _regions.Add(newRegion);
            }
            catch
            {
                newRegion.Dispose();
                throw;
            }

            Interlocked.Exchange(ref _reservedBytes, nextReservedBytes);
            if (!newRegion.TryAllocate(requiredPages, alignmentPages, out var newStartPage, out var newOrder))
                throw new InvalidOperationException("A newly created data-lake region could not satisfy its initial allocation.");

            return CreateAllocation(newRegion, newStartPage, newOrder);
        }
    }

    private DataLakeAllocation CreateAllocation(DataLakeRegion region, int startPage, int order)
    {
        var byteCount = region.GetBlockByteCount(order);
        long nextAllocatedBytes;
        try
        {
            nextAllocatedBytes = checked(AllocatedBytes + checked((long)byteCount));
        }
        catch
        {
            region.Free(startPage, order);
            throw;
        }
        var pointer = region.GetPointer(startPage);
        NativeMemory.Clear(pointer, byteCount);
        Interlocked.Exchange(ref _allocatedBytes, nextAllocatedBytes);
        return new DataLakeAllocation(region, startPage, order, pointer, byteCount);
    }

    internal void FreeAligned(DataLakeAllocation allocation)
    {
        if (allocation.IsDefault) return;

        lock (_allocationGate)
        {
            var nextAllocatedBytes = checked(AllocatedBytes - checked((long)allocation.ByteCount));
            if (nextAllocatedBytes < 0)
                throw new InvalidOperationException("Data-lake allocation accounting cannot become negative.");
            allocation.Region.Free(allocation.StartPage, allocation.Order);
            Interlocked.Exchange(ref _allocatedBytes, nextAllocatedBytes);

            if (IsDisposed && AllocatedBytes == 0)
                ReleaseRegions();
        }
    }

    private void ReleaseRegions()
    {
        foreach (var region in _regions)
            region.Dispose();
        _regions.Clear();
        Interlocked.Exchange(ref _reservedBytes, 0);
    }

    private static int NextPowerOfTwo(int value)
    {
        if (value <= 1) return 1;
        var order = BitOperations.Log2((uint)(value - 1)) + 1;
        if (order >= 31)
            throw new OutOfMemoryException("The requested data-lake region exceeds the supported page-index range.");
        return 1 << order;
    }

    /// <summary>Closes the lake to new allocations and releases regions after all outstanding shelves return their pages.</summary>
    public void Dispose()
    {
        lock (_allocationGate)
        {
            if (Interlocked.Exchange(ref _disposed, 1) != 0) return;
            // Outstanding shelves still reference these regions. Keep them alive until the
            // final shelf releases its allocation; no new allocations are accepted.
            if (AllocatedBytes == 0)
                ReleaseRegions();
        }
        GC.SuppressFinalize(this);
    }
}

internal readonly unsafe struct DataLakeAllocation
{
    public DataLakeRegion Region { get; }
    public int StartPage { get; }
    public int Order { get; }
    public void* Pointer { get; }
    public nuint ByteCount { get; }
    public bool IsDefault => Region is null;

    public DataLakeAllocation(DataLakeRegion region, int startPage, int order, void* pointer, nuint byteCount)
    {
        Region = region;
        StartPage = startPage;
        Order = order;
        Pointer = pointer;
        ByteCount = byteCount;
    }
}

/// <summary>A single aligned native region managed by a buddy page allocator.</summary>
internal sealed unsafe class DataLakeRegion : IDisposable
{
    private readonly byte* _baseAddress;
    private readonly HashSet<int>[] _freeBlocks;
    private readonly Dictionary<int, int> _allocatedBlocks = [];
    private int _disposed;

    public int PageSize { get; }
    public int PageCount { get; }
    public int MaxOrder { get; }
    public nuint ReservedBytes { get; }

    public DataLakeRegion(int pageSize, int pageCount)
    {
        if (pageCount <= 0 || (pageCount & (pageCount - 1)) != 0)
            throw new ArgumentOutOfRangeException(nameof(pageCount));

        PageSize = pageSize;
        PageCount = pageCount;
        MaxOrder = BitOperations.Log2((uint)pageCount);
        ReservedBytes = checked((nuint)pageSize * (nuint)pageCount);

        // Complete all managed metadata setup before reserving native memory. If a managed
        // allocation fails, construction cannot strand an unmanaged region.
        _freeBlocks = new HashSet<int>[MaxOrder + 1];
        for (var order = 0; order < _freeBlocks.Length; order++)
            _freeBlocks[order] = [];
        _freeBlocks[MaxOrder].Add(0);

        _baseAddress = (byte*)NativeMemory.AlignedAlloc(ReservedBytes, ReservedBytes);
        if (_baseAddress == null)
            throw new OutOfMemoryException($"Unable to reserve {ReservedBytes} bytes for a data-lake region.");

        NativeMemory.Clear(_baseAddress, ReservedBytes);
    }

    public bool TryAllocate(int requiredPages, int alignmentPages, out int startPage, out int order)
    {
        ThrowIfDisposed();
        var desiredPages = Math.Max(requiredPages, alignmentPages);
        var desiredOrder = BitOperations.Log2((uint)(desiredPages - 1)) + (desiredPages == 1 ? 0 : 1);
        for (var candidateOrder = desiredOrder; candidateOrder <= MaxOrder; candidateOrder++)
        {
            var free = _freeBlocks[candidateOrder];
            if (free.Count == 0) continue;

            // Reserve all metadata capacity before removing the source block. Allocator
            // bookkeeping must not be left half-split if a managed collection needs memory.
            _allocatedBlocks.EnsureCapacity(_allocatedBlocks.Count + 1);
            for (var split = desiredOrder; split < candidateOrder; split++)
                _freeBlocks[split].EnsureCapacity(_freeBlocks[split].Count + 1);

            using var enumerator = free.GetEnumerator();
            if (!enumerator.MoveNext()) continue;
            var blockStart = enumerator.Current;
            free.Remove(blockStart);

            var splitOrder = candidateOrder;
            while (splitOrder > desiredOrder)
            {
                splitOrder--;
                var rightBuddy = blockStart + (1 << splitOrder);
                _freeBlocks[splitOrder].Add(rightBuddy);
            }

            _allocatedBlocks.Add(blockStart, desiredOrder);
            startPage = blockStart;
            order = desiredOrder;
            return true;
        }

        startPage = 0;
        order = 0;
        return false;
    }

    public void Free(int startPage, int order)
    {
        ThrowIfDisposed();
        if (!_allocatedBlocks.TryGetValue(startPage, out var allocatedOrder) || allocatedOrder != order)
            throw new InvalidOperationException("The page allocation is unknown, already freed, or has an invalid extent.");

        // Reserve hash-table capacity before mutating allocator state. The subsequent
        // coalescing path must not fail halfway through because a free-list resize allocates.
        _allocatedBlocks.EnsureCapacity(_allocatedBlocks.Count);
        for (var candidate = order; candidate <= MaxOrder; candidate++)
            _freeBlocks[candidate].EnsureCapacity(_freeBlocks[candidate].Count + 1);

        _allocatedBlocks.Remove(startPage);
        var mergedStart = startPage;
        var mergedOrder = order;

        while (mergedOrder < MaxOrder)
        {
            var buddy = mergedStart ^ (1 << mergedOrder);
            if (!_freeBlocks[mergedOrder].Remove(buddy)) break;
            mergedStart = Math.Min(mergedStart, buddy);
            mergedOrder++;
        }

        _freeBlocks[mergedOrder].Add(mergedStart);
    }

    public byte* GetPointer(int startPage)
    {
        ThrowIfDisposed();
        return _baseAddress + checked((nuint)startPage * (nuint)PageSize);
    }

    public nuint GetBlockByteCount(int order) =>
        checked((nuint)(1 << order) * (nuint)PageSize);

    public void Dispose()
    {
        if (Volatile.Read(ref _disposed) != 0) return;
        if (_allocatedBlocks.Count != 0)
            throw new InvalidOperationException("A data-lake region cannot be released while page allocations remain outstanding.");
        if (Interlocked.Exchange(ref _disposed, 1) != 0) return;
        NativeMemory.AlignedFree(_baseAddress);
    }

    private void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(Volatile.Read(ref _disposed) != 0, this);
}
