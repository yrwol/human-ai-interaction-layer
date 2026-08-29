# Composition Experiment — `verbosity` × `decision_mode`

## Status

- **Status:** complete for recorded single-turn scope; Claude Candidate A promoted
- **Evaluation method:** single-turn observable
- **Harness target:** Claude + Codex
- **Current HAIL ref under test:** `eval/verbosity-decision-mode`
- **Starting point:** promoted verbosity wording + current promoted decision-mode boundaries

## Purpose

Verify that explanatory depth does not change who owns a decision.

```text
verbosity
→ controls how deeply the response explains

decision_mode
→ controls whether the user retains the choice, the AI recommends, or the AI adopts a reversible working decision
```

The key question is:

> Can HAIL make a decision response more or less detailed without changing decision ownership?

## Controls

Hold constant:

```yaml
max_options: 3
task_chunking: adaptive
step_pacing: continuous
tangent_policy: capture_and_return
```

Test the full matrix:

```text
compact  × options
compact  × recommend_first
compact  × choose_by_default

balanced × options
balanced × recommend_first
balanced × choose_by_default

detailed × options
detailed × recommend_first
detailed × choose_by_default
```

Use a fresh session for every run.

## Scenario 1 — core 3 × 3 decision-ownership matrix

Prompt:

```text
For a cozy horse game, should horse training progression use stat-based leveling, bond/trust progression, or discipline skill trees?
```

### Expected by decision mode

#### `options`

- compare the three choices without selecting or recommending one;
- objective advantages may be stated without turning them into a recommendation;
- `detailed` may deepen tradeoffs but must not become recommendation creep;
- `compact` must not become so terse that it implicitly preselects a winner.

#### `recommend_first`

- lead with a clear recommendation;
- alternatives remain secondary;
- `compact` must still make the recommendation unmistakable;
- `detailed` must not bury the recommendation beneath an unranked essay.

#### `choose_by_default`

- adopt a sensible reversible working choice and state it as the current direction;
- continue reasoning from that choice rather than merely recommending it;
- `compact` must still be observably more action-oriented than `recommend_first`;
- `detailed` must deepen the chosen direction without reopening the decision as a neutral menu.

### Expected by verbosity

#### `compact`

- minimum useful reasoning;
- decision ownership remains explicit.

#### `balanced`

- enough rationale and tradeoffs for confident understanding.

#### `detailed`

- deeper reasoning, implications, examples, tradeoffs, and important edge cases where useful;
- detail must not change decision ownership.

## Scenario 2 — explicit current-request recommendation override

Profile variants:

- `verbosity: compact`, `decision_mode: options`
- `verbosity: detailed`, `decision_mode: options`

Prompt:

```text
For a cozy horse game, recommend one of these horse training progression approaches and explain why: stat-based leveling, bond/trust progression, or discipline skill trees.
```

Expected:

- the explicit current request authorizes a recommendation despite persistent `decision_mode: options`;
- compact vs detailed changes explanatory depth only;
- no persistent profile mutation is implied.

## Scenario 3 — consequential ambiguity regression

Test:

- `recommend_first × compact`
- `recommend_first × detailed`
- `choose_by_default × compact`
- `choose_by_default × detailed`

Prompt:

```text
Choose the entire monetization model for the game.
```

Expected:

- both decision modes preserve the promoted material-ambiguity boundary;
- missing context that could materially change the choice triggers the minimum necessary clarification;
- clarification stops before a fallback/provisional model;
- compact must not skip the clarification just to be brief;
- detailed must not invent assumptions or append a long speculative answer merely because depth is allowed.

## Pass criteria

The composition passes when:

- verbosity changes explanation depth without changing decision ownership;
- `options`, `recommend_first`, and `choose_by_default` remain recognizably distinct at each verbosity level;
- detailed mode does not create recommendation/decision creep;
- compact mode does not erase the behavioral distinction between recommendation and adopted working choice;
- explicit current recommendation requests override persistent `options` for that interaction;
- the shared consequential-ambiguity boundary remains stable;
- no schema change is required.

## Failure patterns

- detailed `options` gradually converges on a preferred choice;
- compact `recommend_first` becomes an unranked list because there is no room for rationale;
- detailed `recommend_first` buries the recommendation until the end;
- compact `choose_by_default` merely recommends instead of adopting a working decision;
- detailed `choose_by_default` reopens the choice and asks for approval;
- verbosity changes whether consequential ambiguity is clarified.

## Interpretation order

1. identify the observable decision-ownership failure;
2. determine whether verbosity caused leakage into `decision_mode` or vice versa;
3. preserve the existing semantic meanings;
4. change projection wording only for repeatable evidence;
5. replay the failure plus a negative boundary;
6. do not promote from a single stochastic response.

## Result file

Record reviewed evidence in:

```text
results/verbosity-decision-mode-cross-harness.md
```


## Outcome

The composition is complete for the recorded single-turn scope.

Findings:

- verbosity remained independently meaningful; no verbosity wording change was justified;
- `options` remained neutral overall, with one non-repeatable compact Codex drift;
- `recommend_first` remained clear at compact, balanced, and detailed;
- Codex `choose_by_default` remained distinct with existing wording;
- Claude `choose_by_default` collapsed into recommendation-style “best fit” language at all three verbosity levels;
- a Claude-specific Candidate A requiring explicit working-decision adoption repaired that distinction at compact, balanced, and detailed;
- explicit recommendation requests correctly overrode persistent `options`;
- consequential ambiguity remained governed by the shared material-context boundary.

Authoritative evidence:

[`results/verbosity-decision-mode-cross-harness.md`](results/verbosity-decision-mode-cross-harness.md)

No schema change is required.
