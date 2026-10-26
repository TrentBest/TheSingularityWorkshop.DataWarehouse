<p align="center"><a href="https://github.com/TrentBest/TheSingularityWorkshop.Visualize"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop" width="720" /></a></p>

# UML — DataShelf slot lifecycle

## 00 Identity

This sequence explains the current TryAllocate → access → clean → free lifecycle in DataShelf<T>. It is **source-checked**, not generated from reflection and not runtime-captured.

## 01 Sequence

```mermaid
sequenceDiagram
    autonumber
    participant Caller
    participant Shelf as DataShelf<T>
    participant Free as Free-slot stack
    participant Bits as Alive / dirty bitmaps
    participant Memory as Aligned native memory
    Caller->>Shelf: TryAllocate(out handle)
    Shelf->>Free: Pop free slot
    Shelf->>Bits: Set alive and dirty bits
    Shelf->>Memory: Clear sizeof(T) bytes at slot offset
    Shelf-->>Caller: handle(shelf, slot, generation)
    Caller->>Shelf: GetRef(handle)
    Shelf->>Shelf: Validate shelf, slot, generation, alive
    Shelf->>Bits: Set dirty bit if not already set
    Shelf-->>Caller: mutable ref<T>
    Caller->>Shelf: MarkClean(handle)
    Shelf->>Shelf: Validate handle
    Shelf->>Bits: Clear dirty bit if set
    Caller->>Shelf: TryFree(handle)
    Shelf->>Shelf: Validate liveness
    Shelf->>Bits: Clear alive and dirty bits
    Shelf->>Shelf: Increment generation; push slot to free stack
    Shelf-->>Caller: true
```

## 02 State transitions

| Event | Alive | Dirty | Generation |
|---|---|---|---|
| Shelf created | false | false | initialized to 1 |
| Allocate slot | true | true | unchanged |
| Mutable GetRef | unchanged | true | unchanged |
| MarkClean succeeds | unchanged | false | unchanged |
| Free live slot | false | false | incremented; wraps to 1 after int.MaxValue |
| Reallocate freed slot | true | true | uses current generation |

## 03 Read the diagram correctly

- The shelf, not the caller, owns the slot bitmaps and free stack.
- A mutable reference marks dirty when obtained, not when an assignment is later performed.
- Freeing invalidates the old handle before the slot can be safely reused through a newly generated handle.
- The diagram omits exceptional branches for invalid handles, exhaustion, and disposal; see the [DataShelf API contract](../api/DataShelf.md).

## 04 Provenance and regeneration

- **Provenance:** manually authored from DataShelf.cs and DataShelfTests.cs on development.
- **Generation:** no automated UML generator is currently claimed.
- **Verification:** compare each operation against the source and tests before changing the implementation.
- **Performance:** this is an explanatory sequence, not benchmark evidence.

<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_COS/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
