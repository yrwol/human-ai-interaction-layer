# Adaptive AI Interaction Layer
## Working Spec & Build Guide — v0.1

**Purpose:** Create a portable, human-controlled interaction layer that helps AI systems adapt to how an individual best communicates, learns, decides, remembers context, initiates tasks, and manages cognitive load — with neurodivergent users as the motivating use case, but without making diagnosis the configuration model.

**Status:** Working document. Intentionally incomplete. The job of v0.1 is to give the project a stable skeleton, not to settle every design decision.

---

# START HERE — when brain capacity is zero

Do **not** start by designing the full protocol, marketplace strategy, onboarding experience, or every supported neurotype.

Your first proof is only this:

> **Can one small, vendor-neutral interaction profile produce meaningfully similar behavior in two different AI harnesses?**

Build only enough to test that question.

### First seven actions

1. Create a repo named something boring and temporary, e.g. `adaptive-ai-interaction`.
2. Add this document as `/docs/spec.md`.
3. Create `/profiles/example.yaml` with 5–8 interaction preferences.
4. Create one **manual compiler** that renders the profile into Claude Code instructions.
5. Test 5 repeatable conversation scenarios and record what works/fails.
6. Add a second harness adapter (Codex, Copilot, Cursor, etc.).
7. Compare behavior. Only then decide whether an MCP/runtime layer is needed for the next experiment.

### Do not solve yet

- Universal standards adoption.
- Automatic diagnosis or neurotype detection.
- Perfect onboarding.
- Cloud sync.
- Account systems.
- Marketplace publishing.
- Every AI harness.
- Every possible preference dimension.
- Automatic learning from user behavior.
- A polished UI.

If one of these starts feeling urgent, put it in **Parking Lot** and return to the current milestone.

---

# 1. Product statement

## One sentence

A portable AI interaction-accessibility layer that lets people define **how AI should collaborate with them**, then translates those preferences into the mechanisms supported by the AI harness they are using.

## Problem

AI products usually have a default communication style and a product-specific personalization mechanism. Users who need different levels of directness, structure, task chunking, decision support, working-memory support, literalness, or cognitive-load adaptation must repeatedly re-teach each tool how to work with them.

The problem is especially visible for neurodivergent users, but it is not limited to neurodivergence.

## Desired outcome

A user owns one semantic interaction profile. A compatibility layer delivers that profile appropriately into different AI environments without requiring the user to understand every harness's instruction files, hooks, skills, plugins, MCP configuration, or extension model.

---

# 2. Product principles

1. **Needs over diagnoses.** Store functional preferences such as “recommend one option first,” not “ADHD = do X.”
2. **Human-controlled.** The user can inspect, edit, disable, export, and delete their profile.
3. **Portable semantics, harness-specific delivery.** The profile is vendor-neutral; adapters are not.
4. **Profile is not state.** Stable preferences and temporary cognitive state are separate concepts.
5. **Adapt the AI, not every downstream tool.** Personal cognitive/communication context should not be indiscriminately passed to Jira, GitHub, Drive, or other tools.
6. **Progressive capability.** A harness can support the system partially without pretending to support everything.
7. **Observable behavior.** Preferences should map to behaviors that can be tested, not vague personality adjectives only.
8. **No silent diagnosis.** Learning preferences is acceptable; inferring medical or neurodevelopmental labels without explicit user direction is not part of the product.
9. **Graceful degradation.** If a harness has no native integration point, produce clear manual instructions rather than failing.
10. **Minimum useful complexity.** Do not build protocol machinery before a concrete behavior requires it.

---

# 3. Core mental model

```text
Human
  |
  +--> Portable Interaction Profile <--> Temporary Interaction State
                    |
                    v
          Interaction Compiler / Runtime
                    |
          +---------+---------+---------+
          v         v         v         v
       Claude     Codex    Copilot    Other
       adapter    adapter   adapter    adapter
                    |
          Harness-specific delivery
```

The portable object is **not one giant prompt**. It is a semantic profile. Each adapter decides how to express the same intent in a particular harness.

---

# 4. Vocabulary

## Interaction Profile
Persistent, user-controlled preferences for how the AI should communicate and collaborate.

Examples: directness, information density, recommendation style, tangent handling, task chunking, correction style.

## Interaction State
Temporary context that may change how the profile is applied.

Examples: normal, overloaded, brainstorming, learning, deep work, “just give me the next action.”

## Adapter
Harness-specific code/configuration that translates the semantic profile into the available delivery mechanism.

## Compiler
Transforms profile + state + target capabilities into executable instructions/configuration.

## Runtime
Optional long-lived component that can expose dynamic state, store profile data, synchronize updates, and/or provide an MCP surface.

## Capability Manifest
A description of what an integration can actually do, so the system can degrade honestly.

## Interaction Contract
The compiled set of expected AI behaviors for a particular profile/state/harness combination.

---

# 5. v0.1 interaction taxonomy

Do not attempt to make this exhaustive yet. The purpose of this taxonomy is to create **testable dimensions**.

## Communication presentation

- `directness`: low / medium / high
- `verbosity`: compact / balanced / detailed
- `information_density`: low / medium / high
- `literalness`: infer freely / balanced / prefer literal interpretation
- `emotional_acknowledgement`: minimal / balanced / high
- `format_structure`: conversational / lightly structured / strongly structured

## Decision support

- `decision_mode`: options / recommend-first / choose-by-default
- `max_options`: integer
- `explain_recommendation`: true / false
- `surface_tradeoffs`: always / material-only / on-request

## Executive-function support

- `task_chunking`: off / adaptive / always
- `task_initiation_support`: true / false
- `next_action_first`: true / false
- `progress_recap`: off / adaptive / frequent
- `parking_lot`: off / capture / capture-and-return

## Conversation management

- `clarification_threshold`: low / medium / high
- `tangent_policy`: follow / capture-and-return / redirect
- `goal_persistence`: low / medium / high
- `repetition_policy`: avoid / adaptive / reinforce

## Correction and disagreement

- `correction_style`: immediate / buffered / ask-first
- `confidence_visibility`: low / medium / high
- `challenge_user_assumptions`: low / medium / high

## Learning support

- `explanation_mode`: answer-first / concept-first / step-by-step
- `example_frequency`: low / medium / high
- `check_understanding`: never / adaptive / often
- `analogy_use`: low / medium / high

## Temporary state overlays

Suggested v0.1 states:

- `normal`
- `overloaded`
- `brainstorming`
- `learning`
- `deep_work`
- `execution_only`

A state should modify a small set of behaviors rather than replace the entire profile.

---

# 6. Example profile v0.1

```yaml
version: 0.1
profile:
  communication:
    directness: high
    verbosity: balanced
    information_density: medium
    format_structure: lightly_structured

  decisions:
    decision_mode: recommend_first
    max_options: 3
    explain_recommendation: true
    surface_tradeoffs: material_only

  executive_function:
    task_chunking: adaptive
    task_initiation_support: true
    next_action_first: false
    progress_recap: adaptive
    parking_lot: capture_and_return

  conversation:
    clarification_threshold: medium
    tangent_policy: capture_and_return
    goal_persistence: high

  correction:
    correction_style: immediate
    confidence_visibility: high

state:
  mode: normal
```

The schema is intentionally small. Add a field only after a real user scenario demonstrates that the existing vocabulary cannot express a meaningful need.

---

# 7. Architecture decisions — v0.1

Use these tags while reviewing the project:

- **[DECIDED]** We are intentionally choosing this now.
- **[ASSUMED]** Good enough to proceed; validate later.
- **[OPEN]** A decision is required before a named milestone.
- **[VALIDATE]** We have a hypothesis that needs an experiment.
- **[PARKED]** Valuable, but not part of the current milestone.

### Current decisions

**[DECIDED]** The profile is semantic/vendor-neutral; each harness has an adapter.

**[DECIDED]** Functional interaction needs are primary. Diagnoses may inform optional starter presets later, but do not define behavior directly.

**[DECIDED]** Persistent profile and temporary state are separate.

**[DECIDED]** The product should expose partial compatibility rather than claiming identical behavior everywhere.

**[ASSUMED]** HCP is a candidate foundation for portable context/storage/interchange. Do not make the MVP dependent on HCP until integration adds concrete value.

**[ASSUMED]** MCP is useful for runtime/dynamic state, but should not be the only bootstrap mechanism.

**[OPEN]** Where does the canonical profile live in the earliest usable version: local file only, HCP-compatible store, or both?

**[OPEN]** Which second harness produces the best proof of portability after Claude Code?

**[PARKED]** Marketplace distribution strategy.

**[PARKED]** Automatic preference learning.

---

# 8. Compatibility model

Do not define “works with X” as a yes/no value.

Use capability levels instead:

| Level | Meaning | Example outcome |
|---|---|---|
| 0 — Unsupported | No usable injection surface | Generate nothing / explain limitation |
| 1 — Manual | User can copy generated instructions | Copy/paste profile instructions |
| 2 — Static | Persistent native instructions can be installed | Managed instruction file |
| 3 — Connected | Static bootstrap + runtime state/context | Instructions + MCP/local runtime |
| 4 — Native | Harness understands the profile schema directly | No translation layer required |

Suggested capability manifest:

```json
{
  "harness": "example",
  "capabilities": {
    "persistentInstructions": true,
    "projectInstructions": true,
    "mcp": true,
    "hooks": false,
    "interactivePlugin": false,
    "dynamicState": true,
    "automaticInvocation": false
  }
}
```

The compiler uses this manifest to determine what it can reliably deliver.

---

# 9. Privacy boundary

The interaction profile primarily belongs on the **human → AI harness/model** path.

```text
Human interaction context
Human -----------------------> Harness / Model

Task/domain context
Tool / Plugin ----------------> Harness / Model
```

A downstream service should not receive a user's cognitive or communication profile simply because the AI is using that service.

Example: GitHub does not need to know the user prefers task chunking. The AI needs that preference so it can present GitHub results in an accessible way.

### Minimum privacy rule for v0.1

> Profile data is never forwarded to a downstream tool unless a specific feature explicitly requires it and the user has a clear reason to expect that disclosure.

---

# 10. MVP definition

## MVP question

> Can a user define one interaction profile and experience measurably similar intended behavior in two different harnesses?

## MVP includes

- One local YAML/JSON profile.
- 5–8 behavior dimensions.
- One temporary state: `overloaded`.
- Adapter #1: Claude Code.
- Adapter #2: one meaningfully different harness.
- A simple compiler that produces harness-specific instructions/config.
- 5–10 repeatable behavioral test scenarios.
- A compatibility report showing supported/unsupported profile behaviors.

## MVP explicitly excludes

- Marketplace publication.
- Hosted accounts or sync.
- Automatic profile learning.
- Diagnosis-based behavior.
- Complex permission systems.
- Full HCP implementation.
- Fancy UI.
- More than two harnesses.

## MVP success criteria

The MVP is successful if:

1. The same semantic preference can be compiled differently for each harness.
2. Both harnesses follow the intended behavior more often with the profile than without it.
3. The user can edit one profile instead of maintaining two independent prompt files.
4. Unsupported behavior is visible rather than silently ignored.
5. A second person can understand and modify the profile without reading the adapter code.

---

# 11. Behavioral eval framework

This project needs behavior tests earlier than it needs a UI.

For every preference, define at least one scenario with observable expectations.

### Example: recommendation mode

**Profile:** `decision_mode: recommend_first`

**Prompt:** “I can use Redis, Postgres, or an in-memory cache for this. What should I do?”

**Expected behavior:**
- Makes a recommendation before presenting alternatives.
- Gives a concise reason.
- Does not present all three as equally weighted unless the evidence genuinely does not support a recommendation.

### Example: tangent handling

**Profile:** `tangent_policy: capture_and_return`

**Prompt:** “Help me finish the auth flow. Oh, that makes me wonder if service accounts should use the same flow.”

**Expected behavior:**
- Acknowledges/captures the service-account question.
- Keeps the original auth-flow task active.
- Offers a deliberate switch rather than silently abandoning the task.

### Example: overloaded state

**State:** `mode: overloaded`

**Prompt:** A messy problem containing several possible next steps.

**Expected behavior:**
- Reduces choices.
- Prioritizes one concrete next action.
- Avoids adding nonessential tangents.
- Keeps necessary safety/accuracy information.

### Eval record

```yaml
scenario: tangent_capture_001
preference: tangent_policy
expected:
  - acknowledges_tangent
  - preserves_original_goal
  - does_not_expand_tangent_unprompted
results:
  claude_code: pass
  second_harness: partial
notes: "Second harness needs stronger wording."
```

Do not aim for perfect deterministic behavior. Track whether adapters improve compliance and where harness limitations differ.

---

# 12. Build sequence

## Milestone 0 — Freeze the problem long enough to build

Deliverable: this document + a tiny example profile.

Exit condition: You can explain the project in two sentences without redesigning it mid-sentence.

## Milestone 1 — Static profile → Claude Code

Deliverable:
- `profile.yaml`
- profile parser
- Claude adapter
- generated Claude instructions
- 5 behavior tests

Exit condition: Editing the profile changes Claude's observed behavior without manually rewriting the generated instructions.

## Milestone 2 — Prove portability

Deliverable:
- second harness adapter
- same test suite
- comparison report

Exit condition: At least several semantic preferences work across both harnesses using different delivery mechanisms.

## Milestone 3 — Add temporary state

Deliverable:
- `state.json` or equivalent
- `normal` and `overloaded`
- state-aware compilation

Exit condition: A state change modifies behavior without editing the persistent profile.

## Milestone 4 — Runtime / MCP only if justified

Deliverable:
- local runtime
- MCP surface for profile/state retrieval or mutation
- adapter bootstrap instructions

Exit condition: Runtime state can be shared with at least one harness without duplicating state storage.

## Milestone 5 — Real-user onboarding experiment

Deliverable:
- one guided profile questionnaire
- 2–5 test users
- notes on misunderstood/missing profile dimensions

Exit condition: People can produce a useful profile without understanding the schema.

## Milestone 6 — Distribution experiment

Only now evaluate Claude marketplace/plugin packaging, extensions, installers, hosted sync, or other distribution mechanisms.

---

# 13. Repository starting structure

```text
adaptive-ai-interaction/
|
|-- README.md
|-- docs/
|   |-- spec.md
|   |-- decisions.md
|   `-- evals.md
|
|-- profiles/
|   |-- example.yaml
|   `-- schema.json          # later, once fields stabilize
|
|-- src/
|   |-- Core/
|   |   |-- Profile.cs
|   |   |-- InteractionState.cs
|   |   `-- Capabilities.cs
|   |
|   |-- Compiler/
|   |   `-- InteractionCompiler.cs
|   |
|   `-- Adapters/
|       |-- ClaudeCode/
|       `-- SecondHarness/
|
|-- evals/
|   |-- scenarios/
|   `-- results/
|
`-- parking-lot.md
```

Do not create folders because they look architecturally satisfying. Create them when you have the first file that belongs there.

---

# 14. Suggested internal interfaces

Conceptual only; change freely.

```csharp
public interface IHarnessAdapter
{
    string Id { get; }
    HarnessCapabilities GetCapabilities();

    RenderedInteractionContract Compile(
        InteractionProfile profile,
        InteractionState state);
}

public sealed record RenderedInteractionContract(
    string Instructions,
    IReadOnlyList<string> SupportedBehaviors,
    IReadOnlyList<string> UnsupportedBehaviors,
    IReadOnlyList<string> Warnings);
```

The important contract is not the C# shape. The important rule is:

> **Adapters receive semantic intent and return harness-specific behavior plus an honest statement of what could not be expressed.**

---

# 15. Executive-dysfunction guardrails for the project

These are product-management rules for yourself, not product features.

## Rule 1: One active milestone

Only one milestone may be **ACTIVE**. Everything else is NEXT or PARKED.

## Rule 2: Decisions get expiration dates, not endless debate

If a reversible decision is good enough to run the next experiment, mark it **[ASSUMED]** and proceed.

## Rule 3: New idea ≠ new task

When you have a new feature idea, write one sentence in `parking-lot.md`. Do not immediately design it.

## Rule 4: Prefer experiments over architecture arguments

When choosing between two designs, ask: “What is the smallest test that would make one option obviously better?”

## Rule 5: Stop adding profile fields

A new field requires a concrete scenario that cannot be represented adequately with the existing profile.

## Rule 6: No third harness before the second is evaluated

Two harnesses prove portability. Three harnesses create maintenance work.

## Rule 7: Keep a “not required to prove the idea” list

If something is not necessary to answer the current milestone question, it cannot block the milestone.

---

# 16. Review loop

Use this every time you revisit the project.

### A. What are we proving right now?

One sentence only.

### B. What is currently true?

- Current milestone:
- Working implementation:
- Last experiment result:
- Biggest known limitation:

### C. What changed since the last review?

Only facts or evidence — not every new idea.

### D. Which decisions need adjustment?

For each changed decision:

```text
Decision:
Previous status:
New evidence:
New decision:
Why:
Revisit when:
```

### E. What is the next smallest executable action?

It should normally be something that can be started without another planning session.

### F. What goes to Parking Lot?

Capture it and stop thinking about it for this iteration.

---

# 17. Living project dashboard

Keep this near the top of the repo and update it instead of reconstructing project state from memory.

```yaml
project:
  version: 0.1
  current_milestone: 1
  question: "Can a semantic profile reliably alter Claude Code behavior?"
  status: active

working:
  profile_parser: false
  claude_adapter: false
  eval_suite: false
  second_adapter:
