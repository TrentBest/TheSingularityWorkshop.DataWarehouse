<p align="center"><a href="https://github.com/TrentBest/TheSingularityWorkshop.Visualize"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop" width="720" /></a></p>

# DataShelf<T> — API and operational contract

## 00 Identity

DataShelf<T> stores a fixed capacity of unmanaged records in page-aligned native memory. Each shelf owns its slot metadata and refers to a DataLake for allocation and release.

**Evidence status:** source-checked against src/TheSingularityWorkshop.DataWarehouse/DataShelf.cs on development. Complexity below is algorithmic analysis of the current implementation, not a benchmark result.

## 01 What it owns

- A single native allocation for Capacity × sizeof(T) bytes, rounded to the requested page size for allocation.
- A free-slot stack and one generation integer per slot.
- Separate alive and dirty bitmaps, each using one bit per slot.
- Alive and dirty counters.
- The shelf identity used to reject handles from other shelves.

The bitmaps are external to the stored T records. The shelf does not allocate a managed object for every record.

## 02 Why the contract is explicit

The direct access path avoids a per-record dictionary lookup. In exchange, capacity is fixed when the shelf is created, T must be unmanaged, and callers must respect handle validity and disposal lifetime. GetRef is mutable access and marks the record dirty before returning the reference; Read does not mark it dirty.

## 03 Operations and complexity

| Operation | Current behavior | Algorithmic cost |
|---|---|---:|
| TryAllocate(out handle) | Pops a free slot, marks alive + dirty, clears record bytes, creates a generation-checked handle | O(1) plus work proportional to sizeof(T) for clearing |
| GetRef(handle) | Validates shelf, bounds, generation, liveness; marks dirty if needed; returns mutable ref | O(1) |
| Read(handle) | Same handle validation; returns read-only ref | O(1) |
| IsAlive(handle) | Returns false for disposed, foreign, out-of-range, stale, or non-alive handles | O(1) |
| IsDirty(handle) | Validates handle and reads dirty bit | O(1) |
| MarkClean(handle) | Validates handle; clears dirty bit if set | O(1) |
| TryFree(handle) | Rejects non-live handles; clears tracking, advances generation, pushes slot to free stack | O(1) |
| CopyDirtySlots(destination) | Scans dirty bitmap words and copies dirty slot indexes until buffer fills or scan ends | O(bitmap words scanned + indexes emitted), worst case O(Capacity) |
| Dispose() | Frees native allocation once and resets visible counts | O(1) release bookkeeping |

**Important distinctions**

- TryAllocate clears sizeof(T) bytes. Its work is constant with respect to shelf capacity, but scales with record size.
- CopyDirtySlots is not constant-time. It scans bitmap words; a small destination can stop the scan early.
- Handle validation is constant-time, but not free: it checks multiple fields and reads metadata.
- Page alignment is an address guarantee, not a guarantee that every record or every access is cache-line aligned.
- Generation values wrap from int.MaxValue to 1. This is a finite generation space; extremely old stale handles could theoretically collide after enough reuse cycles.

## 04 State and failure behavior

- Capacity must be positive.
- Page size must be a power of two and at least 64 bytes.
- Allocation exhaustion returns false and a default handle.
- GetRef, Read, IsDirty, and MarkClean throw InvalidOperationException for invalid, stale, foreign, or non-live handles (except disposed access may throw ObjectDisposedException).
- TryFree returns false when the handle is not currently alive.
- Reuse increments the slot generation, so the old handle no longer validates.
- GetRef marks a slot dirty when the ref is requested. The shelf cannot observe subsequent writes through the returned ref; it marks before exposure.
- Shelf operations are not designed as a thread-safe concurrent mutation API. Do not race access, allocation, freeing, dirty tracking, or disposal without external synchronization.

## 05 Verification

Current unit-test coverage includes page alignment, allocation/liveness/dirty state, direct read/write, clean/free, stale generation rejection, capacity exhaustion, cross-shelf rejection, and dirty-slot copying. See [DataShelfTests.cs](../../tests/TheSingularityWorkshop.DataWarehouse.Tests/DataShelfTests.cs).

Performance comparisons live in the [benchmark repository](https://github.com/TrentBest/TheSingularityWorkshop.Benchmarks.DataWarehouse). Do not infer measured speed from the complexity table.

<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_COS/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
