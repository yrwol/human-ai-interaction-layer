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

## Scenario 2 — explicit three-way comparison

Prompt:

```text
For a cozy horse game, should training use activity-based practice, skill points, or a branching skill tree?
```

Private raw runs:

- `33227840071` — options
- `33227837737` — recommend_first
- `33227842551` — choose_by_default

### Observed behavior

#### `options`

**FAIL in both harnesses.**

Claude immediately said it would lean toward activity-based practice. Codex likewise made activity-based practice the core system and ended with an explicit recommendation.

This is not a subtle formatting issue: both harnesses converted a comparison into a recommendation despite `decision_mode: options`.

Likely cause: the current projection wording says not to force a recommendation “unless one is clearly warranted.” A prompt shaped as “should I use A, B, or C?” gives both harnesses enough latitude to decide that a recommendation is warranted, which defeats the intended ownership distinction.

#### `recommend_first`

**PASS on decision posture; FAIL on option-cap composition.**

Both harnesses clearly led with activity-based practice, which matches the mode.

However, each then introduced a combined activity-based + light-perk approach after discussing the three original systems. That combined approach is a distinct user-selectable design and therefore acts as an additional option beyond the configured cap of three.

#### `choose_by_default`

**PARTIAL / FAIL differentiation and option-cap composition.**

Both harnesses selected activity-based practice, but their behavior remained too close to `recommend_first`:

- Claude framed activity-based practice as the “best fit” and then suggested a hybrid, ending with an offer to explore it.
- Codex framed activity-based practice as the best fit and then constructed a hybrid model combining activity practice, skill points, and light branches.

Neither response made the “current working decision” mechanic especially observable.

Both also introduced a distinct hybrid/synthesis beyond the original three choices.

## Scenario 2 assessment

**Composition failure reproduced across both harnesses.**

Two independent issues are supported by the evidence:

1. `decision_mode: options` needs a narrower recommendation boundary so comparison syntax does not itself authorize choosing.
2. `max_options` needs an explicit anti-leakage rule stating that a hybrid/synthesis counts as another meaningful choice when presented as a distinct approach.

A smaller `choose_by_default` clarification is also justified because the current output is not reliably distinguishable from recommendation-first behavior.

## Candidate C — smallest shared wording repair

Apply the same semantic wording to both harnesses first.

### `options`

```text
In options mode, treat comparison questions such as "should I use A, B, or C?" as requests to present neutral choices and tradeoffs, not as permission to choose. Do not preselect, rank, or recommend an option unless the user explicitly asks you to recommend, choose, decide, or state your preference. If one option has an objective advantage, state it without turning the response into a recommendation.
```

### `choose_by_default`

```text
For a reasonably reversible decision with enough context, choose a sensible default, state it as the current working decision, and continue from it rather than merely recommending it or asking the user to approve it. Ask only when missing information could materially change the decision.
```

### `max_options`

```text
Present no more than <max_options> meaningful user-facing choices at once unless additional choices are necessary for correctness or safety. A hybrid, synthesis, or combined approach counts as another choice when presented as a distinct approach; do not append one after the configured cap is already reached.
```

These changes intentionally do **not** add the stronger retired-branch clarification-stop rule. Scenario 1 provided no evidence that broad ambiguity handling needs additional hardening.

## Next replay

Replay Scenario 2 under all three decision modes using Candidate C, then rerun the informational Scenario 1 boundary to ensure the stronger choice wording does not transform ordinary informational lists into capped choice sets.
