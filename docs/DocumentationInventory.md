# Documentation Inventory — DataWarehouse

This inventory follows the Workshop's FSM_COS documentation principles: edify, not mystify; explain the user problem, design rationale, operational use, limits, and how claims can be verified.

## Scope and evidence labels

- **Implemented** — present in the current repository and linked to source or tests.
- **Source-checked** — diagram or explanation checked against current source, but not generated automatically.
- **Planned** — useful future documentation; not a claim of existing output or tooling.
- **Not applicable** — deliberately excluded for the current product boundary.

## Inventory

| View / artifact | Status | Purpose / evidence |
|---|---|---|
| README / quick start | Implemented | Product identity, rationale, example, scope, verification path |
| README architecture illustration | Source-checked conceptual view | Ownership path, typed shelves, slots, side-band tracking; not byte-accurate |
| README handle lifecycle illustration | Source-checked | Allocation, dirty tracking, clean, free, and generation-advancing reuse |
| DataShelf API and operational contract | Implemented | Operation semantics, failure behavior, complexity, tests, concurrency caveat |
| Class/type diagram | Planned | Show public types and ownership without inventing relationships |
| Type neighborhood pages | Planned | Target type centered, consumers above, dependencies below |
| DataShelf lifecycle sequence | Source-checked | Allocate, mutable access, dirty marking, clean, free, generation change |
| Lifecycle/state transitions | Source-checked | Alive/dirty/generation changes for the current implementation |
| Data-flow / memory-layout diagram | Source-checked conceptual view | README illustration; not a byte-accurate layout |
| Use cases | Planned | Fixed-size records, dirty-slot processing, slot reuse |
| Complexity and allocation analysis | Partial | DataShelf operations documented; other public types and empirical costs still need coverage |
| Test traceability | Partial | Core invariants linked from API doc; full requirement-to-test matrix remains planned |
| Benchmark evidence | Implemented separately | Named workloads and CI artifacts live in the benchmark repository |
| Deployment diagram | Not applicable | Current library is an in-process .NET component |
| MicroBundle views | Not applicable | No MicroBundle dependency or contract in this repository |
| Automated diagram generation / stale-output gate | Planned | No repository-wide generator or completeness gate is claimed today |

## Visual provenance

The SingularityWarehouse repository's existing artwork communicates the higher-level data-fabric story: cross-application information, BIM, brokered capabilities, security, and business value. Those assets are not copied verbatim because their subject and abstractions differ from this low-level library. Their visual lesson—strong hierarchy, restrained technical labels, and a coherent cinematic palette—informs the new DataWarehouse-specific SVG illustrations.

The new illustrations are authored as static SVG assets under `docs/assets/`. They are not interactive, benchmark output, or automatically generated. Update them when the source contract changes.

## Required accuracy rules

1. Keep source-derived facts distinct from conceptual architecture and planned behavior.
2. Every performance claim must identify its operation, input scale, runtime/environment, revision, and report when measured evidence is available.
3. Use O-notation only where the relevant operations and bounds are defensible; include allocations/copies and note unknowns.
4. Do not describe a static diagram as interactive or generated.
5. Update this inventory as documentation is added; a planned view is not evidence of implementation completeness.

## Branding

Use the shared Workshop moniker at the top of primary Markdown documents and the canonical Workshop footer image:

https://raw.githubusercontent.com/TrentBest/FSM_COS/master/Documentation/Branding/TheSingularityWorkshop.png

Keep diagrams product-specific: DataWarehouse visuals should explain shelves, slots, handles, and lifecycle metadata.
