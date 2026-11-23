<p align="center"><a href="https://github.com/TrentBest/TheSingularityWorkshop.Visualize"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop" width="720" /></a></p>

# DataWarehouse — fixed-record design and memory patterns

## 00 Identity

DataWarehouse stores fixed-size unmanaged records. This guide explains how to shape record types and reason about the trade-offs of contiguous typed shelves, side-band metadata, handles, and dirty tracking.

It describes storage patterns, not a downstream application or consumer integration.

## 01 The record contract

A shelf is created for exactly one unmanaged type `T`. Every slot has the same `sizeof(T)`, and slot addressing is:

```text
address(slot) = shelfBase + slot * sizeof(T)
```

Good shelf records are:

- Fixed-layout value types.
- Small enough that clearing and copying a record is predictable.
- Explicit about state and numeric representation.
- Free of managed object references.
- Stable for the lifetime of the shelf's use.

The `unmanaged` constraint prevents managed references in `T`, but it does not automatically make pointer fields safe. If a record contains an unmanaged pointer or external resource handle, the caller owns that resource's lifetime and validity.

DataWarehouse does not provide schema evolution. Changing a record type changes its size and layout; create a compatible shelf and migrate records at a higher layer when necessary.

## 02 Compact state and byte-sized domains

Small state machines often need only a byte per entity. A byte-backed enum keeps the payload explicit:

```csharp
public enum AgentMode : byte
{
    Idle = 0,
    Moving = 1,
    Working = 2,
    Disabled = 3
}

public struct AgentRecord
{
    public AgentMode Mode;
    public byte Flags;
    public ushort Team;
    public float X;
    public float Y;
}
```

A byte can represent 256 distinct bit patterns. DataWarehouse stores the value; it does not interpret the enum, validate its domain, or assign behavior to a state transition.

This is ordinary CPU-side unmanaged memory. A byte-sized state field is not automatically a GPU texture channel, GPU buffer, or graphics resource. GPU registration, upload, synchronization, and lifetime remain separate concerns.

## 03 Array-of-structures and structure-of-arrays

A shelf is an array of records (array-of-structures):

```text
[AgentRecord][AgentRecord][AgentRecord]...
```

This is convenient when a processing step consumes several fields for each entity. If a workload repeatedly processes only one field across many entities, separate shelves can form a structure-of-arrays layout:

```text
[Mode][Mode][Mode]...
[X][X][X]...
[Y][Y][Y]...
```

The library does not choose a layout for the caller. Benchmark the actual access pattern; neither representation is universally faster.

Separate shelves can also be assigned to independent workers. Shared mutation of one shelf still requires external synchronization.

## 04 Handles instead of managed object references

A `DataHandle` contains shelf identity, slot, and generation. Store handles or compact slot indexes in other unmanaged records when relationships are needed.

Do not treat a handle as a permanent identity:
- Freeing a slot invalidates the current handle.
- Reuse advances its generation.
- Generation exhaustion retires the slot rather than wrapping.
- A handle does not keep its shelf alive or authorize access.

The caller owns any higher-level entity identity and relationship semantics.

## 05 Dirty tracking as a work signal

The alive and dirty bitmaps are external to the record payload.

- Allocation marks a new slot alive and dirty.
- Mutable `GetRef` marks dirty before returning the reference.
- Read-only `Read` does not mark dirty.
- `MarkClean` acknowledges the current dirty state.
- `CopyDirtySlots` copies dirty slot indexes into caller-provided memory; it does not clear them.

The bitmaps avoid iterating each record to inspect its state. Enumeration still scans bitmap words and is proportional to the words visited plus indexes emitted; it is not constant-time.

Dirty state is not a change journal, version counter, transaction, or durable acknowledgement. The caller owns scheduling, retry, synchronization, and any persistence boundary.

## 06 Capacity planning and memory cost

Preallocate the expected workload when it is known. This avoids repeated growth and temporary duplicate allocations.

Approximate per-slot managed metadata in the current implementation:

| Metadata | Approximate bytes per slot |
|---|---:|
| Free-slot stack entry | 4 |
| Generation | 4 |
| Alive + dirty bits | 0.25 |
| **Subtotal** | **8.25** |

This excludes array headers, CLR alignment, buddy-block padding, native region overhead, and temporary growth allocations. For a one-byte payload, side-band metadata is substantially larger than the record itself; for larger records, the relative overhead falls.

Use:
- `DataLake.AllocatedBytes` for active buddy-block bytes assigned to shelves.
- `DataLake.ReservedBytes` for native backing regions.
- `DataLake.FreeBytes` for reserved bytes not currently assigned to shelf blocks.
- `DataShelf<T>.AllocatedBytes` for one shelf's current buddy-block extent.

Free bytes may be fragmented and are not necessarily one contiguous allocation.

## 07 Growth and pointer lifetime

`EnsureCapacity` preserves live record bytes, handles, generations, alive bits, dirty bits, and free slots. It allocates a replacement page run before releasing the old run.

Growth temporarily requires both blocks. It invalidates all previously returned refs and raw addresses. Stop workers and quiesce the shelf before growing it. If the old and replacement blocks cannot coexist in the lake, allocation fails and the existing shelf remains unchanged.

## 08 Example

```csharp
using TheSingularityWorkshop.DataWarehouse;

using var warehouse = new DataWarehouse();
var agents = warehouse.CreateShelf<AgentRecord>(capacity: 10_000);

if (!agents.TryAllocate(out var handle))
    throw new InvalidOperationException("The shelf is full.");

agents.GetRef(handle).Mode = AgentMode.Moving;
agents.GetRef(handle).X = 12.5f;

Span<int> dirtySlots = stackalloc int[256];
var dirtyCount = agents.CopyDirtySlots(dirtySlots);

for (var i = 0; i < dirtyCount; i++)
{
    var slot = dirtySlots[i];
    // Resolve or schedule the corresponding live handle at the owning layer.
}

// Acknowledge only after the caller has completed its processing step.
agents.MarkClean(handle);
```

The example intentionally leaves scheduling and durable acknowledgement to the caller. A dirty slot index is not itself a handle; `TryGetHandle` resolves the current live incarnation so the caller does not fabricate a generation.

## 09 Deliberate exclusions

DataWarehouse does not supply:
- Variable-size payloads or object graphs.
- Durable storage, snapshots, transactions, or recovery.
- Security policy or access control.
- GPU buffers, texture registration, or device synchronization.
- Work scheduling, worker ownership enforcement, or event delivery.
- Domain-specific meaning for enums, IDs, flags, or relationships.

<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
