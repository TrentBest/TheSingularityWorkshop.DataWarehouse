<p align="center"><a href="https://github.com/TrentBest/TheSingularityWorkshop.Visualize"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop" width="720" /></a></p>

# UML — DataLake shared region and buddy-page lifecycle

## 00 Identity

This diagram documents the current DataLake page allocator: backing-region creation, buddy splitting, allocation clearing, reclamation, coalescing, reuse, and deferred disposal. It is source-checked against `DataLake.cs` on `development`.

## 01 Region allocation

```mermaid
sequenceDiagram
    autonumber
    participant Shelf as DataShelf<T>
    participant Lake as DataLake
    participant Region as DataLakeRegion
    participant Native as Native memory
    Shelf->>Lake: AllocateAligned(payloadBytes, alignment)
    Lake->>Lake: Validate request and calculate pages/order
    Lake->>Region: TryAllocate(requiredPages, alignmentPages)
    alt Existing region has a suitable buddy block
        Region->>Region: Remove free block and split to requested order
    else No existing region can satisfy request
        Lake->>Region: Create power-of-two backing region
        Region->>Native: AlignedAlloc(regionBytes, regionBytes)
        Native-->>Region: aligned region pointer
        Region->>Region: Initialize maximum-order free block
        Region->>Region: Split block to requested order
    end
    Region-->>Lake: start page + order
    Lake->>Native: Clear allocated block
    Lake-->>Shelf: allocation token + pointer + extent
```

## 02 Reclamation and buddy coalescing

```mermaid
sequenceDiagram
    autonumber
    participant Shelf as DataShelf<T>
    participant Lake as DataLake
    participant Region as DataLakeRegion
    Shelf->>Lake: FreeAligned(allocation token)
    Lake->>Region: Free(startPage, order)
    Region->>Region: Validate active allocation token
    loop While same-order buddy is free
        Region->>Region: Remove buddy and merge into parent block
    end
    Region->>Region: Add merged block to free list
    Lake->>Lake: Decrement active allocation bytes
    Note over Lake,Region: Region remains reserved for future shelf allocations
```

## 03 Deferred lake disposal

- Disposing an open lake closes it to new allocations.
- If active shelf allocations remain, backing regions stay allocated and existing shelves remain usable.
- Each shelf returns its page run through normal disposal.
- When active allocation bytes reach zero, the disposed lake releases all retained native regions.
- Disposing a warehouse first disposes its registered shelves, then disposes the lake.

## 04 Allocator invariants

- Every region has a power-of-two page count.
- Every free block belongs to exactly one order-specific free list.
- Every live allocation is recorded by its start page and order.
- A live block is absent from all free lists.
- Splitting preserves total page coverage.
- Coalescing merges only a free buddy of the same order.
- No region is released while it still has live page allocations.
- Every new allocation block is cleared before it is exposed to a shelf.

## 05 Evidence and limits

- The buddy allocator uses order-specific hash sets and a live allocation map.
- Allocation searches existing regions, then searches free lists from the required order upward.
- Allocation/reclamation is serialized by the lake gate; record access is not.
- This is a source-checked static diagram, not generated output or a benchmark.

<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
