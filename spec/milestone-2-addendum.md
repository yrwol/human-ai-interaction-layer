# Milestone 2 Spec Addendum

This addendum records changes in understanding validated by the Claude-to-Codex portability experiment. It supplements `spec/draft/draft.md` and `spec/milestone-1-addendum.md`.

## Validated

**[VALIDATED]** The same vendor-neutral semantic profile can be translated into two meaningfully different harnesses without changing the profile schema.

**[VALIDATED]** Portability does not require identical behavior or equal enforcement across harnesses. It requires preservation of semantic intent with harness-specific translation.

**[VALIDATED]** Enforcement quality belongs to the harness/adapter compatibility boundary rather than the human-owned profile. Claude and Codex showed materially different strengths across the same five semantic dimensions.

**[OBSERVED]** Codex strongly enforced the `max_options` numeric distinction that Claude followed only weakly, while Claude more clearly differentiated `decision_mode` and `task_chunking`.

**[OBSERVED]** `tangent_policy` moved directionally in both harnesses and currently appears to be one of the more portable qualitative dimensions.

## Compatibility principle

**[PRINCIPLE] Preference-level compatibility.** HAIL compatibility should eventually describe individual semantic dimensions rather than only declaring a harness globally compatible or incompatible.

A future compatibility model should be capable of distinguishing at least:

- strong observed enforcement;
- moderate observed enforcement;
- weak observed enforcement;
- unsupported capability; and
- untested behavior.

These labels are not yet a frozen schema. Additional repeated evaluation may change the taxonomy.

**[PRINCIPLE] Do not mutate human intent to fit a harness.** Weak enforcement in one harness is not a reason to silently rewrite or remove the user's semantic preference. The adapter should express the intent as effectively as possible, while compatibility information communicates limitations.

## Portability definition

For the current HAIL work, portability means:

> A human-owned semantic preference can remain unchanged while multiple harness-specific implementations translate that preference into their native interaction mechanisms, with observable enforcement quality allowed to vary by harness.

Portability does not promise deterministic compliance, identical wording, identical response structure, or identical enforcement strength.

## Milestone 2 outcome

Milestone 2 has sufficient evidence to pass its core portability objective:

1. The same Profile A and Profile B schemas were used unchanged.
2. Claude and Codex used different delivery mechanisms.
3. Both harnesses produced observable preference-driven behavior on at least some dimensions.
4. Their differing strengths demonstrated why adapters and preference-level compatibility reporting are necessary.

## Next decision boundary

Do not infer from Milestone 2 that HAIL now needs a shared runtime, MCP server, desktop application, or packaged executable.

The next implementation should be chosen by asking what capability is required to make HAIL useful to a normal human without exposing configuration plumbing. Harness-native profile management remains preferred where sufficient.
