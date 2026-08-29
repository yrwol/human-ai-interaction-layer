# Codex Decision / Option Hardening — Unvalidated Candidate History

## Status

- **Source:** retired branch `eval/codex-hardening-first-two`
- **Date of experiment:** 2026-08-26
- **Evidence status:** candidate history only; **not promoted evidence**
- **Current architecture status:** source branch predates the bundled runtime references, discoverable management-skill split, YAML serialization repair, and later verbosity/task-chunking promotions
- **Use:** preserve useful hypotheses for a future Codex replay without preserving obsolete implementation code

## Why this record exists

The retired Codex hardening branch contained seven unique commits that attempted to port the already-hardened Claude `decision_mode` and `max_options` behavior into Codex and then tighten several observed or anticipated loopholes.

The branch did **not** contain a committed result record, assessment, promotion decision, or completed PR. Current HAIL documentation therefore correctly treats cross-harness hardening for these semantics as incomplete.

Do not treat any wording below as validated merely because it existed on the branch.

## Experiment shape

The branch added four temporary eval profiles:

- `decision_mode: options`
- `decision_mode: recommend_first`
- `decision_mode: choose_by_default`
- `max_options: 3` with `decision_mode: recommend_first`

It then applied three rounds of projection tightening to the Codex root skill.

Representative source commits:

- `a036aa82666615b3fcb9958bbc8c3b22e22c6888` — add Codex options eval profile
- `e14f01fb57705e654c9fa1cb5607f2a98197d3d4` — add recommend-first eval profile
- `2246982735f1102640d546e53740dd523cd91a2b` — add choose-by-default eval profile
- `79f8922fbf0a286423acacc8492528595b370bcc` — port hardened decision/max-options wording to Codex
- `5718438ef394b360b54fbad8341ba3d35ace429f` — add max-options eval profile
- `07b31f72149cf42f0971638eaefecd25cb7f1311` — harden decision boundaries and option leakage
- `4760d06379716b3f3f0ede0cf85dcb11d6b04055` — close remaining decision/option loopholes

## Candidate hypotheses worth preserving

### Shared material-ambiguity boundary

The branch proposed making this boundary explicit across all decision modes:

> Decision style must not authorize invented material assumptions. For consequential, hard-to-reverse, or materially underdetermined decisions, ask only for the minimum information needed before selecting or recommending a direction.

The final branch wording additionally required the model to stop after a materially necessary clarification rather than simultaneously supplying a fallback decision. That stronger stop behavior remains **unvalidated** and may interact with pacing.

### `decision_mode: options`

The branch attempted to make options mode operationally neutral:

- comparison prompts should not silently become recommendations;
- clarification choices should not be preselected or labeled recommended;
- objective advantages may be stated without converting them into a choice;
- the response should not append a hidden preferred direction, synthesis, or hybrid after presenting neutral options.

This is a plausible Codex hardening hypothesis, not promoted wording.

### `decision_mode: recommend_first`

The branch retained the Claude-derived behavior:

- when enough material context exists, lead with the recommendation;
- do not begin with an unranked menu;
- alternatives are secondary tradeoff context.

This aligns with the current semantic contract but still requires Codex evidence before cross-harness promotion.

### `decision_mode: choose_by_default`

The branch attempted to strengthen the working-state distinction:

- only when the user has delegated a reasonably reversible choice and enough context exists;
- choose a sensible default;
- treat it as current working state;
- continue using it rather than stopping for approval;
- state only non-material assumptions briefly.

This is specifically intended to keep `choose_by_default` behaviorally distinct from `recommend_first`.

### `max_options`

The branch expanded the classification boundary beyond literal alternatives:

- ideas;
- suggestions;
- recommendations;
- possibilities;
- candidates;
- examples to choose from;
- open-ended brainstorming when the output functions as user choices.

It also proposed these anti-leakage rules:

- bonus choices, nested alternatives, and honorable mentions still count;
- a hybrid/synthesis/combined approach counts as another option when presented as a distinct approach;
- after the configured cap is reached, a closing recommendation should select from the already-surfaced options rather than introduce a new hybrid.

The last point is explicitly a `max_options × decision_mode` composition hypothesis and should be tested through the dedicated composition experiment rather than silently embedded as proven field behavior.

## What must not be rescued from the source branch

Do **not** cherry-pick or merge the old root Codex skill implementation.

The source branch predates current architecture and would regress:

- bundled runtime profile references;
- centralized normalization/default/schema contracts;
- discoverable action skills;
- YAML-safe `task_chunking: "off"` writes;
- later promoted verbosity wording;
- later promoted task-chunking wording.

The temporary eval profiles are also trivial to recreate from current schema if needed and are not themselves evidence.

## How to reuse this later

When `decision_mode` and `max_options` return to active cross-harness hardening:

1. branch fresh from current `main`;
2. begin with current promoted/retained semantic wording, not the retired branch implementation;
3. replay the Claude-derived candidate unchanged in Codex first;
4. use the hypotheses above only when a concrete Codex failure justifies them;
5. record raw results and assessment under `evals/prompt-hardening/results/`;
6. use `max-options-decision-mode-composition.md` for hybrid/cap/ambiguity interactions;
7. promote only the smallest wording change supported by evidence.

## Bottom line

The retired branch contained **useful research hypotheses, not promotable code or evidence**.

Preserve this record; retire the branch.
