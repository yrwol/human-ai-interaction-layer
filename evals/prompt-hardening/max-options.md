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

## Promoted cross-harness wording

The 2026-08-29 cross-harness composition replay promoted this classification and enforcement wording for the tested single-turn scope:

```text
When the user asks for ideas, suggestions, recommendations, possibilities, alternatives, candidates, examples to choose from, or other choice-like outputs, surface no more than <max_options> primary options at one time by default.

Treat open-ended brainstorming requests as subject to this limit when the resulting items function as choices for the user, even if the user does not explicitly call them alternatives.

Prefer the strongest or most relevant options rather than giving an exhaustive list. Do not evade the limit through bonus choices, nested alternatives, honorable mentions, or additional suggestions elsewhere in the response.

A hybrid, synthesis, or combined approach counts as another option when it is presented as a distinct approach the user could choose; include it within the configured cap rather than appending it after the cap has already been reached.

This limit applies to meaningful choices presented to the user, not to ordinary informational lists, steps, attributes, facts, or other non-choice content.

If the user explicitly requests a different number, follow that request for the current interaction.
```

## Why this wording exists

The earlier phrase:

```text
when presenting alternatives
```

was too narrow. Earlier Claude evidence showed brainstorming could escape the rule, and the 2026-08-29 cross-harness Candidate C replay reproduced the same problem in both harnesses: Claude surfaced eight breed choices and Codex surfaced twelve under `max_options: 3`. Candidate D's explicit brainstorming classification reduced both to three while preserving useful explanation.

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

The focused composition replay confirmed that hybrids/syntheses count when they function as distinct choices, explicit current-request counts override the persistent default, and ordinary informational lists remain outside the cap. Claude showed some run-to-run ambiguity variance on the informational control, but repeat runs did not reproduce a stable regression.

See [`max-options-decision-mode-composition.md`](max-options-decision-mode-composition.md) and [`results/decision-max-options-ambiguity-cross-harness.md`](results/decision-max-options-ambiguity-cross-harness.md) for the completed evaluation.

## Results

Authoritative current cross-harness composition evidence:

[`results/decision-max-options-ambiguity-cross-harness.md`](results/decision-max-options-ambiguity-cross-harness.md)

Earlier Claude-only hardening evidence remains useful history:

[`results/max-options-sonnet-5-high.md`](results/max-options-sonnet-5-high.md)

Use [`template.md`](template.md) for future model, effort-level, harness, or wording iterations instead of appending raw outputs to this experiment definition.
