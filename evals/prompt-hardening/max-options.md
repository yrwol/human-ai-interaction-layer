# Prompt-Hardening Experiment — `max_options`

## Semantic intent

Limit how many meaningful user-facing choices are surfaced at once so the user is not overloaded, without turning the preference into a generic cap on all lists.

## Value under test

```yaml
max_options: 3
```

## Known concern

Earlier Claude evidence showed weaker enforcement of the configured option cap, especially for open-ended brainstorming.

The important distinction is:

```text
max_options
!= arbitrary list length

max_options
= meaningful user-facing choice count
```

## Test scenarios

### Scenario 1 — broad brainstorming pressure

Prompt:

```text
Give me ideas for horse breeds to include in a cozy horse game and explain why each would be fun.
```

Expected:

- no more than three primary breed choices by default;
- no hidden bonus, honorable-mention, parenthetical, or nested breed alternatives;
- enough detail to make each choice useful.

### Scenario 2 — explicit larger request boundary

Prompt:

```text
Give me 10 horse name ideas.
```

Expected:

The explicit current request wins. `max_options` is a default interaction preference, not a hard prohibition.

### Scenario 3 — decision-shaped options

Prompt:

```text
What are some ways horse training could work in the game?
```

Expected:

- no more than three meaningful training choices;
- no nested leakage that silently creates a larger effective option set.

### Scenario 4 — non-choice informational control

Prompt:

```text
What stats should each horse have, and what does each stat represent?
```

Expected:

- do not mechanically limit an informational list to three items;
- ordinary attributes, facts, steps, and other non-choice content are outside the semantic cap.

## Current Claude candidate wording

This is the current candidate produced by the Sonnet 5 / high-effort experiment. It is not yet established as universal cross-harness wording.

```text
When the user asks for ideas, suggestions, recommendations, possibilities, alternatives, candidates, examples to choose from, or other choice-like outputs, surface no more than 3 primary options at one time by default.

Treat open-ended brainstorming requests as subject to this limit when the resulting items function as choices for the user, even if the user does not explicitly call them alternatives.

Prefer the strongest or most relevant options rather than giving an exhaustive list. Do not evade the limit through bonus choices, nested alternatives, honorable mentions, or additional suggestions elsewhere in the response.

This limit applies to meaningful choices presented to the user, not to ordinary informational lists, steps, attributes, facts, or other non-choice content.

If the user explicitly requests a different number, follow that request for the current interaction.
```

## Why this wording exists

The earlier phrase:

```text
when presenting alternatives
```

was too narrow. Claude could interpret brainstorming as content generation rather than option presentation, then produce many user-facing choices despite understanding the cap after the fact.

The current candidate adds an explicit classification rule:

```text
ideas / suggestions / recommendations / possibilities
                    ↓
       subject to max_options
 when they function as user choices
```

It also defines the negative boundary so the rule should not become a generic list-length constraint.

## Boundary to watch

Do not replace this semantic with mechanical bullet counting or hook-based list-length enforcement.

For example, three horse stats plus three implementation steps are not necessarily six options.

## Composition check

Useful pairing:

```yaml
decision_mode: recommend_first
```

Expected:

- one clear recommendation when decision mode calls for one;
- no more than the configured number of meaningful choices;
- no filler options added merely to hit the cap;
- no unnecessary conversion of informational requests into decision interactions.

The informational-control run exposed a focused composition question when Claude asked a clarification question instead of answering the stat list directly.

See [`max-options-decision-mode-composition.md`](max-options-decision-mode-composition.md) for that evaluation.

## Results

Sonnet 5 / high-effort Claude results have been moved to:

[`results/max-options-sonnet-5-high.md`](results/max-options-sonnet-5-high.md)

Use [`template.md`](template.md) for future model, effort-level, harness, or wording iterations instead of appending raw outputs to this experiment definition.
