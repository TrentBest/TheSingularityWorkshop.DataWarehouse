namespace TheSingularityWorkshop.DataWarehouse;

/// <summary>A compact, validated reference to one live slot in a data shelf.</summary>
/// <remarks>The shelf identity and generation prevent accidental cross-shelf and stale-handle access.</remarks>
public readonly record struct DataHandle(int ShelfId, int Slot, int Generation)
{
    public bool IsDefault => ShelfId == 0 || Slot < 0 || Generation <= 0;
}

internal static class ShelfIdentity
{
    private static int _next;
    public static int Next() => Interlocked.Increment(ref _next);
}
