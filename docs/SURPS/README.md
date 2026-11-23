# DataWarehouse — SURPS Sourcebooks

<p align="center">
  <a href="https://github.com/TrentBest/TheSingularityWorkshop.Visualize">
    <img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop" width="720" />
  </a>
</p>

> **SURPS domain:** storage, preservation, logistics, information infrastructure, and the places and systems that make them possible.

This sourcebook collection defines the *world domain* of warehouses and data centers across eras, environments, civilizations, and scales. It is separate from the low-level .NET library implementation in this repository.

## Domain boundary

The domain includes physical stores of goods and materials; archives, libraries, treasuries, granaries, depots, repositories, cold stores, server rooms, data centers, distributed compute facilities, orbital infrastructure, and speculative storage systems. It covers the people, institutions, economies, technologies, hazards, logistics, and social consequences surrounding them.

The domain does not assume that every warehouse is a building, every data center is a server farm, or every civilization uses modern industrial concepts. A culture may store knowledge in trained custodians, encoded artifacts, living organisms, oral lineages, distributed monuments, or mechanisms that do not resemble computers.

## LOD structure

- **LOD 0 — Core concepts:** storage, inventory, preservation, retrieval, custody, throughput, capacity, access, loss, and distribution. These concepts can exist without a named facility or a detailed simulation.
- **LOD 1 — Domain overview:** warehouse and data-center families, their purposes, operating models, lifecycle, and relationships to settlements, trade, governance, science, and conflict.
- **LOD 2 — Sourcebooks:** focused books that define a facility family, historical period, technological regime, environment, or operational specialty.
- **LOD 3+ — Encounter / implementation detail:** specific sites, floor plans, inventories, staff, machinery, workflows, incidents, and simulation-ready entities. Detail is introduced only when a scenario needs it.

LOD is a detail-selection mechanism, not a claim that every setting progresses through one technological ladder. A low-tech archive can be explored at high detail; a far-future orbital data lattice can remain a low-detail background fact.

## Books

| Book | Coverage |
|---|---|
| [The Warehouse Across Time and Space](The-Warehouse-Across-Time-and-Space.md) | Physical goods, materials, stockpiles, archives, depots, cold chains, logistics, and non-modern civilizations |
| [The Data Center Across Time and Space](The-Data-Center-Across-Time-and-Space.md) | Information storage and processing from human memory and physical media through mechanical, electronic, networked, and speculative systems |
| [Storage Environments and Extreme Facilities](Storage-Environments-and-Extreme-Facilities.md) | Underground, underwater, polar, desert, airborne, orbital, interplanetary, mobile, hostile, and unusual storage contexts |
| [Warehouse and Data-Center Archetypes](Warehouse-and-Data-Center-Archetypes.md) | Reusable facility patterns spanning household reserves, granaries, depots, archives, computing centers, remote outposts, and off-world knowledge cores |
| [SURPS Facility Atlas — Named Sites Across Time and Space](Facility-Atlas-Named-Sites-Across-Time-and-Space.md) | Eight LOD 3–4 constructed facilities with layouts, operating roles, dependencies, incident scenarios, and simulation hooks |

## Shared sourcebook grammar

Each book should define:
1. **Identity and purpose** — what is stored, why, by whom, and for whom.
2. **Historical and technological range** — parallel possibilities, not a single mandatory progression.
3. **Facility anatomy** — zones, containers, infrastructure, access boundaries, and environmental controls.
4. **Operations** — intake, classification, placement, maintenance, retrieval, dispatch, audit, and disposal.
5. **People and institutions** — owners, workers, specialists, customers, regulators, custodians, and adversaries.
6. **Economy and logistics** — capacity, scarcity, throughput, transport, labor, energy, and cost.
7. **Failure and conflict** — spoilage, corruption, theft, outage, contamination, fire, flooding, sabotage, obsolescence, and recovery.
8. **LOD guidance** — what remains abstract at each level and what detail is introduced when zooming in.
9. **Simulation hooks** — state, measurable quantities, events, constraints, and relationships that a SURPS-enabled simulation may choose to model.

## Canonical distinction

A **warehouse** primarily holds and moves things. A **data center** primarily preserves, transforms, and serves information and computation. Real facilities can combine both: a logistics warehouse may run a data center; a data center may contain extensive physical stores, spare-parts depots, batteries, fuel, and archives. The books remain distinct so neither domain is reduced to the other.

These books describe domain possibilities. They do not assert that any particular facility, technology, mechanic, or simulation behavior is already implemented in the DataWarehouse library.

<p align="center">
  <img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="420" />
</p>
