# Electricity, Fields, and Signals — The Physical Foundations of Electronics

<p align="center">
  <a href="https://github.com/TrentBest/TheSingularityWorkshop.Visualize"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop" width="720" /></a>
</p>

> **Sourcebook:** physical foundations for the connected-world matrix. This is conceptual education and world-domain reference, not an electrical installation or repair manual.

## 1. Why electronics begins with physics

Electronic systems manipulate energy and information through physical materials and fields. A circuit diagram is a useful abstraction, but the circuit exists in a physical environment with finite materials, geometry, temperature, tolerances, noise, and energy limits. The purpose of this book is to connect those foundations to the electronic components and digital systems described in neighboring sourcebooks.

## 2. Charge, current, and voltage

- **Electric charge** is a property of matter associated with electromagnetic interaction.
- **Current** describes the rate of charge flow through a defined cross-section.
- **Voltage** describes electric potential difference between two points; it is not a substance that flows.
- **Resistance** characterizes opposition to current in a simplified circuit model.
- **Power** describes the rate of energy transfer. Electrical power depends on the relationship between voltage and current.

Introduce conventional current direction and electron motion as distinct conventions. Explain reference nodes and why a voltage is measured between points. Avoid treating ground as an infinite sink or universal zero independent of the circuit context.

## 3. Fields and materials

Electric and magnetic fields describe interactions and energy in space. Conductors, insulators, semiconductors, and magnetic materials behave differently because of their material properties and structure. Connect these ideas to capacitors, inductors, diodes, transistors, motors, sensors, antennas, and electromagnetic compatibility.

## 4. Basic circuit relationships

Use Ohm's law and conservation principles as introductory models, clearly stating their assumptions and limits. Introduce series and parallel relationships, voltage and current division, energy storage, transient response, and steady-state behavior. Teach learners to identify the system boundary, reference points, units, and assumptions before manipulating equations.

## 5. Capacitance, inductance, and time

Capacitors and inductors store energy in electric and magnetic fields. Their behavior depends on changing voltage/current and circuit context. Explain charging and discharging curves, time constants as a model, filtering, resonance at a conceptual level, and why switching systems cannot always be understood from a static schematic alone.

## 6. Signals

A signal is a varying physical quantity used to represent information or control behavior. Introduce amplitude, offset, frequency, period, phase, waveform, pulse width, noise, distortion, bandwidth, sampling, and quantization. Distinguish an analog signal from a digital interpretation of a signal. Explain that digital receivers use thresholds and timing windows and therefore have finite noise margins.

## 7. Measurement and uncertainty

Discuss measurement as an interaction with the system, not a perfect view from outside. Introduce instrument range, resolution, accuracy, calibration, loading, bandwidth, sampling, uncertainty, repeatability, and documentation. Learners should report units and conditions, distinguish observation from interpretation, and avoid false precision.

## 8. Energy, heat, and reliability

Energy is conserved, but useful electrical systems dissipate energy and produce heat. Component ratings, cooling, ambient conditions, material aging, vibration, contamination, and manufacturing variation influence reliability. The simulation may use simplified thermal and energy budgets, but must never present game values as real-world ratings or safety limits.

## 9. Conceptual lab sequence

1. Identify charge, current, voltage, resistance, and power in a simple diagram.
2. Compare series and parallel circuit models using symbolic values.
3. Trace energy storage in a capacitor and inductor conceptually.
4. Compare periodic and non-periodic signals.
5. Identify noise, offset, distortion, and sampling effects in provided waveforms.
6. Explain how a physical signal becomes a digital logic level.
7. Record a measurement with units, instrument assumptions, and uncertainty.
8. Investigate a fictional sensor failure and distinguish cause, symptom, and evidence.

Each lab should use a scientific-method pattern: question, hypothesis, variables, method, observations, analysis, uncertainty, and conclusion. Use approved educational simulators or low-voltage learning equipment with appropriate supervision; this sourcebook does not replace qualified safety instruction.

## 10. Simulation hooks

Suggested domain entities include energy source, conductor, component, node, signal, sensor, measurement instrument, thermal environment, and load. Suggested state includes potential difference, current class, power budget, signal quality, temperature class, component health, and measurement confidence. Events may include signal changed, supply degraded, sensor drifted, measurement recorded, or component failed. These are conceptual simulation properties, not a prescribed electrical solver or DataWarehouse schema.

## 11. LOD and connections

- **LOD 0:** energy, signal, and electronic capability are broad world facts.
- **LOD 1:** identify sources, loads, signals, and major component families.
- **LOD 2:** introduce circuit relationships, waveforms, measurement, and basic energy storage.
- **LOD 3:** model a fictional device's signal path, operating envelope, faults, and maintenance history.
- **LOD 4+:** introduce deeper field or component models only where the educational goal justifies them.

Physics owns fundamental laws and material models. Components and Electronic Devices applies those foundations to devices; Digital Logic and Circuitry develops switching and computation. Learning owns curriculum and assessment.


<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="420" /></p>
