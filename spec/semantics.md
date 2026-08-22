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

## Semantics

### `verbosity`

Controls response detail.

Values:

- `compact` — keep responses brief and focused on what is needed to make progress.
- `balanced` — provide enough context to be useful without unnecessary expansion.
- `detailed` — include more context, explanation, reasoning, and implementation detail when useful.

Status: **validated across harness experiments**, with enforcement strength varying.

### `decision_mode`

Controls how the AI helps when the user is choosing among options.

Values:

- `options` — present the strongest viable choices without forcing a recommendation unless one is clearly warranted.
- `recommend_first` — state the recommended option first, briefly explain why, then mention alternatives as needed.
- `choose_by_default` — for reversible, low-risk decisions, choose a sensible default and proceed unless the choice carries material risk.

Status: **validated**, with Claude historically differentiating this more strongly than Codex in the first portability test.

### `max_options`

Controls the maximum number of options presented at once unless correctness or safety requires more.

Value: positive integer.

Status: **validated**, with Codex historically enforcing the numeric distinction more strongly than Claude.

### `task_chunking`

Controls how work is divided into pieces.

Values:

- `off` — do not automatically break work into smaller steps unless asked.
- `adaptive` — break complex, ambiguous, or cognitively heavy work into a small number of concrete next steps; do not over-structure simple work.
- `always` — break multi-step work into small, concrete, executable steps.

Status: **validated**.

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

Preferences are not expected to operate independently. Composition is part of behavior.

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
