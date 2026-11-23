# SURPS Scenario Event Catalog — Actions, Evidence, and State Change

<p align="center">
  <a href="https://github.com/TrentBest/TheSingularityWorkshop.Visualize">
    <img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop" width="720" />
  </a>
</p>

> **Purpose:** A reusable vocabulary for describing what happens inside warehouses, archives, data centers, logistics networks, and extreme facilities during a SURPS scenario.

This catalog defines domain events for world-building and simulation design. It is a conceptual vocabulary, not a software event schema or a requirement to implement event sourcing.

## 1. Event, state, and evidence

An **event** is an attributable occurrence: something an actor or process attempted or completed at a point in simulation time. A **state** is the condition of a subject at a particular observation point. An **evidence item** is what supports the claim that an event occurred or that a resulting state is valid.

Keep these distinct:

- An event records an occurrence; a state snapshot describes a condition.
- An attempted action is not automatically a successful transition.
- A command, permission, or schedule entry is not proof of completion.
- A later correction should preserve the fact that an earlier record or observation existed.
- Time is scenario time. A setting may use local clocks, calendars, delayed communications, or uncertain chronology.

### 1.1 Common event description

For scenario writing, describe an event with the following fields when they matter. These are authoring prompts, not a prescribed record layout.

| Field | Meaning |
|---|---|
| Event identity | A stable name or scenario-local identifier |
| Event time | When the occurrence is said to have happened |
| Actor | Person, institution, machine, organism, or environmental process responsible |
| Subject | Item, lot, record, facility, service, route, or relationship affected |
| Action | The event family and specific operation |
| Quantity / scope | Amount or extent affected, with units and uncertainty |
| Preconditions | Conditions that must hold before the transition is valid |
| Prior condition | Relevant state immediately before the event |
| Result | Success, partial success, refusal, failure, or unresolved outcome |
| Authority | Permission, delegation, policy, or customary legitimacy relied upon |
| Evidence | Witness, seal, instrument reading, receipt, checksum, sensor, or other support |
| Consequence | Follow-on effects, obligations, risks, or unresolved questions |

A scenario may omit irrelevant fields, but should not silently invent missing evidence or authority.

## 2. Event families

### 2.1 Arrival and intake

| Event | Preconditions and validation | Possible state effects |
|---|---|---|
| Arrive | A subject reaches a recognized boundary or receiving point; route and time may be uncertain | Location becomes observed at the receiving boundary; custody may remain with carrier |
| Receive | An authorized recipient accepts physical or informational custody | Custody changes; quantity or collection membership may be recorded |
| Reject receipt | Acceptance criteria fail or recipient lacks authority/capacity | No accepted stock increase; subject may remain with carrier or enter dispute |
| Register | Identity and relevant attributes are recorded with stated confidence | A discoverable description or inventory claim is created |
| Weigh / measure | Instrument or method is available and its limits are understood | Measurement evidence is added; measured quantity may differ from declared quantity |
| Inspect | An inspection method and scope are identified | Condition, contamination, integrity, or compliance assessment is recorded |
| Sample | Sampling authority and method are valid | A sample is separated; chain of custody and representativeness become relevant |

**Important distinction:** arrival, receipt, registration, and acceptance are separate claims. A wagon at the gate is not accepted inventory; a file copied into a cache is not necessarily an authoritative archive record.

### 2.2 Classification and placement

| Event | Preconditions and validation | Possible state effects |
|---|---|---|
| Classify | Sufficient identifying evidence or an explicit provisional label | Category, handling rule, or uncertainty is attached |
| Grade | A declared standard and assessment method exist | Quality grade or confidence changes |
| Quarantine | A hazard, dispute, integrity concern, or policy trigger is recorded | Subject becomes restricted; ordinary allocation is blocked |
| Put away | A suitable destination exists and access/handling rules are satisfied | Recorded location changes; movement path may be logged |
| Relocate | Destination and authority are valid; movement is feasible | Location changes while identity and custody may remain stable |
| Consolidate | Subjects are compatible and their identities remain traceable | Lots or containers are grouped without erasing constituent identity |
| Split | A parent quantity or collection is known and allocation is accountable | Child subjects are created; total quantity and provenance reconcile |

Classification is not a physical move. A label change does not itself relocate, consume, transfer ownership, or validate an item.

### 2.3 Reservation, allocation, and release

| Event | Preconditions and validation | Possible state effects |
|---|---|---|
| Reserve | Subject is eligible and sufficient uncommitted quantity/capacity exists | Available quantity decreases; physical presence does not |
| Amend reservation | Existing reservation is identified and change is authorized | Reserved quantity, beneficiary, priority, or expiry changes |
| Release reservation | Reservation is identified and release is authorized or expiry rules apply | Quantity returns to allocatable pool if no other restriction applies |
| Allocate | A valid demand and allocation authority exist | A specific subject/quantity is committed to a purpose |
| Cancel allocation | Allocation is identified and cancellation is permitted | Commitment is reversed; physical stock remains unless separately moved |
| Expire | A defined time or condition is reached | Reservation or authorization may lapse; consequence follows stated policy |

Reservation is a claim on availability, not disappearance of the physical subject. Quarantined, disputed, spoiled, or otherwise ineligible material does not become available merely because a reservation is released.

### 2.4 Transfer, issue, and use

| Event | Preconditions and validation | Possible state effects |
|---|---|---|
| Dispatch | Release authority, destination, and transport arrangement are valid | Subject leaves controlled stock; custody may pass to carrier |
| Transfer custody | Sending and receiving parties or a defined handoff protocol acknowledge transfer | Custodian changes; ownership may remain unchanged |
| Transfer ownership | A recognized authority or agreement supports the change | Ownership claim changes; custody and location may not |
| Deliver | Subject reaches destination and delivery evidence is recorded | Destination receipt process may begin |
| Issue | Authorized stock is handed to a user or department | Stock leaves general inventory; recipient and purpose are recorded |
| Consume | A subject is used, expended, or irreversibly incorporated | Usable quantity decreases; cause and beneficiary are recorded |
| Return | Previously issued subject is returned and assessed | It may re-enter stock, quarantine, repair, or disposal |
| Abandon | Responsible party relinquishes control under stated circumstances | Custody becomes uncertain or transfers according to local rules |

Movement, custody, ownership, and consumption are independent dimensions. Do not infer one from another.

### 2.5 Condition, degradation, and loss

| Event | Preconditions and validation | Possible state effects |
|---|---|---|
| Preserve | A treatment or environmental control is applied | Deterioration risk may decrease; treatment evidence is recorded |
| Maintain | A defined maintenance action is completed | Condition or serviceability may improve |
| Degrade | Time, environment, wear, or damage reduces condition | Quality, capacity, or service level decreases |
| Spoil | A threshold or assessment makes material unusable for its intended purpose | Usable quantity decreases or is reclassified |
| Contaminate | A contaminant or exposure pathway is identified or suspected | Affected scope may be restricted pending assessment |
| Damage | Physical or informational integrity is harmed | Repair, quarantine, replacement, or loss may follow |
| Lose | A subject cannot be located or accounted for after a defined search/threshold | Recorded availability decreases; cause may remain unknown |
| Steal | Unauthorized taking is evidenced or credibly alleged | Custody is violated; ownership may be disputed rather than automatically changed |
| Dispose | Authorized destruction, discard, or release from controlled retention occurs | Subject leaves active holdings; required evidence and downstream effects remain |

A report of loss is not proof of theft. A suspected contamination is not a measured contamination. Preserve the distinction between observation, inference, and confirmed cause.

### 2.6 Audit, discrepancy, and governance

| Event | Preconditions and validation | Possible state effects |
|---|---|---|
| Count / inventory | Scope, method, and observation time are defined | Observed quantity or presence is recorded |
| Reconcile | Recorded and observed states are compared | Discrepancy is opened, explained, corrected, or left unresolved |
| Correct record | A correction authority and supporting evidence exist | Current assertion changes while prior assertion remains part of history |
| Approve | An authorized actor accepts a proposed action or state | Permission is granted within its stated scope and validity |
| Deny | A request is refused by an authorized actor or rule | Requested transition does not occur; reason may be recorded |
| Appeal | A recognized review path exists | Decision enters review; original decision remains visible |
| Attest | A witness or authority makes a bounded claim | Evidence strength changes; attestation does not prove beyond its scope |
| Revoke authority | Delegation, credential, or mandate is withdrawn | Future actions may be blocked; prior valid actions are not retroactively erased |

A reconciliation should explain a discrepancy rather than silently overwrite it. Corrections preserve the history of what was previously believed and why it changed.

### 2.7 Service and infrastructure

| Event | Preconditions and validation | Possible state effects |
|---|---|---|
| Start service | Required dependencies and authorization are present | Service becomes available after stated checks |
| Outage | A service or dependency becomes unavailable | Capability or throughput falls; dependent operations may queue or fail |
| Degrade service | Capacity, quality, or coverage falls below normal | Reduced service level is recorded without implying total failure |
| Isolate | A component or zone is separated to contain risk | Connectivity or access is restricted; other zones may remain operational |
| Repair | A fault is acted upon by qualified personnel or process | Physical fault may be corrected; service is not yet necessarily restored |
| Restore | Service is re-enabled after repair or failover | Availability resumes, potentially at reduced capability |
| Verify recovery | Defined tests or observations support safe/valid operation | Recovery claim gains evidence; outstanding limitations remain visible |
| Fail over | A backup system assumes some defined function | Authority, freshness, capacity, and split-brain risks must be considered |
| Resupply | Fuel, power, consumables, parts, or staff reach the facility | Dependency reserves may increase after acceptance |

Repair complete is not equivalent to service restored; service restored is not equivalent to recovery verified.

### 2.8 Information, provenance, and media lifecycle

| Event | Preconditions and validation | Possible state effects |
|---|---|---|
| Capture | A source and capture method are identified | New information object or observation is created |
| Ingest | Intake rules and destination are satisfied | Information enters a managed collection; source remains identifiable |
| Replicate | Source and target relationship is declared | A copy is created; authority and freshness remain separate questions |
| Validate provenance | Origin, custody, transformations, and evidence are assessed | Confidence or trust assessment changes |
| Validate integrity | A suitable comparison or verification method is applied | Integrity status is recorded for a defined scope and time |
| Transform | A representation is converted or derived | New version/derivative is created; lineage points to inputs and method |
| Migrate media | Information is moved to a different physical or technological medium | Preservation risk changes; fidelity and completeness require validation |
| Supersede | A newer version is designated for a purpose | Preferred version changes; prior versions need not be destroyed |
| Recover | A lost, damaged, or inaccessible information state is reconstructed | Recovered state is marked with evidence, gaps, and confidence |
| Retire / erase | Retention authority and scope are established | Access or retention changes; legal, cultural, and evidentiary obligations may remain |

A newer replica is not automatically authoritative. A checksum can support integrity for a particular comparison, but does not establish authorship, truth, legitimacy, or provenance.

## 3. Transition discipline

For any event that changes state, answer these questions:

1. **What is the subject?** Identify the item, quantity, service, record, facility, or relationship affected.
2. **What was true before?** Record the relevant prior condition and its evidence.
3. **Who or what acted?** Distinguish human decision, delegated process, machine action, and environmental cause.
4. **Was the action permitted?** Identify authority and its scope; do not assume physical ability grants legitimacy.
5. **What changed?** Separate location, quantity, condition, custody, ownership, access, authority, and information status.
6. **What did not change?** Explicitly preserve dimensions that the event does not affect.
7. **How is the result known?** Record evidence, uncertainty, and observation delay.
8. **What follows?** Identify obligations, dependent events, queues, risks, and unresolved discrepancies.

### 3.1 Reversibility and compensation

Some events can be compensated; their history cannot be made not to have happened.

- A reservation can be released; the reservation event remains part of the scenario history.
- A mistaken classification can be corrected; the earlier classification and its consequences remain explainable.
- A custody transfer can be followed by a return transfer; the first handoff is not erased.
- A consumed resource cannot normally be restored by changing a ledger entry. A replacement delivery is a new event.
- A corrupted record may be recovered from a replica; recovery does not prove the recovered copy was authoritative.
- A failed approval can be appealed or superseded; the original decision remains attributable.

Use **compensation** for a new action that counteracts an earlier effect. Use **correction** for an inaccurate assertion. Use **reversal** only when the domain genuinely permits the original state transition to be undone.

### 3.2 Duplicate and delayed observations

A repeated report may describe one occurrence, multiple occurrences, or a retransmission. The scenario author should state how the world distinguishes them: receipt number, seal, witness, physical count, source identity, time window, or another domain-appropriate clue.

Do not assume that two similar observations are duplicate events. Nor should a delayed message be treated as a new physical movement simply because it arrived later. Record event time and report/observation time separately when the difference matters.

## 4. Cross-domain event sequences

These examples connect to the existing facility scenario packs. Names and outcomes are illustrative.

### 4.1 Namar Granary — damp grain and disputed delivery

1. A cart **arrives** at the receiving court.
2. The storehouse **inspects** the seal and **measures** the delivered sacks.
3. The receiving steward **rejects** a disputed portion while **receiving** the accepted portion.
4. The accepted sacks are **registered**, **classified**, and **put away** in a dry bay.
5. A later inspection **detects degradation** in a separate bay and **quarantines** the affected lot.
6. The inventory is **counted** and **reconciled** against the expected reserve.
7. A council authority **approves** a ration allocation from eligible stock.
8. The storehouse **issues** grain; household representatives acknowledge receipt.
9. A correction explains the original delivery discrepancy without erasing the first count.

### 4.2 Meridian Fulfillment Campus — diverter failure at carrier cutoff

1. Orders are **allocated** against eligible stock and **reserved** for dispatch.
2. Pickers **retrieve** and **consolidate** the required items.
3. A conveyor diverter **fails**, causing a service **outage** in one zone.
4. Supervisors **isolate** the affected line and **degrade service** to manual throughput.
5. A technician **repairs** the diverter; a test event **verifies recovery**.
6. Unaffected orders **dispatch** on schedule; delayed orders are **replanned** and their reservations amended.
7. Carrier **handoff** evidence records which parcels actually left the campus.
8. The shift **reconciles** queued, dispatched, cancelled, and still-reserved orders.

### 4.3 Asterion Polar Edge Station — resupply delay and energy rationing

1. A scheduled resupply fails to **arrive**; the missed delivery becomes an explicit incident.
2. A station inventory **count** establishes observed food, water, fuel, and battery reserves.
3. Command authority **approves** a reduced operating schedule and **allocates** essential reserves.
4. Nonessential compute services **degrade** or **shut down**; essential environmental monitoring remains active.
5. A generator fault causes an **outage**; operators **isolate** the fault and use stored energy.
6. Replacement parts later **arrive**, are **inspected**, and are **received** into custody.
7. Qualified staff **repair** the generator and **verify recovery** under a defined load.
8. The station **reconciles** actual consumption against the ration plan and updates its next resupply request.

### 4.4 Selene Archive One — contested source and delayed replication

1. A remote collection **ingests** a newly received record and captures its source claims.
2. A replica **transfers** across a delayed communications link.
3. The receiving archive **validates integrity**, but provenance remains unresolved.
4. A curator **quarantines** the record from authoritative publication pending review.
5. A second source arrives; archivists **validate provenance** and compare the conflicting claims.
6. An authorized council **approves** a bounded authority designation for one version.
7. The archive **supersedes** its preferred public reference while preserving both versions and the dispute history.
8. A later **reconciliation** documents which claims were confirmed, rejected, or left uncertain.

## 5. Invariants and anti-patterns

Use these as review questions when authoring scenarios:

- Physical movement does not by itself transfer ownership.
- Receipt does not imply inspection, acceptance, or quality approval.
- Reservation changes availability, not physical presence.
- Quarantine or dispute cannot silently become ordinary available stock.
- Consumption, spoilage, and disposal require an attributable cause or explicitly unknown cause.
- A count is an observation, not an automatic correction of the record.
- A correction preserves the prior assertion and the reason for change.
- A newer replica does not automatically establish authority.
- Integrity evidence does not establish truth or provenance.
- Repair completion does not prove service restoration; restoration does not prove verified recovery.
- A delayed report does not necessarily indicate a delayed event.
- A missing record does not prove that an event did not occur.

Avoid these anti-patterns:

- **Magic ledger movement:** stock changes without a physical, informational, or explicitly abstract cause.
- **Custody/ownership collapse:** one field or event silently represents both.
- **Instant perfect knowledge:** remote actors know an event before communication or observation could reach them.
- **Silent retcon:** discrepancies vanish when a record is edited.
- **Replica equals truth:** newest or most numerous copies automatically win.
- **Repair equals recovery:** a component is declared operational without a verification step.
- **Unbounded event spam:** every animation frame becomes a domain event even when no meaningful state or evidence changes.

## 6. Failure chains and measurable outcomes

A single event often creates a chain. Model the links rather than jumping directly from cause to final outcome.

| Initiating event | Intermediate effects | Possible outcomes / measures |
|---|---|---|
| Flooding | Access restriction, wet stock, power fault, inspection backlog | Usable stock lost, recovery time, affected zones, claims resolved |
| Missed resupply | Reserve drawdown, rationing, service prioritization | Days of reserve, unmet demand, mission capability |
| Identity fraud | Misregistration, unauthorized allocation, custody dispute | Affected subjects, detection delay, restitution, trust |
| Cooling failure | Temperature excursion, quarantine, accelerated degradation | Exposure duration, affected quantity, verified safe capacity |
| Corrupt replication | Divergent versions, uncertain provenance, publication hold | Recovery point, unresolved conflicts, authoritative source confidence |
| Staff shortage | Queue growth, missed checks, unsafe work concentration | Backlog, qualification coverage, time-to-service, deferred maintenance |
| Power instability | Failover, reduced compute, delayed processing | Essential load served, service level, data loss window |
| Theft or unexplained loss | Count discrepancy, investigation, access review | Unaccounted quantity, evidence quality, custody gaps |

Useful measures include:

- **Availability:** eligible quantity or service capacity divided by the relevant total.
- **Throughput:** accepted, processed, or dispatched quantity per scenario time unit.
- **Backlog:** work awaiting a valid next action.
- **Dwell time:** elapsed time between arrival and departure or next stage.
- **Loss rate:** quantity or value lost over a stated period and denominator.
- **Recovery time:** time from disruption to a defined restored-and-verified service level.
- **Reserve endurance:** estimated time until a critical reserve reaches a stated threshold under a stated demand model.
- **Provenance confidence:** an explicitly qualitative or quantitative assessment tied to evidence and scope.
- **Discrepancy age:** time between first observation of a mismatch and its resolution or formal acceptance as unresolved.

Every metric needs a scope, unit, time window, and interpretation. A ratio without a denominator or a recovery time without a target service level is not a complete measure.

## 7. Applying the catalog at different LODs

| LOD | Event detail |
|---|---|
| LOD 0 | Mention broad change: a reserve is replenished, an archive is compromised, or a station loses power |
| LOD 1 | Identify event family, actor, affected subject, and major consequence |
| LOD 2 | Add preconditions, authority, quantity/scope, and relevant evidence |
| LOD 3 | Add sequence, locations, roles, schedules, dependencies, and incident branches |
| LOD 4+ | Add contested evidence, partial completion, uncertainty, compensation, hidden causes, and measurable outcomes |

Do not force every event into maximum detail. Increase fidelity where it changes decisions, consequences, or the experience being simulated.

## 8. Boundary with DataWarehouse implementation

This catalog is SURPS domain vocabulary. It does **not** prescribe:

- a DataWarehouse record or event schema;
- a shared allocator, page model, slot layout, or memory representation;
- persistence, event sourcing, serialization, replication, or recovery implementation;
- concurrency, transaction, ordering, or idempotency guarantees in a software library;
- a public API, dependency, benchmark, or runtime feature.

The DataWarehouse library remains a low-level in-process storage component. These event concepts may inform future applications or simulations, but no implementation claim follows from their presence in this sourcebook.

## Related sourcebooks

- [SURPS Sourcebooks](README.md)
- [The Warehouse Across Time and Space](The-Warehouse-Across-Time-and-Space.md)
- [The Data Center Across Time and Space](The-Data-Center-Across-Time-and-Space.md)
- [Facility Atlas — Named Sites Across Time and Space](Facility-Atlas-Named-Sites-Across-Time-and-Space.md)
- [Facility Scenario Packs — Inventories, Rosters, Schedules, and Incidents](Facility-Scenario-Packs-Inventories-Rosters-Schedules-and-Incidents.md)

<p align="center">
  <img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="420" />
</p>
