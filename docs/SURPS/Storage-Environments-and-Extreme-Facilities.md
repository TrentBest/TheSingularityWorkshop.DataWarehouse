# Storage Environments and Extreme Facilities

<p align="center">
  <a href="https://github.com/TrentBest/TheSingularityWorkshop.Visualize">
    <img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop" width="720" />
  </a>
</p>

**SURPS sourcebook · Place, environment, mobility, and survival**

## 1. Purpose

Storage systems are shaped by where they exist. This book examines facilities whose environment, isolation, mobility, or scale changes the normal assumptions of warehousing and information infrastructure.

A facility may be ordinary in purpose but extraordinary in location: a grain store in a floodplain, a server room in a desert, a seed vault in permafrost, or a supply depot aboard a ship. Conversely, an unusual environment may create entirely new forms of storage.

## 2. Terrestrial environments

### Underground and subterranean
Caves, mines, bunkers, buried silos, vaults, tunnels, and deep geological repositories can offer concealment, thermal stability, or protection from surface hazards. They also introduce groundwater, drainage, access, ventilation, structural, fire, and evacuation challenges.

### Underwater and coastal
Submerged caches, underwater research stations, seabed cable facilities, floating depots, ports, and coastal warehouses face pressure, corrosion, salt, storms, flooding, difficult access, and specialized maintenance. Water may be a resource, a transport route, a cooling medium, or a threat.

### Polar, alpine, and permafrost
Cold environments can reduce some forms of biological degradation but introduce freeze-thaw cycles, ice movement, extreme weather, isolation, and costly access. A cold location is not automatically a safe archive: materials and systems have their own temperature limits.

### Desert and arid
Dry environments may aid preservation for some materials, while dust, heat, large temperature swings, water scarcity, and remote logistics create other risks. Cooling and maintenance can become dominant constraints for electronic facilities.

### Tropical, wetland, and forest
High humidity, biological activity, flooding, corrosion, mold, pests, vegetation, and difficult transport shape construction and operations. Elevated structures, sealed containers, drainage, and frequent inspection may be central.

### Urban and industrial
Dense cities offer labor, customers, power, networks, transport, and services, but introduce land cost, congestion, shared infrastructure, regulation, noise, and concentrated failure exposure.

## 3. Mobile and expeditionary storage

- **Caravans and wagons:** portable inventories whose capacity, protection, and replenishment depend on route, animals, weather, and security.
- **Ships and submarines:** stores integrated into a vessel's mass, stability, power, crew, mission duration, and emergency procedures.
- **Aircraft and airborne platforms:** constrained by mass, volume, vibration, power, and limited maintenance access.
- **Mobile command and data units:** deployable compute, communications, and records systems supporting expeditions, disaster response, or military operations.
- **Nomadic and migratory systems:** distributed caches and portable knowledge carried with a moving community.
- **Autonomous robotic depots:** mobile or self-relocating stores that manage their own routes, replenishment, and maintenance.

Mobility creates a different inventory problem: the facility itself is in transit, and location, route, travel time, and mission state become part of the storage model.

## 4. Off-world facilities

### High orbit and orbital platforms
Orbital depots and data facilities must account for launch mass, radiation, thermal cycling, power generation, communication windows, debris risk, docking, and repair logistics.

### The Moon
Lunar stores may support habitats, science, construction, mobility, and industrial operations. Dust, vacuum, thermal extremes, radiation, transport, and the cost of replacement shape design.

### Mars and planetary surfaces
Long supply chains and communication delay favor local production, repair capability, reserve capacity, and autonomous operations. A depot may be a critical life-support asset rather than merely a convenience.

### Asteroids and small bodies
Resource extraction may be paired with processing stores, propellant depots, robotic workshops, and caches. Low gravity and irregular terrain change anchoring, containment, handling, and transport.

### Interplanetary and interstellar infrastructure
Relays, probes, generation ships, and distant settlements may preserve knowledge and supplies across long periods without resupply. Redundancy, autonomous repair, media longevity, and institutional continuity become existential concerns.

## 5. Environmental design questions

For any unusual facility, establish:
- ambient temperature and variation;
- pressure and atmosphere;
- humidity, water, dust, and corrosive exposure;
- gravity, vibration, radiation, and mechanical loading;
- available power, fuel, water, and cooling;
- communication and transport access;
- repair capability and replacement lead time;
- evacuation, isolation, and emergency shelter options;
- environmental impact and waste handling.

Use only the variables relevant to the setting. Avoid giving every facility an exhaustive engineering simulation when its environment has no meaningful effect on the scenario.

## 6. Isolation and autonomy

A remote facility may be unable to rely on immediate external support. It may need:
- local reserves and spare parts;
- autonomous monitoring and control;
- staff cross-training or robotic maintenance;
- independent power and environmental systems;
- local decision authority;
- robust records and delayed synchronization;
- procedures for extended communication loss;
- a defined safe state when resources run low.

Autonomy creates governance questions: who may change the mission, how authority is delegated, and what happens when the facility's instructions become obsolete?

## 7. Hazards and cascading failure

Environmental and operational hazards interact. A storm can cut transport, interrupt power, and prevent staff from reaching a site. A cooling failure can become a compute outage; an outage can disable logistics systems; the logistics failure can then prevent delivery of replacement parts.

Model the dependency chain that matters to the scenario rather than treating hazards as isolated random events.

## 8. LOD guidance

- **LOD 0:** the storage system exists in a broad environmental context.
- **LOD 1:** identify environment, isolation, mobility, major resource constraints, and dominant hazards.
- **LOD 2:** define site arrangement, environmental controls, supply routes, autonomy, emergency reserves, and maintenance strategy.
- **LOD 3+:** define structures, equipment, access routes, environmental zones, individual stores, crew, autonomous agents, emergency procedures, and specific failure chains.

## 9. Simulation hooks

Candidate state includes location, route, environmental conditions, structural integrity, power, cooling, supplies, communication delay, repair capacity, autonomy mode, and hazard exposure.

Candidate events include departure, arrival, resupply, isolation, environmental alarm, structural damage, route closure, communication loss, emergency rationing, autonomous intervention, and restoration.

These are sourcebook hooks, not implemented mechanics.
