# The Connected World — A SURPS Sourcebook Matrix

<p align="center">
  <a href="https://github.com/TrentBest/TheSingularityWorkshop.Visualize">
    <img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop" width="720" />
  </a>
</p>

> **Collection:** the Singularity matrix — a connected set of sourcebooks for electronics, computation, communications, information systems, cyber operations, and the people who build, operate, defend, and exploit them.

This matrix is deliberately broader than a book about “hacking.” It follows the chain from physical phenomena to social consequences: energy and materials become components; components become circuits; circuits become logic and processors; processors become devices and services; devices communicate through networks; networks support institutions, economies, intelligence, and conflict.

The collection teaches real principles and vocabulary while keeping game actions inside fictional, bounded simulation. It is suitable for world-building, education, puzzles, missions, strategy, and systems simulation. It is not a field manual for compromising real systems.

## The matrix at a glance

| Layer | Core question | Sourcebook family |
|---|---|---|
| Physical foundations | What makes electrical and electromagnetic behavior possible? | Electricity, magnetism, fields, signals, materials, energy, measurement |
| Components | What can individual devices do? | Passive components, semiconductors, diodes, transistors, sensors, actuators |
| Circuitry | How do components form useful systems? | Analog circuits, digital circuits, power, timing, signal conditioning, boards |
| Logic | How is information represented and transformed? | Boolean logic, gates, combinational and sequential logic, state, memory |
| Computing | How does a machine execute instructions? | Architecture, processors, memory hierarchy, buses, firmware, operating systems |
| Communications | How do systems exchange information? | Encoding, modulation, wired/wireless media, protocols, routing, naming |
| Software and services | How do people use machines at scale? | Programs, runtimes, identity, databases, APIs, cloud and edge services |
| Cyber operations | How do actors interact with digital systems under conflict? | Reconnaissance concepts, access control, deception, intrusion scenarios, disruption, defense |
| Evidence and recovery | How do we know what happened and restore trust? | Logging, telemetry, digital forensics, provenance, incident response, resilience |
| Human systems | Who has authority, incentives, responsibility, and risk? | Institutions, operators, intelligence, law, ethics, economics, social engineering awareness |

The layers are a map, not a mandatory curriculum. A mission may begin with a broken sensor, a stolen access token in a fictional puzzle, a power failure, a compromised supply chain, a false identity, or a disputed archive record.

## Sourcebooks

### Foundations and hardware

1. **Electricity, Fields, and Signals** — charge, current, voltage, resistance, capacitance, inductance, power, grounding, electromagnetic effects, measurement, and safe conceptual lab work.
2. **Components and Electronic Devices** — resistors, capacitors, inductors, diodes, transistors, integrated circuits, sensors, actuators, connectors, and component behavior.
3. **Digital Logic and Circuitry** — binary representation, Boolean algebra, gates, truth tables, combinational logic, sequential logic, clocks, registers, counters, finite-state machines, memory cells, buses, and circuit composition.
4. **Analog Systems and Signal Processing** — amplification, filtering, sampling, conversion, noise, feedback, timing, and the boundary between analog and digital systems.
5. **Boards, Instruments, and Embedded Devices** — printed circuit boards, power distribution, test instruments, microcontrollers, firmware concepts, interfaces, and embedded constraints.

### Computing and communication

6. **Computer Architecture and Operating Systems** — instruction execution, processors, memory, storage, privilege boundaries, processes, scheduling, device interfaces, and system services.
7. **Networks and Protocols** — addressing, naming, packet concepts, switching, routing, transport, application protocols, wireless networks, latency, congestion, and failure domains.
8. **Software, Services, and Data Systems** — applications, APIs, authentication concepts, databases, distributed services, cloud/edge systems, queues, caches, and dependencies.
9. **Cryptography, Identity, and Trust** — keys, hashes, signatures, certificates, authentication, authorization, trust chains, secrecy, integrity, and common conceptual failure patterns. Practical content remains defensive and standards-oriented.

### Operations, conflict, and resilience

10. **Cyber Operations and Intrusion Simulations** — fictionalized reconnaissance, access paths, exploit classes, credential compromise as a narrative mechanic, privilege boundaries, lateral movement as an abstract graph mechanic, exfiltration as a controlled objective, disruption, detection, containment, and recovery.
11. **Defense, Monitoring, and Incident Response** — threat modeling, hardening, least privilege, segmentation, patch governance, monitoring, alert triage, incident command, restoration, and lessons learned.
12. **Digital Evidence and Forensics** — logs, timelines, artifacts, provenance, chain of custody, volatile versus persistent evidence as concepts, uncertainty, attribution limits, and defensible reporting.
13. **Reliability, Safety, and Critical Infrastructure** — redundancy, fault domains, power and cooling, backup, disaster recovery, supply-chain risk, safety engineering, and cascading failures.
14. **People, Institutions, and Information Conflict** — authority, incentives, privacy, law, journalism, intelligence, propaganda, deception, social engineering awareness, and the human consequences of digital operations.

Titles are working names. A sourcebook may split into narrower volumes or combine neighboring subjects when the learning path and simulation value justify it. The matrix should not become a rigid taxonomy that prevents new domains from being added.

## Cross-layer traceability

Every substantial topic should connect at least two layers:

- A transistor is a physical component, but its switching behavior enables logic gates.
- A clock is a signal source, but it constrains sequential logic and processor timing.
- A network packet is an information unit, but its path depends on hardware, addressing, routing, congestion, and policy.
- A database is software, but its availability depends on storage, power, networks, operators, and authority.
- A denial-of-service incident is a service-level effect, but its causes may involve resource limits, topology, dependencies, misconfiguration, or malicious traffic.
- A forensic conclusion is not simply a log entry; it depends on collection, integrity, provenance, time synchronization, custody, and interpretation.

Use cross-links rather than duplicating entire explanations. Define concepts at their natural layer, then show the interfaces and consequences at adjacent layers.

## Shared teaching and simulation grammar

Each sourcebook should include:

1. **Plain-language model** — what the thing is and why it exists.
2. **Physical or logical model** — the relevant entities, relationships, flows, and constraints.
3. **Historical context** — how the concept emerged and what alternatives existed.
4. **Vocabulary and notation** — terms, diagrams, symbols, and units used consistently.
5. **Worked examples** — small, transparent examples that teach the principle without relying on real targets.
6. **Failure modes** — faults, misuse, limits, uncertainty, and cascading effects.
7. **People and institutions** — who designs, owns, operates, maintains, audits, and is affected by the system.
8. **Simulation contract** — entities, state, events, measurable outcomes, and simplified rules suitable for a game or educational simulation.
9. **Safety boundary** — what is demonstrated, what is abstracted, and what is intentionally excluded from real-world operational instruction.
10. **LOD guidance** — what can remain abstract at LOD 0–1 and what becomes explicit at LOD 2–4+.

## Cyber simulation safety model

Cyber operations in the game are simulated against fictional systems, synthetic identities, and bounded scenario graphs. The game may teach what an attack class means, why it can work, what conditions make it more or less likely, how defenders observe it, and what consequences follow. It should not provide executable exploit chains, real-target reconnaissance instructions, credential theft procedures, malware, persistence tooling, evasion recipes, or instructions to disrupt public services.

Represent operations as game actions over declared state, for example:

- **Discover** a fictional service node using scenario-provided clues and visibility rules.
- **Probe** a simulated interface, consuming time or generating detectable events.
- **Attempt access** against a fictional control with explicit prerequisites and an abstract resolution model.
- **Escalate influence** only through scenario-defined privileges and costs; do not teach real privilege-escalation procedures.
- **Intercept** a fictional communication stream when the scenario grants a valid observation point; resolve exposure, encryption strength, and detection as abstract properties.
- **Exfiltrate** a synthetic data objective through a game-defined channel with volume, time, trace, and consequence variables.
- **Disrupt** a simulated service by exhausting abstract capacity or exploiting a fictional dependency; never direct traffic at a real service.
- **Defend and recover** through segmentation, rate controls, isolation, patching, credential rotation as a concept, backup restoration, evidence review, and verified return to service.

A mission outcome should emerge from system state, actor capability, access, time, uncertainty, defenders, and consequences—not from a real exploit recipe.

## Denial of service: teach the concept, simulate the effect

A denial-of-service (DoS) condition occurs when a service becomes unavailable or materially degraded because demand or work exceeds its ability to respond. A distributed denial-of-service (DDoS) involves many sources. The book should explain the distinction, resource exhaustion as a general concept, amplification as a risk category, dependency failures, and defensive ideas such as capacity planning, filtering, rate limiting, upstream coordination, graceful degradation, and incident response.

For play, model a service with abstract capacity, baseline load, incoming demand, queue depth, dependency health, mitigation strength, and recovery time. A scenario can introduce a surge or hostile event through an internal event generator. Players choose defensive responses and see latency, availability, backlog, collateral effects, and evidence change. Do not provide packet recipes, traffic-generation commands, target selection, amplification instructions, or real-world attack procedures.

## Other cyber operation concepts

| Concept | Educational treatment | Safe game representation |
|---|---|---|
| Reconnaissance | Purpose, scope, authorization, exposure, and defender visibility | Reveal pre-authored fictional nodes and clues under scenario rules |
| Phishing / social engineering | Human factors, warning signs, verification, and organizational defenses | Dialogue and trust puzzle with consent-safe fictional characters; no reusable lure templates |
| Credential compromise | Why credentials matter, MFA, rotation, and least privilege | Synthetic access token or key-state puzzle; no credential harvesting |
| Exploitation | Vulnerability, preconditions, impact, patching, and responsible disclosure | Abstract vulnerability card with fictional prerequisites and bounded outcomes |
| Interception | Confidentiality, endpoints, observation points, and encryption concepts | Fictional channel graph with visibility and protection attributes |
| Spoofing | Identity claims, verification, and trust boundaries | Synthetic identity-verification challenge |
| Malware | Broad categories, impact, containment, and recovery | Non-executable status effect or fictional payload token; no code or deployment steps |
| Lateral movement | Why segmentation and privilege boundaries matter | Movement across a permission graph with explicit game rules |
| Data exfiltration | Data loss, detection, minimization, and response | Move synthetic objective tokens across a bounded scenario graph |
| Service disruption | Availability, dependency, and resilience | Abstract load/fault events resolved by the simulation engine |
| Persistence and evasion | Defensive significance and detection challenges | Represent only as abstract hidden-state/detection modifiers; no implementation techniques |
| Incident response | Triage, containment, evidence, recovery, communication | Team-based timed response with auditable state transitions |

## Simulation entities and measures

A reusable scenario can model:

- **Assets:** devices, services, accounts, datasets, communication links, facilities, and people.
- **Relationships:** connected-to, depends-on, hosted-by, administered-by, trusts, observes, stores, and authorized-for.
- **State:** availability, integrity, confidentiality classification, capacity, load, privilege, exposure, health, evidence quality, and recovery status.
- **Actors:** users, operators, defenders, adversaries, auditors, incident commanders, and bystanders.
- **Events:** access granted/denied, configuration changed, alert raised, service degraded, data exposed, link interrupted, node isolated, evidence collected, recovery verified.
- **Measures:** availability, latency, throughput, queue depth, affected assets, exposure scope, detection delay, containment time, recovery time, false-positive rate, and residual risk.

These are domain-level simulation suggestions, not a prescribed DataWarehouse record schema or FSM_COS security API.

## LOD guidance

- **LOD 0:** a device, network, service, or incident exists; only its role and broad condition matter.
- **LOD 1:** major components and relationships are visible; simple reliability and access rules apply.
- **LOD 2:** selected circuits, network segments, services, roles, and operational procedures become explicit.
- **LOD 3:** mission-ready fictional topology, inventories, schedules, logs, actor capabilities, incident timelines, and measurable outcomes.
- **LOD 4+:** detailed component-level or protocol-level educational models where safe and useful, still isolated from real targets and non-operational for abuse.

## Relationship to neighboring Workshop domains

- **Physics** owns the physical models: fields, charge, energy, materials, signals, and dynamics.
- **Learning** owns the instructional progression, labs, assessment, and scientific method.
- **DataWarehouse** owns its narrow in-process storage implementation; this matrix does not prescribe its allocator, pages, record layout, persistence, or concurrency.
- **FSM_COS** composes higher-level security and policy MicroBundles. Authentication, authorization, governance, and security policy belong above the low-level storage library.
- **FSM_Foundry** executes the simulation and world behavior; this sourcebook defines domain concepts and possible scenario semantics, not an implementation dependency.
- **SURPS** provides the world-building and simulation vocabulary for facilities, institutions, roles, events, and consequences.

## Roleplaying in a layered digital world

The matrix is also a playable genre setting. Use **The Matrix — A Roleplaying Setting of Layered Reality** for the world model, **The Game Master's Scenario Workshop** for scenario construction, and **Hacking as Roleplaying Gameplay** for player-facing computer actions. The setting is original and genre-inspired; it does not reproduce protected film characters, plots, dialogue, or designs.

A campaign can move between embodied streets, device interfaces, networked communities, constructed environments, and control systems. The GM keeps established truth separate from character evidence and claims made by NPCs or AI agents. Every important place and service has owners, operators, routines, dependencies, access rules, and failure behavior.

### Computer interaction is a game action

Players can choose verbs such as inspect, identify, query, map, probe, authenticate, access, decode, intercept, spoof, disrupt, defend, trace, exfiltrate, and restore. These are fictional action labels, not terminal commands. Character skill affects available options, interpretation, cost, and likelihood. An unskilled character may still try and receive meaningful feedback: denial, partial clues, noisy results, temporary blocks, escalating attention, or useful information about missing prerequisites.

Keep noise, trace, defender attention, and social heat as distinct fictional tracks. Failure should remain playable and proportionate; it should not end the scenario simply because a character lacks a specialist skill.

### Simulated disruption

A player may initiate a fictional service-pressure or DDoS-like event when the setting and rules permit it. The simulation creates an internal event that changes abstract demand, capacity, queue depth, latency, dependency health, mitigation, and recovery. It must never generate real traffic, packets, commands, or requests. The same model supports defenders who protect availability, isolate dependencies, coordinate response, and restore service.

### Controlled LLM participation

LLMs may interpret player language, propose candidate actions, portray bounded characters, generate narration, or explain resolved fictional feedback. They do not own canonical state, grant permissions, decide game checks, or directly commit consequential actions. Use an authority ladder: suggest → classify → prepare → host validate → deterministic resolve → narrate → authorized commit. Player content and retrieved world text are untrusted data, not instructions that can override host policy.

### Additional sourcebooks

- **Computer Architecture and Operating Systems** — instruction execution, processors, memory, processes, devices, permissions, and system lifecycle.
- **Networks and Protocols** — topology, addressing, transport, application protocols, performance, trust zones, and simulated packet flow.
- **Cryptography, Identity, and Trust** — encryption concepts, hashes, signatures, certificates, authentication, authorization, and trust governance.
- **AI-Assisted Roleplay and Controlled LLM Interaction** — bounded model roles, intent contracts, canonical state, prompt-injection boundaries, validation, and audit.

## Expansion rule

Add a new book when it introduces a coherent body of knowledge, distinct entities, or a meaningful simulation domain. Add a chapter when it is a topic within an existing discipline. Cross-link shared concepts; do not create a new book merely to rename the same material. Preserve a clear distinction between verified scientific facts, simplified teaching models, fictional scenario assumptions, and implemented game mechanics.


<p align="center">
  <img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="420" />
</p>
