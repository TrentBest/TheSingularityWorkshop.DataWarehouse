<p align="center">
  <a href="https://github.com/TrentBest/TheSingularityWorkshop.Visualize">
    <img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop" width="720" />
  </a>
</p>

# TheSingularityWorkshop.DataWarehouse

**A platform-neutral, page-aligned unmanaged data store designed for direct, predictable access.**

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Status](https://img.shields.io/badge/status-reconstruction%20in%20progress-00A6A6)](#current-scope)

The DataWarehouse is a reconstruction of the original Unity-era memory technology, not the higher-level AEC-facing [SingularityWarehouse](https://github.com/TrentBest/SingularityWarehouse). The original used unsafe raw storage and external life/dirty bitmasks. This implementation preserves those ideas without requiring Unity.

## Why it exists

General-purpose object graphs and hash lookups are convenient, but can add indirection and per-object overhead to hot paths. DataWarehouse instead places fixed-size unmanaged records in a page-aligned shelf and resolves a handle to a slot by arithmetic:

`address = shelfBase + slot × sizeof(T)`

The access path is constant-time in the algorithmic sense: it validates a shelf identifier, slot, and generation, then calculates the record address. Real latency still depends on CPU caches, memory topology, contention, and workload; benchmarks—not the name of the algorithm—must establish practical performance.

## Storage vocabulary

- **Data Lake** — the aligned unmanaged allocation boundary.
- **DataWarehouse** — owns the lake and creates shelves.
- **Data Shelf** — page-aligned storage for one unmanaged record type.
- **Slot / bay** — one fixed-size record position inside a shelf.
- **Alive tracker** — an external bitmap indicating which slots are allocated.
- **Dirty tracker** — an external bitmap indicating which live slots have changed or need initialization/processing.
- **Data handle** — shelf identity + slot index + generation, allowing O(1) validation and stale-handle rejection.

## See it in a minute

```csharp
using TheSingularityWorkshop.DataWarehouse;

struct AgentState
{
    public int Mode;
    public float X;
    public float Y;
}

using var warehouse = new DataWarehouse(defaultPageSize: 4096);
var agents = warehouse.CreateShelf<AgentState>(capacity: 100_000);

if (!agents.TryAllocate(out var handle))
    throw new InvalidOperationException("No free slots.");

agents.GetRef(handle).Mode = 2; // Mutable access marks the slot dirty.
agents.GetRef(handle).X = 12.5f;

ref readonly var state = ref agents.Read(handle);
Console.WriteLine(state.Mode);

agents.MarkClean(handle); // Acknowledge processing.
agents.TryFree(handle);   // Reclaims the slot; old handles become invalid.
```

## Current scope

The initial implementation includes:
- aligned unmanaged shelf allocation with configurable power-of-two page alignment;
- fixed-size unmanaged records and O(1) slot addressing;
- free-stack allocation and reclamation;
- separate alive and dirty bitmaps stored outside record memory;
- generation-checked handles to reject stale or cross-shelf access;
- dirty-slot enumeration into caller-provided memory;
- deterministic disposal and unit tests for core invariants.

This is a deliberately small foundation. It does **not** yet claim thread-safe concurrent mutation, typed variable-size allocation, persistence, serialization, compaction, GPU interop, or a general-purpose object database. Those should be added only when requirements and benchmark evidence justify them.

## Build and test

Requires the .NET 8 SDK.

```sh
dotnet test tests/TheSingularityWorkshop.DataWarehouse.Tests/TheSingularityWorkshop.DataWarehouse.Tests.csproj -c Release
```

Run the separate [DataWarehouse benchmark suite](https://github.com/TrentBest/TheSingularityWorkshop.Benchmarks.DataWarehouse) with the library repository checked out beside it.

## Design boundary

This repository owns low-level memory layout, slot lifecycle, and direct access. It does not absorb the AEC-facing responsibilities of SingularityWarehouse, nor does it require FSM_COS, MicroBundleDomain, Ontology, Unity, or any UI framework.

## Further reading

- [SingularityWarehouse — the higher-level AEC data fabric](https://github.com/TrentBest/SingularityWarehouse)
- [DataWarehouse benchmark laboratory](https://github.com/TrentBest/TheSingularityWorkshop.Benchmarks.DataWarehouse)

<p align="center">
  <img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/Avatar.jpg" alt="The Singularity Workshop avatar" width="96" />
</p>
