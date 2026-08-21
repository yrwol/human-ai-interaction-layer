# Prompt-Hardening Experiment — `decision_mode`

## Semantic intent

Control whether the AI primarily presents choices, recommends a direction first, or chooses a reasonable default when the user has delegated the decision.

## Values to exercise

```yaml
decision_mode: options | recommend_first | choose_by_default
```

Primary focus: make `recommend_first` behavior unmistakable without turning it into `choose_by_default`.

## Known concern

Earlier Codex evidence showed weaker differentiation for `decision_mode`.

## Scenario 1 — multiple viable designs

Prompt:

```text
For a cozy horse game, should training use activity-based practice, skill points, or a branching skill tree?
```

Expected:

- `options`: present viable choices without forcing a preferred answer.
- `recommend_first`: lead with the recommended system and concise reasoning, then mention alternatives if useful.
- `choose_by_default`: make the choice when the decision is reasonably reversible and proceed from it.

## Scenario 2 — user explicitly delegates

Prompt:

```text
Pick how horse temperament should affect gameplay. I don't want to decide.
```

Expected:

- `options`: can still surface choices because the persistent mode favors options, but should respect the explicit delegation enough to stay useful.
- `recommend_first`: make a strong recommendation.
- `choose_by_default`: choose and proceed without unnecessary approval-seeking.

## Scenario 3 — consequential / underspecified boundary

Prompt:

```text
Choose the entire monetization model for the game.
```

Expected:

Even `choose_by_default` should not blindly invent consequential assumptions when important constraints are missing. The preference changes collaboration style; it does not override judgment, safety, or material ambiguity.

## Candidate wording direction

### `options`

```text
When there are multiple reasonable paths, present the strongest options and their meaningful tradeoffs. Do not manufacture a recommendation unless the user asks for one or one option is clearly superior.
```

### `recommend_first`

```text
When a decision is needed, lead with the option you recommend and briefly explain why. Do not begin with an unranked list of alternatives. Include other options only when they add meaningful value or tradeoff context.
```

### `choose_by_default`

```text
When the user has delegated a reasonably reversible decision, choose a sensible default and continue from it instead of asking them to select from options. Surface the choice and any material assumption. Ask only when the missing information could materially change the decision.
```

## Boundary to watch

`recommend_first` must remain different from `choose_by_default`:

```text
recommend_first = "I recommend X because Y."
choose_by_default = "I'll use X because Y, and continue from there."
```

## Composition check

Useful pairing:

```yaml
max_options: 3
```

Check that option limits constrain alternatives without preventing a clear recommendation.

## Results

Use [`template.md`](template.md) structure for each candidate iteration.
