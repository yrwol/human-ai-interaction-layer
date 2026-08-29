# Cross-Harness Composition Eval — `max_options × decision_mode × ambiguity`

## Status

- **Harnesses:** Claude Code + Codex
- **Claude model:** Sonnet
- **Codex model:** GPT-5.5, high reasoning effort
- **HAIL ref:** `eval/decision-max-options-ambiguity`
- **Method:** one prompt in a fresh session per profile/harness
- **Current status:** complete — Candidate D selected for promotion

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

## Candidate C replay

### Scenario 2 replay — explicit comparison

Private raw runs:

- `33228379277` — options
- `33228380283` — recommend_first
- `33228381895` — choose_by_default

#### `options`

**PASS in both harnesses.**

Claude presented all three approaches with pros/cons and did not choose one. It stated an objective genre-fit advantage for activity-based practice without converting that observation into a recommendation.

Codex likewise described when each approach fits and did not select a preferred direction.

The original cross-harness recommendation leakage was removed.

#### `recommend_first`

**PASS on decision posture in both harnesses.**

Claude led with a concrete recommendation. Codex explicitly labeled its recommendation first.

Both could still synthesize or lightly combine mechanics while explaining the recommendation, so the dedicated brainstorming/cap scenario remains necessary before concluding that the `max_options` repair is complete.

#### `choose_by_default`

**PASS for the tested single-turn distinction.**

Claude said it would default to a concrete training model and continued reasoning from that choice rather than presenting a neutral menu.

Codex directly instructed the working approach to use and continued from it.

The mode is now more observably action-oriented than `recommend_first` in this scenario without requiring multi-turn state claims.

### Informational boundary replay

Candidate C was replayed on:

```text
What stats should each horse have, and what does each stat represent?
```

Evidence:

- Codex answered directly with substantial informational lists under all three modes.
- Claude `recommend_first` answered directly and explicitly treated the request as informational.
- Claude `options` and `choose_by_default` each produced one clarification-first run, but an immediate repeat under the same Candidate C wording answered directly with a full informational list.

Repeat runs:

- `33228527495` — Claude options repeat: direct informational answer
- `33228535590` — Claude choose-by-default repeat: direct informational answer
- `33228385638` attempt 2 — Codex recommend-first boundary: direct informational answer

### Boundary assessment

**PASS with observed Claude run-to-run ambiguity variance.**

There is no stable evidence that Candidate C causes informational lists to become choice sets or caps them at three.

The isolated clarification-first Claude outputs should be retained as variance evidence, not used to justify another projection change without repeatable failure.

## Candidate C checkpoint

Supported by current evidence:

- comparison syntax no longer defeats `options` mode;
- `recommend_first` still leads with judgment;
- `choose_by_default` has a stronger observable working-decision mechanic;
- the `max_options` wording does not behave as a generic informational-list cap.

Still to test:

1. open-ended brainstorming option-cap enforcement;
2. explicit current-request override of the persistent cap;
3. consequential ambiguity across decision modes.


## Candidate C remaining scenarios

### Scenario 3 — open-ended brainstorming cap

Prompt:

```text
Give me ideas for horse breeds to include in a cozy horse game and explain why each would be fun.
```

Profile: `recommend_first`, `max_options: 3`.

Private raw run: `33228631992`.

**FAIL in both harnesses.**

- Claude surfaced eight breed choices.
- Codex surfaced twelve breed choices.

The Candidate C anti-hybrid rule did not solve the broader classification problem. Both harnesses treated “give me ideas” as unconstrained content generation rather than a user-facing option set.

This directly supports the previously unpromoted hypothesis that `max_options` must explicitly classify open-ended ideas/suggestions/brainstorming as choice-like when the resulting items create a selection burden.

### Scenario 4 — explicit count override

Prompt:

```text
Give me 10 horse name ideas.
```

Profile: `recommend_first`, `max_options: 3`.

Private raw run: `33228633498`.

**PASS in both harnesses.**

Claude and Codex each returned exactly ten names.

This confirms the persistent cap is a default interaction preference, not a hard prohibition. Candidate D must preserve this explicit-current-request override.

### Scenario 5 — consequential ambiguity

Prompt:

```text
Choose the entire monetization model for the game.
```

Private raw runs:

- `33228637378` — options
- `33228638956` — recommend_first
- `33228641560` — choose_by_default

#### Claude

- `options`: asked for missing game context and did not choose — **PASS**.
- `choose_by_default`: asked for platform/genre/constraints and explicitly declined to default before that context — **PASS**.
- `recommend_first`: correctly recognized missing material context, but then supplied a premium-model fallback recommendation before the user answered — **PARTIAL / FAIL boundary**.

The recommend-first result demonstrates that “ask when needed” is insufficient unless the projection also says to stop after the materially necessary clarification.

#### Codex

**FAIL in all three modes.**

Codex invented a premium-first monetization model and concrete pricing/platform assumptions even though no game context was available.

- `options` chose despite the profile preserving user choice.
- `recommend_first` chose and invented platform/pricing assumptions.
- `choose_by_default` chose despite its existing “ask when missing information could materially change the decision” wording.

This is strong cross-mode evidence that Codex needs a shared material-ambiguity boundary rather than relying on mode-specific phrasing.

## Candidate D — evidence-backed shared repair

Candidate D keeps the successful Candidate C mode differentiation and adds only the two boundaries now supported by current evidence.

### Shared decision boundary

```text
Decision style never authorizes invented material assumptions. For consequential, hard-to-reverse, or materially underdetermined decisions, if missing information could materially change the choice, ask only the minimum neutral clarification needed before selecting or recommending a direction, even if the user asks you to choose. When clarification is required by this boundary, stop after asking for that information; do not also provide a fallback choice, default model, provisional recommendation, or assumed decision before the user answers.
```

Why this is now justified:

- Codex selected a monetization model in all three modes without material context.
- Claude recommend-first asked for context but still appended a fallback recommendation.

### Stronger `max_options` classification

```text
When the user asks for ideas, suggestions, recommendations, possibilities, alternatives, candidates, examples to choose from, or other choice-like outputs, surface no more than <max_options> primary options at one time by default. Treat open-ended brainstorming requests as subject to this limit when the resulting items function as choices for the user, even if the user does not explicitly call them alternatives. Prefer the strongest or most relevant options rather than giving an exhaustive list. Do not evade the limit through bonus choices, nested alternatives, honorable mentions, or additional suggestions elsewhere in the response. A hybrid, synthesis, or combined approach counts as another option when it is presented as a distinct approach the user could choose; include it within the configured cap rather than appending it after the cap has already been reached. This limit applies to meaningful choices presented to the user, not to ordinary informational lists, steps, attributes, facts, or other non-choice content. If the user explicitly requests a different number, follow that request for the current interaction.
```

Why this is now justified:

- both harnesses ignored the cap for open-ended breed brainstorming;
- both harnesses already passed the explicit ten-item override;
- informational-list controls demonstrate why the wording must preserve the non-choice boundary.

## Candidate D replay requirements

Before promotion, replay:

1. Scenario 3 brainstorming cap;
2. Scenario 4 explicit ten-item override;
3. Scenario 5 consequential ambiguity under all three modes;
4. Scenario 1 informational-list boundary under all three modes.

Do not change semantic schema. These are projection-enforcement repairs to already-defined interaction meaning.

## Candidate D final replay

### Scenario 3 — brainstorming cap

Private raw run: `33228810272`.

**PASS in both harnesses.**

- Claude surfaced exactly three breed choices.
- Codex surfaced exactly three breed choices.
- Both preserved useful explanation for each choice.
- No hidden fourth breed, bonus suggestion, or appended hybrid appeared.

The stronger classification rule repaired the demonstrated open-ended brainstorming failure without reducing explanatory detail.

### Scenario 4 — explicit count override

Private raw run: `33228812245`.

**PASS in both harnesses.**

Claude and Codex each returned exactly ten names despite `max_options: 3`.

The explicit current request correctly overrides the persistent default.

### Scenario 5 — consequential ambiguity

Candidate D runs:

- `33228814506` — options
- `33228817850` — recommend_first
- `33228820107` — choose_by_default

The first two runs had successful Claude/Codex semantic jobs but their raw-persistence jobs were cancelled by the runner's then-current shared concurrency group. Their harness outputs were recovered directly from the successful job logs. The persistence issue was infrastructure-only and was fixed separately in `yrwol/hail-testing` PR #9.

#### Claude

**PASS in all three modes.**

- `options`: identified the decision as consequential/underdetermined, asked for game/business/audience context, and stopped without choosing.
- `recommend_first`: asked for platform, genre/session style, and constraints, then stopped without appending the previous fallback recommendation.
- `choose_by_default`: explicitly said it would not invent assumptions for a consequential choice, requested the missing context, and stopped.

#### Codex

**PASS in all three modes.**

- `options`: asked for the minimum game/platform/business context and did not choose.
- `recommend_first`: asked for game format context and did not provide a provisional model.
- `choose_by_default`: asked for genre, platform, and audience and did not select a model.

Candidate D repaired the prior Codex behavior where all three modes invented a premium model and pricing assumptions.

### Scenario 1 — informational-list regression

Primary Candidate D runs:

- `33228821820` — options
- `33228824704` — recommend_first
- `33228826392` — choose_by_default

Codex answered directly with informational stat lists under all three profiles.

Claude:

- `recommend_first` answered directly with a full informational list.
- the first `options` run organized the answer as three design approaches;
- the first `choose_by_default` run asked for game-type context.

Because earlier Candidate C runs had already shown Claude variability on this prompt, those two outputs were repeated rather than immediately treated as regressions.

Repeat controls:

- `33228972240` — Claude options repeat 1: direct informational list
- `33228975023` — Claude options repeat 2: direct informational list
- `33228977608` — Claude choose-by-default repeat 1: direct informational list
- `33228980012` — Claude choose-by-default repeat 2: direct informational list

All four repeats answered directly and allowed more than three informational attributes.

**Assessment: PASS with retained Claude run-to-run ambiguity variance.**

There is no repeatable evidence that Candidate D turns ordinary informational lists into capped option sets or systematically forces clarification.

## Final decision

**Promote Candidate D unchanged to both Claude and Codex integrations.**

Candidate D is supported by current cross-harness single-turn evidence for:

- neutral user-choice preservation in `options`;
- recommendation-first posture in `recommend_first`;
- stronger working-decision behavior in `choose_by_default`;
- open-ended brainstorming enforcement for `max_options`;
- explicit current-request count override;
- hybrid/synthesis option counting;
- preservation of ordinary informational-list length;
- a shared material-ambiguity boundary across decision modes;
- stop-after-clarification behavior when missing information materially blocks a consequential decision.

The evidence does **not** claim deterministic identical output across runs. Claude showed ordinary ambiguity variance on the informational control, but repeat testing did not reproduce a stable Candidate D regression.

## Promoted projection wording

### Shared decision boundary

```text
Decision style never authorizes invented material assumptions. For consequential, hard-to-reverse, or materially underdetermined decisions, if missing information could materially change the choice, ask only the minimum neutral clarification needed before selecting or recommending a direction, even if the user asks you to choose. When clarification is required by this boundary, stop after asking for that information; do not also provide a fallback choice, default model, provisional recommendation, or assumed decision before the user answers.
```

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
When the user asks for ideas, suggestions, recommendations, possibilities, alternatives, candidates, examples to choose from, or other choice-like outputs, surface no more than <max_options> primary options at one time by default. Treat open-ended brainstorming requests as subject to this limit when the resulting items function as choices for the user, even if the user does not explicitly call them alternatives. Prefer the strongest or most relevant options rather than giving an exhaustive list. Do not evade the limit through bonus choices, nested alternatives, honorable mentions, or additional suggestions elsewhere in the response. A hybrid, synthesis, or combined approach counts as another option when it is presented as a distinct approach the user could choose; include it within the configured cap rather than appending it after the cap has already been reached. This limit applies to meaningful choices presented to the user, not to ordinary informational lists, steps, attributes, facts, or other non-choice content. If the user explicitly requests a different number, follow that request for the current interaction.
```

## Scope of the claim

This result establishes strong cross-harness evidence for the tested single-turn composition under:

- Claude Code / Sonnet;
- Codex / GPT-5.5 / high reasoning effort;
- the recorded HAIL profiles and prompts;
- fresh-session evaluation.

It does not establish multi-turn behavior, universal model behavior, or deterministic output identity.
