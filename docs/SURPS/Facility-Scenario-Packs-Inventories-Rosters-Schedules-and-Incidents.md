# SURPS Facility Scenario Packs — Inventories, Rosters, Schedules, and Incidents

<p align="center">
  <a href="https://github.com/TrentBest/TheSingularityWorkshop.Visualize">
    <img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop" width="720" />
  </a>
</p>

**SURPS scenario supplement · LOD 3–4 · Constructed play data**

> **Scenario-data notice:** Every quantity, name, role count, threshold, schedule, and event time in this supplement is invented for repeatable world-building and simulation. These are not historical statistics, engineering recommendations, safety limits, or operational standards. Adapt them to the setting and scenario.

## 1. How to use a pack

Each pack is a coherent starting state, not a universal template. It supplies:
- an initial inventory snapshot with units and condition;
- a roster with authority and coverage;
- a repeating operating schedule;
- dependencies and resource constraints;
- a timed incident sequence with observable state changes;
- objectives and measurable outcomes.

The snapshots are deliberately finite. A simulation should model arrivals, consumption, spoilage, maintenance, and transfers as events rather than treating the starting inventory as inexhaustible.

### State dimensions

Track at least six distinct dimensions where they matter:

| Dimension | Example state |
|---|---|
| Physical | quantity, location, condition, contamination |
| Operational | available, reserved, in transit, held, under repair |
| Institutional | owner, custodian, approval, dispute |
| Informational | recorded quantity, provenance, confidence, discrepancy |
| Human | present, assigned, fatigued, injured, informed |
| Environmental | temperature, humidity, water, power, weather |

Do not collapse these into a single “available” flag. A crate can physically exist while being quarantined, unrecorded, inaccessible, or not authorized for issue.

## 2. Namar Granary — harvest reserve and ration integrity

**Scenario premise:** Late harvest has filled the compound, but damp weather and a disputed delivery threaten the winter reserve.

### Initial stock snapshot

| Stock | Quantity | Unit | Condition / status |
|---|---:|---|---|
| Food grain | 420 | sacks | 390 accepted; 30 held for re-drying |
| Seed grain | 96 | sacks | sealed; next planting reserve |
| Dried legumes | 74 | sacks | accepted |
| Salt | 18 | jars | dry store |
| Oil | 22 | jars | 4 assigned to lamp reserve |
| Empty grain sacks | 55 | sacks | serviceable |
| Firewood | 12 | bundles | drying yard fuel |
| Ledger tablets | 3 | tablets | one current, one duplicate, one archive copy |

**Scenario unit note:** A “sack” is an abstract local unit. Do not infer a modern mass from it unless a campaign defines one.

### Roster and authority

| Role | Count | Primary responsibility | Authority boundary |
|---|---:|---|---|
| Granary steward | 1 | stock decisions and daily priorities | may authorize routine issue |
| Scribes | 2 | receipts, balances, household claims | record but do not release stock alone |
| Storekeepers | 4 | bin placement, inspection, issue | physical access under witness |
| Drying-yard workers | 3 | turn and protect damp grain | cannot reclassify disputed stock |
| Gate watch | 2 | entry, seal, visitor log | may stop access; cannot alter ledgers |
| Community witnesses | 2 rotating | observe major receipts and emergency issues | attest, do not command |

### Operating schedule

| Local time | Activity | Required roles |
|---|---|---|
| Dawn | inspect seals, roof, drainage, and pest signs | steward, watch, storekeeper |
| Morning | receive deliveries and sample grain | scribe, storekeeper, witnesses |
| Midday | turn drying grain; reconcile accepted quantities | yard crew, scribe |
| Afternoon | routine household issue window | steward, storekeepers, witnesses |
| Dusk | secure doors, record balances, transfer watch | steward, scribe, watch |

### Incident timeline

| Time | Event | Expected state change |
|---|---|---|
| Day 1, morning | A delivery arrives 12 sacks short of its declared count | receipt enters discrepancy state; do not silently amend manifest |
| Day 1, midday | Rain begins before the drying yard is cleared | exposed stock condition degrades; labor is diverted |
| Day 1, afternoon | A household requests emergency issue beyond its allotment | request awaits steward decision and witness |
| Day 2, dawn | A seal is found intact but the ledger balance differs by 6 sacks | physical and informational states diverge; isolate affected bin |
| Day 2, afternoon | A second witness confirms the prior day's count | investigation can narrow the discrepancy window |

**Success measures:** seed reserve remains protected; accepted food stock is correctly classified; every issue has authority and witness; discrepancies remain visible until resolved.

**Failure paths:** damp grain is accepted prematurely; seed reserve is consumed for routine demand; emergency issue bypasses custody; ledger correction overwrites the original discrepancy.

## 3. Meridian Fulfillment Campus — peak demand and sortation fault

**Scenario premise:** A seasonal demand surge coincides with a diverter failure shortly before the final carrier collection.

### Initial inventory and work queues

| Item / queue | Quantity | Unit | State |
|---|---:|---|---|
| Reserve pallet positions | 1,200 | positions | 1,020 occupied |
| Automated container positions | 8,000 | positions | 6,750 occupied |
| Open customer orders | 3,600 | orders | 2,100 within today's dispatch promise |
| Exception queue | 84 | order lines | identity, damage, or address review |
| Packed orders awaiting sort | 640 | parcels | carrier assignment pending |
| Empty shipping cartons | 4,500 | cartons | mixed sizes |
| Label stock | 9,000 | labels | two printer-compatible batches |
| Conveyor segments | 14 | segments | 13 available, 1 degraded |
| Sortation diverters | 32 | units | 31 available, 1 intermittent |

### Shift roster

| Role | Day shift | Swing shift | Responsibility |
|---|---:|---:|---|
| Operations lead | 1 | 1 | priority and safe-stop decisions |
| Inbound associates | 12 | 6 | unload, identify, receive |
| Inventory control | 4 | 2 | location and discrepancy resolution |
| Replenishment / pick | 18 | 12 | stock movement and order picks |
| Goods-to-person operators | 8 | 6 | automated station operation |
| Pack associates | 16 | 10 | pack, label, verify |
| Sortation technicians | 3 | 2 | jam response and controlled restart |
| Maintenance | 2 | 1 | mechanical and electrical repair |
| Safety / first response | 2 | 1 | incident response and exclusion zones |

### Operating schedule

| Window | Planned activity | Control point |
|---|---|---|
| 05:30–06:00 | shift handoff and equipment status | unresolved faults explicitly accepted |
| 06:00–10:00 | inbound peak and replenishment | receiving identity / condition checks |
| 10:00–14:00 | pick and pack peak | order priority and exception isolation |
| 14:00–17:00 | carrier wave consolidation | dispatch cutoff and trailer capacity |
| 17:00–17:30 | reconciliation and handoff | queue, inventory, and fault state |

### Incident timeline

| Time | Event | Expected state change |
|---|---|---|
| 14:05 | Diverter 17 begins missing assigned lanes | affected parcels enter recirculation or exception state |
| 14:12 | Recirculation queue reaches 180 parcels | throughput falls; queue capacity becomes a constraint |
| 14:16 | Technician requests a controlled segment stop | operations lead authorizes isolation; personnel clear the zone |
| 14:28 | Manual dispatch lane opens at reduced capacity | selected priority orders bypass the failed segment |
| 14:45 | Root cause identified as a damaged actuator | repair consumes spare part and technician time |
| 15:10 | Controlled restart test passes | segment returns to service under observation |
| 15:25 | Late parcels are reconciled against carrier manifests | no parcel is assumed dispatched solely because it left sortation |

**Success measures:** safe isolation; accurate parcel custody; priority orders dispatched without falsifying timestamps; all late orders remain visible; restart is verified before normal throughput resumes.

**Failure paths:** operators clear a jam without isolation; recirculation is mistaken for progress; manual bypass loses scan events; the system marks parcels shipped before carrier handoff.

## 4. Asterion Polar Edge Station — isolation and energy rationing

**Scenario premise:** A weather system delays resupply while a generator fault reduces the station's available energy margin.

### Initial stores and service state

| Resource | Quantity / state | Unit | Notes |
|---|---:|---|---|
| Potable water | 2,400 | liters | planned consumption 120 L/day |
| Food reserve | 36 | person-days | six crew, six days at baseline ration |
| Medical reserve | 1 | kit | routine station kit; no major trauma assumption |
| Generator A | available | unit | primary |
| Generator B | degraded | unit | intermittent output |
| Battery reserve | 78 | % | scenario state of charge |
| Heating loop | available | system | critical habitat service |
| Communications | intermittent | link | low-bandwidth window expected |
| Scientific payloads | 5 | instruments | 2 may be suspended |
| Repair spares | 3 | kits | one compatible with Generator B |

### Crew roster

| Role | Count | Coverage |
|---|---:|---|
| Station commander | 1 | authority, priorities, external coordination |
| Power / systems engineer | 1 | generation, distribution, repair |
| Habitat / life-support specialist | 1 | water, air, thermal systems |
| Medical and safety officer | 1 | health, risk, emergency readiness |
| Science operators | 2 | instruments, data quality, local analysis |

Crew members have secondary cross-training, but the scenario should track fatigue and avoid assuming one person can perform two simultaneous critical tasks.

### Daily schedule

| Local time | Activity |
|---|---|
| 06:00 | overnight systems review and crew condition check |
| 07:00 | power and life-support inspection |
| 08:00–12:00 | primary science / maintenance block |
| 12:00 | meal, water, and energy budget reconciliation |
| 13:00–17:00 | science, repair, and local data processing |
| 18:00 | external communications window if available |
| 20:00 | essential-load review and next-day plan |
| 22:00 | quiet period; rotating watch remains staffed |

### Incident timeline

| Elapsed time | Event | Expected state change |
|---|---|---|
| H+00:00 | Generator B output becomes unstable | power margin falls; fault is logged |
| H+00:20 | Weather forecast closes resupply window for 72 hours | autonomy horizon changes |
| H+00:35 | Commander suspends two nonessential instruments | load drops; science throughput falls |
| H+01:10 | Engineer begins isolated diagnosis | repair crew and spare inventory become committed |
| H+03:00 | Battery reserve reaches scenario threshold of 60% | next load-shedding decision is triggered |
| H+05:00 | Generator B stabilizes after a controlled repair | service remains under observation |
| H+24:00 | Crew reviews revised reserve horizon | ration, work, and science plan updated |

**Success measures:** habitat heat and life support remain stable; power priorities are explicit; reserve projections update from actual consumption; repair work respects isolation; science data are preserved locally.

**Failure paths:** nonessential loads continue unchecked; projected autonomy ignores weather delay; fatigue is omitted; a repaired generator is treated as fully reliable without observation.

## 5. Selene Archive One — contested provenance and delayed replicas

**Scenario premise:** A high-value scientific record arrives through a delayed communications window with a valid-looking signature but incomplete provenance.

### Initial archive state

| Collection / service | Quantity / state | Unit | Notes |
|---|---:|---|---|
| Authoritative local collections | 12,400 | records | validated local authority |
| Online working replicas | 3 | copies | current to last synchronization |
| Offline recovery replicas | 2 | copies | one recently integrity-checked |
| External exchange queues | 17 | packages | awaiting outbound or inbound validation |
| Quarantined submissions | 4 | packages | unresolved provenance |
| Integrity audit capacity | 240 | records/hour | scenario throughput |
| Media migration queue | 320 | records | due this cycle |
| Communications window | 2 | hours/day | delayed and variable |

### Custody roster

| Role | Count | Responsibility |
|---|---:|---|
| Archive authority | 1 | final approval and dispute resolution |
| Provenance reviewers | 2 | origin, signature chain, and source validation |
| Integrity operators | 2 | checks, replication, migration |
| Science domain custodians | 3 | assess meaning and scientific context |
| Systems engineer | 1 | local services and recovery media |

No single operator may both submit and approve a contested authoritative revision in this scenario.

### Daily schedule

| Window | Activity |
|---|---|
| 00:00–06:00 | local integrity checks and replica comparison |
| 06:00–08:00 | communications window and transfer staging |
| 08:00–12:00 | provenance review and scientific validation |
| 12:00–14:00 | approved ingest and controlled replication |
| 14:00–16:00 | media migration and recovery-copy verification |
| 16:00–18:00 | audit review, custody handoff, and incident report |
| Remaining time | low-priority work deferred; local preservation continues |

### Incident timeline

| Elapsed time | Event | Expected state change |
|---|---|---|
| H+00:00 | New package arrives with a valid signature but missing upstream custody record | submission remains untrusted / quarantined |
| H+00:15 | Sender requests immediate promotion due to a publication deadline | urgency is recorded but does not confer authority |
| H+01:00 | Domain custodian confirms scientific content is plausible | semantic plausibility does not resolve provenance |
| H+03:00 | Second source provides a matching digest but not the missing custody link | integrity confidence improves; provenance remains incomplete |
| H+08:00 | Communications window closes | external verification is deferred |
| H+24:00 | Archive authority orders preservation of both candidate versions | no destructive overwrite; dispute remains explicit |

**Success measures:** source package preserved; validation dimensions remain separate; approval is attributable; replicas do not silently promote an untrusted version; the audit trail records why a decision was deferred.

**Failure paths:** newest timestamp wins automatically; digest match is mistaken for provenance; quarantine is deleted to meet a deadline; replicas converge by overwriting the only trustworthy copy.

## 6. Scenario event and state contract

A scenario runner can represent events with a compact domain-neutral record:

| Field | Meaning |
|---|---|
| EventId | stable identifier within the scenario |
| OccurredAt | simulation time, not necessarily wall-clock time |
| Actor | person, institution, system, or environmental cause |
| Subject | stock, asset, service, location, or record affected |
| Action | receive, inspect, move, reserve, issue, consume, fail, repair, approve, replicate |
| Quantity | optional amount and unit |
| PriorState | expected state before the event |
| ResultState | state after the event |
| Authority | approval or custody evidence |
| Evidence | observation, ledger entry, sensor, witness, or signature |
| Consequence | downstream effects to evaluate |

This is a scenario vocabulary, not a prescribed DataWarehouse record layout or API.

### Invariants worth testing

1. A physical movement changes location or custody; it does not automatically imply acceptance or ownership transfer.
2. A reservation reduces freely allocatable stock without making the physical item disappear.
3. Quarantined or disputed material cannot be silently treated as available.
4. Consumption and spoilage reduce usable quantity and retain an auditable cause.
5. A replica update does not establish authority merely because it is newer.
6. Staff assignments cannot exceed presence, qualification, or simultaneous-work constraints.
7. Service restoration follows a verification event, not merely a repair-complete claim.
8. A correction preserves the discrepancy and its resolution history.

## 7. Building a live scenario from a pack

1. Select the facility and define the scenario's time horizon.
2. Copy the initial snapshot into scenario-owned state; never mutate the sourcebook's reference values.
3. Instantiate staff, shifts, and authority relationships.
4. Choose which dependencies are simulated explicitly and which remain abstract.
5. Schedule ordinary events before injecting incidents. A failure is meaningful only against a functioning baseline.
6. Record every state transition with its actor, evidence, and authority.
7. Evaluate both operational outcomes and information quality: what happened, what was known, and what was recorded may differ.
8. Reset from the original snapshot to compare alternate decisions.

## 8. Relationship to the DataWarehouse library

These packs define SURPS domain scenarios, not low-level storage implementation. They do not prescribe DataWarehouse allocation, page layout, record representation, persistence, concurrency, or API behavior. Scenario state may be implemented by a higher-level application or simulation system; the library remains governed by its own source, tests, and documented contracts.

<p align="center">
  <img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="420" />
</p>
