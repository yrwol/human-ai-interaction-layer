# Composition Experiment — `verbosity` × `max_options`

## Purpose

Verify that response depth and simultaneous choice load remain independently controllable when composed.

```text
verbosity
→ controls explanatory depth inside the response

max_options
→ controls how many meaningful choices are surfaced at once
```

This is a focused composition boundary, not part of either field's individual hardening contract.

## Evaluation support

The core scenarios are single-turn observable and may be run through the current `hail-testing` workflow. Do not infer multi-turn persistence from these runs.

Record harness, model, effort/reasoning mode, full profile, exact projection wording, fresh-session status, and date. Store evidence under `results/` using `template.md`.

Suggested result filename:

```text
results/verbosity-max-options-<model>-<effort>.md
```

## Core scenario

Prompt:

```text
Give me three approaches for designing horse training progression in a cozy horse game, and explain the tradeoffs of each.
```

Hold all other profile values constant. Use:

```yaml
max_options: 3
```

Compare `verbosity: compact`, `balanced`, and `detailed`.

## Expected behavior

- all verbosity values surface no more than three meaningful approaches;
- `compact` explains each approach only enough to distinguish it;
- `balanced` adds useful rationale and tradeoffs;
- `detailed` adds deeper reasoning, interactions, implementation implications, examples, or edge cases where useful;
- increased verbosity must not create bonus approaches, nested alternatives, honorable mentions, or additional decision candidates that effectively escape the cap.

## Boundary control — informational list

Prompt:

```text
List the core attributes a horse could have in a cozy horse game and explain what each attribute represents.
```

Expected:

- `max_options` does not cap an ordinary informational list merely because it contains multiple items;
- verbosity changes explanatory depth without converting the list into a decision set.

## Pass criteria

The composition passes when:

- option count remains stable while explanatory depth changes;
- `detailed` deepens the permitted choices instead of expanding the choice set;
- `compact` does not remove information necessary to compare the choices;
- informational lists remain outside the choice cap;
- no schema change is required.

## Failure patterns

- detailed mode adds extra alternatives to create more content;
- nested sub-options become de facto additional choices;
- compact mode collapses meaningful distinctions between the allowed options;
- the option cap is applied mechanically to non-choice informational content.

## Interpretation rule

If this composition fails, identify which projection leaks across the semantic boundary before changing wording. Do not redefine verbosity as choice count or `max_options` as response length.