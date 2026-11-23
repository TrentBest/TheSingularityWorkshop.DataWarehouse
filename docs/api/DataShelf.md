<p align="center"><a href="https://github.com/TrentBest/TheSingularityWorkshop.Visualize"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop" width="720" /></a></p>

# DataShelf<T> — API and operational contract

## 00 Identity

`DataShelf<T>` stores a dynamically growable capacity of unmanaged records in a contiguous page run allocated from a shared `DataLake`. Each shelf owns its slot metadata and allocation token; the lake owns the native backing region.

**Evidence status:** source-checked against `DataShelf.cs` and `DataLake.cs` on `development`. Complexity below is algorithmic analysis, not benchmark evidence.

## 01 What it owns

- A contiguous page-run allocation from the lake, large enough for `Capacity × sizeof(T)` and requested alignment.
- A free-slot stack and one generation integer per slot.
- Separate alive and dirty bitmaps, each using one bit per slot.
- Alive and dirty counters.
- The shelf identity used to reject handles from other shelves.
- A public `BaseAddress` diagnostic property returning the current allocation address as `nint`. It is an unsafe escape hatch, not a supported record-access API.
- `AllocatedBytes`, the current buddy-block extent. It remains the last extent value after disposal as a size diagnostic.

The bitmaps and generation/free metadata are external to stored `T` payloads. The shelf does not allocate a managed object for every record.

DataWarehouse is deliberately security-agnostic. It stores records and maintains storage metadata; it does not authenticate callers, authorize access, interpret identities or permissions, or load security MicroBundles. FSM_COS composes the higher-level security boundary.

## 02 Operations and complexity

| Operation | Current behavior | Algorithmic cost |
|---|---|---:|
| `TryAllocate(out handle)` | Pops a free slot, marks alive + dirty, clears record bytes, creates a generation-checked handle | O(1) plus `sizeof(T)` clearing |
| `GetRef(handle)` | Validates shelf, bounds, generation, liveness; marks dirty if needed; returns mutable ref | O(1) |
| `Read(handle)` | Same handle validation; returns read-only ref | O(1) |
| `IsAlive(handle)` | Returns false for disposed, foreign, out-of-range, stale, or non-alive handles | O(1) |
| `TryGetHandle(slot, out handle)` | Resolves a live slot index to its current generation-checked handle | O(1) |
| `IsDirty(handle)` | Validates handle and reads dirty bit | O(1) |
| `MarkClean(handle)` | Validates handle; clears dirty bit if set | O(1) |
| `TryFree(handle)` | Rejects non-live handles; clears tracking, advances generation, pushes slot to free stack unless retired | O(1) |
| `CopyDirtySlots(destination)` | Scans bitmap words and copies dirty slot indexes until buffer fills or scan ends | O(bitmap words scanned + indexes emitted) |
| `EnsureCapacity(minimumCapacity)` | Allocates a replacement lake block, copies payload and metadata, preserves handles, and returns old block | O(old payload bytes + old/new metadata) |
| `Dispose()` | Returns the shelf's page block to the lake once and clears visible counts | O(buddy coalescing orders) |

`CopyDirtySlots` returns slot indexes, not handles. Resolve each still-live slot with `TryGetHandle` before processing it; the returned handle carries the current generation and must still be used under the shelf's synchronization contract.

**Important distinctions**

- `TryAllocate` clears `sizeof(T)` bytes. Its work is constant with respect to shelf capacity, but scales with record size.
- `CopyDirtySlots` is not constant-time. It scans bitmap words; a small destination can stop the scan early.
- Handle validation is constant-time, but not free: it checks multiple fields and reads metadata.
- The shelf's contiguous base is aligned; individual records are not separately padded to cache-line boundaries.
- A slot whose generation reaches `int.MaxValue` is permanently retired instead of wrapping.

## 03 Capacity growth and ref lifetime

`EnsureCapacity(minimumCapacity)` is a synchronous relocation operation.

- A non-positive capacity throws `ArgumentOutOfRangeException`.
- A request less than or equal to current capacity is a no-op.
- New metadata arrays are allocated before requesting the replacement native block.
- Existing payload bytes, alive bits, dirty bits, generations, and free-slot entries are copied.
- New slots are zero-initialized, start with generation 1, and enter the free stack.
- Existing handles remain valid because shelf ID, slot indexes, and generations are preserved.
- The old lake page block is returned after the new pointer and metadata are committed.

**Critical lifetime rule:** every ref and raw address obtained before growth becomes invalid after growth. The caller must stop all workers and ensure no outstanding ref or pointer is in use before calling `EnsureCapacity`. The shelf does not attempt to detect escaped refs.

Growth can temporarily require both old and new blocks to be available. If the lake cannot allocate the replacement, the existing shelf remains unchanged and the allocation failure propagates.

## 04 State and failure behavior

- Initial capacity must be positive.
- Page-size alignment must be a power of two and at least 64 bytes.
- If `sizeof(T)` exceeds page size, the shelf allocation spans enough pages to contain the record payload.
- Allocation exhaustion returns false and a default handle.
- `GetRef`, `Read`, `IsDirty`, and `MarkClean` throw `InvalidOperationException` for invalid, stale, foreign, or non-live handles (disposed access may throw `ObjectDisposedException`).
- `TryFree` returns false when the handle is not currently alive.
- Reuse increments the slot generation, so the old handle no longer validates.
- `GetRef` marks a slot dirty when the ref is requested. The shelf cannot observe subsequent writes through the returned ref.
- `BaseAddress` returns zero after disposal. Any previously copied pointer remains unsafe and must not be used.
- `AllocatedBytes` reports the last block extent, not live lake usage after disposal. Use `DataLake.AllocatedBytes` for current active shelf-allocation bytes.

## 05 Concurrency and worker ownership

DataShelf<T> is a storage/metadata unit, not a synchronized work queue. Its allocator stack, generations, bitmaps, and counters are mutable and are not protected by internal locks or atomics.

The performance-first parallelism model is **independent shelf ownership**:

- Create separate shelves for independently processed pages/regions and assign each shelf to one worker or owner for the duration of its access.
- Different shelves may be processed in parallel; lake allocation coordination is not on the per-record access path.
- Do not concurrently allocate, free, grow, mutate, or inspect mutable tracking state on the same shelf from multiple workers unless the caller provides external synchronization.
- A read-only phase may be shared only when the caller guarantees that no worker mutates, frees, grows, or disposes the shelf or its records during that phase.
- A worker must stop using all refs and addresses obtained from a shelf before that shelf is grown, freed, or disposed.

This contract does not claim lock-free or thread-safe shared-shelf access. Higher-level tools must preserve page/shelf ownership when scheduling work.

## 06 Native-memory ownership and disposal

- A shelf owns one active page-run allocation token from the lake; the lake owns the backing region.
- `DataShelf<T>.Dispose()` returns the page run exactly once.
- Freed page blocks are coalesced and retained for reuse while the lake remains open.
- `DataWarehouse.Dispose()` disposes its registered shelves before disposing the lake.
- `DataLake.Dispose()` closes allocation to new shelves. If allocations remain, backing regions stay alive until the final shelf returns its pages; then the lake releases the regions.
- Security policy is not enforced by DataWarehouse. A valid DataHandle is not an authorization decision.

## 07 Verification

Current tests cover page alignment and extent rounding, large records, allocation/liveness/dirty state, zeroed reuse, read-only access, repeated dirty transitions, stale generations, capacity exhaustion, cross-shelf and invalid handles, bitmap word boundaries, dirty-slot copy buffers, shelf growth and handle preservation, disposal, shared-region allocation, buddy coalescing, region reuse, oversized regions, and deferred lake disposal.

See [DataShelfTests.cs](../../tests/TheSingularityWorkshop.DataWarehouse.Tests/DataShelfTests.cs) and [DataLake allocation contract](DataLake.md).

Performance comparisons live in the [benchmark repository](https://github.com/TrentBest/TheSingularityWorkshop.Benchmarks.DataWarehouse). Do not infer measured speed from the complexity table.

<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
