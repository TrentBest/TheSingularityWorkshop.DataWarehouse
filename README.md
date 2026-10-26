<p align="center">
  <a href="https://github.com/TrentBest/TheSingularityWorkshop.Visualize">
    <img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop" width="720" />
  </a>
</p>

# TheSingularityWorkshop.DataWarehouse

[![Tests](https://github.com/TrentBest/TheSingularityWorkshop.DataWarehouse/actions/workflows/tests.yml/badge.svg?branch=development)](https://github.com/TrentBest/TheSingularityWorkshop.DataWarehouse/actions/workflows/tests.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Last commit](https://img.shields.io/github/last-commit/TrentBest/TheSingularityWorkshop.DataWarehouse/development)](https://github.com/TrentBest/TheSingularityWorkshop.DataWarehouse/commits/development)

**A platform-neutral, page-aligned unmanaged data store designed for direct, predictable access.**

## 00 Identity

DataWarehouse reconstructs the low-level memory technology from an earlier Unity-era implementation. Its vocabulary is physical on purpose: a **lake** owns allocation, **shelves** hold fixed-size records, **slots** are bays, and **handles** identify records across allocation lifetimes.

This is not the higher-level AEC-facing [SingularityWarehouse](https://github.com/TrentBest/SingularityWarehouse). SingularityWarehouse explores a public-facing data fabric across tools and domains; DataWarehouse is the lower-level memory substrate. The projects can complement one another, but they have different responsibilities and performance goals.

## A visual tour

The SingularityWarehouse project established a strong visual narrative: big ideas made tangible, clear boundaries, and visuals that help the reader understand the system before diving into code. This repository carries that visual ambition forward with **new, product-specific technical illustrations**. The earlier visuals explain information ecosystems, BIM workflows, brokered capabilities, and business value; these visuals explain allocation ownership, fixed-size records, side-band tracking, and handle lifetime.

<p align="center">
  <img src="docs/assets/DataWarehouse-Architecture.svg" alt="DataWarehouse ownership path from warehouse and aligned lake allocation through typed shelves to fixed-size slots, with alive, dirty, and generation tracking" width="100%" />
</p>

*Conceptual, source-checked architecture view. It is not a byte-accurate memory-layout diagram or a measured performance result.*

## 01 What

DataWarehouse places fixed-size unmanaged records in aligned shelf memory. A handle identifies a shelf, slot, and generation; separate bitmaps track slot liveness and dirty state.

The core address calculation is:

`address = shelfBase + slot × sizeof(T)`

The slot calculation is constant-time in the algorithmic sense. Handle validation and memory access have real costs, and practical latency depends on CPU caches, memory topology, and workload.

## 02 Why

General-purpose object graphs and hash lookups are convenient, but may introduce indirection and per-object overhead in hot paths. DataWarehouse explores a more explicit trade-off: fixed-size records, predictable slot addressing, and lifecycle metadata kept outside the record payload.

The goal is not to declare every workload faster. The goal is to make the storage model measurable and to establish where it helps.

## 03 How

- **Data Lake** — aligned unmanaged allocation boundary.
- **DataWarehouse** — owns the lake and creates shelves.
- **Data Shelf** — page-aligned storage for one unmanaged record type.
- **Slot / bay** — one fixed-size record position.
- **Alive tracker** — external bitmap for allocated slots.
- **Dirty tracker** — external bitmap for changed or pending slots.
- **Data handle** — shelf identity + slot index + generation; supports constant-time validation and stale-handle rejection.

Current implementation includes aligned allocation, free-stack allocation/reclamation, alive and dirty bitmaps, generation-checked handles, dirty-slot enumeration into caller-provided memory, deterministic disposal, and unit tests.

<p align="center">
  <img src="docs/assets/DataWarehouse-Handle-Lifecycle.svg" alt="DataWarehouse handle lifecycle from allocation through mutation and cleaning to freeing and generation-advancing slot reuse" width="100%" />
</p>

*Source-checked lifecycle summary. The detailed sequence and caveats live in [DataShelf lifecycle](docs/UML/DataShelf-Lifecycle.md).*

It does **not** currently promise thread-safe concurrent mutation, variable-size allocation, persistence, serialization, compaction, GPU interop, or general-purpose object-database behavior.

## 04 See it in a minute

Requires the .NET 8 SDK.

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

Run the tests:

```sh
dotnet test tests/TheSingularityWorkshop.DataWarehouse.Tests/TheSingularityWorkshop.DataWarehouse.Tests.csproj -c Release
```

The [DataWarehouse benchmark laboratory](https://github.com/TrentBest/TheSingularityWorkshop.Benchmarks.DataWarehouse) measures named access and lifecycle workloads. Check its reports before drawing performance conclusions.

## 05 Available documentation and theory

- [Documentation inventory and diagram status](docs/DocumentationInventory.md)
- [DataShelf API and operational contract](docs/api/DataShelf.md)
- [DataShelf lifecycle sequence](docs/UML/DataShelf-Lifecycle.md)
- [Visual assets](docs/assets/)
- [Source code](src/TheSingularityWorkshop.DataWarehouse/)
- [Unit tests](tests/TheSingularityWorkshop.DataWarehouse.Tests/)
- [Benchmark laboratory](https://github.com/TrentBest/TheSingularityWorkshop.Benchmarks.DataWarehouse)
- [SingularityWarehouse — higher-level AEC data fabric](https://github.com/TrentBest/SingularityWarehouse)

## Design boundary and verification

This repository owns low-level memory layout, slot lifecycle, and direct access. It does not require Unity, FSM_COS, MicroBundleDomain, Ontology, or a UI framework. Complexity statements describe algorithmic structure, not guaranteed wall-clock speed; benchmark claims must name their workload and environment.

<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_COS/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
