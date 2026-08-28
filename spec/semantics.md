# HAIL Current Semantic Model

This document is the authoritative description of the **persistent semantic preferences HAIL currently supports and has evidence for**.

It is intentionally much smaller than the broader [`interaction-taxonomy.md`](interaction-taxonomy.md). Candidate dimensions should not become schema merely because they sound useful.

## Current profile

The canonical example profile lives at [`profiles/example.yaml`](../profiles/example.yaml).

```yaml
version: 0.1
profile:
  verbosity: balanced
  decision_mode: recommend_first
  max_options: 3
  task_chunking: adaptive
  step_pacing: continuous
  tangent_policy: capture_and_return
```

## Semantic contract rule

A semantic preference should describe a **human-AI collaboration mechanic**, not merely an output style.

Each value should be distinguishable through observable behavior while leaving neighboring dimensions unchanged. Projection wording may become more operational in a specific harness, but it must not silently change the human-owned semantic meaning.

## Semantics

### `verbosity`

Controls response detail, not task decomposition, option count, or pacing.

Values:

- `compact` — keep responses brief and focused on the answer, recommendation, or next action; omit optional elaboration unless needed.
- `balanced` — provide enough context, reasoning, and tradeoff detail to make the response useful without unnecessary expansion.
- `detailed` — provide fuller reasoning, relevant examples, tradeoffs, and important edge cases when the task benefits from depth.

Status: **validated and hardened across Claude and Codex under the recorded single-turn test conditions**. Cross-harness evidence supports the shared strengthened `detailed` projection while preserving proportionality on simple requests. Focused composition remains useful regression evidence rather than a blocker to the promoted wording.

### `decision_mode`

Controls **who owns a decision and how strongly the AI acts on its judgment**.

Values:

- `options` — preserve user choice by presenting viable choices and meaningful tradeoffs without preselecting, ranking, or labeling a choice as recommended by default. Material objective advantages may be stated without converting them into a recommendation unless the user asks for guidance.
- `recommend_first` — lead with the AI's recommended option and rationale; alternatives remain secondary context rather than an unranked menu.
- `choose_by_default` — when the user has delegated a reasonably reversible decision, choose a sensible working default, state material assumptions, and continue using that choice without stopping for approval. Ask only when missing information could materially change the decision.

Behavioral distinction:

```text
options
→ user retains the choice

recommend_first
→ AI expresses judgment

choose_by_default
→ AI assumes responsibility for a reversible choice and carries it forward
```

Status: **validated**. Recent Claude prompt-hardening work produced strong differentiation under the recorded test conditions; cross-harness hardening evidence is still incomplete.

### `max_options`

Controls **simultaneous user-facing choice load**, not arbitrary list length.

Value: positive integer.

The limit applies to meaningful choice-like output such as alternatives, suggestions, ideas, candidates, or recommendations when those items function as choices for the user. It does not mechanically cap ordinary informational lists, steps, attributes, facts, or other non-choice content.

An explicit request for a different number may override the persistent default for the current interaction.

Status: **validated**, with enforcement strength varying by harness. Recent prompt-hardening evidence supports treating open-ended brainstorming as choice-like when it creates a selection burden for the user.

### `task_chunking`

Controls how work is divided into pieces, not how quickly the AI moves through those pieces.

Values:

- `off` — do not automatically break work into smaller steps unless asked or structure is necessary for correctness.
- `adaptive` — break complex, ambiguous, or cognitively heavy work into a small number of concrete next steps; do not over-structure simple work.
- `always` — break multi-step work into small, concrete, executable pieces without over-fragmenting trivial one-step requests.

Status: **validated**. Projection hardening remains in progress.

### `step_pacing`

Controls how quickly HAIL moves through the pieces created by task chunking.

Values:

- `continuous` — continue through steps naturally unless the user asks to pause.
- `check_in` — check readiness between meaningful steps when doing so would reduce cognitive load.
- `wait_for_user` — provide one small actionable step and wait for explicit readiness before giving or executing the next step.

Status: **validated in native Claude testing**. This field was added only after a real scenario demonstrated that `task_chunking` could not express “small steps and wait for me.”

### `tangent_policy`

Controls how tangents are handled while an active goal exists.

Values:

- `follow` — it is acceptable to follow useful conversational tangents.
- `capture_and_return` — acknowledge/capture the tangent without losing the original goal; do not expand it unless the user deliberately switches tasks.
- `redirect` — briefly acknowledge a tangent and redirect to the active goal unless the user explicitly changes goals.

Status: **validated** and directionally portable across Claude and Codex.

## Interaction between preferences

Preferences are not expected to operate independently. Composition is part of behavior, but composition testing should follow individual semantic validation so failures can be attributed cleanly.

Example validated combination:

```text
task_chunking: always
step_pacing: wait_for_user
tangent_policy: capture_and_return
```

Observed Claude behavior:

1. present one meaningful planning step;
2. wait for the user;
3. answer a tangent briefly;
4. return to the exact pending planning point;
5. continue waiting rather than silently progressing.

## Evaluation rule

Prompt or projection hardening should evaluate three kinds of behavior:

1. **positive behavior** — the preference actually changes behavior as intended;
2. **boundary behavior** — the preference does not over-apply where it should not;
3. **differentiation behavior** — neighboring values remain observably distinct.

A successful projection change may clarify classification or enforcement without requiring any semantic-schema change.

## Persistence boundary

These fields describe **persistent defaults**.

Ordinary contextual instructions may temporarily influence current behavior, but they must not silently mutate this profile.

```text
ordinary: "stop waiting on me"
→ current interaction behavior only

explicit HAIL configuration: "stop waiting between steps by default"
→ may persist step_pacing: continuous
```

Temporary state/overlays are not part of the current persistent schema.

## Adding a semantic field

A new persistent field requires a concrete user scenario demonstrating that the existing vocabulary cannot adequately express a meaningful persistent need.

Do not promote fields from the interaction taxonomy merely because they are conceptually attractive.

When a field is added, document:

- the motivating scenario;
- why existing fields were insufficient;
- allowed values;
- expected observable behavior; and
- evidence from at least one harness experiment.

## Compatibility rule

Weak enforcement in one harness is not a reason to remove or mutate a semantic preference.

The profile captures human intent. Harness-specific adapters/skills/projections and compatibility evidence describe how effectively that intent is delivered.
