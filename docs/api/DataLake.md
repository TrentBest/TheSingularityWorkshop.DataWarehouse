<p align="center"><a href="https://github.com/TrentBest/TheSingularityWorkshop.Visualize"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop" width="720" /></a></p>

# DataLake — shared backing and page-allocation contract

## 00 Identity

`DataLake` owns the shared native backing regions from which typed shelves receive contiguous page runs. It is the physical allocation boundary beneath DataWarehouse; it is not a record index, object database, persistence engine, or security service.

**Evidence:** source-checked against `DataLake.cs` and `DataShelf.cs` on `development`.

## 01 Region model

- A lake has one configured base `PageSize` and normal `RegionSize`.
- Each region is one page-aligned native allocation.
- A region is divided into power-of-two page blocks managed by a buddy allocator.
- A shelf receives a contiguous block large enough for its payload and requested alignment.
- Multiple shelves can occupy different blocks in the same region.
- A request larger than the normal region receives a dedicated power-of-two region large enough to satisfy it.
- Freed blocks are coalesced with free buddies and retained for reuse.

The lake allocates native memory per backing region, not per shelf. Individual shelf allocation and reclamation operate on page runs inside those regions.

## 02 Configuration

`DataLake(pageSize, regionSize)` requires:

- `pageSize` is a power of two and at least 64 bytes.
- `regionSize` is a power of two, at least one page, and an exact multiple of page size.

The warehouse defaults are 4 KiB pages and 4 MiB regions. Both are configurable at warehouse construction. Shelf page-size overrides remain supported; the lake provides at least its base-page alignment, and larger requested alignment is satisfied by allocating a sufficiently large aligned buddy block.

## 03 Allocation

`AllocateAligned(byteCount, alignment)` is internal. Under the lake allocation gate it:

1. Rejects new requests after lake disposal.
2. Validates nonzero size and power-of-two alignment.
3. Converts the request to a minimum page count and an alignment-page requirement.
4. Searches existing regions for a suitable buddy block.
5. If none fits, creates a new region sized to the larger of normal region size or the request, rounded to a power of two.
6. Splits a larger free buddy block until the requested order is reached.
7. Clears the allocated block before exposing it, preventing stale bytes from a previous shelf lifetime from leaking into a new allocation.
8. Updates active-allocation accounting and returns an internal allocation token containing region, start page, order, pointer, and byte extent.

Buddy allocation uses free lists by order. It avoids scanning individual records; the allocation path is proportional to the number of buddy orders and the number of regions considered, not to the number of records stored.

## 04 Reclamation and coalescing

`FreeAligned(allocation)` returns the exact block represented by the allocation token. The region validates that the start-page/order pair is currently allocated, removes it from the active allocation map, and repeatedly coalesces it with a free buddy of the same order.

The resulting larger block is returned to the appropriate free list. This makes adjacent reclaimed page ranges available for future larger allocations.

The allocation token is internal. Shelves guard normal release with idempotent disposal; the region also rejects unknown, already-freed, or mismatched extents.

## 05 Accounting

| Property | Meaning |
|---|---|
| `AllocatedBytes` | Buddy-block bytes currently assigned to live shelf allocations, including buddy rounding |
| `ReservedBytes` | Total native memory reserved by all backing regions |
| `FreeBytes` | Reserved bytes not currently assigned to shelf blocks |
| `RegionCount` | Number of backing regions currently retained |
| `PageSize` | Base allocation page unit |
| `RegionSize` | Normal region size; oversized requests may create larger regions |

`FreeBytes` includes both immediately allocatable blocks and space affected by buddy fragmentation. It is not a guarantee that one contiguous block of that size can satisfy a request.

## 06 Disposal and outstanding shelves

Disposal closes the lake to new allocations. It releases all backing regions immediately only when no shelf allocations remain.

If shelves are still live, their regions remain valid so existing records can still be read, mutated, and released. When the final outstanding shelf allocation is returned, the disposed lake releases every region.

Normal aggregate ownership order remains:

`DataWarehouse.Dispose()` → dispose registered shelves → dispose DataLake.

Directly disposing the publicly exposed lake while shelves are active is supported as a close-to-new-allocations operation, not as immediate memory destruction. The warehouse/shelf owners remain responsible for returning all active allocations.

## 07 Synchronization and failure boundaries

- The lake gate serializes region allocation, page reclamation, accounting transitions, and lake disposal.
- Record reads and writes do not pass through this gate.
- Independent shelves can be processed in parallel without per-record lake locks.
- Shared mutation of one shelf remains externally synchronized.
- Region free-list and allocation-map capacity is reserved before mutating buddy state, reducing the chance of partially applied allocator transitions if managed metadata growth fails.
- Native region allocation failure raises `OutOfMemoryException`; managed metadata allocation may also fail before native reservation.
- Checked page-count, region-size, and byte-accounting conversions reject unsupported extents rather than silently wrapping.

The lake does not promise lock-free allocation, a fixed latency, NUMA placement, GPU registration, persistence, crash recovery, or cross-process address stability.

## 08 Domain boundary

DataLake owns native backing regions and page allocation/reclamation. DataShelf owns typed record layout and slot metadata. DataWarehouse owns the aggregate lifecycle.

DataLake does not know record types, handles, alive/dirty semantics, security policy, serialization, or domain meaning.

<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
