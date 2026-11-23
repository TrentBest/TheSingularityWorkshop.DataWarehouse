# Computer Architecture and Operating Systems — From Instructions to Services

<p align="center">
  <a href="https://github.com/TrentBest/TheSingularityWorkshop.Visualize">
    <img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop" width="720" />
  </a>
</p>

> **SURPS sourcebook:** the internal life of computers, devices, and operating environments for believable settings and safe simulation.

## Scope
A computer combines a processor, working memory, persistent storage, interconnects, input/output devices, firmware, and software. This book teaches architecture and operating behavior; it does not describe exploiting kernels, bypassing isolation, or attacking real machines.

## Architecture foundations
The processor fetches, decodes, and executes instructions. Registers hold immediate working values; an arithmetic/logic unit performs operations; control logic coordinates execution. Modern processors add pipelines, caches, multiple cores, accelerators, and speculative techniques, each with performance and complexity tradeoffs.

## Memory and storage hierarchy
Registers, caches, main memory, and persistent storage differ in speed, capacity, cost, and persistence. Locality explains why caches help performance. Virtual memory gives processes managed address spaces; paging and memory pressure affect responsiveness. Storage adds files, metadata, permissions, buffering, redundancy, and recovery.

## Operating-system responsibilities
An operating system coordinates processes and threads, scheduling, memory, files, devices, networking, users, permissions, and services. A process is an executing program with resources and an address space; threads are execution paths within a process. Scheduling shares processor time. Interrupts and device interfaces let hardware signal events. Drivers mediate device-specific behavior.

## Boot and lifecycle
A simplified lifecycle is power-on → firmware initialization → boot selection → operating-system loading → service startup → user session → shutdown or failure recovery. Platforms vary; this is a teaching model, not a universal exact sequence.

## Security concepts
Explain user and service identities, authentication, authorization, privilege separation, process isolation, updates, secure configuration, backups, and audit records. Teach the purpose of boundaries and common classes of misconfiguration without exploitation steps. Fictional permissions can be represented as explicit graph edges.

## Failure and operational realism
Useful failures include thermal throttling, memory pressure, storage exhaustion, corrupted files, device failure, driver incompatibility, clock drift, failed updates, dependency outage, and operator error. Each has symptoms, diagnostic evidence, affected users, mitigation, and recovery.

## Game simulation model
Represent a device with processor capacity, memory pressure, storage capacity and health, operating state, connected devices, services, users, permissions, update status, and observable events. Keep instruction execution abstract unless a learning scene requires it. Player actions use the companion hacking-game vocabulary.

## LOD
LOD 0: computer or service role. LOD 1: major components and health. LOD 2: processes, memory/storage pressure, users, and dependencies. LOD 3: fictional inventories, service maps, logs, and incidents. LOD 4+: selected architecture concepts for educational labs, isolated from real attack procedures.


<p align="center">
  <img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="420" />
</p>
