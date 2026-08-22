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

## Current Claude candidate wording

These are the current promotion candidates from the Sonnet 5 / high-effort experiment. They are not yet universal cross-harness wording.

### `options`

```text
When presenting choices for the user to answer, including clarification questions, do not preselect, rank, or label an option as recommended unless the user asks for guidance. If one option has a material, objective advantage, state that advantage without converting the response into a recommendation unless the user asks you to choose.
```

### `recommend_first`

```text
When a decision is needed, lead with the option you recommend and briefly explain why. Do not begin with an unranked list of alternatives. Include other options only when they add meaningful value or tradeoff context.
```

### `choose_by_default`

```text
When the user has delegated a reasonably reversible decision, choose a sensible default and treat it as the current working decision. Continue the task using that choice rather than stopping to ask for approval or presenting the choice back to the user. State the choice and any material assumption briefly. Ask only when missing information could materially change the decision.
```

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

See [`max-options-decision-mode-composition.md`](max-options-decision-mode-composition.md) for the focused composition evaluation.

## Results

Sonnet 5 / high-effort Claude results have been moved to:

[`results/decision-mode-sonnet-5-high.md`](results/decision-mode-sonnet-5-high.md)

Use [`template.md`](template.md) for future model, effort-level, harness, or wording iterations rather than appending raw outputs to this experiment definition.
