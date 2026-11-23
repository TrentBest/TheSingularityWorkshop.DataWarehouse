# SURPS Facility Diagrams — Plans, Flows, and Dependencies

<p align="center">
  <a href="https://github.com/TrentBest/TheSingularityWorkshop.Visualize">
    <img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop" width="720" />
  </a>
</p>

**SURPS visual supplement · LOD 3–4 · Static, authored Mermaid diagrams**

> **Accuracy note:** These are conceptual diagrams for the constructed sites in the Facility Atlas. They are not measured architectural drawings, engineering plans, evacuation certifications, or claims about real historical facilities. Arrows indicate a modeled relationship or flow, not a guaranteed physical route.

## 1. Diagram conventions

- **Zones** are spaces with a distinct operational purpose.
- **Flows** show movement of goods, people, information, energy, or authority.
- **Dependencies** show services or conditions required for another function to operate.
- **Control points** are places where custody, authorization, inspection, or safety decisions occur.
- **Failure propagation** shows how a local fault can affect downstream functions.
- Solid arrows represent ordinary modeled flow; dashed arrows represent oversight, authorization, or information feedback.

Use separate diagrams for physical movement and authority. A single diagram that tries to show every relationship becomes unreadable and hides important distinctions.

## 2. Namar Granary — compound plan and stock flow

### Conceptual compound plan

The plan emphasizes separation between receiving and issue, distinct seed and food stocks, and public witness points.

```mermaid
flowchart TB
    RIVER["Canal landing"] --> RG["Receiving gate"]
    ROAD["Harvest road"] --> RG
    subgraph COMPOUND["Raised granary compound"]
      RG --> RC["Receiving court"]
      RC --> TP["Tally porch"]
      TP --> DY["Drying yard"]
      DY --> Q{"Quality accepted?"}
      Q -->|No| REJ["Reject / re-dry / investigate"]
      Q -->|Yes| SPLIT{"Assign stock purpose"}
      SPLIT --> SEED["Seed store"]
      SPLIT --> FOOD["Food-grain bins"]
      SPLIT --> INST["Institutional reserve"]
      SEED --> AUD["Steward + scribe reconciliation"]
      FOOD --> AUD
      INST --> AUD
      AUD --> IC["Issue court"]
      IC --> HH["Household allotments"]
      IC --> ER["Emergency ration issue"]
      WR["Watch room"] -. "gate and seal custody" .-> RG
      WR -. "witnessed access" .-> IC
    end
```

### Intake, storage, and issue

```mermaid
flowchart LR
    A["Harvest delivery"] --> B["Sample and measure"]
    B --> C{"Acceptable quality?"}
    C -->|No| D["Reject, dry, or downgrade"]
    C -->|Yes| E["Record origin, crop, quantity, quality"]
    E --> F["Assign bin and intended use"]
    F --> G["Store and inspect"]
    G --> H["Authorize issue"]
    H --> I["Measure and witness release"]
    I --> J["Update physical and written balances"]
    J --> K{"Reconciliation agrees?"}
    K -->|Yes| L["Close transaction"]
    K -->|No| M["Hold affected bin and investigate"]
```

**Scenario levers:** drying-yard capacity, rain exposure, accepted yield, pest loss, seed reserve threshold, issue rate, and ledger discrepancy.

## 3. Deep Ledger of Khar-Dûm — vertical districts and life-support dependencies

This cross-section treats the vault as an inhabited infrastructure system. The deeper chambers are not automatically safer: they may be harder to ventilate, drain, inspect, or reach.

```mermaid
flowchart TB
    SURF["Surface receiving and defense"] --> UPPER["Upper receiving hall"]
    UPPER --> CIVIC["Civic stores"]
    UPPER --> FORGE["Forge reserve"]
    UPPER --> RECORDS["Record vault"]
    CIVIC --> REFUGE["Refuge galleries"]
    CISTERN["Separated cistern galleries"] --> CIVIC
    CISTERN --> REFUGE
    AIR["Ventilation shafts and fans"] --> CIVIC
    AIR --> FORGE
    AIR --> RECORDS
    DRAIN["Sumps and drainage galleries"] --> CISTERN
    DRAIN --> DEEP["Deep Seal district"]
    FORGE --> WORKS["Workshops and repair crews"]
    RECORDS --> COUNCIL["Council and guild decisions"]
    DEEP --> QUORUM["Multi-custodian access"]
    classDef critical stroke-width:3px
    class CISTERN,AIR,DRAIN critical
```

### Dependency and failure propagation

```mermaid
flowchart LR
    LEAK["Groundwater intrusion"] --> DRAINLOAD["Drainage load rises"]
    DRAINLOAD --> ACCESS["Gallery access restricted"]
    ACCESS --> PARTS["Machine-part reserve delayed"]
    PARTS --> REPAIR["Repair capacity falls"]
    REPAIR --> VENT["Ventilation maintenance delayed"]
    VENT --> AIR["Air quality margin falls"]
    AIR --> REFUGE["Refuge capacity reduced"]
    LEAK --> SEED["Seed and culture vault threatened"]
    SEED --> FOOD["Future food resilience falls"]
```

**Scenario levers:** water ingress, sump capacity, air quality, gallery access, reserve days, repair labor, and authority to open restricted passages.

## 4. North Quay Cold-Chain Exchange — cargo path and thermal control

### Cargo path

```mermaid
flowchart LR
    SHIP["Ship / rail arrival"] --> CHECK["Manifest and temperature check"]
    CHECK --> TRIAGE{"Cargo condition"}
    TRIAGE -->|Suspect| HOLD["Quarantine bay"]
    TRIAGE -->|Accepted| CLASS{"Required range"}
    CLASS -->|Chilled| CHILL["Chilled rooms"]
    CLASS -->|Frozen| FREEZE["Frozen rooms"]
    CLASS -->|Ambient| AMB["Ambient staging"]
    HOLD --> DECIDE["Inspect, release, return, or dispose"]
    CHILL --> PACK["Grade and pack"]
    FREEZE --> PACK
    AMB --> PACK
    PACK --> DISPATCH["Rail / road dispatch"]
```

### Thermal service dependency

```mermaid
flowchart TB
    GRID["Utility power"] --> MAIN["Primary refrigeration"]
    FUEL["Fuel reserve"] --> BACKUP["Backup generator"]
    BACKUP --> AUX["Emergency refrigeration"]
    MAIN --> ZONES["Cold zones"]
    AUX --> ZONES
    DOORS["Door cycles and dwell time"] --> LOAD["Thermal load"]
    LOAD --> MAIN
    LOAD --> AUX
    SENSORS["Temperature sensors"] --> ALARM["Alarm and operator response"]
    ZONES --> SENSORS
    ALARM --> PRIORITY["Prioritize cargo zones"]
    PRIORITY --> ZONES
```

**Scenario levers:** compressor availability, backup coverage, door-open duration, temperature excursion time, cargo sensitivity, and dispatch delay.

## 5. Meridian Fulfillment Campus — material flow and exception handling

```mermaid
flowchart LR
    YARD["Inbound yard"] --> IN["Unload and identify"]
    IN --> QC{"Identity and condition valid?"}
    QC -->|No| EX["Exception mezzanine"]
    QC -->|Yes| PUT["Put-away decision"]
    PUT --> RES["Reserve pallet storage"]
    PUT --> ASRS["Automated container storage"]
    RES --> PICK["Replenish / pick"]
    ASRS --> GTP["Goods-to-person"]
    EX --> MAN["Manual inspection and correction"]
    MAN --> PUT
    PICK --> CONS["Order consolidation"]
    GTP --> CONS
    CONS --> PACK["Pack and label"]
    PACK --> SORT["Sort by carrier and window"]
    SORT --> DOCK["Outbound staging and docks"]
    DOCK --> CARRIER["Carrier departure"]
```

### Failure isolation

```mermaid
flowchart TB
    ROUTE["Routing service"] --> LOOP["Conveyor loop"]
    LOOP --> DIV["Downstream diverter"]
    DIV --> OUT["Outbound sortation"]
    DIV -->|Offline| RECIRC["Recirculation queue grows"]
    RECIRC --> CAP["Loop capacity exhausted"]
    CAP --> STOP["Safe stop and isolate segment"]
    STOP --> MANUAL["Reduced manual dispatch"]
    STOP --> MAINT["Maintenance clears fault"]
    MAINT --> TEST["Controlled restart test"]
    TEST --> LOOP
```

**Scenario levers:** order arrival rate, conveyor utilization, exception frequency, diverter availability, manual dispatch capacity, staffing, and carrier cut-off times.

## 6. Asterion Polar Edge Station — autonomy and essential-load hierarchy

```mermaid
flowchart TB
    POWER["Generation and stored energy"] --> BUS["Station power bus"]
    BUS --> LIFE["Life support and habitat heat"]
    BUS --> COMMS["Communications"]
    BUS --> DATA["Local compute and storage"]
    BUS --> SCI["Scientific instruments"]
    BUS --> SHOP["Workshop and nonessential loads"]
    WEATHER["Weather and access window"] --> RESUP["Resupply / evacuation"]
    RESUP --> FOOD["Food and medical reserve"]
    RESUP --> FUEL["Fuel and spare parts"]
    FUEL --> POWER
    DATA --> LOCAL["Local analysis and data preservation"]
    SCI --> LOCAL
    LOCAL --> COMMS
    CREW["Crew capacity and fatigue"] --> SHOP
    CREW --> SCI
    CREW --> LIFE
    classDef essential stroke-width:3px
    class LIFE,POWER,BUS essential
```

**Operating rule:** when energy margin falls, the station sheds loads according to an explicit priority policy. Life support and safe habitat conditions outrank scientific throughput. A communication outage does not imply local data loss if local storage and power remain healthy.

**Scenario levers:** energy reserve, thermal margin, crew fatigue, instrument calibration, weather window, communication latency, and days of autonomy.

## 7. Selene Archive One — provenance and replica authority

```mermaid
flowchart TB
    SOURCE["Originating authority"] --> SUBMIT["Proposed record or revision"]
    SUBMIT --> VALIDATE["Signature, provenance, and integrity checks"]
    VALIDATE --> REVIEW{"Required authority approves?"}
    REVIEW -->|No| QUAR["Quarantine / preserve as untrusted"]
    REVIEW -->|Yes| LOCAL["Validated local authoritative copy"]
    LOCAL --> ONLINE["Online working replica"]
    LOCAL --> OFFLINE["Offline recovery replica"]
    LOCAL --> EXT["External exchanged replica"]
    ONLINE --> AUDIT["Audit and reconciliation"]
    OFFLINE --> AUDIT
    EXT -->|Delayed or conflicting update| CONFLICT["Version and authority review"]
    CONFLICT --> VALIDATE
    AUDIT --> MIGRATE["Scheduled media migration"]
    MIGRATE --> OFFLINE
```

A later timestamp is not sufficient authority. The scenario must distinguish recency, integrity, provenance, approval, and local operational validation.

**Scenario levers:** replica lag, signature-chain completeness, approval quorum, local validation state, media integrity, and communication delay.

## 8. Reusable diagram checklist

Before publishing a facility diagram, check:

- **Purpose:** does the diagram answer one clear question?
- **Boundary:** is the facility boundary explicit?
- **Direction:** are flow arrows directional and meaningful?
- **Custody:** does it show who accepts, authorizes, moves, or releases an asset?
- **Dependencies:** are power, water, climate, transport, staffing, and information dependencies visible where relevant?
- **Failure:** can a reader see at least one plausible fault path?
- **LOD:** is detail appropriate to the selected level?
- **Evidence:** are constructed values labeled, and are historical claims sourced separately?
- **Separation:** does the diagram describe the SURPS domain rather than imply an implemented DataWarehouse API?

## 9. Relationship to DataWarehouse implementation

These are domain diagrams for SURPS settings. They do not specify the .NET library's allocator, page layout, record representation, concurrency guarantees, persistence format, or API. The software repository's source and tests remain authoritative for those behaviors.

<p align="center">
  <img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="420" />
</p>
