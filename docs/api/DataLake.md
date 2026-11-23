<p align="center"><a href="https://github.com/TrentBest/TheSingularityWorkshop.Visualize"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop" width="720" /></a></p>

# DataLake — allocation ownership contract

## 00 Identity

`DataLake` is the internal native-memory allocation boundary used by `DataShelf<T>`. It allocates page-aligned unmanaged blocks, accounts for their rounded allocation extents, and releases those blocks. It is not a record index, shared virtual address space, page cache, or general-purpose allocator.

**Evidence:** source-checked against `src/TheSingularityWorkshop.DataWarehouse/DataLake.cs` on `development`.

## 01 Responsibilities

- Allocate aligned native memory through `NativeMemory.AlignedAlloc`.
- Clear newly allocated memory before returning it.
- Validate nonzero byte count and power-of-two alignment that is at least pointer-sized.
- Round the native allocation size up to an alignment multiple.
- Track currently allocated bytes.
- Reject new allocations after disposal.
- Release an individual allocation through `FreeAligned`, even after the lake has been closed to new allocations.

The lake does not know which records occupy a block. `DataShelf<T>` owns record indexing, slot metadata, and the allocation lifetime.

## 02 Allocation contract

`AllocateAligned(byteCount, alignment)` is internal. Its current behavior is:

1. Enter the allocation gate.
2. Reject a disposed lake.
3. Reject zero bytes.
4. Reject invalid alignment.
5. Round the allocation extent upward to an alignment multiple using checked arithmetic.
6. Check that the extent fits the signed byte-accounting range.
7. Allocate aligned native memory.
8. Throw `OutOfMemoryException` if the native allocator returns null.
9. Clear the entire rounded allocation extent.
10. Add the rounded extent to `AllocatedBytes` and return the pointer.

The accounting value represents bytes reserved from the native allocator, not only the requested record payload. A shelf's `AllocatedBytes` and the lake's total therefore agree for each live shelf allocation.

## 03 Release contract

`FreeAligned(pointer, allocationSize)` ignores a null pointer, releases a non-null pointer with `NativeMemory.AlignedFree`, and subtracts the supplied allocation extent from accounting.

The method is internal and relies on its owner to provide the exact pointer/size pair once. It does not maintain an allocation registry or independently detect double-free, foreign pointers, or incorrect sizes. The shelf's idempotent disposal guard is the normal protection against duplicate release.

## 04 Disposal and ownership

`DataLake.Dispose()` closes the allocation service to future allocations. It does not enumerate or free outstanding allocations. This distinction is intentional: shelves own their individual allocations, and the warehouse owns its registered shelves.

Normal ownership order:

`DataWarehouse.Dispose()` → dispose registered shelves → close DataLake.

A shelf can still release its allocation after the lake has been disposed. Directly disposing the publicly exposed `DataWarehouse.Lake` while shelves are active is therefore a misuse of the aggregate ownership API: it prevents new shelves but does not invalidate or release existing shelf storage.

## 05 Synchronization and limits

- The allocation gate serializes allocation against lake disposal.
- `AllocatedBytes` uses an interlocked read/update.
- Individual frees are not performed under the allocation gate; the native allocator and accounting update are separate operations.
- This is not a promise of concurrent safety for shelf operations.
- There is no allocation registry, compaction, relocation, persistence, recovery, NUMA policy, GPU registration, or shared page pool in the current implementation.

## 06 Invariants

| Invariant | Owner |
|---|---|
| No allocation begins after lake disposal | DataLake allocation gate |
| Allocation extent is aligned and accounting-safe | DataLake |
| Newly allocated extent is zeroed | DataLake |
| Each allocation is released once | DataShelf disposal guard |
| Aggregate disposal closes creation after disposing owned shelves | DataWarehouse |

## 07 Open design questions

These are explicit future decisions, not current features:

- Should the public `Lake` property be replaced with read-only diagnostics so callers cannot close the allocator independently?
- Should an allocation registry exist for diagnostics/leak detection, or would its overhead and complexity undermine the narrow allocator boundary?
- Should page size remain a caller-selected alignment unit, or should the API distinguish allocation alignment from a platform page-size concept?
- Should the lake ever evolve into a shared arena, or should each shelf continue to own one independent native allocation?

Do not add any of these capabilities implicitly. Each changes ownership, performance, or failure semantics.

<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
