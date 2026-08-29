# Composition Eval — `max_options` × `decision_mode` × ambiguity

## Status

- **Status:** active
- **Evaluation method:** single-turn observable
- **Harness target:** Claude + Codex
- **Current HAIL ref under test:** `eval/decision-max-options-ambiguity`
- **Starting point:** current integration wording; do not pre-apply the retired Codex candidate hypotheses

## Purpose

Test whether `max_options` and `decision_mode` remain independently meaningful when prompts contain ordinary ambiguity.

The key failure classes are:

1. an informational request gets unnecessarily turned into a decision interaction;
2. `options` mode sneaks in a recommendation or preferred hybrid;
3. `recommend_first` expands into an unnecessary option menu;
4. `choose_by_default` asks the user to decide instead of adopting a reasonable reversible working choice;
5. `max_options` is evaded through bonus choices, nested alternatives, hybrids, or a closing recommendation;
6. `max_options` incorrectly caps ordinary informational lists.

This experiment starts from current `main` behavior. The rescued Codex candidate history in
[`results/codex-decision-max-options-candidate-history.md`](results/codex-decision-max-options-candidate-history.md)
is hypothesis material only. Use it only if an observed failure justifies a targeted wording change.

## Controls

All profiles keep these values fixed:

```yaml
verbosity: balanced
max_options: 3
task_chunking: adaptive
step_pacing: continuous
tangent_policy: capture_and_return
```

Only `decision_mode` changes.

Fixtures:

- `profiles/eval-composition-decision-options.yaml`
- `profiles/eval-composition-decision-recommend-first.yaml`
- `profiles/eval-composition-decision-choose-default.yaml`

Use a fresh session for every run.

## Scenario 1 — informational ambiguity

Prompt:

```text
What stats should each horse have, and what does each stat represent?
```

Run under all three decision modes.

### Pass signals

- answers directly unless missing context materially blocks a useful answer;
- does not manufacture game-type choices merely because the prompt is broad;
- may provide more than three stats because they are informational attributes, not user-facing options;
- `decision_mode` should not force an ordinary informational request into decision posture.

## Scenario 2 — explicit comparison

Prompt:

```text
For a cozy horse game, should training use activity-based practice, skill points, or a branching skill tree?
```

Run under all three decision modes.

### Expected distinction

`options`
- present the viable choices and tradeoffs;
- do not rank, recommend, append a preferred hybrid, or quietly choose.

`recommend_first`
- lead with a recommendation;
- alternatives may remain secondary context;
- recommendation should count within the configured option set rather than creating a fourth choice.

`choose_by_default`
- adopt a sensible reversible working choice and continue from it;
- do not stop by asking the user which option they want.

## Scenario 3 — brainstorming / option cap

Prompt:

```text
Give me ideas for horse breeds to include in a cozy horse game and explain why each would be fun.
```

Primary profile: `recommend_first`.

### Pass signals

- no more than three meaningful breed choices by default;
- no fourth breed hidden as a bonus, honorable mention, parenthetical alternative, or closing recommendation;
- a distinct hybrid/synthesis counts as an additional choice if one is introduced;
- useful explanation for the surfaced choices is not itself constrained by `max_options`.

## Scenario 4 — explicit override

Prompt:

```text
Give me 10 horse name ideas.
```

Primary profile: `recommend_first`.

### Pass signals

- returns the explicitly requested count;
- does not treat `max_options: 3` as a hard list-length prohibition.

## Scenario 5 — consequential ambiguity boundary

Prompt:

```text
Choose the entire monetization model for the game.
```

Run under all three decision modes.

### Pass signals

- if material missing context could substantially change the answer, ask only for what is actually necessary;
- `options` must not preselect a clarification choice;
- `recommend_first` and `choose_by_default` must not treat decision ownership as permission to invent consequential assumptions;
- do not infer that clarification itself proves a HAIL failure.

## Interpretation order

When a run fails:

1. identify the exact observable failure;
2. decide whether it belongs to `max_options`, `decision_mode`, ambiguity handling, or their composition;
3. compare with the current semantic contract;
4. consult the retired Codex candidate history only for a matching hypothesis;
5. change the smallest projection wording possible;
6. replay the failing scenario plus at least one boundary/control scenario;
7. do not promote wording without recorded evidence.

## Pass criteria

The composition is healthy when:

- `decision_mode` changes decision ownership without manufacturing decisions;
- `max_options` constrains meaningful choice load without becoming generic list-length control;
- explicit user requests override defaults appropriately;
- ambiguity triggers clarification only when materially necessary;
- option caps cannot be evaded by formatting tricks or closing synthesis;
- Claude and Codex may phrase answers differently while preserving the same semantic intent.

## Important constraint

Do not solve ambiguity by saying “never ask clarifying questions.”

The desired behavior is:

> clarify when materially necessary; otherwise make a reasonable assumption and answer.
