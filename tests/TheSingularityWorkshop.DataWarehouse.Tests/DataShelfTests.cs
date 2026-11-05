using System.Runtime.InteropServices;
using TheSingularityWorkshop.DataWarehouse;
using Xunit;

namespace TheSingularityWorkshop.DataWarehouse.Tests;

public sealed class DataShelfTests
{
    private struct Record
    {
        public long Id;
        public int State;
    }

    [StructLayout(LayoutKind.Sequential, Size = 8192)]
    private struct LargeRecord
    {
        public byte Value;
    }

    [StructLayout(LayoutKind.Sequential, Size = 5000)]
    private struct NonPageMultipleLargeRecord
    {
        public byte Value;
    }

    [Fact]
    public void ShelfBaseAddress_IsPageAligned()
    {
        using var warehouse = new DataWarehouse(4096);
        using var shelf = warehouse.CreateShelf<Record>(1000);
        Assert.Equal(0, (long)shelf.BaseAddress % shelf.PageSize);
    }

    [Fact]
    public void Allocation_IsAliveAndDirty_AndMutableAccessIsDirect()
    {
        using var warehouse = new DataWarehouse();
        using var shelf = warehouse.CreateShelf<Record>(8);

        Assert.True(shelf.TryAllocate(out var handle));
        Assert.True(shelf.IsAlive(handle));
        Assert.True(shelf.IsDirty(handle));
        shelf.GetRef(handle).Id = 42;
        shelf.GetRef(handle).State = 7;

        Assert.Equal(42, shelf.Read(handle).Id);
        Assert.Equal(7, shelf.Read(handle).State);
        Assert.Equal(1, shelf.AliveCount);
        Assert.Equal(1, shelf.DirtyCount);
    }

    [Fact]
    public void MarkClean_AndFree_UpdateExternalBitTrackers()
    {
        using var warehouse = new DataWarehouse();
        using var shelf = warehouse.CreateShelf<Record>(8);
        Assert.True(shelf.TryAllocate(out var handle));
        Assert.True(shelf.MarkClean(handle));
        Assert.False(shelf.IsDirty(handle));
        Assert.Equal(0, shelf.DirtyCount);

        Assert.True(shelf.TryFree(handle));
        Assert.False(shelf.IsAlive(handle));
        Assert.Equal(0, shelf.AliveCount);
        Assert.False(shelf.TryFree(handle));
    }

    [Fact]
    public void ReuseInvalidatesStaleGeneration()
    {
        using var warehouse = new DataWarehouse();
        using var shelf = warehouse.CreateShelf<Record>(1);
        Assert.True(shelf.TryAllocate(out var original));
        Assert.True(shelf.TryFree(original));
        Assert.True(shelf.TryAllocate(out var replacement));

        Assert.Equal(original.Slot, replacement.Slot);
        Assert.NotEqual(original.Generation, replacement.Generation);
        Assert.False(shelf.IsAlive(original));
        Assert.Throws<InvalidOperationException>(() => ReadValue(shelf, original));
    }

    [Fact]
    public void ExhaustedShelfReturnsFalseWithoutAllocating()
    {
        using var warehouse = new DataWarehouse();
        using var shelf = warehouse.CreateShelf<Record>(1);
        Assert.True(shelf.TryAllocate(out _));
        Assert.False(shelf.TryAllocate(out var failed));
        Assert.True(failed.IsDefault);
    }

    [Fact]
    public void WrongShelfHandleIsRejected()
    {
        using var warehouse = new DataWarehouse();
        using var first = warehouse.CreateShelf<Record>(2);
        using var second = warehouse.CreateShelf<Record>(2);
        Assert.True(first.TryAllocate(out var handle));
        Assert.Throws<InvalidOperationException>(() => ReadValue(second, handle));
    }

    [Fact]
    public void DirtySlotsCanBeCopiedWithoutPerRecordObjects()
    {
        using var warehouse = new DataWarehouse();
        using var shelf = warehouse.CreateShelf<Record>(130);
        Assert.True(shelf.TryAllocate(out var first));
        Assert.True(shelf.TryAllocate(out var second));
        Assert.True(shelf.MarkClean(first));

        Span<int> dirty = stackalloc int[4];
        var count = shelf.CopyDirtySlots(dirty);
        Assert.Equal(1, count);
        Assert.Equal(second.Slot, dirty[0]);
    }

    [Theory]
    [InlineData(64, 128)]
    [InlineData(4096, 4096)]
    [InlineData(16384, 16384)]
    public void Allocation_RoundsExtentToConfiguredPageSize(int pageSize, long expectedBytes)
    {
        using var warehouse = new DataWarehouse(pageSize);
        using var shelf = warehouse.CreateShelf<Record>(5);

        Assert.Equal(expectedBytes, shelf.AllocatedBytes);
        Assert.Equal(expectedBytes, warehouse.Lake.AllocatedBytes);
        Assert.Equal(0, (long)shelf.BaseAddress % pageSize);
    }

    [Fact]
    public void ShelfDispose_ReleasesAllocationExactlyOnce()
    {
        using var warehouse = new DataWarehouse();
        var shelf = warehouse.CreateShelf<Record>(17);
        var allocated = shelf.AllocatedBytes;

        Assert.True(allocated > 0);
        Assert.Equal(allocated, warehouse.Lake.AllocatedBytes);

        shelf.Dispose();
        Assert.Equal(0, warehouse.Lake.AllocatedBytes);
        Assert.Equal(0, shelf.AliveCount);
        Assert.Equal(0, shelf.DirtyCount);

        shelf.Dispose();
        Assert.Equal(0, warehouse.Lake.AllocatedBytes);
    }

    [Fact]
    public void WarehouseDispose_ReleasesAllOutstandingShelfAllocations()
    {
        var warehouse = new DataWarehouse();
        _ = warehouse.CreateShelf<Record>(3);
        _ = warehouse.CreateShelf<Record>(19);

        Assert.True(warehouse.Lake.AllocatedBytes > 0);
        warehouse.Dispose();

        Assert.Equal(0, warehouse.Lake.AllocatedBytes);
        warehouse.Dispose();
        Assert.Equal(0, warehouse.Lake.AllocatedBytes);
    }

    [Fact]
    public void PartialPageAllocation_UsesRoundedAllocationAccounting()
    {
        using var warehouse = new DataWarehouse(4096);
        using var shelf = warehouse.CreateShelf<Record>(1);

        Assert.Equal(4096, shelf.AllocatedBytes);
        Assert.Equal(shelf.AllocatedBytes, warehouse.Lake.AllocatedBytes);

        shelf.Dispose();
        Assert.Equal(0, warehouse.Lake.AllocatedBytes);
    }


    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(63)]
    [InlineData(65)]
    [InlineData(4095)]
    public void ShelfRejectsInvalidPageSizes(int pageSize)
    {
        using var warehouse = new DataWarehouse();
        Assert.Throws<ArgumentOutOfRangeException>(() => warehouse.CreateShelf<Record>(1, pageSize));
    }

    [Fact]
    public void ElementLargerThanPageSize_RoundsAllocationExtentToPageMultiple()
    {
        using var warehouse = new DataWarehouse(4096);
        using var shelf = warehouse.CreateShelf<LargeRecord>(1);

        Assert.Equal(8192, shelf.ElementSize);
        Assert.Equal(8192, shelf.AllocatedBytes);
        Assert.Equal(8192, warehouse.Lake.AllocatedBytes);
        Assert.Equal(0, (long)shelf.BaseAddress % shelf.PageSize);
    }

    [Fact]
    public void MultipleLargeElements_RoundTotalExtentToPageMultiple()
    {
        using var warehouse = new DataWarehouse(4096);
        using var shelf = warehouse.CreateShelf<NonPageMultipleLargeRecord>(2);

        Assert.Equal(5000, shelf.ElementSize);
        Assert.Equal(12288, shelf.AllocatedBytes);
        Assert.Equal(12288, warehouse.Lake.AllocatedBytes);
        Assert.Equal(0, (long)shelf.BaseAddress % shelf.PageSize);
    }


    [Fact]
    public void GenerationExhaustion_RetiresSlotInsteadOfRevalidatingOldHandles()
    {
        using var warehouse = new DataWarehouse();
        using var shelf = warehouse.CreateShelf<Record>(1);
        Assert.True(shelf.TryAllocate(out var handle));

        var generationsField = typeof(DataShelf<Record>).GetField(
            "_generations",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!;
        var generations = (int[])generationsField.GetValue(shelf)!;
        generations[handle.Slot] = int.MaxValue;

        var exhaustedHandle = handle with { Generation = int.MaxValue };
        Assert.True(shelf.TryFree(exhaustedHandle));
        Assert.False(shelf.IsAlive(exhaustedHandle));
        Assert.False(shelf.TryAllocate(out _));
    }

    [Fact]
    public void LakeDispose_PreventsNewAllocationsButExistingShelfCanReleaseMemory()
    {
        using var warehouse = new DataWarehouse();
        var shelf = warehouse.CreateShelf<Record>(2);
        Assert.True(shelf.TryAllocate(out var handle));

        warehouse.Lake.Dispose();

        Assert.Equal(42, SetAndRead(shelf, handle, 42));
        Assert.Throws<ObjectDisposedException>(() => warehouse.CreateShelf<Record>(1));

        shelf.Dispose();
        Assert.Equal(0, warehouse.Lake.AllocatedBytes);
    }

    private static long SetAndRead(DataShelf<Record> shelf, DataHandle handle, long value)
    {
        shelf.GetRef(handle).Id = value;
        return shelf.Read(handle).Id;
    }

    private static long ReadValue(DataShelf<Record> shelf, DataHandle handle) => shelf.Read(handle).Id;
}
