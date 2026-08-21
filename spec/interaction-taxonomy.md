# HAIL Interaction Taxonomy

This document preserves the broader catalog of interaction dimensions identified during HAIL's initial design work.

It is intentionally larger than the current schema.

**Important:** inclusion here does **not** mean a field is implemented, validated, or appropriate for persistent storage. The authoritative current semantic model is [`semantics.md`](semantics.md).

## Status vocabulary

- **validated** — part of the current semantic model with behavioral evidence.
- **candidate** — useful interaction dimension identified for future experiments.
- **parked** — intentionally not being designed now because another prerequisite or experiment should come first.
- **conceptual** — useful language/modeling idea, but not necessarily destined to become a profile field.

## Communication presentation

| Dimension | Candidate values / intent | Status |
|---|---|---|
| `directness` | low / medium / high | candidate |
| `verbosity` | compact / balanced / detailed | **validated** |
| `information_density` | low / medium / high | candidate |
| `literalness` | infer freely / balanced / prefer literal interpretation | candidate |
| `emotional_acknowledgement` | minimal / balanced / high | candidate |
| `format_structure` | conversational / lightly structured / strongly structured | candidate |

## Decision support

| Dimension | Candidate values / intent | Status |
|---|---|---|
| `decision_mode` | options / recommend-first / choose-by-default | **validated** |
| `max_options` | positive integer | **validated** |
| `explain_recommendation` | true / false | candidate |
| `surface_tradeoffs` | always / material-only / on-request | candidate |

## Executive-function support

| Dimension | Candidate values / intent | Status |
|---|---|---|
| `task_chunking` | off / adaptive / always | **validated** |
| `step_pacing` | continuous / check-in / wait-for-user | **validated** — discovered through testing |
| `task_initiation_support` | true / false | candidate |
| `next_action_first` | true / false | candidate |
| `progress_recap` | off / adaptive / frequent | candidate |
| `parking_lot` | off / capture / capture-and-return | candidate; overlaps conceptually with tangent handling and should not be added without a distinct scenario |

## Conversation management

| Dimension | Candidate values / intent | Status |
|---|---|---|
| `clarification_threshold` | low / medium / high | candidate |
| `tangent_policy` | follow / capture-and-return / redirect | **validated** |
| `goal_persistence` | low / medium / high | candidate |
| `repetition_policy` | avoid / adaptive / reinforce | candidate |

## Correction and disagreement

| Dimension | Candidate values / intent | Status |
|---|---|---|
| `correction_style` | immediate / buffered / ask-first | candidate |
| `confidence_visibility` | low / medium / high | candidate |
| `challenge_user_assumptions` | low / medium / high | candidate |

## Learning support

| Dimension | Candidate values / intent | Status |
|---|---|---|
| `explanation_mode` | answer-first / concept-first / step-by-step | candidate |
| `example_frequency` | low / medium / high | candidate |
| `check_understanding` | never / adaptive / often | candidate |
| `analogy_use` | low / medium / high | candidate |

## Review confidence and AI literacy

HAIL also has focused specification work around helping users evaluate AI output, surface important assumptions, calibrate skepticism, and recognize when further questioning is no longer useful. See `draft-guidance.md` when available on the target branch.

Potential concepts from that work include:

- surfacing material assumptions;
- suggesting one high-value verification action;
- confidence/uncertainty visibility;
- independent review;
- verification-loop detection;
- domain-local scaffolding;
- calibration prompts;
- escalation to authoritative evidence or human expertise.

Status: **feature-spec / conceptual**, not current persistent-profile schema.

## Temporary interaction state

The original HAIL model explicitly distinguishes persistent profile from temporary state.

Candidate state labels identified early include:

- `normal`
- `overloaded`
- `brainstorming`
- `learning`
- `deep_work`
- `execution_only`

Possible contextual requests include:

- “I'm overwhelmed today.”
- “Just give me the next action.”
- “For this task, stop waiting on me.”
- “We're brainstorming; don't collapse choices too early.”

Status: **parked pending a dedicated state experiment**.

A state should modify a small set of behaviors rather than replace the persistent profile. Scope, expiry, precedence, persistence across sessions, and whether state may ever be inferred remain unresolved.

## Autonomy and execution behavior

Native testing surfaced a possible distinction between pacing and autonomy.

Examples:

- “Keep going through the steps” concerns pacing.
- “Make the decisions for me” concerns decision authority.
- “Do not start executing tools/agents without me” concerns execution autonomy.

Status: **conceptual / unvalidated**.

Do not add an `autonomy` field until a persistent user scenario demonstrates that current semantics and ordinary harness controls cannot express the need.

## Compatibility dimensions

Compatibility itself may need multiple dimensions rather than a single “works with HAIL” level.

Candidate observations include:

- persistent instruction support;
- explicit skill/plugin invocation;
- dynamic/contextual state support;
- safe preservation of existing user instructions;
- automatic refresh behavior;
- per-preference observed enforcement strength;
- whether a preference is unsupported versus merely weakly followed.

Status: **conceptual**, with preference-level enforcement already validated as important.

## Rule for promotion into the semantic model

A taxonomy entry becomes a current HAIL semantic preference only after evidence shows:

1. a real interaction need exists;
2. the need is meaningfully persistent rather than merely contextual;
3. existing fields cannot represent it cleanly;
4. values can be defined in behaviorally observable terms; and
5. at least one harness experiment demonstrates useful enforcement.

This taxonomy is intentionally a place to **remember possibilities without turning every possibility into architecture**.
