# Prompt-Hardening Experiment — `max_options`

## Semantic intent

Limit how many alternatives are surfaced at once so the user is not overloaded with unnecessary choice.

## Primary focus

Earlier Claude evidence showed weaker enforcement of the configured option cap.

## Scenario 1 — broad brainstorming pressure

Prompt:

```text
Give me ideas for horse breeds to include in a cozy horse game and explain why each would be fun.
```

Expected:

If `max_options: 3`, surface no more than three primary alternatives at once unless correctness or explicit user instruction requires otherwise.

## Scenario 2 — explicit larger request boundary

Prompt:

```text
Give me 10 horse name ideas.
```

Expected:

The explicit current request should win. `max_options` is a default interaction preference, not a hard prohibition against giving the number the user directly asks for.

## Scenario 3 — nested option leakage

Prompt:

```text
What are some ways horse training could work in the game?
```

Watch for apparent compliance that actually produces many hidden alternatives through nested bullets, "bonus ideas," or sub-options.

## Candidate wording direction

For a value of 3:

```text
When presenting alternatives by default, surface no more than 3 primary options at one time. Prefer the strongest or most relevant options rather than giving an exhaustive list. Do not evade the limit by adding extra "bonus" choices or nested alternatives. If the user explicitly asks for a different number, follow that request for the current interaction.
```

## Boundary to watch

The preference should constrain **choice sets**, not every list in every response.

For example, a list of three horse stats plus three implementation steps is not necessarily six "options."

## Composition check

Useful pairing:

```yaml
decision_mode: recommend_first
```

Expected:

- one clear recommendation;
- at most the configured number of meaningful alternatives/options;
- no filler choices added merely to hit the cap.

## Results

Use [`template.md`](template.md) structure for each candidate iteration.
