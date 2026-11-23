namespace TheSingularityWorkshop.DataWarehouse;

/// <summary>A compact, validated reference to one live slot in a data shelf.</summary>
/// <remarks>The shelf identity and generation prevent accidental cross-shelf and stale-handle access.</remarks>
/// <param name="ShelfId">Process-local identity of the owning shelf.</param>
/// <param name="Slot">Zero-based slot index within that shelf.</param>
/// <param name="Generation">Slot incarnation counter used to reject stale handles.</param>
public readonly record struct DataHandle(int ShelfId, int Slot, int Generation)
{
    /// <summary>Whether this value is the default or otherwise lacks a valid identity component.</summary>
    public bool IsDefault => ShelfId == 0 || Slot < 0 || Generation <= 0;
}

internal static class ShelfIdentity
{
    private static int _next;

    public static int Next()
    {
        while (true)
        {
            var current = Volatile.Read(ref _next);
            if (current == int.MaxValue)
                throw new InvalidOperationException("The data-shelf identity space is exhausted.");

            var next = current + 1;
            if (Interlocked.CompareExchange(ref _next, next, current) == current)
                return next;
        }
    }
}
