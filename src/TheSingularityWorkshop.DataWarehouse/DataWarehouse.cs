namespace TheSingularityWorkshop.DataWarehouse;

/// <summary>The aggregate owner and factory for a shared DataLake and its typed shelves.</summary>
public sealed class DataWarehouse : IDisposable
{
    private readonly LinkedList<IDisposable> _shelves = [];
    private readonly object _gate = new();
    private int _disposed;

    /// <summary>The shared page allocator and owner of native backing regions.</summary>
    public DataLake Lake { get; }
    /// <summary>The default per-shelf page alignment in bytes.</summary>
    public int DefaultPageSize { get; }
    /// <summary>The normal native backing-region size in bytes.</summary>
    public int DefaultRegionSize { get; }

    /// <summary>Number of shelves currently registered for aggregate disposal.</summary>
    public int ShelfCount
    {
        get { lock (_gate) return _shelves.Count; }
    }

    /// <summary>Creates a warehouse with a shared lake and configurable page/region geometry.</summary>
    /// <param name="defaultPageSize">Default page alignment for created shelves.</param>
    /// <param name="defaultRegionSize">Normal native backing-region size for the shared lake.</param>
    public DataWarehouse(int defaultPageSize = 4096, int defaultRegionSize = 4 * 1024 * 1024)
    {
        // Let DataLake own the canonical validation rules for page and region geometry.
        Lake = new DataLake(defaultPageSize, defaultRegionSize);
        DefaultPageSize = defaultPageSize;
        DefaultRegionSize = defaultRegionSize;
    }

    /// <summary>Creates and tracks a typed shelf with the requested initial slot capacity.</summary>
    /// <typeparam name="T">Unmanaged record type stored in the shelf.</typeparam>
    /// <param name="capacity">Initial number of slots.</param>
    /// <param name="pageSize">Optional per-shelf alignment override.</param>
    public DataShelf<T> CreateShelf<T>(int capacity, int? pageSize = null) where T : unmanaged
    {
        ObjectDisposedException.ThrowIf(Volatile.Read(ref _disposed) != 0, this);
        var shelf = new DataShelf<T>(Lake, capacity, pageSize ?? DefaultPageSize);
        lock (_gate)
        {
            if (Volatile.Read(ref _disposed) != 0)
            {
                shelf.Dispose();
                throw new ObjectDisposedException(nameof(DataWarehouse));
            }

            try
            {
                var node = _shelves.AddLast(shelf);
                shelf.AttachOwner(node, UnregisterShelf);
            }
            catch
            {
                // The shelf already owns pages in the shared lake. If managed ownership
                // registration fails, return those pages before propagating the failure.
                shelf.Dispose();
                throw;
            }
        }
        return shelf;
    }

    private void UnregisterShelf(LinkedListNode<IDisposable> node)
    {
        lock (_gate)
        {
            if (node.List == _shelves)
                _shelves.Remove(node);
        }
    }

    /// <summary>Disposes registered shelves in reverse creation order, then closes the lake.</summary>
    public void Dispose()
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0) return;
        lock (_gate)
        {
            while (_shelves.Last is { } node)
            {
                _shelves.RemoveLast();
                node.Value.Dispose();
            }
        }
        Lake.Dispose();
        GC.SuppressFinalize(this);
    }
}
