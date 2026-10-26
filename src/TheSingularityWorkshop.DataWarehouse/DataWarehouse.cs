namespace TheSingularityWorkshop.DataWarehouse;

/// <summary>The owner and factory for a Data Lake and its page-aligned shelves.</summary>
public sealed class DataWarehouse : IDisposable
{
    private readonly List<IDisposable> _shelves = [];
    private readonly object _gate = new();
    private int _disposed;

    public DataLake Lake { get; } = new();
    public int DefaultPageSize { get; }

    public DataWarehouse(int defaultPageSize = 4096)
    {
        if (defaultPageSize < 64 || (defaultPageSize & (defaultPageSize - 1)) != 0)
            throw new ArgumentOutOfRangeException(nameof(defaultPageSize), "Page size must be a power of two and at least 64 bytes.");
        DefaultPageSize = defaultPageSize;
    }

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
            _shelves.Add(shelf);
        }
        return shelf;
    }

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
