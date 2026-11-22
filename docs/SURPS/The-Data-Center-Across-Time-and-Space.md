# The Data Center Across Time and Space

<p align="center">
  <a href="https://github.com/TrentBest/TheSingularityWorkshop.Visualize">
    <img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop" width="720" />
  </a>
</p>

**SURPS sourcebook · Information, computation, and continuity**

## 1. Identity

A data center is an organized system that preserves, processes, or serves information and computation for other people, institutions, machines, or environments. In its modern form it is a facility containing computing, storage, networking, power, cooling, monitoring, and security infrastructure. The broader domain is much older and much wider than the modern building.

The essential functions are:
- **Remember:** preserve information across time.
- **Transform:** calculate, interpret, classify, or modify information.
- **Communicate:** move information between people, devices, institutions, or places.
- **Coordinate:** provide shared records, services, decisions, or control.
- **Recover:** restore trustworthy operation after loss, damage, or interruption.

A society can perform these functions without electricity, digital computers, or a dedicated data-center building.

## 2. Information infrastructure through time

### Human and cultural memory
- Oral historians, genealogists, navigators, law-speakers, trained record keepers, and specialist custodians preserve knowledge in living memory.
- Memory is distributed among people and communities; continuity depends on training, ritual, apprenticeship, redundancy, and social stability.
- Loss may occur through death, displacement, language change, persecution, or the collapse of institutions.
- In some societies, people themselves are the archive and the processing system.

### Marks, tokens, and physical records
- Notched tallies, knotted cords, clay tokens, seals, tablets, papyrus, parchment, paper, books, maps, and registers encode information in durable or portable forms.
- Archives, libraries, scriptoria, temple record rooms, courts, observatories, and administrative bureaus organize retrieval and authority.
- Indexes, catalogues, cross-references, copying, translation, and trained interpretation become essential infrastructure.
- A record's authenticity and custody may matter as much as its content.

### Mechanical and electromechanical systems
- Abacuses, geared calculators, automata, punched cards, tabulators, telegraph networks, telephone exchanges, and mechanical control systems store or transform information.
- Computation may be distributed among operators, machines, and communication networks.
- Power, maintenance, spare parts, standardized notation, and trained operators constrain service.
- A large calculation bureau or communications exchange can be a data center in function even if it does not resemble a modern server facility.

### Electronic computing and institutional data centers
- Mainframes, minicomputers, tape libraries, disk arrays, batch-processing rooms, university computing centers, government installations, and corporate server rooms.
- Facilities centralize expensive equipment and specialist operations while creating shared dependencies.
- Work scheduling, backups, access control, environmental conditioning, power conditioning, and disaster recovery become formal disciplines.
- Data centers may support science, finance, government, logistics, communications, defense, entertainment, and public services.

### Networked and cloud-era systems
- Colocation facilities, hyperscale campuses, edge sites, content delivery networks, cloud regions, distributed databases, and globally replicated services.
- Computation and storage are spread across sites to manage latency, resilience, capacity, regulation, and cost.
- The apparent service may be centralized to a user while its actual infrastructure is geographically distributed.
- Network routes, identity systems, software dependencies, energy markets, cooling water, supply chains, and human operations all affect availability.

### Post-terrestrial and speculative systems
- Orbital compute platforms, lunar archives, interplanetary message relays, generation ships' knowledge cores, autonomous probes, planetary sensor webs, and distributed machine civilizations.
- Long communication delays change what “central” and “real time” mean. A remote facility may operate autonomously for years before receiving new instructions.
- Radiation, vacuum, thermal cycling, micrometeoroids, launch constraints, and repair distance shape architecture.
- Information may be stored in engineered matter, biological substrates, exotic media, or distributed physical processes. Treat such technologies as setting-specific inventions, not established facts.

## 3. The data-center system

A modern electronic data center commonly combines:

- **Compute:** processors, accelerators, memory, hosts, and specialized appliances.
- **Storage:** volatile memory, solid-state media, magnetic disks, tape, archival systems, and backup media.
- **Network:** switches, routers, optical links, cabling, external carriers, and management networks.
- **Power:** utility feeds, transformers, switchgear, uninterruptible power supplies, batteries, generators, and fuel systems.
- **Thermal management:** airflow, heat exchangers, chillers, pumps, cooling towers, liquid cooling, and controls.
- **Physical plant:** buildings, racks, raised or slab floors, cable pathways, loading areas, fire systems, and environmental sensors.
- **Operations:** monitoring, provisioning, maintenance, incident response, capacity planning, and change control.
- **Governance:** ownership, contracts, data custody, access rules, jurisdiction, audit, and service obligations.

Not every installation has every subsystem. A remote edge cabinet, research vessel, household server room, and hyperscale campus have radically different scale and staffing.

## 4. Facility families

| Family | Primary role | Typical defining constraint |
|---|---|---|
| Archive / records center | Long-term preservation and retrieval | Media decay, authenticity, cataloguing |
| Computing bureau | Shared calculation and analysis | Scheduling, operator expertise, capacity |
| Enterprise server room | Internal institutional services | Reliability, staffing, integration |
| Colocation facility | Hosted customer equipment | Tenant separation, power, cooling, contracts |
| Hyperscale campus | Large-scale compute and storage | Energy, network, land, supply chain |
| Edge facility | Nearby low-latency services | Remote maintenance, limited capacity |
| Research / scientific center | Specialized workloads and instruments | Data rates, precision, reproducibility |
| Mobile / expeditionary center | Local compute in transit or isolation | Power, ruggedness, connectivity |
| Orbital / planetary center | Off-world information infrastructure | Repair delay, environment, autonomy |

Archives and libraries are not automatically data centers in the modern technical sense. They are part of the broader information-infrastructure family and can be modeled at the appropriate LOD.

## 5. Operating model

A facility's service lifecycle may include:
1. Acquire or generate data.
2. Validate, classify, and assign ownership or custody.
3. Ingest into working or archival storage.
4. Index, replicate, protect, and monitor.
5. Schedule and execute computation.
6. Serve results or data to authorized consumers.
7. Detect faults and reconcile replicas or records.
8. Back up, migrate, refresh, or retire media and systems.
9. Recover service after interruption.
10. Audit, retain, archive, or dispose of information under governing rules.

The workflow differs by era. A medieval archive may rely on scribes and catalogues; a distributed machine civilization may use autonomous replication and consensus. The sourcebook should preserve functional comparability without pretending their mechanisms are identical.

## 6. Reliability, continuity, and trust

A data center is not reliable merely because it has many servers. Service depends on the full chain: power, cooling, compute, storage, network, software, operators, suppliers, and external dependencies.

Relevant concepts:
- availability and service continuity;
- durability and risk of irreversible loss;
- integrity and detection of corruption;
- confidentiality and controlled disclosure;
- authenticity and provenance;
- replication, backup, and restoration;
- geographic and organizational failure domains;
- recovery time and acceptable data loss;
- change management and operational discipline.

Security policy and access governance belong to the institutions and higher-level systems that operate the facility. This sourcebook describes their domain role; it does not prescribe security architecture for the DataWarehouse library.

## 7. People and institutions

Potential roles include archivist, librarian, scribe, records officer, operator, systems administrator, network engineer, storage engineer, facilities engineer, electrician, cooling specialist, security officer, auditor, researcher, customer, regulator, and incident commander.

Questions for a setting:
- Who owns the infrastructure and the information?
- Who can read, alter, copy, erase, or withhold records?
- Is the facility a public utility, state instrument, private service, temple, guild, or autonomous entity?
- Who is responsible when the information is wrong or unavailable?
- Can the facility continue operating when disconnected from its owners?
- What knowledge is lost if its staff or operators disappear?

## 8. Energy, heat, and geography

Modern electronic facilities are shaped by electricity, heat removal, network connectivity, land, water, regulation, and access to skilled labor. Earlier information institutions were shaped by writing materials, climate, copying labor, transport, and political stability. Future facilities may be shaped by radiation, communication delay, autonomous maintenance, and access to rare materials.

Geography is not a decorative backdrop. It affects latency, resilience, jurisdiction, cooling, power supply, repair, and the political value of the facility.

## 9. Failure and conflict

Potential incidents include:
- media decay, bit rot, lost indexes, damaged records, and incompatible formats;
- power loss, cooling failure, overheating, fire, flood, and structural damage;
- hardware failure, network partition, software defects, corrupted updates, and cascading dependencies;
- operator error, insider abuse, sabotage, theft, coercion, and institutional censorship;
- failed backups, untested restoration, replica divergence, and accidental deletion;
- supplier disruption, fuel shortage, water restriction, sanctions, war, and jurisdictional conflict;
- communication delay, isolation, autonomous drift, and loss of synchronization between distant sites.

A scenario should distinguish loss of service from loss of data, and loss of data from loss of trust in the data.

## 10. LOD guidance

- **LOD 0:** a society has a way to remember, process, and communicate information.
- **LOD 1:** identify information-infrastructure family, purpose, scale, ownership, consumers, and major dependencies.
- **LOD 2:** define compute/storage/network or equivalent systems, facility anatomy, staffing, operating cycle, energy and cooling, continuity, and governance.
- **LOD 3+:** define actual racks or equivalent equipment, network paths, service dependencies, data flows, operators, maintenance windows, incident playbooks, and specific records or workloads.

A setting can keep most data centers at LOD 1 while elevating one ancient archive, city exchange, research campus, or orbital node to LOD 3 because it matters to the story.

## 11. Simulation hooks

Possible state: compute capacity, storage capacity, utilization, power availability, thermal margin, network connectivity, data integrity, replica health, service availability, staffing, maintenance state, and recovery readiness.

Possible events: ingest, query, processing job, replication, migration, backup, restore, corruption detection, partition, outage, thermal alarm, unauthorized access, equipment replacement, and facility isolation.

These are candidate domain mechanics. A SURPS integration may select them; this repository does not yet implement a data-center simulator.

## 12. Relationship to the DataWarehouse library

The library in this repository explores fixed-size unmanaged records, aligned memory, shelves, slots, handles, and lifecycle metadata. That is one possible low-level storage mechanism within a much larger information ecosystem. It is not a complete data center, database, archive, cloud platform, or distributed storage service.

The SURPS book describes the wider domain. Keep the software contract narrow and evidence-based.
