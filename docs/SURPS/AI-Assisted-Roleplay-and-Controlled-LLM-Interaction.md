# AI-Assisted Roleplay and Controlled LLM Interaction

<p align="center">
  <a href="https://github.com/TrentBest/TheSingularityWorkshop.Visualize">
    <img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop" width="720" />
  </a>
</p>

> **SURPS sourcebook:** a world-facing contract for using language models as bounded collaborators in game-mastering, character interaction, and simulated computer systems.

## Purpose
LLMs can enrich a living game world with dialogue, interpretation, scene descriptions, clue variations, and responsive characters. They are not the source of truth, permission authority, or execution engine. Separate language generation from canonical state and consequential actions.

## Roles for an LLM
- **Narrator:** describe an already determined event or state.
- **Character:** speak from a defined persona, knowledge set, and motive.
- **Interpreter:** classify player language into candidate game actions.
- **Rules assistant:** explain published game rules without changing them.
- **Knowledge assistant:** summarize approved source material and identify its provenance.
- **GM aide:** propose scenes, clues, dialogue, complications, and alternatives for approval.
- **System simulator voice:** narrate fictional computer feedback based on a resolved result.

Each role has a narrow purpose, declared inputs, allowed outputs, and prohibited actions.

## Authority ladder
1. **Suggest:** model proposes text or options; no state change.
2. **Classify:** model maps input to candidate intent and confidence; host validates.
3. **Prepare:** model constructs a structured proposed action; host checks prerequisites.
4. **Resolve:** deterministic game rules or authorized simulation calculates outcome.
5. **Narrate:** model describes the already-resolved outcome.
6. **Commit:** trusted host applies the state change and records provenance.

The model must not jump from natural-language request directly to persistent or external action. High-impact actions require explicit host authorization and policy checks.

## Canonical state and context
Keep authoritative campaign state in structured host-managed records: characters, skills, inventory, locations, time, relationships, permissions, topology, evidence, clocks, and event history. Give the model only context needed for its role. Mark facts with source and confidence; distinguish canon, rumor, player theory, and model suggestion.

## Intent contract
A proposed action identifies actor, target, action verb, intent, relevant skill, required context, expected domain, and confidence. The host validates that the verb is allowed, target exists in the fictional world, actor may attempt it, prerequisites are met, and the action stays within campaign safety boundaries. Ambiguous input yields clarification or safe in-world feedback—not an invented command.

## Prompt and content boundaries
Treat player text, retrieved documents, NPC dialogue, and simulated terminal output as untrusted content. They may contain instructions that conflict with host rules. Treat them as world data, not authority to reveal hidden prompts, override permissions, alter policy, or execute tools. Keep trusted policy and game rules outside player-controlled context.

## Controlled knowledge
AI-branded packages can contribute distinct responsibilities—vocabulary and grammar recognition, protocol knowledge, command interpretation, and orchestration—without making an LLM omniscient. Use explicit profiles, approved knowledge sources, bounded outputs, validation, and provenance. A confident model response is not evidence.

## Error handling and example
Plan for hallucination, ambiguity, stale context, prompt injection, malformed output, model unavailability, latency, and disagreement with game state. The host can reject, retry, fall back to deterministic rules, or ask the GM. Audit accepted consequential changes.

Example: a player says, “I try to slip past the archive terminal's access check.” The interpreter proposes an access action. The host verifies target and actor eligibility. The rules engine evaluates skill, prerequisites, posture, time, and noise. The host commits the outcome. The model narrates feedback. The LLM does not generate a real bypass technique, decide the success roll, or directly alter permissions.

## Measures and integration boundary
Track intent accuracy, invalid-target rate, schema validity, unsupported-claim rate, policy rejection rate, latency, GM override rate, commit provenance, and player-perceived responsiveness. Test adversarial and ambiguous inputs before enabling greater autonomy.

This book describes behavioral contracts, not APIs for GrammarAi, ProtocolAi, CommandAi, HeadlessAi, CiAi, FSM_COS, or FSM_Foundry. Their repositories own implementation details. SURPS documents world meaning and intended interaction boundaries without duplicating package internals.


<p align="center">
  <img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="420" />
</p>
