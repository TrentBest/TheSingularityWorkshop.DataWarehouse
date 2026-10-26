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
| Public API and type reference | Planned | Per-type purpose, key members, contracts, limitations, source and test links |
| Class/type diagram | Planned | Show public types and ownership without inventing relationships |
| Type neighborhood pages | Planned | Target type centered, consumers above, dependencies below |
| Sequence diagrams | Planned | Allocation, mutable access/dirty marking, read, free, disposal |
| Lifecycle/state diagrams | Planned | Slot states, generation changes, stale-handle rejection |
| Data-flow / memory-layout diagram | Source-checked conceptual view | Current README overview only; not a byte-accurate layout |
| Use cases | Planned | Fixed-size records, dirty-slot processing, slot reuse |
| Complexity and allocation analysis | Partial | README states constant-time slot addressing; full operation-by-operation analysis requires source review and measurement |
| Test traceability | Partial | Existing tests cover core invariants; a requirement-to-test matrix is planned |
| Benchmark evidence | Implemented separately | Named workloads and CI artifacts live in the benchmark repository |
| Deployment diagram | Not applicable | Current library is an in-process .NET component |
| MicroBundle views | Not applicable | No MicroBundle dependency or contract in this repository |
| Automated diagram generation / stale-output gate | Planned | No repository-wide generator or completeness gate is claimed today |

## Required accuracy rules

1. Keep source-derived facts distinct from conceptual architecture and planned behavior.
2. Every performance claim must identify its operation, input scale, runtime/environment, revision, and report when measured evidence is available.
3. Use O-notation only where the relevant operations and bounds are defensible; include allocations/copies and note unknowns.
4. Do not describe a static diagram as interactive or generated.
5. Update this inventory as documentation is added; a planned view is not evidence of implementation.

## Branding

Use the shared Workshop moniker at the top of primary Markdown documents and the canonical Workshop footer image:

`https://raw.githubusercontent.com/TrentBest/FSM_COS/master/Documentation/Branding/TheSingularityWorkshop.png`

Keep diagrams product-specific: DataWarehouse visuals should explain shelves, slots, handles, and lifecycle metadata.
