namespace TheSingularityWorkshop.DataWarehouse;

/// <summary>The aggregate owner and factory for a shared DataLake and its typed shelves.</summary>
public sealed class DataWarehouse : IDisposable
{
    private readonly List<IDisposable> _shelves = [];
    private readonly object _gate = new();
    private int _disposed;

    /// <summary>The shared page allocator and owner of native backing regions.</summary>
    public DataLake Lake { get; }
    /// <summary>The default per-shelf page alignment in bytes.</summary>
    public int DefaultPageSize { get; }
    /// <summary>The normal native backing-region size in bytes.</summary>
    public int DefaultRegionSize { get; }

    /// <summary>Creates a warehouse with a shared lake and configurable page/region geometry.</summary>
    public DataWarehouse(int defaultPageSize = 4096, int defaultRegionSize = 4 * 1024 * 1024)
    {
        // Let DataLake own the canonical validation rules for page and region geometry.
        Lake = new DataLake(defaultPageSize, defaultRegionSize);
        DefaultPageSize = defaultPageSize;
        DefaultRegionSize = defaultRegionSize;
    }

    /// <summary>Creates and tracks a typed shelf with the requested initial slot capacity.</summary>
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
                _shelves.Add(shelf);
            }
            catch
            {
                // The shelf already owns pages in the shared lake. If managed ownership
                // registration fails (for example, List growth throws), return those pages
                // before propagating the original failure.
                shelf.Dispose();
                throw;
            }
        }
        return shelf;
    }

    /// <summary>Disposes registered shelves in reverse creation order, then closes the lake.</summary>
    public void Dispose()
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0) return;
        lock (_gate)
        {
            for (var i = _shelves.Count - 1; i >= 0; i--) _shelves[i].Dispose();
            _shelves.Clear();
        }
        Lake.Dispose();
        GC.SuppressFinalize(this);
    }
}
