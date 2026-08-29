# Cross-Harness Composition Eval — `max_options × decision_mode × ambiguity`

## Status

- **Harnesses:** Claude Code + Codex
- **Claude model:** Sonnet
- **Codex model:** GPT-5.5, high reasoning effort
- **HAIL ref:** `eval/decision-max-options-ambiguity`
- **Method:** one prompt in a fresh session per profile/harness
- **Current status:** in progress

## Scenario 1 — informational ambiguity

Prompt:

```text
What stats should each horse have, and what does each stat represent?
```

Profiles:

- `decision_mode: options`, `max_options: 3`
- `decision_mode: recommend_first`, `max_options: 3`
- `decision_mode: choose_by_default`, `max_options: 3`

Private raw runs:

- `33227680825` — options
- `33227679250` — recommend_first
- `33227683283` — choose_by_default

### Claude

All three profiles answered the informational request directly.

- `options` supplied three core stats and then additional situational stats; it did not ask the user to choose a game type first.
- `recommend_first` proposed four core stats plus three clearly secondary additions; it did not mechanically cap the informational list at three.
- `choose_by_default` supplied a broader core/secondary stat set and only invited later tailoring after answering.

No run manufactured a blocking decision interaction.

### Codex

All three profiles answered directly with substantial informational stat lists.

- none capped the response at three facts;
- none asked the user to select a game type before answering;
- none converted ordinary ambiguity into a required decision.

The exact stat sets differed naturally across fresh sessions, but the interaction mechanic remained stable.

## Scenario 1 assessment

**PASS.**

The earlier Claude failure where an informational stat prompt became a game-type clarification menu did not reproduce under the current integrations in either harness.

Current evidence therefore does **not** justify adding stronger ambiguity-handling or clarification-suppression wording.

This also supports the intended negative boundary:

```text
max_options
!= arbitrary informational list length
```

### Important nuance

Claude `options` happened to present three “core” stats before mentioning additional situational stats. That is not treated as a `max_options` failure because the prompt requested informational attributes, not a set of alternatives for the user to choose among.

Do not infer semantic enforcement from item count alone.

## Next scenario

Run the same explicit three-way design choice under all three decision modes:

```text
For a cozy horse game, should training use activity-based practice, skill points, or a branching skill tree?
```

This directly tests whether decision ownership remains distinct while `max_options: 3` stays constant.
