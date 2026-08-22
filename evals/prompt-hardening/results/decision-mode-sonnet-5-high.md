# Prompt-Hardening Experiment — `decision_mode`

## Semantic intent

Control whether the AI primarily presents choices, recommends a direction first, or chooses a reasonable working default when the user has delegated the decision.

## Current semantic value under test

```yaml
decision_mode: options | recommend_first | choose_by_default
```

## Test conditions

- Harness: Claude Code
- Model: Sonnet 5
- Effort: high
- Profile also included the other current HAIL preferences, including `max_options: 3`.
- These results demonstrate behavior under this test condition; they are not yet evidence of universal cross-model or cross-harness durability.

## Baseline projection wording

### Claude

```text
When helping with decisions, give your recommended option first, then briefly explain why before presenting alternatives.
```

### Codex

```text
Not evaluated in this result set.
```

## Observed failure mode

### Claude

- Expected: visibly distinct collaboration behavior for `options`, `recommend_first`, and `choose_by_default`.
- Actual: earlier wording allowed `options` to drift toward recommendations and `choose_by_default` to behave like recommendation-first without clearly committing to a working decision.
- Failure pattern: weak differentiation between modes.
- Strength before hardening: moderate / inconsistent.

### Codex

- Not evaluated in this result set.

## Test scenarios

### Scenario 1 — multiple viable designs

Prompt:

```text
For a cozy horse game, should training use activity-based practice, skill points, or a branching skill tree?
```

Expected observable behavior:

- `options`: present viable choices and tradeoffs without ranking or choosing.
- `recommend_first`: lead with a recommendation and rationale.
- `choose_by_default`: establish a reasonable working choice and proceed from it.

### Scenario 2 — user explicitly delegates

Prompt:

```text
Tell me how horse temperament should affect gameplay. I don't want to decide.
```

Expected observable behavior:

- explicit delegation should prevent unnecessary approval-seeking;
- the modes should still differ in whether they present, recommend, or adopt a direction.

### Scenario 3 — consequential / underspecified boundary

Prompt:

```text
Choose the entire monetization model for the game.
```

Expected observable behavior:

- material ambiguity should still trigger clarification;
- decision style must not authorize invented consequential assumptions;
- once enough context exists, the selected mode should resume its intended behavior.

## Candidate wording A

### Rationale

Make each mode behaviorally observable rather than stylistically different.

### Claude candidate — `options`

```text
Present the strongest viable options and their meaningful tradeoffs without ranking or selecting among them by default. If one option has a material, objective advantage, state that advantage without converting the response into a recommendation unless the user asks you to choose.
```

### Claude candidate — `recommend_first`

```text
When a decision is needed, lead with the option you recommend and briefly explain why. Do not begin with an unranked list of alternatives. Include other options only when they add meaningful value or tradeoff context.
```

### Claude candidate — `choose_by_default`

```text
When the user has delegated a reasonably reversible decision, choose a sensible default and continue from it instead of asking them to select from options. Surface the choice and any material assumption. Ask only when the missing information could materially change the decision.
```

### Codex candidate

```text
Not evaluated in this result set.
```

## Candidate A results

### Claude — `options`

Scenario 1 initially became much more neutral, but Scenario 3 clarification UI still leaked a recommendation label such as:

```text
Cosmetic-only / no power for sale (Recommended)
```

Assessment:

- Scenario compliance: improved substantially.
- New unwanted behavior: recommendation leakage inside clarification choices.
- Semantic drift observed: no.
- Strength: moderate-to-strong, but not ready.

### Claude — `recommend_first`

Scenario 1:

```text
Activity-based practice is the strongest fit — recommend that as the core system.

Why: Cozy games succeed by making progress feel like an extension of play, not a management task...

Where the other two still have a place:
- Skill points work well as a light seasoning on top of activity-based training...
- Branching skill tree is worth it only if you want meaningful build specialization...

My suggestion: activity-based training as the loop, with occasional milestone points for small customization...
```

Scenario 2 began with a concrete recommendation:

```text
Here's the system I'd build: temperament creates a risk/reward tradeoff between control and performance ceiling, not a pure stat modifier.
```

Scenario 3 first gathered materially necessary context, then produced:

```text
Given a browser game optimized for reach, with pay-to-win off the table, here's the model I'd choose:

Recommended model: Free-to-play, cosmetic/convenience monetization + optional rewarded ads
```

Assessment:

- Scenario compliance: strong across all three scenarios.
- New unwanted behavior: none material.
- Semantic drift observed: no.
- Strength: strong.

### Claude — `choose_by_default`

Candidate A made choices, but Scenario 1 and 2 still frequently ended with approval/refinement questions and did not strongly demonstrate treating the choice as ongoing working state.

Assessment:

- Scenario compliance: partial.
- New unwanted behavior: behavior remained too close to `recommend_first`.
- Semantic drift observed: no.
- Strength: moderate.

### Codex raw result

```text
Not evaluated in this result set.
```

## Candidate wording B

### Rationale

Address concrete failures only: neutralize clarification choices for `options`, and make working-state continuation explicit for `choose_by_default`.

### Claude candidate — `options` v2

```text
When presenting choices for the user to answer, including clarification questions, do not preselect, rank, or label an option as recommended unless the user asks for guidance. If one option has a material, objective advantage, state that advantage without converting the response into a recommendation unless the user asks you to choose.
```

### Claude candidate — `choose_by_default` v2

```text
When the user has delegated a reasonably reversible decision, choose a sensible default and treat it as the current working decision. Continue the task using that choice rather than stopping to ask for approval or presenting the choice back to the user. State the choice and any material assumption briefly. Ask only when missing information could materially change the decision.
```

### Codex candidate

```text
Not evaluated in this result set.
```

## Candidate B results

### Claude — `options` v2

Scenario 3 clarification choices became neutral:

```text
What's the primary business goal for monetization?

1. Maximize revenue per player
2. Maximize player goodwill/retention
3. Simple, low-maintenance revenue
```

No option was preselected or labeled recommended. After the user supplied constraints, Claude synthesized a concrete answer rather than remaining artificially neutral:

```text
Given a web-based game with retention/goodwill as the priority, here's the model:

Free-to-play core + optional cosmetics + opt-in rewarded ads
```

Assessment:

- Scenario compliance: strong.
- New unwanted behavior: none observed.
- Semantic drift observed: no.
- Strength: strong.

### Claude — `choose_by_default` v2

Scenario 1 established a concrete hybrid choice immediately:

```text
Skill points, layered onto activity-based practice: horses get better at specific things ... and points earned that way unlock a light branching tree for flavor/specialization.
```

The follow-up question was refinement-oriented rather than approval-seeking.

Scenario 2 provided the clearest working-state evidence:

```text
Working decision: temperament isn't a stat block — it's a risk/reward gate on performance, expressed through two axes:
...
I'll treat this as the working model going forward — let me know if you want to redirect any part of it.
```

Scenario 3 correctly refused to invent material game context, asked for a minimal description, then committed:

```text
Given the genre (browser-based horse breed/own/train/show ...), I'll go with the model that's proven to work there rather than asking further:

Monetization model: Free-to-play, non-pay-to-win, with three revenue levers
...
That's the working model — let me know if you want to swap any lever ... otherwise I'll treat this as decided and build against it.
```

Assessment:

- Scenario compliance: strong.
- New unwanted behavior: none material.
- Semantic drift observed: no.
- Strength: strong.

## Composition check

Interacting preference:

```yaml
max_options: 3
```

Expected composition:

- option limits constrain alternatives without preventing a recommendation or working choice;
- `decision_mode` controls decision posture, while `max_options` controls the number of meaningful choices surfaced.

Observed Claude behavior:

```text
No material conflict observed in this test set. A dedicated composition evaluation remains appropriate before broad promotion.
```

Observed Codex behavior:

```text
Not evaluated.
```

## Decision

Chosen wording:

### Claude — `options`

```text
When presenting choices for the user to answer, including clarification questions, do not preselect, rank, or label an option as recommended unless the user asks for guidance. If one option has a material, objective advantage, state that advantage without converting the response into a recommendation unless the user asks you to choose.
```

### Claude — `recommend_first`

```text
When a decision is needed, lead with the option you recommend and briefly explain why. Do not begin with an unranked list of alternatives. Include other options only when they add meaningful value or tradeoff context.
```

### Claude — `choose_by_default`

```text
When the user has delegated a reasonably reversible decision, choose a sensible default and treat it as the current working decision. Continue the task using that choice rather than stopping to ask for approval or presenting the choice back to the user. State the choice and any material assumption briefly. Ask only when missing information could materially change the decision.
```

### Codex

```text
No decision yet; port the same semantic wording unchanged first and evaluate before introducing harness-specific phrasing.
```

Why this wording was selected:

- each Claude mode now produces a visibly different collaboration mechanic;
- material ambiguity remains a boundary across modes;
- the refinements addressed observed failures without changing the semantic schema.

Rejected alternatives and why:

- original `options` wording: allowed recommendation leakage in clarification choices;
- original `choose_by_default` wording: did not make continuation/working-state adoption observable enough.

## Outcome

- Claude (Sonnet 5, high effort): strong
- Codex: untested in this result set
- Cross-harness semantic intent preserved: not yet established by this result set
- Ready to port into integration: Claude candidate yes; cross-harness promotion pending Codex evaluation

## Notes

The next durability check should keep these semantic wordings unchanged and test them in Codex before adding harness-specific enforcement language.
