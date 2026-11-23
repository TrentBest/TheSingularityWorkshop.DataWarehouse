using System.Numerics;

namespace TheSingularityWorkshop.DataWarehouse;

/// <summary>Pure sizing and buddy-order calculations shared by the lake allocator.</summary>
internal static class DataLakeSizing
{
    public static nuint CalculatePayloadBytes(nuint elementCount, nuint elementSize)
    {
        if (elementCount == 0)
            throw new ArgumentOutOfRangeException(nameof(elementCount));
        if (elementSize == 0)
            throw new ArgumentOutOfRangeException(nameof(elementSize));

        return checked(elementCount * elementSize);
    }

    public static int CalculateRequiredPages(nuint byteCount, int pageSize)
    {
        if (byteCount == 0)
            throw new ArgumentOutOfRangeException(nameof(byteCount));
        if (pageSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(pageSize));

        var nativePageSize = (nuint)pageSize;
        var pages = byteCount / nativePageSize;
        if (byteCount % nativePageSize != 0)
            pages = checked(pages + 1);

        if (pages > int.MaxValue)
            throw new OutOfMemoryException("The requested allocation exceeds the supported page-index range.");

        return (int)pages;
    }

    public static int CalculateBuddyOrder(int requiredPages)
    {
        if (requiredPages <= 0)
            throw new ArgumentOutOfRangeException(nameof(requiredPages));

        return requiredPages == 1
            ? 0
            : BitOperations.Log2((uint)(requiredPages - 1)) + 1;
    }

    public static int NextPowerOfTwo(int value)
    {
        if (value <= 1) return 1;

        var order = BitOperations.Log2((uint)(value - 1)) + 1;
        if (order >= 31)
            throw new OutOfMemoryException("The requested data-lake region exceeds the supported page-index range.");

        return 1 << order;
    }
}
