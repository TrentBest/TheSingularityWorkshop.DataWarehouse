<p align="center"><a href="https://github.com/TrentBest/TheSingularityWorkshop.Visualize"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop" width="720" /></a></p>

# DataWarehouse — verification plan and invariants

## 00 Identity

This document maps the storage domain to properties that can be tested or otherwise verified. It separates current test evidence from important gaps. A documented invariant is not considered verified merely because it appears in this file.

## 01 Core invariants

| ID | Invariant | Current evidence | Verification gap |
|---|---|---|---|
| DW-001 | Every shelf has positive fixed capacity | Constructor guard | Add explicit capacity boundary tests |
| DW-002 | Page alignment is a power of two and at least 64 | Constructor guards | Constructor/default and override test matrix |
| DW-003 | Shelf base address satisfies requested alignment | Alignment tests | Multiple types and page sizes |
| DW-004 | Allocation extent is rounded to page-size multiple | Extent tests | Very large overflow boundaries |
| DW-005 | New native allocation is zeroed | Allocation path clears memory | Explicit record-zero test |
| DW-006 | Only one unmanaged T occupies a shelf | Generic constraint | Compile-time contract |
| DW-007 | A live handle matches shelf, slot, generation, and alive bit | Validate | Invalid bounds/default/stale/foreign matrix |
| DW-008 | Allocate increases alive and dirty counts exactly once | Allocation test | Repeated allocate/clean/reallocate matrix |
| DW-009 | Mutable access marks dirty before exposing ref | Access test | Repeated access does not double count |
| DW-010 | Read-only access does not mark dirty | Implementation | Dedicated test |
| DW-011 | MarkClean decrements dirty count only on transition | Implementation | Repeated clean test |
| DW-012 | Free clears alive and dirty state | Free test | Free both clean and dirty records |
| DW-013 | Reuse advances generation | Stale-handle test | Repeated reuse test |
| DW-014 | Generation never wraps and exhausted slots retire | Reflection-assisted test | Documented exceptional boundary |
| DW-015 | Dirty-slot copy emits only dirty indexes | Dirty-copy test | Empty/truncated destination and bitmap word boundaries |
| DW-016 | Shelf disposal releases allocation exactly once | Disposal test | Operations-after-dispose matrix |
| DW-017 | Warehouse disposal releases every registered shelf | Aggregate disposal test | Repeated and concurrent create/dispose boundary |
| DW-018 | Lake rejects allocations after disposal | Lifecycle test | Accounting under mixed allocation/free |
| DW-019 | Existing shelves can release memory after lake close | Lifecycle test | Already represented by current test |
| DW-020 | No shared-shelf concurrency guarantee is claimed | API contract | Documentation and examples must remain explicit |

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
- Allocation, alive and dirty state, mutable access, and readback.
- Clean and free transitions.
- Stale generation rejection after slot reuse.
- Capacity exhaustion and default failure handle.
- Cross-shelf handle rejection.
- Dirty slot enumeration.
- Rounded allocation extents and accounting.
- Invalid page-size rejection.
- Records larger than the configured page size.
- Exact-once shelf disposal and aggregate warehouse disposal.
- Generation exhaustion and slot retirement.
- Lake disposal preventing new allocations while allowing existing shelves to release memory.

See `tests/TheSingularityWorkshop.DataWarehouse.Tests/DataShelfTests.cs`. The test suite currently concentrates on DataShelf behavior; dedicated DataLake and DataWarehouse contract suites would make the type boundaries clearer.

## 04 Missing verification before declaring the domain complete

1. Zero-initialization test for newly allocated and reused slots.
2. Read-only access leaves dirty state unchanged.
3. Repeated `GetRef` does not increment `DirtyCount` more than once.
4. Repeated `MarkClean` is idempotent and returns false after the first transition.
5. Dirty-copy behavior for empty, undersized, and exact-size destinations.
6. Bitmap word boundaries at capacities 63, 64, 65, 127, 128, and 129.
7. Invalid/default/out-of-range handles and disposed-shelf behavior.
8. Warehouse constructor and shelf-capacity validation.
9. Shelf disposal while records are live, followed by use attempts.
10. Multi-shelf accounting through mixed individual and aggregate disposal.
11. Checked-overflow behavior for capacity × record size and rounded extent.
12. Failure-path behavior when managed metadata allocation or native allocation cannot complete.
13. A clear decision and test for public exposure of `BaseAddress` and `DataWarehouse.Lake`.

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
