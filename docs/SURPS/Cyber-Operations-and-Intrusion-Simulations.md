# Cyber Operations and Intrusion Simulations — Digital Conflict Without Real-World Intrusion

<p align="center">
  <a href="https://github.com/TrentBest/TheSingularityWorkshop.Visualize">
    <img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop" width="720" />
  </a>
</p>

> **Sourcebook:** cyber concepts, adversarial scenarios, defensive operations, and safe game mechanics.

This book is for understanding how digital systems are exposed, defended, disrupted, investigated, and restored. It supports fictional missions and educational simulations. It does not teach readers to compromise real systems.

## 1. The system is the playing field

A cyber scenario should begin with a fictional system model, not a real-world target. Represent devices, services, identities, data stores, communication links, trust relationships, dependencies, operators, and defenders. Every node has declared visibility, capabilities, condition, and authority boundaries.

A useful scenario graph distinguishes:

- **Asset:** something of value or operational importance.
- **Service:** a capability delivered to users or other systems.
- **Identity:** a represented person, process, or service principal.
- **Privilege:** an explicitly granted set of permitted actions.
- **Trust relationship:** a reason one entity accepts claims or requests from another.
- **Dependency:** a component or service whose condition affects another.
- **Observation point:** a place where an actor can legitimately observe scenario information.
- **Control:** a policy or mechanism that permits, limits, detects, or blocks an action.

## 2. Threat modeling

Teach assets, threat actors, attack surfaces, trust boundaries, vulnerabilities, preconditions, impacts, likelihood, and mitigations. Keep the model contextual: the same weakness can have different consequences depending on exposure, privilege, compensating controls, and the value of the affected asset.

Use fictional actor profiles with motives such as profit, espionage, disruption, coercion, curiosity, activism, or insider grievance. Do not turn motive labels into assumptions about real people or groups.

## 3. Operation lifecycle as a narrative model

A high-level operation may be described as a sequence of objectives: understand the fictional environment, identify a scenario weakness, attempt a bounded action, achieve or fail an objective, trigger defender observations, and resolve consequences. This is a storytelling and simulation structure—not a procedural intrusion checklist.

For gameplay, each action must be a declared game action with prerequisites, cost, uncertainty, possible detection, and a logged outcome. Do not translate action cards into commands, scripts, exploit payloads, real reconnaissance steps, or instructions for bypassing protections.

## 4. Access, identity, and authorization

Distinguish authentication (which identity is being represented), authorization (what that identity may do), accounting/audit (what was recorded), and governance (who defines the rules). Introduce least privilege, multifactor authentication, credential lifecycle, separation of duties, privileged accounts, service identities, and revocation as defensive concepts.

In the game, use synthetic identities, fictional tokens, and explicit permission graphs. A puzzle can ask players to infer which fictional role is allowed to open a record, approve a transfer, or operate a service. It must not ask them to obtain or reuse real credentials.

## 5. Interception and communications security

Explain the confidentiality and integrity goals of communications, endpoints, trust, key management, encryption as a concept, metadata exposure, and the limits of attribution. Interception missions should use fictional channels with predeclared observation points and abstract protection attributes. Outcomes can depend on access to the channel, endpoint security, key status, timing, and detection—not on real packet capture or interception instructions.

## 6. Service disruption and denial of service

A DoS condition is a loss or degradation of availability because a service cannot meet demand. A DDoS uses many sources. Teach the impact on users, dependencies, queues, capacity, and recovery. Discuss defenses at a conceptual level: capacity planning, rate controls, filtering, upstream coordination, redundancy, graceful degradation, and incident response.

The simulation should generate synthetic demand internally. Model baseline load, available capacity, demand, queue depth, latency, dependency health, mitigation, and recovery. Never provide traffic-generation instructions, attack parameters, amplification methods, target selection, or a way to direct disruptive traffic at real systems.

## 7. Exploits and vulnerabilities as abstract scenario objects

A vulnerability is a weakness that can affect confidentiality, integrity, or availability under particular conditions. A fictional exploit object may specify:

- affected fictional component;
- required scenario conditions;
- abstract difficulty or uncertainty;
- potential impact category;
- observable indicators;
- defensive mitigations;
- remediation and verification conditions.

Do not include executable payloads, exploit code, reproduction steps for real vulnerabilities, real target details, or operational bypass techniques. Real-world vulnerability education should focus on secure design, patching, responsible disclosure, and defensive interpretation.

## 8. Detection, incident response, and evidence

A defender's work includes monitoring, alert triage, scoping, containment, eradication, recovery, communication, and lessons learned. Explain that alerts are signals, not proof; logs may be incomplete; clocks may disagree; attribution may be uncertain; and evidence must retain provenance and custody.

A game incident should create evidence objects with source, collection time, event time, collector, integrity status, confidence, and chain-of-custody history. This aligns with the SURPS Scenario Event Catalog without defining a software schema.

## 9. Scenario design patterns

| Mission pattern | Player objective | Defender / world response | Learning outcome |
|---|---|---|---|
| Fictional records-room mystery | Determine which synthetic identity accessed a record | Audit events and incomplete evidence | Logs, identity, provenance, uncertainty |
| Intercepted dispatch | Recover the meaning of a fictional message | Channel visibility and protection attributes | Confidentiality, metadata, trust |
| Service under pressure | Keep a fictional service available during a generated surge | Queue growth, rate controls, failover | DoS impact, capacity, resilience |
| Insider access dispute | Establish whether an actor exceeded assigned authority | Role graph, approvals, audit trail | Authorization, separation of duties |
| Compromised outpost | Restore a remote facility's trusted operation | Isolation, delayed communications, scarce staff | Incident command, recovery, dependency |
| Data integrity investigation | Determine which synthetic copy is authoritative | Provenance and replica disagreement | Integrity, evidence, authority |
| Supply-chain anomaly | Trace an unexpected component or update | Vendor trust, validation, rollback | Supply-chain risk and verification |

## 10. Simulation state, events, and scoring

Suggested state dimensions include service availability, response latency, queue depth, identity privilege, data classification, integrity confidence, network connectivity, detection confidence, incident severity, evidence quality, and recovery status.

Suggested events include access requested/granted/denied, alert raised, configuration changed, service degraded, asset isolated, evidence collected, data exposure confirmed, backup restored, and recovery verified. Score outcomes using objective completion, collateral impact, exposure, service continuity, evidence quality, detection delay, containment time, and recovery time. Avoid scoring only on “successful intrusion”; reward restraint, verification, and harm reduction.

## 11. Ethics, law, and player boundaries

Scenarios must be fictional and explicitly authorized within the game world. Do not invite players to test mechanics against public websites, employers, schools, government services, other players' devices, or any system outside the simulation. Avoid realistic instructions for credential theft, malware, stealth, persistence, evasion, unauthorized access, or service disruption. Use consent, clear boundaries, and consequences as part of the design.

## 12. LOD guidance and integration

- **LOD 0:** a digital incident or service exists; describe its consequence.
- **LOD 1:** identify assets, actors, trust boundaries, and broad defenses.
- **LOD 2:** expose a fictional topology, permission graph, and event timeline.
- **LOD 3:** provide mission state, synthetic identities, scenario logs, response roles, and measurable objectives.
- **LOD 4+:** add detailed but non-operational educational models, isolated from real targets.

This sourcebook defines domain vocabulary and scenario semantics. Physics owns physical foundations; Learning owns instructional design; DataWarehouse remains a low-level storage library; FSM_COS owns higher-level security and policy composition; FSM_Foundry executes world behavior. No real-world security test, exploit execution, or software API is specified here.


<p align="center">
  <img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="420" />
</p>
