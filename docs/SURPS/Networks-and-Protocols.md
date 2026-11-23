# Networks and Protocols — Connected Systems and Information Flow

<p align="center">
  <a href="https://github.com/TrentBest/TheSingularityWorkshop.Visualize">
    <img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop" width="720" />
  </a>
</p>

> **SURPS sourcebook:** networking concepts, topology, protocol roles, service dependencies, and fictional network simulation.

## Scope
Networks connect devices and services through communication media and shared conventions. This book teaches conceptual architecture and failure behavior, not instructions for attacking, intercepting, scanning, or disrupting real networks.

## Network model
A network scenario may contain endpoints, switches, routers, wireless access points, gateways, name services, firewalls, links, and application services. Topology describes connections; policy determines permitted flows. Diagrams should distinguish physical links, logical relationships, trust boundaries, and dependencies.

## Layered communication
Layered models separate concerns: physical signaling, link delivery, network addressing and forwarding, transport behavior, and application meaning. OSI and TCP/IP are conceptual frameworks; real implementations do not always map perfectly to textbook layers.

## Addressing and naming
Explain local identifiers, network addresses, ports as service endpoints, naming systems such as DNS, and the difference between a name and the destination it resolves to. Use fictional address spaces and names in games. Do not expose real-world targets or require live discovery.

## Transport and application behavior
TCP provides a reliable ordered byte stream with connection management; UDP provides datagrams without the same built-in delivery guarantees. Application protocols define requests, responses, formats, and semantics. Teach with diagrams and toy messages rather than operational attack payloads.

## Performance and failure
Latency, jitter, bandwidth, packet loss, congestion, queueing, routing changes, link failure, name-resolution failure, and service dependency outages affect user experience. A service can be healthy while its path is impaired, or a network can be healthy while an application fails.

## Security and simulation
Discuss segmentation, secure configuration, encryption concepts, identity, authorization, certificate trust, monitoring, and defense in depth. A fictional topology declares which actors can see which links and services. Interception resolves only at scenario-defined observation points using abstract protection attributes.

Entities include endpoint, interface, link, network, route, name, service, protocol, trust zone, policy, queue, and dependency. State includes link health, capacity, latency, loss, congestion, reachability, policy, and service health. Events include link degraded, route changed, name unavailable, service unreachable, policy denied, queue saturated, and recovery verified.

Player verbs such as map, probe, intercept, defend, and trace operate only on the fictional scenario graph. The game returns authored or generated scenario information; it must not send packets, resolve real targets, run scans, or create external traffic.

## LOD
LOD 0: connected/disconnected. LOD 1: major zones and services. LOD 2: topology, routes, dependencies, and policies. LOD 3: fictional traffic events, schedules, logs, and response roles. LOD 4+: educational packet-flow visualization using synthetic examples only.


<p align="center">
  <img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="420" />
</p>
