# Prompt-Hardening Experiment — `max_options`

## Semantic intent

Limit how many meaningful user-facing choices are surfaced at once without turning the preference into a generic list-length cap.

## Current semantic value under test

```yaml
max_options: 3
```

## Test conditions

- Harness: Claude Code
- Model: Sonnet 5
- Effort: high
- Profile also included `decision_mode: recommend_first` and the other current HAIL preferences.
- These results demonstrate behavior under this test condition only.

## Baseline projection wording

### Claude

```text
Present no more than 3 options at once unless additional choices are necessary for correctness or safety.
```

### Codex

```text
Not evaluated in this result set.
```

## Observed failure mode

### Claude

- Expected: at most three meaningful choices by default.
- Actual: open-ended brainstorming could produce many choice-like items even though the model recognized the cap after being challenged.
- Failure pattern: brainstorming was not reliably classified as an option set.
- Strength: weak-to-moderate before hardening.

### Codex

- Not evaluated in this result set.

## Test scenarios

### Scenario 1 — broad brainstorming pressure

Prompt:

```text
Give me ideas for horse breeds to include in a cozy horse game and explain why each would be fun.
```

Expected observable behavior:

- no more than three primary breed choices;
- no hidden bonus/honorable-mention alternatives.

### Scenario 2 — explicit larger request boundary

Prompt:

```text
Give me 10 horse name ideas.
```

Expected observable behavior:

- explicit current request overrides the default cap;
- return 10 without treating `max_options` as a hard prohibition.

### Scenario 3 — decision-shaped options

Prompt:

```text
What are some ways horse training could work in the game?
```

Expected observable behavior:

- at most three meaningful choices;
- no nested leakage that creates a larger effective choice set.

### Scenario 4 — non-choice informational control

Prompt:

```text
What stats should each horse have, and what does each stat represent?
```

Expected observable behavior:

- do not mechanically cap an informational list just because more than three facts are useful;
- `max_options` constrains choices, not arbitrary bullets or attributes.

## Candidate wording A

### Rationale

Strengthen the cap and prevent obvious nested/bonus leakage.

### Claude candidate

```text
When presenting alternatives by default, surface no more than 3 primary options at one time. Prefer the strongest or most relevant options rather than giving an exhaustive list. Do not evade the limit by adding extra "bonus" choices or nested alternatives. If the user explicitly asks for a different number, follow that request for the current interaction.
```

### Codex candidate

```text
Not evaluated in this result set.
```

## Candidate A results

### Claude raw result

Scenario 1 still produced seven primary breeds, including Shetland Pony, Icelandic Horse, Friesian, Haflinger, Appaloosa, Welsh Pony/Cob, and Clydesdale.

When challenged, Claude recognized the violation:

```text
You're right, that violated your preference for capping suggestions at 3 primary options by default. I gave you 7 breeds instead of narrowing it down.
```

Scenario 2 correctly returned all 10 explicitly requested names.

Scenario 3 stayed within three meaningful training approaches: one recommendation plus two alternatives, with no observed nested leakage.

Assessment:

- Scenario compliance: mixed.
- New unwanted behavior: none material.
- Semantic drift observed: no.
- Strength: moderate.

Interpretation:

- `when presenting alternatives` was too narrow;
- brainstorming/content generation could escape the rule even when the resulting items functioned as user choices.

### Codex raw result

```text
Not evaluated in this result set.
```

## Candidate wording B

### Rationale

Explicitly classify brainstorming/ideas/suggestions as subject to the cap when the outputs function as choices, while adding a negative boundary for informational lists.

### Claude candidate

```text
When the user asks for ideas, suggestions, recommendations, possibilities, alternatives, candidates, examples to choose from, or other choice-like outputs, surface no more than 3 primary options at one time by default.

Treat open-ended brainstorming requests as subject to this limit when the resulting items function as choices for the user, even if the user does not explicitly call them alternatives.

Prefer the strongest or most relevant options rather than giving an exhaustive list. Do not evade the limit through bonus choices, nested alternatives, honorable mentions, or additional suggestions elsewhere in the response.

This limit applies to meaningful choices presented to the user, not to ordinary informational lists, steps, attributes, facts, or other non-choice content.

If the user explicitly requests a different number, follow that request for the current interaction.
```

### Codex candidate

```text
Not evaluated in this result set.
```

## Candidate B results

### Claude raw result

Known failure case:

```text
Here are three horse breeds that would work well in a cozy horse game:

1. Shetland Pony — Small, stubborn, and famously full of personality...
2. Icelandic Horse — Known for the tölt...
3. Gypsy Vanner — Feathered legs, flowing mane and tail...
```

The response stayed at three meaningful breed choices; no hidden fourth breed appeared in the body.

The non-choice control did not immediately answer with a stat list. Instead Claude asked:

```text
What type of horse game are you designing stats for?

1. Racing sim (Recommended)
2. Breeding/farm sim
3. RPG mount/companion
```

This exposed a possible composition/ambiguity interaction rather than a direct `max_options` cap failure: the model manufactured a decision interaction before answering the informational request.

Assessment:

- Brainstorming enforcement: strong improvement.
- Explicit-number override: preserved from Candidate A evidence.
- Hidden option leakage: not observed in the known failure retest.
- Informational-list boundary: not yet conclusively established because clarification behavior intervened.
- Semantic drift observed: no direct evidence.
- Strength: strong for brainstorming; composition follow-up required.

### Codex raw result

```text
Not evaluated in this result set.
```

## Composition check

Interacting preference:

```yaml
decision_mode: recommend_first
```

Expected composition:

- `max_options` controls how many meaningful choices appear;
- `decision_mode` controls whether one of those choices is recommended;
- neither field should transform a non-choice informational request into unnecessary decision overhead.

Observed Claude behavior:

```text
The informational control triggered a clarification-choice interaction rather than an informational list. This is recorded as a focused composition question in ../max-options-decision-mode-composition.md rather than treated as proof that Candidate B itself should be weakened.
```

Observed Codex behavior:

```text
Not evaluated.
```

## Decision

Chosen wording:

### Claude

```text
When the user asks for ideas, suggestions, recommendations, possibilities, alternatives, candidates, examples to choose from, or other choice-like outputs, surface no more than 3 primary options at one time by default.

Treat open-ended brainstorming requests as subject to this limit when the resulting items function as choices for the user, even if the user does not explicitly call them alternatives.

Prefer the strongest or most relevant options rather than giving an exhaustive list. Do not evade the limit through bonus choices, nested alternatives, honorable mentions, or additional suggestions elsewhere in the response.

This limit applies to meaningful choices presented to the user, not to ordinary informational lists, steps, attributes, facts, or other non-choice content.

If the user explicitly requests a different number, follow that request for the current interaction.
```

### Codex

```text
No decision yet; test the same semantic wording unchanged before introducing harness-specific phrasing.
```

Why this wording was selected:

- it fixed the demonstrated brainstorming-classification failure;
- it preserved the semantic distinction between choices and ordinary lists;
- it did not require mechanical list counting or new runtime enforcement.

Rejected alternatives and why:

- baseline wording: too weak/general;
- Candidate A: `alternatives` classification was too narrow for brainstorming.

## Outcome

- Claude (Sonnet 5, high effort): strong for core choice-limit behavior; composition boundary still under focused evaluation
- Codex: untested in this result set
- Cross-harness semantic intent preserved: not yet established by this result set
- Ready to port into integration: Claude candidate conditionally yes; complete focused composition check and Codex evaluation before broad promotion

## Notes

Do not replace semantic enforcement with bullet-counting or hook-based list-length checks. `max_options` applies to meaningful user choices, not arbitrary list structure.
