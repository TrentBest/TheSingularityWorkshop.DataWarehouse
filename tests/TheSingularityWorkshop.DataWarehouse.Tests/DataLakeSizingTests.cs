using TheSingularityWorkshop.DataWarehouse;
using Xunit;

namespace TheSingularityWorkshop.DataWarehouse.Tests;

public sealed class DataLakeSizingTests
{
    [Theory]
    [InlineData(1, 1, 1)]
    [InlineData(64, 4, 256)]
    [InlineData(1024, 16, 16384)]
    public void CalculatePayloadBytes_ReturnsCheckedProduct(ulong elementCount, ulong elementSize, ulong expected)
    {
        Assert.Equal((nuint)expected,
            DataLakeSizing.CalculatePayloadBytes((nuint)elementCount, (nuint)elementSize));
    }

    [Fact]
    public void CalculatePayloadBytes_RejectsZeroOperands()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            DataLakeSizing.CalculatePayloadBytes(0, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            DataLakeSizing.CalculatePayloadBytes(1, 0));
    }

    [Fact]
    public void CalculatePayloadBytes_RejectsNativeSizeOverflow()
    {
        Assert.Throws<OverflowException>(() =>
            DataLakeSizing.CalculatePayloadBytes(nuint.MaxValue, 2));
    }

    [Theory]
    [InlineData(1, 64, 1)]
    [InlineData(64, 64, 1)]
    [InlineData(65, 64, 2)]
    [InlineData(128, 64, 2)]
    [InlineData(129, 64, 3)]
    [InlineData(4096, 4096, 1)]
    [InlineData(4097, 4096, 2)]
    public void CalculateRequiredPages_RoundsUpWithoutAllocating(
        ulong byteCount, int pageSize, int expectedPages)
    {
        Assert.Equal(expectedPages, DataLakeSizing.CalculateRequiredPages((nuint)byteCount, pageSize));
    }

    [Fact]
    public void CalculateRequiredPages_AcceptsLargestSupportedPageCount()
    {
        var maximumRepresentablePages = (int)Math.Min(
            (ulong)int.MaxValue,
            (ulong)(nuint.MaxValue / (nuint)64));
        var bytes = checked((nuint)maximumRepresentablePages * (nuint)64);

        Assert.Equal(maximumRepresentablePages, DataLakeSizing.CalculateRequiredPages(bytes, 64));
    }

    [Fact]
    public void CalculateRequiredPages_RejectsPageCountBeyondSupportedIndexRange()
    {
        Assert.Throws<OutOfMemoryException>(() =>
            DataLakeSizing.CalculateRequiredPages(nuint.MaxValue, 64));
    }

    [Theory]
    [InlineData(0)]
    public void CalculateRequiredPages_RejectsZeroByteCount(ulong byteCount)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            DataLakeSizing.CalculateRequiredPages((nuint)byteCount, 64));
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(2, 1)]
    [InlineData(3, 2)]
    [InlineData(4, 2)]
    [InlineData(5, 3)]
    [InlineData(8, 3)]
    [InlineData(9, 4)]
    [InlineData(1024, 10)]
    [InlineData(2147483647, 31)]
    public void CalculateBuddyOrder_ReturnsSmallestCoveringPowerOfTwo(int requiredPages, int expectedOrder)
    {
        Assert.Equal(expectedOrder, DataLakeSizing.CalculateBuddyOrder(requiredPages));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CalculateBuddyOrder_RejectsNonPositiveRequests(int requiredPages)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            DataLakeSizing.CalculateBuddyOrder(requiredPages));
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 4)]
    [InlineData(4, 4)]
    [InlineData(5, 8)]
    [InlineData(1024, 1024)]
    [InlineData(1073741824, 1073741824)]
    public void NextPowerOfTwo_ReturnsSmallestSupportedPowerOfTwo(int value, int expected)
    {
        Assert.Equal(expected, DataLakeSizing.NextPowerOfTwo(value));
    }

    [Theory]
    [InlineData(1073741825)]
    [InlineData(2147483647)]
    public void NextPowerOfTwo_RejectsUnsupportedRegionSize(int value)
    {
        Assert.Throws<OutOfMemoryException>(() => DataLakeSizing.NextPowerOfTwo(value));
    }
}
