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

## Profile normalization and compatibility

This document is the **authoritative semantic source** for profile normalization and backward-compatibility behavior.

Harness integrations and individual management skills must consume the same normalized profile semantics rather than independently embedding migration rules.

Normalization happens before an action interprets, displays, changes, resets, validates, or projects a profile:

```text
stored profile
→ normalize compatibility
→ apply explicit current configuration intent
→ validate current semantic profile
→ action-specific behavior
→ persist/project only when the action permits mutation
```

Current compatibility rules:

- A valid older profile that predates `step_pacing` is interpreted as `step_pacing: continuous`.
- Existing HAIL profiles containing plain YAML `task_chunking: off` are semantically `task_chunking: off`. HAIL treats the profile format using YAML 1.2 scalar meaning; compatibility with YAML 1.1-style parsers must not change that semantic value.

Precedence:

1. an explicit value supplied by the current intentional HAIL configuration request;
2. an explicitly stored current-schema value;
3. a compatibility-derived value defined by this section.

Compatibility-derived values are interpretation defaults, not permission to mutate storage. A read-only action such as `show` must never rewrite a profile merely to materialize a normalized value. A mutating action may persist the complete current-schema profile after applying the user's intentional change and validation.

### YAML serialization safety

The canonical semantic value is `task_chunking: off`, but HAIL-owned writes SHOULD serialize that scalar as:

```yaml
task_chunking: "off"
```

Some YAML 1.1 parsers interpret plain `off` as boolean `false`. Quoting the value preserves the intended string across YAML parser versions without changing HAIL semantics.

Existing unquoted HAIL profiles remain valid and must be interpreted as semantic `off`; read-only operations must not rewrite them solely to add quotes. When an explicit mutating HAIL action writes the complete current profile, it may normalize the serialized representation to quoted `"off"`.

New compatibility behavior belongs **here first**. Do not add one-off migration rules to `show`, `setup`, `change`, `reset`, or other user-facing skill contracts.

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

- `options` — preserve user choice by presenting viable choices and meaningful tradeoffs without preselecting, ranking, or recommending a choice by default. A comparison question alone does not authorize the AI to choose; material objective advantages may be stated without converting them into a recommendation unless the user explicitly asks for guidance or a decision.
- `recommend_first` — lead with the AI's recommended option and rationale; alternatives remain secondary context rather than an unranked menu.
- `choose_by_default` — when the user has delegated a reasonably reversible decision and enough material context exists, choose a sensible working default, state only non-material assumptions, and continue using that choice without stopping for approval.

All decision modes share a material-ambiguity boundary: decision style never authorizes invented material assumptions. For consequential, hard-to-reverse, or materially underdetermined decisions, if missing information could materially change the choice, ask only the minimum clarification needed before selecting or recommending a direction. When such clarification is required, do not also provide a fallback choice, provisional recommendation, or assumed decision before the user answers.

Behavioral distinction:

```text
options
→ user retains the choice

recommend_first
→ AI expresses judgment

choose_by_default
→ AI assumes responsibility for a reversible choice and carries it forward
```

Status: **validated and hardened across Claude and Codex under the recorded single-turn composition conditions**. Cross-harness evidence supports neutral comparison behavior in `options`, recommendation-first posture in `recommend_first`, working-decision behavior in `choose_by_default`, and the shared material-ambiguity boundary. The latest `verbosity × decision_mode` composition showed that Claude needs more explicit harness-specific adoption wording for `choose_by_default` than Codex, while the vendor-neutral semantic meaning remains unchanged.

### `max_options`

Controls **simultaneous user-facing choice load**, not arbitrary list length.

Value: positive integer.

The limit applies to meaningful choice-like output such as alternatives, suggestions, ideas, candidates, or recommendations when those items function as choices for the user. It does not mechanically cap ordinary informational lists, steps, attributes, facts, or other non-choice content.

An explicit request for a different number may override the persistent default for the current interaction.

Status: **validated and cross-harness hardened under the recorded single-turn composition conditions**. Current evidence supports treating open-ended brainstorming as choice-like when it creates a selection burden, counting hybrids/syntheses as choices when presented as distinct approaches, preserving explicit current-request count overrides, and leaving ordinary informational lists outside the cap. Prompt-only enforcement is not deterministic: both tested harnesses can still occasionally append a distinct closing synthesis/hybrid after the configured choice limit despite explicit projection wording. That is an enforcement limitation, not a semantic-schema gap.

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