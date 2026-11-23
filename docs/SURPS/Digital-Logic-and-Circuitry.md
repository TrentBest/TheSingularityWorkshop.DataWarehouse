# Digital Logic and Circuitry — From Switching Devices to State Machines

<p align="center">
  <a href="https://github.com/TrentBest/TheSingularityWorkshop.Visualize">
    <img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop" width="720" />
  </a>
</p>

> **Sourcebook:** hardware and logic foundations for the connected-world matrix.

This book connects physical electronic devices to the digital behavior of computers. It begins with signals and switching, develops Boolean logic and circuit composition, then reaches clocks, memory, state machines, and the interfaces that make larger systems possible.

It is an educational domain sourcebook, not a board-design certification, electrical safety manual, or claim that the current DataWarehouse library implements hardware simulation.

## 1. The physical-to-digital bridge

Digital systems use distinguishable signal ranges to represent discrete states. Binary notation is common because two-state systems can be engineered with useful noise margins, but real electrical signals are continuous physical quantities. A logic level is an interpretation of a voltage range under a device's electrical specifications—not a magical perfect zero or one.

Introduce voltage, current, resistance, power, reference/ground, input and output, loading, noise, timing, and signal integrity at an appropriate level. Explain that measurement instruments and component datasheets matter in real circuits; simulation examples should clearly identify their idealizations.

## 2. Components and switching

| Component family | Role in the conceptual model | Important behavior |
|---|---|---|
| Resistor | Limits or sets current/voltage relationships | Resistance, power dissipation, tolerance |
| Capacitor | Stores energy in an electric field; affects timing and filtering | Capacitance, charge/discharge, frequency response |
| Inductor | Stores energy in a magnetic field; affects current change | Inductance, transient response, frequency behavior |
| Diode | Direction-dependent conduction | Forward behavior, reverse behavior, ratings |
| Transistor | Controlled electronic switching or amplification | Control input, operating regions, switching limits |
| Integrated circuit | Packages interconnected electronic functions | Supply, pins, timing, logic family, limits |
| Sensor / actuator | Bridges physical environment and electronic system | Measurement range, response, calibration, drive requirements |

A transistor can act as a switch. Switching devices can be composed into gates. Gates can be composed into arithmetic, control, storage, and interface circuits. This hierarchy is the key conceptual ladder from device physics to computation.

## 3. Binary representation and Boolean algebra

Teach bits, binary numbers, place value, unsigned representation, signed representation at a conceptual level, and the distinction between a value and its encoding. Introduce Boolean variables and operators: NOT, AND, OR, XOR, NAND, and NOR.

Truth tables should make behavior inspectable. For example:

| A | B | AND | OR | XOR |
|---:|---:|---:|---:|---:|
| 0 | 0 | 0 | 0 | 0 |
| 0 | 1 | 0 | 1 | 1 |
| 1 | 0 | 0 | 1 | 1 |
| 1 | 1 | 1 | 1 | 0 |

Explain that NAND and NOR are functionally complete: suitable combinations can construct other Boolean functions. Use this as a reasoning exercise, not as a claim that every physical design is built from one gate family.

## 4. Combinational logic

Combinational circuits produce outputs from current inputs, subject to physical propagation delay. Core examples:

- encoders and decoders;
- multiplexers and demultiplexers;
- comparators;
- adders and subtractors;
- parity generation and checking;
- simple arithmetic and logic units.

For each circuit, define inputs, outputs, truth table or Boolean expression, intended role, and limits. Distinguish ideal logical behavior from propagation delay, fan-out, power, and electrical constraints.

## 5. Sequential logic and memory

Sequential circuits depend on current inputs and stored state. Introduce latches and flip-flops conceptually, clock edges, setup and hold time, registers, counters, shift registers, finite-state machines, and memory arrays.

A useful state-machine description contains:

- a finite set of states;
- inputs or events;
- transition conditions;
- outputs or actions;
- reset and initialization behavior;
- timing assumptions.

Explain synchronous versus asynchronous behavior, metastability as a real design concern, and why timing analysis matters. Keep circuit-level hazards distinct from abstract game-state transitions.

## 6. Clocks, buses, and interfaces

Explore clock generation and distribution, clock domains, data/control/address buses, serial and parallel communication, synchronization, handshakes, interrupts, and peripheral interfaces. Emphasize that an interface contract includes electrical levels, timing, framing, direction, and semantics.

## 7. Power, thermal behavior, and reliability

Digital logic is not independent of its physical environment. Power delivery, voltage regulation, switching activity, heat removal, component ratings, electromagnetic interference, connectors, and manufacturing variation affect reliability. For world simulation, these can be represented by simplified capacity, temperature, fault, and maintenance variables. Never present simplified game thresholds as real-world safety limits.

## 8. Labs and learning progression

Suggested conceptual labs:

1. Convert between decimal and binary; explain each place value.
2. Build truth tables for the basic Boolean operators.
3. Compose gates to create a simple comparator.
4. Trace a half-adder and full-adder logically.
5. Design a multiplexer as a selection problem.
6. Trace a register across clock events using a paper state table.
7. Model a traffic-light controller as a finite-state machine.
8. Introduce a fault (stuck signal, missing clock, power loss) and predict the observable result.
9. Compare ideal and delayed signal behavior in a safe educational simulator.
10. Explain how a sensor reading becomes a digital value consumed by software.

Each lab should state the question, hypothesis, variables, method, observations, analysis, uncertainty, and conclusion. This ties the electronics material to the scientific method rather than presenting circuits as disconnected recipes.

## 9. Simulation vocabulary

| Entity | Example state | Example event |
|---|---|---|
| Signal | level, frequency, quality | level changed, noise introduced |
| Gate | function, input count, delay class | input changed, output settled |
| Register | stored bit pattern, clock state | clock edge, load, clear |
| Counter | current value, modulus | increment, reset, overflow |
| State machine | current state, transition table | input event, transition |
| Power rail | available capacity, health | brownout, restoration |
| Board | components, connectivity, thermal condition | component fault, inspection |

These are suggested simulation concepts only. The game may simplify analog behavior, timing, and physical effects, but must label simplifications and avoid implying engineering fidelity where none exists.

## 10. LOD and domain boundaries

- **LOD 0:** electronics exists as a capability or environmental fact.
- **LOD 1:** major device types and signal paths are identified.
- **LOD 2:** gates, registers, buses, and simple state machines are represented.
- **LOD 3:** a mission-relevant board or device has components, interfaces, operating limits, and fault states.
- **LOD 4+:** deeper device and timing models are introduced only when the educational or simulation objective requires them.

Physics owns the underlying physical laws and material behavior. This book organizes their electronic application and digital abstraction. Learning owns instructional sequencing. FSM_Foundry may execute simulations; no runtime contract is defined here.


<p align="center">
  <img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="420" />
</p>
