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
        Assert.Equal(16384, shelf.AllocatedBytes);
        Assert.Equal(16384, warehouse.Lake.AllocatedBytes);
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


    [Fact]
    public void NewAndReusedSlots_AreZeroInitialized()
    {
        using var warehouse = new DataWarehouse();
        using var shelf = warehouse.CreateShelf<Record>(1);

        Assert.True(shelf.TryAllocate(out var first));
        shelf.GetRef(first).Id = 99;
        shelf.GetRef(first).State = 7;
        Assert.True(shelf.TryFree(first));

        Assert.True(shelf.TryAllocate(out var second));
        Assert.Equal(0, shelf.Read(second).Id);
        Assert.Equal(0, shelf.Read(second).State);
    }

    [Fact]
    public void Read_DoesNotMarkCleanRecordDirty()
    {
        using var warehouse = new DataWarehouse();
        using var shelf = warehouse.CreateShelf<Record>(1);
        Assert.True(shelf.TryAllocate(out var handle));
        Assert.True(shelf.MarkClean(handle));

        _ = shelf.Read(handle).Id;

        Assert.False(shelf.IsDirty(handle));
        Assert.Equal(0, shelf.DirtyCount);
    }

    [Fact]
    public void RepeatedMutableAccess_OnlyCountsOneDirtyTransition()
    {
        using var warehouse = new DataWarehouse();
        using var shelf = warehouse.CreateShelf<Record>(1);
        Assert.True(shelf.TryAllocate(out var handle));
        Assert.Equal(1, shelf.DirtyCount);

        _ = shelf.GetRef(handle).Id;
        _ = shelf.GetRef(handle).State;

        Assert.Equal(1, shelf.DirtyCount);
        Assert.True(shelf.MarkClean(handle));
        Assert.False(shelf.MarkClean(handle));
        Assert.Equal(0, shelf.DirtyCount);
    }

    [Fact]
    public void CopyDirtySlots_EmptyAndUndersizedBuffersHaveDefinedBehavior()
    {
        using var warehouse = new DataWarehouse();
        using var shelf = warehouse.CreateShelf<Record>(4);
        Assert.True(shelf.TryAllocate(out _));
        Assert.True(shelf.TryAllocate(out _));
        Assert.True(shelf.TryAllocate(out _));

        Span<int> empty = Span<int>.Empty;
        Assert.Equal(0, shelf.CopyDirtySlots(empty));

        Span<int> one = stackalloc int[1];
        Assert.Equal(1, shelf.CopyDirtySlots(one));
        Assert.Equal(0, one[0]);
    }

    [Theory]
    [InlineData(63)]
    [InlineData(64)]
    [InlineData(65)]
    [InlineData(127)]
    [InlineData(128)]
    [InlineData(129)]
    public void DirtyBitmap_WordBoundariesEnumerateCorrectSlots(int capacity)
    {
        using var warehouse = new DataWarehouse();
        using var shelf = warehouse.CreateShelf<Record>(capacity);
        for (var i = 0; i < capacity; i++)
        {
            Assert.True(shelf.TryAllocate(out var handle));
            if (i != 0 && i != capacity - 1)
                Assert.True(shelf.MarkClean(handle));
        }

        Span<int> dirty = stackalloc int[2];
        var count = shelf.CopyDirtySlots(dirty);

        Assert.Equal(capacity == 1 ? 1 : 2, count);
        Assert.Equal(0, dirty[0]);
        if (capacity > 1)
            Assert.Equal(capacity - 1, dirty[1]);
    }

    [Fact]
    public void InvalidDefaultAndOutOfRangeHandlesAreRejected()
    {
        using var warehouse = new DataWarehouse();
        using var shelf = warehouse.CreateShelf<Record>(2);
        Assert.True(shelf.TryAllocate(out var valid));

        Assert.False(shelf.IsAlive(default));
        Assert.Throws<InvalidOperationException>(() => ReadValue(shelf, default));
        Assert.Throws<InvalidOperationException>(() => ReadValue(shelf, valid with { Slot = 2 }));
        Assert.Throws<InvalidOperationException>(() => ReadValue(shelf, valid with { Generation = 0 }));
    }

    [Fact]
    public void ShelfOperations_ThrowAfterDisposal()
    {
        using var warehouse = new DataWarehouse();
        var shelf = warehouse.CreateShelf<Record>(1);
        Assert.True(shelf.TryAllocate(out var handle));
        shelf.Dispose();

        Assert.False(shelf.IsAlive(handle));
        Assert.Throws<ObjectDisposedException>(() => ReadValue(shelf, handle));
        Assert.Throws<ObjectDisposedException>(() => shelf.TryAllocate(out _));
        Assert.Throws<ObjectDisposedException>(() => shelf.CopyDirtySlots(Span<int>.Empty));
    }

    [Fact]
    public void WarehouseAndShelfRejectInvalidCapacitiesAndPageSizes()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new DataWarehouse(63));
        Assert.Throws<ArgumentOutOfRangeException>(() => new DataWarehouse(65));

        using var warehouse = new DataWarehouse();
        Assert.Throws<ArgumentOutOfRangeException>(() => warehouse.CreateShelf<Record>(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => warehouse.CreateShelf<Record>(-1));
    }

    [Fact]
    public void IndividualShelfDisposal_UpdatesAggregateLakeAccounting()
    {
        using var warehouse = new DataWarehouse();
        var first = warehouse.CreateShelf<Record>(2);
        var second = warehouse.CreateShelf<Record>(3);
        var expected = first.AllocatedBytes + second.AllocatedBytes;
        Assert.Equal(expected, warehouse.Lake.AllocatedBytes);

        first.Dispose();

        Assert.Equal(second.AllocatedBytes, warehouse.Lake.AllocatedBytes);
        second.Dispose();
        Assert.Equal(0, warehouse.Lake.AllocatedBytes);
    }


    [Fact]
    public void MultipleShelves_SuballocateFromOneSharedBackingRegion()
    {
        using var warehouse = new DataWarehouse(4096, 16384);
        using var first = warehouse.CreateShelf<Record>(1);
        using var second = warehouse.CreateShelf<Record>(1);

        Assert.Equal(1, warehouse.Lake.RegionCount);
        Assert.Equal(16384, warehouse.Lake.ReservedBytes);
        Assert.Equal(first.AllocatedBytes + second.AllocatedBytes, warehouse.Lake.AllocatedBytes);
        Assert.NotEqual(first.BaseAddress, second.BaseAddress);
    }

    [Fact]
    public void FreedBuddyPages_CoalesceForLargerShelfAllocation()
    {
        using var warehouse = new DataWarehouse(4096, 16384);
        var first = warehouse.CreateShelf<Record>(1);
        var second = warehouse.CreateShelf<Record>(1);
        Assert.Equal(1, warehouse.Lake.RegionCount);

        first.Dispose();
        second.Dispose();
        Assert.Equal(0, warehouse.Lake.AllocatedBytes);

        using var larger = warehouse.CreateShelf<Record>(257); // 4,112 payload bytes => two pages.
        Assert.Equal(1, warehouse.Lake.RegionCount);
        Assert.Equal(8192, larger.AllocatedBytes);
        Assert.Equal(8192, warehouse.Lake.AllocatedBytes);
    }

    [Fact]
    public void EnsureCapacity_GrowsShelfAndPreservesExistingHandlesAndState()
    {
        using var warehouse = new DataWarehouse(4096, 16384);
        using var shelf = warehouse.CreateShelf<Record>(1);
        Assert.True(shelf.TryAllocate(out var original));
        shelf.GetRef(original).Id = 1234;
        shelf.GetRef(original).State = 9;
        Assert.True(shelf.MarkClean(original));

        var previousAddress = shelf.BaseAddress;
        shelf.EnsureCapacity(257);

        Assert.Equal(257, shelf.Capacity);
        Assert.NotEqual(previousAddress, shelf.BaseAddress);
        Assert.True(shelf.IsAlive(original));
        Assert.False(shelf.IsDirty(original));
        Assert.Equal(1234, shelf.Read(original).Id);
        Assert.Equal(9, shelf.Read(original).State);

        Assert.True(shelf.TryAllocate(out var added));
        Assert.Equal(1, added.Slot);
        Assert.Equal(0, shelf.Read(added).Id);
        Assert.Equal(0, shelf.Read(added).State);
        Assert.Equal(2, shelf.AliveCount);
        Assert.Equal(1, shelf.DirtyCount);
    }

    [Fact]
    public void EnsureCapacity_NoOpsForExistingCapacityAndRejectsInvalidValues()
    {
        using var warehouse = new DataWarehouse(4096, 16384);
        using var shelf = warehouse.CreateShelf<Record>(4);
        var address = shelf.BaseAddress;

        shelf.EnsureCapacity(4);
        shelf.EnsureCapacity(2);

        Assert.Equal(4, shelf.Capacity);
        Assert.Equal(address, shelf.BaseAddress);
        Assert.Throws<ArgumentOutOfRangeException>(() => shelf.EnsureCapacity(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => shelf.EnsureCapacity(-1));
    }

    [Fact]
    public void LakeDispose_DefersRegionReleaseUntilEveryShelfReturnsItsPages()
    {
        using var warehouse = new DataWarehouse(4096, 16384);
        var first = warehouse.CreateShelf<Record>(1);
        var second = warehouse.CreateShelf<Record>(1);

        warehouse.Lake.Dispose();

        Assert.True(warehouse.Lake.IsDisposed);
        Assert.Equal(16384, warehouse.Lake.ReservedBytes);
        Assert.Throws<ObjectDisposedException>(() => warehouse.CreateShelf<Record>(1));

        first.Dispose();
        Assert.Equal(16384, warehouse.Lake.ReservedBytes);
        second.Dispose();

        Assert.Equal(0, warehouse.Lake.AllocatedBytes);
        Assert.Equal(0, warehouse.Lake.ReservedBytes);
        Assert.Equal(0, warehouse.Lake.RegionCount);
    }

    [Fact]
    public void OversizedShelf_ReceivesDedicatedPowerOfTwoBackingRegion()
    {
        using var warehouse = new DataWarehouse(4096, 16384);
        using var shelf = warehouse.CreateShelf<Record>(1025); // 16,400 bytes => five pages.

        Assert.Equal(32768, shelf.AllocatedBytes);
        Assert.Equal(32768, warehouse.Lake.ReservedBytes);
        Assert.Equal(1, warehouse.Lake.RegionCount);
    }

    [Fact]
    public void LakeRetainsFreedRegionsForReuseUntilDisposed()
    {
        using var warehouse = new DataWarehouse(4096, 16384);
        var first = warehouse.CreateShelf<Record>(1);
        var address = first.BaseAddress;
        first.Dispose();

        Assert.Equal(0, warehouse.Lake.AllocatedBytes);
        Assert.Equal(16384, warehouse.Lake.ReservedBytes);
        Assert.Equal(1, warehouse.Lake.RegionCount);

        using var second = warehouse.CreateShelf<Record>(1);
        Assert.Equal(1, warehouse.Lake.RegionCount);
        Assert.Equal(address, second.BaseAddress);
    }


    [Fact]
    public void FreedPages_AreClearedBeforeAnotherShelfReusesThem()
    {
        using var warehouse = new DataWarehouse(4096, 16384);
        var first = warehouse.CreateShelf<Record>(1);
        Assert.True(first.TryAllocate(out var firstHandle));
        first.GetRef(firstHandle).Id = 0x5A5A5A5A;
        var releasedAddress = first.BaseAddress;
        first.Dispose();

        using var second = warehouse.CreateShelf<Record>(1);
        Assert.Equal(releasedAddress, second.BaseAddress);
        Assert.True(second.TryAllocate(out var secondHandle));
        Assert.Equal(0, second.Read(secondHandle).Id);
    }

    [Fact]
    public void BuddyAllocator_RandomizedAllocationsDoNotOverlapAndFullyCoalesce()
    {
        using var warehouse = new DataWarehouse(4096, 1024 * 1024);
        var random = new Random(19771123);
        var shelves = new List<DataShelf<Record>>();
        var ranges = new List<(nuint Start, nuint End)>();

        for (var i = 0; i < 80; i++)
        {
            var shelf = warehouse.CreateShelf<Record>(random.Next(1, 300));
            var start = (nuint)shelf.BaseAddress;
            var end = checked(start + (nuint)shelf.AllocatedBytes);

            foreach (var existing in ranges)
                Assert.False(start < existing.End && existing.Start < end, "Live shelf allocations must never overlap.");

            ranges.Add((start, end));
            shelves.Add(shelf);
        }

        foreach (var shelf in shelves.OrderBy(_ => random.Next()))
            shelf.Dispose();

        Assert.Equal(0, warehouse.Lake.AllocatedBytes);
        Assert.Equal(warehouse.Lake.ReservedBytes, warehouse.Lake.FreeBytes);

        using var probe = warehouse.CreateShelf<Record>(1024);
        Assert.Equal(1, warehouse.Lake.RegionCount);
    }

    [Fact]
    public void EnsureCapacity_CrossesBitmapWordsAndPreservesDirtyState()
    {
        using var warehouse = new DataWarehouse(4096, 65536);
        using var shelf = warehouse.CreateShelf<Record>(65);
        var handles = new DataHandle[65];

        for (var i = 0; i < handles.Length; i++)
        {
            Assert.True(shelf.TryAllocate(out handles[i]));
            shelf.GetRef(handles[i]).Id = i + 100;
            if (i % 2 == 0)
                Assert.True(shelf.MarkClean(handles[i]));
        }

        shelf.EnsureCapacity(129);

        Assert.Equal(65, shelf.AliveCount);
        Assert.Equal(32, shelf.DirtyCount);
        for (var i = 0; i < handles.Length; i++)
        {
            Assert.True(shelf.IsAlive(handles[i]));
            Assert.Equal(i % 2 != 0, shelf.IsDirty(handles[i]));
            Assert.Equal(i + 100, shelf.Read(handles[i]).Id);
        }

        Span<int> dirty = stackalloc int[32];
        Assert.Equal(32, shelf.CopyDirtySlots(dirty));
        for (var i = 0; i < dirty.Length; i++)
            Assert.Equal((i * 2) + 1, dirty[i]);

        Assert.True(shelf.TryAllocate(out var firstAdded));
        Assert.Equal(65, firstAdded.Slot);
    }

    [Fact]
    public void ShelfAlignmentOverride_CanExceedLakeBasePageSize()
    {
        using var warehouse = new DataWarehouse(4096, 16384);
        using var shelf = warehouse.CreateShelf<Record>(1, pageSize: 8192);

        Assert.Equal(0, (long)shelf.BaseAddress % 8192);
        Assert.True(shelf.AllocatedBytes >= 8192);
    }

    [Fact]
    public void EnsureCapacity_AfterLakeCloseFailsWithoutChangingExistingShelf()
    {
        using var warehouse = new DataWarehouse(4096, 16384);
        using var shelf = warehouse.CreateShelf<Record>(1);
        Assert.True(shelf.TryAllocate(out var handle));
        shelf.GetRef(handle).Id = 77;

        warehouse.Lake.Dispose();

        Assert.Throws<ObjectDisposedException>(() => shelf.EnsureCapacity(257));
        Assert.Equal(1, shelf.Capacity);
        Assert.Equal(77, shelf.Read(handle).Id);
    }

    [Fact]
    public void WarehouseRejectsInvalidRegionGeometry()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new DataWarehouse(4096, 10000));
        Assert.Throws<ArgumentOutOfRangeException>(() => new DataWarehouse(8192, 4096));
    }


    [Fact]
    public void TryGetHandle_ResolvesLiveDirtySlotsAndRejectsFreedSlots()
    {
        using var warehouse = new DataWarehouse(4096, 16384);
        using var shelf = warehouse.CreateShelf<Record>(4);
        Assert.True(shelf.TryAllocate(out var original));

        Span<int> dirtySlots = stackalloc int[4];
        Assert.Equal(1, shelf.CopyDirtySlots(dirtySlots));
        Assert.Equal(original.Slot, dirtySlots[0]);

        Assert.True(shelf.TryGetHandle(dirtySlots[0], out var resolved));
        Assert.Equal(original, resolved);

        Assert.True(shelf.TryFree(original));
        Assert.False(shelf.TryGetHandle(original.Slot, out var freed));
        Assert.True(freed.IsDefault);
        Assert.False(shelf.TryGetHandle(-1, out _));
        Assert.False(shelf.TryGetHandle(shelf.Capacity, out _));
    }


    [Fact]
    public void IndividuallyDisposedShelves_AreUnregisteredFromWarehouse()
    {
        using var warehouse = new DataWarehouse(4096, 16384);
        var first = warehouse.CreateShelf<Record>(1);
        var second = warehouse.CreateShelf<Record>(1);
        Assert.Equal(2, warehouse.ShelfCount);

        first.Dispose();
        Assert.Equal(1, warehouse.ShelfCount);
        first.Dispose();
        Assert.Equal(1, warehouse.ShelfCount);

        second.Dispose();
        Assert.Equal(0, warehouse.ShelfCount);

        using var third = warehouse.CreateShelf<Record>(1);
        Assert.Equal(1, warehouse.ShelfCount);
        warehouse.Dispose();
        Assert.Equal(0, warehouse.ShelfCount);
    }

    private static long SetAndRead(DataShelf<Record> shelf, DataHandle handle, long value)
    {
        shelf.GetRef(handle).Id = value;
        return shelf.Read(handle).Id;
    }

    private static long ReadValue(DataShelf<Record> shelf, DataHandle handle) => shelf.Read(handle).Id;
}
