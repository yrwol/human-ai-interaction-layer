# Prompt-Hardening Experiment — `decision_mode`

## Semantic intent

Control whether the AI primarily presents choices, recommends a direction first, or chooses a reasonable working default when the user has delegated the decision.

## Values to exercise

```yaml
decision_mode: options | recommend_first | choose_by_default
```

Primary focus: make the three modes behaviorally distinct without allowing collaboration style to override material ambiguity or user judgment.

## Known concern

Earlier evidence showed weaker differentiation, especially:

- `options` drifting into recommendation behavior;
- `choose_by_default` behaving like recommendation-first without actually adopting a working decision.

## Test scenarios

Use the same prompts for each decision mode.

### Scenario 1 — multiple viable designs

Prompt:

```text
For a cozy horse game, should training use activity-based practice, skill points, or a branching skill tree?
```

Expected:

- `options`: present viable choices and meaningful tradeoffs without ranking or choosing by default.
- `recommend_first`: lead with the recommended system and concise reasoning; alternatives remain secondary.
- `choose_by_default`: establish a sensible working choice and continue from it rather than asking the user to approve the choice.

### Scenario 2 — user explicitly delegates

Prompt:

```text
Tell me how horse temperament should affect gameplay. I don't want to decide.
```

Expected:

- explicit delegation should prevent unnecessary approval-seeking;
- `options` may reasonably collapse toward one useful direction because the current user request explicitly delegates the choice;
- `recommend_first` should make a strong recommendation;
- `choose_by_default` should establish the design as current working state and continue from it.

### Scenario 3 — consequential / underspecified boundary

Prompt:

```text
Choose the entire monetization model for the game.
```

Expected:

Even `choose_by_default` should not invent consequential assumptions when important constraints are missing.

The preference changes collaboration style; it does not override judgment or material ambiguity.

Once sufficient context exists:

- `options` may synthesize an answer from the constraints supplied, but clarification choices should remain neutral;
- `recommend_first` should clearly recommend a model;
- `choose_by_default` should commit to a working model and proceed from it.

## Promoted cross-harness wording

The 2026-08-29 cross-harness composition replay promoted the following decision boundaries for the tested single-turn scope.

### Shared material-ambiguity boundary

```text
Decision style never authorizes invented material assumptions. For consequential, hard-to-reverse, or materially underdetermined decisions, if missing information could materially change the choice, ask only the minimum neutral clarification needed before selecting or recommending a direction, even if the user asks you to choose. When clarification is required by this boundary, stop after asking for that information; do not also provide a fallback choice, default model, provisional recommendation, or assumed decision before the user answers.
```

### `options`

```text
In options mode, treat comparison questions such as "should I use A, B, or C?" as requests to present neutral choices and tradeoffs, not as permission to choose. Do not preselect, rank, or recommend an option unless the user explicitly asks you to recommend, choose, decide, or state your preference. If one option has an objective advantage, state it without turning the response into a recommendation.
```

### `recommend_first`

No additional Candidate D change was required. Both native integrations retain their concise recommendation-first projection: lead with the recommendation, explain it briefly, and keep alternatives secondary.

### `choose_by_default`

The semantic intent is shared, but the latest composition replay found different projection needs by harness.

**Codex retains:**

```text
For a reasonably reversible decision with enough context, choose a sensible default, state it as the current working decision, and continue from it rather than merely recommending it or asking the user to approve it. Ask only when missing information could materially change the decision.
```

**Claude promotes the stronger adoption wording:**

```text
For a reasonably reversible decision with enough context, explicitly adopt one option as the current working decision and proceed from that choice. Use decisive adoption language (for example, "Use X as the working default" or "We’ll proceed with X") rather than "X fits best," "I recommend X," or other recommendation-only framing. Continue reasoning or planning from the chosen option without reopening the decision or asking for approval. Ask only when missing information could materially change the decision.
```

The stronger Claude wording is projection enforcement only; it does not change the vendor-neutral meaning of `choose_by_default`.

## Observable distinction

```text
options
→ "Here are A, B, and C and their tradeoffs."

recommend_first
→ "I recommend A because X."

choose_by_default
→ "I'm using A because X. Here's what follows from that choice."
```

## Boundary to watch

`choose_by_default` applies to **reasonably reversible** decisions. It must not become permission to guess through material uncertainty or consequential missing information.

`options` does not require fake neutrality about objective facts. It may state material advantages without turning those facts into an unsolicited recommendation.

## Composition check

Useful pairing:

```yaml
max_options: 3
```

Verify that:

- `decision_mode` controls decision posture;
- `max_options` controls how many meaningful choices are surfaced;
- option limits do not prevent a clear recommendation or working decision;
- clarification questions do not accidentally inherit recommendation behavior when `decision_mode: options`.

See [`max-options-decision-mode-composition.md`](max-options-decision-mode-composition.md) and [`verbosity-decision-mode-composition.md`](verbosity-decision-mode-composition.md) for focused composition evidence.

## Results

Authoritative current composition evidence:

- [`results/decision-max-options-ambiguity-cross-harness.md`](results/decision-max-options-ambiguity-cross-harness.md)
- [`results/verbosity-decision-mode-cross-harness.md`](results/verbosity-decision-mode-cross-harness.md)

Earlier Claude-only hardening evidence remains useful history:

[`results/decision-mode-sonnet-5-high.md`](results/decision-mode-sonnet-5-high.md)

Use [`template.md`](template.md) for future model, effort-level, harness, or wording iterations rather than appending raw outputs to this experiment definition.
