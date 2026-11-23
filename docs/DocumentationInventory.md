# Documentation Inventory — DataWarehouse

This inventory follows the Workshop documentation standard: edify, not mystify; explain the user problem, design rationale, operational use, limits, and how claims can be verified.

## Scope and evidence labels

- **Implemented** — present in the current repository and linked to source or tests.
- **Source-checked** — diagram or explanation checked against current source, but not generated automatically.
- **Planned** — useful future documentation; not a claim of existing output or tooling.
- **Not applicable** — deliberately excluded for the current product boundary.

## Inventory

| View / artifact | Status | Purpose / evidence |
|---|---|---|
| README / quick start | Implemented | Product identity, rationale, example, scope, verification path |
| Architecture and domain model | Implemented (source-checked) | Vocabulary, ownership topology, type responsibilities, memory model, lifecycle, concurrency, non-goals |
| DataLake allocation contract | Implemented (source-checked) | Shared backing regions, buddy page allocation/splitting/coalescing, clearing, active/reserved accounting, reuse, deferred disposal |
| DataHandle identity contract | Implemented (source-checked) | Shelf/slot/generation validation, stale-handle prevention, generation retirement, lifetime and security boundary |
| DataLake region-allocation lifecycle | Source-checked | Region creation, buddy allocation, reclamation/coalescing, region reuse, deferred disposal |
| Record design and memory patterns | Implemented | Fixed-layout unmanaged records, compact state, AoS/SoA patterns, handles, dirty processing, metadata cost, growth and lifetime |
| DataShelf API and operational contract | Implemented | Operation semantics, dirty-slot-to-handle resolution, failure behavior, complexity, dynamic capacity growth, relocation/lifetime, tests, concurrency caveat |
| Class/type diagram | Planned | Show public types and ownership without inventing relationships |
| Type neighborhood pages | Partial | DataLake, DataHandle, and DataShelf contracts documented; DataWarehouse public factory/disposal contract is covered in the architecture page |
| DataShelf lifecycle sequence | Source-checked | Allocate, mutable access, dirty marking, clean, free, generation change |
| Lifecycle/state transitions | Source-checked | Alive/dirty/generation changes for the current implementation |
| Data-flow / memory-layout diagram | Source-checked conceptual view | README illustration; not a byte-accurate layout |
| Use cases | Planned | Fixed-size records, dirty-slot processing, slot reuse |
| Complexity and allocation analysis | Partial | DataShelf operations and structural metadata costs documented; empirical costs remain in benchmark repository |
| Verification plan and invariants | Implemented (coverage audit) | Slot and allocator invariants, current test evidence, explicit missing test matrix, completion gate |
| Test traceability | Partial | Invariant-to-test matrix exists; dedicated tests are still needed for listed gaps |
| Benchmark evidence | Implemented separately | Named workloads and CI artifacts live in the benchmark repository |
| Deployment diagram | Not applicable | Current library is an in-process .NET component |
| MicroBundle views | Not applicable | No MicroBundle dependency or contract in this repository |
| SURPS domain sourcebooks | Implemented (growing collection) | Warehouse, data-center, extreme-environment, archetype, and eight-site LOD 3–4 facility-atlas coverage; separate from library implementation |
| SURPS facility diagrams | Implemented (authored static Mermaid) | Conceptual site plans, operational flows, dependency maps, and failure propagation for six constructed settings; not engineering drawings |
| SURPS facility scenario packs | Implemented (constructed scenario data) | Four facility packs with stock snapshots, staffing and authority, schedules, incidents, invariants, and outcome measures; values are illustrative |
| SURPS scenario event catalog | Implemented (domain vocabulary) | Event families, preconditions, evidence, state-transition discipline, cross-domain sequences, invariants, failure chains, and measures; not a software event schema |
| SURPS connected-world matrix | Implemented (sourcebook index) | Cross-layer map from physical electronics through digital logic, computing, communications, cyber operations, evidence, resilience, and human institutions |
| SURPS electricity, fields, and signals | Implemented (educational domain sourcebook) | Physical foundations of electronics, circuit relationships, signals, measurement, uncertainty, energy, and conceptual labs |
| SURPS components and electronic devices | Implemented (educational domain sourcebook) | Passive and semiconductor devices, integrated circuits, sensors, actuators, boards, interfaces, lifecycle, and failure concepts |
| SURPS digital logic and circuitry | Implemented (educational domain sourcebook) | Components, switching, Boolean logic, combinational and sequential circuits, clocks, memory, state machines, and conceptual labs |
| SURPS cyber operations and intrusion simulations | Implemented (safe fictional scenario sourcebook) | Abstract access, interception, exploit classes, DoS effects, detection, incident response, evidence, and recovery; excludes real-target procedures and executable attack techniques |
| SURPS Matrix roleplaying setting | Implemented (genre setting sourcebook) | Original layered-reality campaign model, factions, institutions, world truth, evidence, and simulation hooks |
| SURPS Game Master's Scenario Workshop | Implemented (GM craft sourcebook) | Scenario packets, causal chains, scene design, operational realism, clue provenance, clocks, consequences, and AI boundaries |
| SURPS hacking as roleplaying gameplay | Implemented (fictional action sourcebook) | Player computer-action vocabulary, skill-based outcomes, playable failure, separate noise/trace/attention/heat, and internal-only simulated service pressure |
| SURPS computer architecture and operating systems | Implemented (educational domain sourcebook) | Processor, memory, OS responsibilities, processes, permissions, lifecycle, and failure concepts |
| SURPS networks and protocols | Implemented (educational domain sourcebook) | Network topology, layered communication, addressing, transport, performance, trust, and synthetic simulation |
| SURPS cryptography, identity, and trust | Implemented (educational domain sourcebook) | Confidentiality, integrity, cryptographic primitives, certificates, authentication, authorization, and trust governance |
| SURPS AI-assisted roleplay and controlled LLM interaction | Implemented (interaction-governance sourcebook) | Bounded LLM roles, intent contracts, host authority ladder, canonical state, untrusted context, validation, and audit |
| Automated diagram generation / stale-output gate | Planned | No repository-wide generator or completeness gate is claimed today |

## Visual provenance

The illustrations are authored specifically for DataWarehouse under `docs/assets/`. They are static SVG assets, not interactive, benchmark output, or automatically generated. Update them when the source contract changes.

## Required accuracy rules

1. Keep source-derived facts distinct from conceptual architecture and planned behavior.
2. Every performance claim must identify its operation, input scale, runtime/environment, revision, and report when measured evidence is available.
3. Use O-notation only where the relevant operations and bounds are defensible; include allocations/copies and note unknowns.
4. Do not describe a static diagram as interactive or generated.
5. Update this inventory as documentation is added; a planned view is not evidence of implementation completeness.

## Branding

Use the shared Workshop moniker at the top of primary Markdown documents and the canonical Workshop footer image:

https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png

Keep diagrams product-specific: DataWarehouse visuals should explain shelves, slots, handles, and lifecycle metadata.
