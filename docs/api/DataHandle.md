<p align="center"><a href="https://github.com/TrentBest/TheSingularityWorkshop.Visualize"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop" width="720" /></a></p>

# DataHandle — slot identity and validity contract

## 00 Identity

`DataHandle` is an immutable value that identifies one incarnation of one slot in one shelf. Its three fields are `ShelfId`, `Slot`, and `Generation`.

A handle is deliberately small and self-contained. It is not a pointer, object reference, durable identifier, security credential, or promise that the slot remains live indefinitely.

**Evidence:** source-checked against `src/TheSingularityWorkshop.DataWarehouse/DataHandle.cs` and `DataShelf<T>` on `development`.

## 01 The three identity dimensions

| Field | Meaning | Validation |
|---|---|---|
| `ShelfId` | Process-local identity of the shelf | Must equal the receiving shelf's `Id` |
| `Slot` | Zero-based slot index | Must be within `[0, Capacity)` |
| `Generation` | Incarnation counter for that slot | Must equal the slot's current generation |

All three must match, and the slot's alive bit must be set, for `DataShelf<T>.Validate` to accept the handle.

This prevents:
- Using a handle from another shelf.
- Using a handle after its slot has been freed.
- Using an old handle after the same slot has been reused.
- Treating a non-live slot as a valid record.

## 02 Generation and reuse

Every slot begins at generation 1. Freeing a live slot increments its generation before returning the slot to the free stack. A later allocation returns a handle containing that new generation.

If a slot reaches `int.MaxValue`, freeing it retires the slot permanently. Its generation does not wrap, so an ancient handle cannot become valid again through generation rollover.

Generation validation protects against stale slot reuse within the lifetime of this process. It is not a distributed identity or persistence scheme.

## 03 Default values

`DataHandle.IsDefault` is true when `ShelfId` is zero, `Slot` is negative, or `Generation` is non-positive. The default struct value is therefore recognized as default.

A default handle is not a valid record reference. APIs that require a live handle reject it.

## 04 Lifetime and ownership

The handle does not keep a shelf alive and does not prevent disposal. A caller must keep the owning shelf alive and must stop using all refs and raw addresses before the shelf is disposed.

After shelf disposal, `IsAlive` returns false. Operations that validate a handle throw `ObjectDisposedException` because the storage has been released.

## 05 Security boundary

A valid handle proves only that its shelf, slot, generation, and alive state match. It does not prove that the caller is authorized to read or mutate the record. Access control belongs in a higher-level boundary; DataWarehouse remains security-agnostic.

## 06 Complexity and limits

Handle construction is a value-type operation. Validation checks a fixed number of fields and one alive bit, so its algorithmic work is O(1). It still incurs actual CPU and memory costs, and does not guarantee a particular latency.

Shelf IDs are monotonically assigned process-local integers. The allocator fails when the positive `int` identity space is exhausted rather than wrapping and reusing an identity.

<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
