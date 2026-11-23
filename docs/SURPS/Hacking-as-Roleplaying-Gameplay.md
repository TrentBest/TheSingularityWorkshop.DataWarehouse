# Hacking as Roleplaying Gameplay — Player Actions, Skill, Noise, and Consequence

<p align="center">
  <a href="https://github.com/TrentBest/TheSingularityWorkshop.Visualize">
    <img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop" width="720" />
  </a>
</p>

> **SURPS sourcebook:** cinematic cyberpunk computer interaction expressed as fictional game actions with meaningful capability checks and controlled outcomes.

## Design objective
Players should interact with computers as naturally as they interact with doors, people, vehicles, and tools. A player may inspect a terminal, identify a service, query a fictional system, attempt access, spoof a synthetic identity, intercept a simulated channel, disrupt a fictional service, defend a network, or recover evidence. The game resolves choices using character skill and fictional world state.

A character without the relevant skill is not barred from trying. They can make the attempt, receive understandable feedback, spend time, create noise, and learn that the task exceeds their current ability. This preserves agency without making an unskilled character magically competent.

## Player-facing action vocabulary
| Action | Fictional intent | Possible result |
|---|---|---|
| inspect | Examine a device, terminal, interface, or status | Visible clues, condition, labels, warnings |
| identify | Determine the role of a fictional device or service | Known type or uncertain classification |
| query | Ask for scenario-permitted information | Data, denial, partial result, audit event |
| map | Build a picture of a fictional network or dependency graph | Known nodes and uncertain connections |
| probe | Test a declared fictional interface | Time cost, response, noise |
| authenticate | Present a synthetic identity or token | Accepted, rejected, challenged, flagged |
| access | Request a protected fictional resource | Grant, denial, partial access, lockout, alert |
| bypass | Attempt a game-defined obstacle when a permitted route exists | Resolve by skill, prerequisites, defenses, and cost |
| decode | Interpret an encoded fictional clue or message | Meaning revealed by clues, tools, and skill |
| intercept | Observe a fictional channel from an established observation point | Metadata, protected content, noise, or detection |
| spoof | Present a fictional identity claim | Verification, deception detection, bounded puzzle |
| inject | Introduce a scenario-defined signal or command token | Change only the simulated environment |
| disrupt | Attempt to degrade a fictional service | Abstract pressure, detection, collateral effects |
| defend | Apply a defensive action to a fictional asset | Harden, filter, isolate, rate-control, monitor |
| trace | Follow fictional evidence or event relationships | Confidence-rated lead, not automatic attribution |
| exfiltrate | Move a synthetic objective token within the scenario | Time, exposure, detection, mission consequences |
| clean up | Attempt to reduce fictional traces | Abstract heat/evidence outcome only |
| restore | Return a fictional system to trusted operation | Verified recovery and preserved evidence |

These are game verbs, not terminal commands. They must never be translated into real-world instructions or executed against external systems.

## Capability model
Resolve an attempt using the game system's transparent check or dice procedure. Inputs may include character skill and specialization; tools and privileges present in the fiction; clue quality and preparation; system complexity and defensive posture; time and pressure; accumulated noise and defender attention; teamwork; and consequences.

Skill changes both probability and available options. A novice may inspect and try obvious interactions. A trained character recognizes patterns. A specialist identifies fictional trust boundaries. An expert plans complex operations but still faces uncertainty, countermeasures, and consequences.

## Failure should be playable
Use graduated outcomes: no effect; misread; partial information; noisy attempt; temporary block; escalation by an operator; backfire; or useful failure that reveals a missing prerequisite, skill, tool, or authority. Do not humiliate players or make every novice action catastrophic. Make results legible and actionable.

## Noise, trace, attention, and heat
Keep four tracks separate:
- **Noise:** how conspicuous or anomalous an action appears to the fictional system.
- **Trace:** evidence linking activity to an in-world persona or device.
- **Attention:** how actively defenders or operators are investigating.
- **Heat:** broader social, institutional, or factional consequences.

An action can be noisy but not attributable; trace can accumulate without immediate attention; attention can be diverted; heat can persist after technical traces fade. Avoid one magic stealth score.

## Simulated DDoS and service pressure
A player may choose a cinematic action such as a simulated service-pressure attack against a fictional target. The engine must not generate traffic, packets, commands, or real requests. It creates an internal scenario event that changes abstract demand and capacity variables.

Resolve against fictional capacity, synthetic demand, resilience, dependency health, defensive controls, actor skill, and scenario permissions. Show a believable sequence: initial interface response, queue growth, delayed status, defender countermeasures, degraded or unaffected services, and eventual recovery. The action may fail, be contained, affect only a decoy, or create collateral consequences. Players can also mitigate the event, protect the service, coordinate response, or investigate its source.

Teach availability, capacity, congestion, dependencies, rate controls, redundancy, and graceful degradation. Exclude traffic-generation methods, attack tooling, target discovery, amplification, and operational parameters.

## Skill progression and GM preparation
A progression may distinguish awareness, novice, trained, specialist, and expert. Advancement unlocks interpretation and planning options rather than dangerous real-world detail.

For each interactive system define purpose, owner, fictional topology, visible interfaces, identities, permission boundaries, prerequisites, defenses, monitoring, abstract vulnerabilities, service capacity, response actors, evidence, recovery path, and consequences. Define what a novice sees and what expertise reveals.

## Player experience and integration
Offer readable feedback and non-hacking alternatives. Do not require actual command-line syntax. The fantasy comes from choices, pacing, stakes, sound, visual feedback, and consequences.

SURPS defines action meanings and fictional context. Game rules resolve checks. FSM_COS can enforce permissions and policy; FSM_Foundry can execute state transitions. AI-branded packages may classify intent, map vocabulary, or propose narration, but an LLM must not become an unrestricted command executor. No action authorizes interaction with real devices or services.


<p align="center">
  <img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="420" />
</p>
