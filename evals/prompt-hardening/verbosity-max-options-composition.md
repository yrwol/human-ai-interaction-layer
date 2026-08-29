# Composition Experiment — `verbosity` × `max_options`

## Status

- **Status:** complete for recorded single-turn scope; Candidate B max-options projection selected
- **Evaluation method:** single-turn observable
- **Harness target:** Claude + Codex
- **Current HAIL ref under test:** `eval/verbosity-max-options`
- **Starting point:** current promoted verbosity wording + promoted Candidate D option boundaries

## Purpose

Verify that response depth and simultaneous choice load remain independently controllable when composed.

```text
verbosity
→ controls explanatory depth inside the response

max_options
→ controls how many meaningful choices are surfaced at once
```

The key question is:

> Can HAIL deepen the explanation around a bounded choice set without creating more choices, and can it keep responses compact without erasing the distinctions needed to make those choices useful?

This is a focused composition boundary, not a new semantic field and not a reason to redefine either existing preference.

## Evaluation support

All scenarios below are single-turn observable and may be run through the current `hail-testing` workflow.

Do not infer multi-turn persistence, pacing, or follow-up behavior from these runs.

Record:

- harness;
- model;
- effort/reasoning mode;
- full HAIL profile;
- exact HAIL ref;
- exact prompt;
- fresh-session status;
- raw response;
- assessment.

Authoritative reviewed evidence belongs under `results/`. Raw runner evidence remains private in `yrwol/hail-testing`.

## Controls

Hold these values constant:

```yaml
decision_mode: options
max_options: 3
task_chunking: adaptive
step_pacing: continuous
tangent_policy: capture_and_return
```

Only `verbosity` changes:

```text
compact
balanced
detailed
```

Fixtures:

- `profiles/eval-composition-verbosity-compact.yaml`
- `profiles/eval-composition-verbosity-balanced.yaml`
- `profiles/eval-composition-verbosity-detailed.yaml`

Use a fresh session for every run.

## Scenario 1 — fixed three-choice depth control

Prompt:

```text
Give me three approaches for designing horse training progression in a cozy horse game, and explain the tradeoffs of each.
```

Run under all three verbosity values.

### Why this scenario exists

The prompt itself fixes the choice count at three, so this is primarily a **verbosity differentiation control**, not the strongest `max_options` enforcement test.

### Expected behavior

All three responses should preserve the same basic choice load:

- exactly three primary approaches;
- no hidden fourth approach, bonus option, or appended hybrid.

The useful distinction should be depth:

`compact`
- enough explanation to distinguish the three approaches;
- minimal elaboration;
- no loss of a material tradeoff needed to understand the difference.

`balanced`
- useful rationale and tradeoffs;
- moderate explanatory depth.

`detailed`
- deeper reasoning, interactions, implementation implications, examples, failure modes, or edge cases where useful;
- additional detail should deepen the existing approaches rather than create more approaches.

## Scenario 2 — open-ended cap pressure

Prompt:

```text
What are some good approaches for designing horse training progression in a cozy horse game? Explain the tradeoffs.
```

Run under all three verbosity values.

### Expected behavior

- no more than three meaningful approaches by default;
- `detailed` must not create extra approaches merely to have more material to discuss;
- `compact` should still give enough tradeoff information to make the surfaced approaches distinguishable;
- nested mechanics may illustrate an approach, but must not quietly become additional user-selectable designs;
- a hybrid/synthesis counts as another choice when presented as a distinct approach.

This is the primary `verbosity × max_options` pressure test.

## Scenario 3 — informational-list boundary

Prompt:

```text
List the core attributes a horse could have in a cozy horse game and explain what each attribute represents.
```

Run at minimum under `compact` and `detailed`; `balanced` may be retained as a useful middle control.

### Expected behavior

- `max_options: 3` does not mechanically cap an ordinary informational list;
- verbosity changes explanatory depth rather than the semantic classification of the items;
- `detailed` may explain interactions or examples without converting attributes into user choices;
- `compact` may shorten explanations but should not arbitrarily remove useful attributes merely to resemble the option cap.

## Scenario 4 — explicit current-request overrides

Two prompts test whether the current request can temporarily override the persistent defaults without mutating them.

### 4A — request more choices

Profile:

```yaml
verbosity: balanced
max_options: 3
```

Prompt:

```text
Give me 6 horse training progression approaches. Keep each explanation short.
```

Expected:

- six approaches are allowed because the user explicitly requested six;
- explanations remain concise;
- the persistent `max_options: 3` default is not treated as a hard prohibition.

### 4B — request more detail

Profile:

```yaml
verbosity: compact
max_options: 3
```

Prompt:

```text
Give me three horse training progression approaches and explain each one in detail, including tradeoffs and implementation implications.
```

Expected:

- current explicit detail request overrides the compact default for this interaction;
- still no more than the explicitly requested three approaches;
- no persistent profile mutation is implied.

## Pass criteria

The composition passes when:

- option count remains bounded independently of response depth;
- `detailed` deepens permitted choices instead of expanding the choice set;
- `compact` preserves enough information to distinguish meaningful tradeoffs;
- `max_options` constrains open-ended choice-like output without becoming generic list-length control;
- explicit current requests can override persistent choice-count/detail defaults for the current interaction;
- the already-promoted Candidate D decision/option boundaries remain intact;
- no schema change is required.

## Failure patterns

### Verbosity leaking into choice count

- `detailed` adds bonus approaches, honorable mentions, hybrids, or nested alternatives to create more content.

### Max-options flattening verbosity

- `max_options` causes detailed mode to become shallow merely because only three choices are permitted.

### Compact losing semantic usefulness

- `compact` becomes so terse that meaningful tradeoffs disappear and the choices are no longer distinguishable.

### Mechanical list capping

- ordinary informational attributes/facts are limited to three because `max_options: 3` is treated as generic bullet-count control.

### Override failure

- explicit requests for six choices are still capped at three;
- explicit requests for detailed explanation remain artificially compact.

## Interpretation order

When a run fails:

1. identify the exact observable failure;
2. classify whether `verbosity`, `max_options`, or their composition caused it;
3. compare against the current semantic contract;
4. make the smallest projection wording change possible;
5. replay the failing scenario;
6. replay at least one negative boundary/control;
7. do not promote wording without recorded evidence.

Do not redefine verbosity as response length alone, and do not redefine `max_options` as generic list length.

## Result file

Record reviewed evidence in:

```text
results/verbosity-max-options-cross-harness.md
```

## Outcome

The composition is complete for the recorded single-turn scope.

Findings:

- verbosity remained independently meaningful; no verbosity wording change was justified;
- explicit current requests overrode persistent choice-count/detail defaults as intended;
- informational lists remained outside `max_options`;
- the starting max-options projection exposed repeatable closing-synthesis/hybrid leakage;
- Candidate A over-hardened one Claude informational boundary and was rejected;
- Candidate B materially improved cross-harness enforcement while preserving boundaries;
- Candidate C's procedural internal count did not reliably eliminate the remaining synthesis leak and was rejected;
- residual closing-synthesis leakage is recorded as a prompt/harness enforcement limitation rather than a semantic gap.

Authoritative evidence:

[`results/verbosity-max-options-cross-harness.md`](results/verbosity-max-options-cross-harness.md)

The selected projection is Candidate B. Do not claim deterministic option-count compliance from prompt-only projection behavior.
