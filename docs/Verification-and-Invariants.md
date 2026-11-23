<p align="center"><a href="https://github.com/TrentBest/TheSingularityWorkshop.Visualize"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop" width="720" /></a></p>

# DataWarehouse — verification plan and invariants

## 00 Identity

This document maps the storage domain to properties that can be tested or otherwise verified. It separates current test evidence from important gaps. A documented invariant is not considered verified merely because it appears in this file.

## 01 Core invariants

| ID | Invariant | Current evidence | Verification gap |
|---|---|---|---|
| DW-001 | Every shelf has positive fixed capacity | Constructor guard; zero and negative capacity tests | No known basic boundary gap |
| DW-002 | Page alignment is a power of two and at least 64 | Constructor guards; invalid default and override tests | Broader valid page-size matrix remains useful |
| DW-003 | Shelf base address satisfies requested alignment | Alignment tests | Multiple types and page sizes |
| DW-004 | Allocation extent is a power-of-two buddy block covering payload and alignment | Extent tests and oversized-region test | Very large overflow boundaries |
| DW-005 | New and reused record slots are zeroed | Allocation path plus new/reused slot test | Rounded padding bytes are not directly inspected |
| DW-006 | Only one unmanaged T occupies a shelf | Generic constraint | Compile-time contract |
| DW-007 | A live handle matches shelf, slot, generation, and alive bit | Validate | Invalid bounds/default/stale/foreign matrix |
| DW-008 | Allocate increases alive and dirty counts exactly once | Allocation test | Repeated allocate/clean/reallocate matrix |
| DW-009 | Mutable access marks dirty before exposing ref | Access test | Repeated access does not double count |
| DW-010 | Read-only access does not mark dirty | Dedicated read-only test | None identified for single-threaded behavior |
| DW-011 | MarkClean decrements dirty count only on transition | Repeated clean/idempotence test | None identified for single-threaded behavior |
| DW-012 | Free clears alive and dirty state | Free test | Free both clean and dirty records |
| DW-013 | Reuse advances generation | Stale-handle test | Repeated reuse test |
| DW-014 | Generation never wraps and exhausted slots retire | Reflection-assisted test | Documented exceptional boundary |
| DW-015 | Dirty-slot copy emits only dirty indexes | Empty/undersized destination tests and 63/64/65/127/128/129 capacity boundaries | Concurrent mutation remains outside contract |
| DW-016 | Shelf disposal releases allocation exactly once | Exact-once release and operations-after-dispose tests | Raw ref/address use after disposal cannot be made safe by this API |
| DW-017 | Warehouse disposal releases every registered shelf | Aggregate, repeated, and individual-shelf accounting tests | Deterministic race/fault injection remains absent |
| DW-018 | Lake rejects allocations after disposal | Lifecycle test | Accounting under mixed allocation/free |
| DW-019 | Existing shelves can release memory after lake close | Lifecycle test | Already represented by current test |
| DW-020 | No shared-shelf concurrency guarantee is claimed | API contract | Documentation and examples must remain explicit |
| DW-021 | Multiple shelves share native backing regions | Shared-region test | Mixed allocation pressure across many regions |
| DW-022 | Buddy blocks split to satisfy requests and coalesce on free | Buddy coalescing and randomized allocation tests | Broader randomized allocate/free interleaving |
| DW-023 | Freed blocks are cleared before a new shelf receives them | Same-shelf and cross-shelf zeroed reuse tests | Full-block byte scan is not exposed by the public API |
| DW-024 | Shelf growth preserves live payload, handles, generations, and bitmaps | EnsureCapacity growth and 65-to-129 bitmap-boundary tests | Growth across several repeated reallocations |
| DW-025 | Shelf growth returns the old block only after replacement is committed | EnsureCapacity implementation and growth test | Fault injection during copy/commit is not available |
| DW-026 | Disposed lake defers region release until all shelves return allocations | Deferred-disposal test with multiple live shelves | Multiple regions with mixed outstanding allocations |
| DW-027 | Regions larger than the default are allocated as power-of-two backing blocks | Oversized shelf region test | Very large page-index limits |
| DW-028 | Live shelf page runs never overlap | Deterministic randomized range-overlap test | More randomized seeds and mixed frees |
| DW-029 | Reclaimed buddy blocks remain reusable and fully coalesce | Randomized release and subsequent larger allocation | Stress over multiple region sizes |
| DW-030 | Larger shelf alignment overrides are honored by the lake | 8 KiB override over 4 KiB base-page test | More alignment ratios and platform validation |
| DW-031 | Dirty slot indexes can be resolved to current live handles | TryGetHandle test | Concurrent changes remain externally synchronized |

## 02 Required state relationships

For every shelf at a quiescent point:

- `0 ≤ AliveCount ≤ Capacity`
- `0 ≤ DirtyCount ≤ AliveCount`
- The alive counter equals the number of set bits in the alive bitmap.
- The dirty counter equals the number of set bits in the dirty bitmap.
- Every live slot has a positive current generation.
- Every free-stack entry refers to a non-live, non-retired slot and appears at most once.
- Every retired generation-exhausted slot is non-live and absent from the free stack.
- Every returned handle identifies exactly one live shelf/slot incarnation.

The quiescent-point qualification matters: the current implementation does not synchronize concurrent reads and writes of these structures.

## 03 Current automated test coverage

The current test suite exercises:

- Page-aligned base addresses.
- Allocation, zero initialization on new and reused slots, alive and dirty state, mutable access, and readback.
- Read-only access preserving clean state; repeated mutable access and repeated clean transitions.
- Clean and free transitions.
- Stale generation rejection after slot reuse.
- Capacity exhaustion and default failure handle.
- Cross-shelf handle rejection.
- Dirty slot enumeration with empty and undersized destinations and bitmap word-boundary capacities.
- Default and out-of-range handle rejection; live handle resolution from dirty slot indexes; shelf operations after disposal.
- Rounded allocation extents and accounting.
- Invalid page-size rejection.
- Records larger than the configured page size.
- Exact-once shelf disposal, post-disposal behavior, aggregate warehouse disposal, and mixed individual-shelf allocation accounting.
- Warehouse default-page-size and shelf-capacity validation.
- Generation exhaustion and slot retirement.
- Shared-region allocation, buddy coalescing, randomized non-overlapping allocations, freed-region reuse, cross-shelf stale-byte clearing, oversized backing regions, and deferred region disposal after lake closure.
- Shelf capacity growth preserving existing handles, payloads, and side-band state across a bitmap-word boundary.
- Larger shelf alignment overrides and growth rejection after lake closure.

See `tests/TheSingularityWorkshop.DataWarehouse.Tests/DataShelfTests.cs`. The test suite currently concentrates on DataShelf behavior; dedicated DataLake and DataWarehouse contract suites would make the type boundaries clearer.

## 04 Missing verification before declaring the domain complete

1. Exact-size dirty-copy buffer coverage in addition to empty and undersized buffers.
2. Additional valid page-size and record-size combinations, including more architecture-sensitive alignment cases.
3. Checked-overflow behavior for capacity × record size and rounded extent.
4. Failure-path behavior when managed metadata allocation or native allocation cannot complete.
5. A deterministic fault-injection test for failure while registering a newly allocated shelf with its warehouse.
6. A clear API decision on public exposure of `BaseAddress` and `DataWarehouse.Lake`; current docs describe the hazards but do not remove the escape hatches.
7. A dedicated test for the `DataWarehouse.CreateShelf` create/dispose race if a deterministic synchronization seam is introduced.

Concurrency stress tests are not appropriate until a concurrency contract is selected. The current contract is externally synchronized single-shelf access / independent shelf ownership.

## 05 Evidence rules

- A passing test proves only the tested behavior on that revision and environment.
- A workflow badge is not proof that a particular commit passed unless its run and commit SHA are inspected.
- A structural complexity statement is not a benchmark.
- A benchmark result must identify revision, runtime, machine, workload, record size, capacity, and measurement method.
- A source-checked diagram is not generated documentation.
- Planned capabilities must remain labeled planned until implemented and verified.

## 06 Completion gate

The DataWarehouse domain is ready for a consumer/use-case discussion only when:

- Every public type and public member has an accurate contract.
- Every lifecycle and ownership transition is described.
- Every state invariant has test coverage or an explicit reason it cannot be tested.
- Failure, disposal, and overflow behavior are documented.
- Memory and metadata costs are explained without overstating performance.
- Thread-safety and raw-pointer boundaries are unmistakable.
- README, API docs, diagrams, inventory, and tests agree with source.
- Current exact-commit build/test evidence is available.

This gate deliberately precedes downstream integration and consumer discovery.

<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
