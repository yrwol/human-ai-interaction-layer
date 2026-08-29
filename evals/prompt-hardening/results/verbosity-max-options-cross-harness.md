# Cross-Harness Composition Eval — `verbosity × max_options`

## Status

- **Harnesses:** Claude Code + Codex
- **Claude model:** Sonnet
- **Codex model:** GPT-5.5, high reasoning effort
- **HAIL ref:** `eval/verbosity-max-options`
- **Method:** one prompt in a fresh session per profile/harness
- **Current status:** in progress
- **Starting projection:** current promoted verbosity wording + Candidate D decision/option boundaries from `main`

## Profiles

All profiles use:

```yaml
decision_mode: options
max_options: 3
task_chunking: adaptive
step_pacing: continuous
tangent_policy: capture_and_return
```

Only `verbosity` changes.

Fixtures:

- `profiles/eval-composition-verbosity-compact.yaml`
- `profiles/eval-composition-verbosity-balanced.yaml`
- `profiles/eval-composition-verbosity-detailed.yaml`

## Scenario 1 — fixed three-choice depth control

Prompt:

```text
Give me three approaches for designing horse training progression in a cozy horse game, and explain the tradeoffs of each.
```

### Expected

- three primary approaches in all profiles;
- compact preserves minimum useful distinctions;
- balanced adds moderate rationale/tradeoffs;
- detailed deepens the same choice set without introducing more approaches.

## Scenario 2 — open-ended cap pressure

Prompt:

```text
What are some good approaches for designing horse training progression in a cozy horse game? Explain the tradeoffs.
```

### Expected

- no more than three meaningful approaches under any verbosity;
- detailed adds depth rather than choices;
- compact remains decision-useful.

## Scenario 3 — informational-list boundary

Prompt:

```text
List the core attributes a horse could have in a cozy horse game and explain what each attribute represents.
```

### Expected

- ordinary informational items are not mechanically capped at three;
- verbosity changes explanation depth, not semantic classification.

## Scenario 4 — explicit current-request overrides

### Choice-count override

```text
Give me 6 horse training progression approaches. Keep each explanation short.
```

Expected: six approaches despite persistent `max_options: 3`.

### Detail override

Profile: `verbosity: compact`.

```text
Give me three horse training progression approaches and explain each one in detail, including tradeoffs and implementation implications.
```

Expected: current explicit detail request wins without changing the persistent profile.

## Evidence discipline

Raw runner records remain private in `yrwol/hail-testing`. Record run IDs and reviewed findings here. Do not promote any projection wording change from a single stochastic response without a repeat or meaningful cross-harness pattern.


## Launched runs

### Scenario 1 — fixed three-choice depth control

- `33233429854` — compact
- `33233432832` — balanced
- `33233435980` — detailed

### Scenario 2 — open-ended cap pressure

- `33233446636` — compact
- `33233448385` — balanced
- `33233450593` — detailed

### Scenario 3 — informational-list boundary

- `33233452331` — compact
- `33233454299` — detailed

### Scenario 4 — explicit current-request overrides

- `33233455887` — balanced profile, explicit six-choice request
- `33233458116` — compact profile, explicit detailed-explanation request

All runs target Claude Sonnet + Codex GPT-5.5/high and use fresh sessions against `eval/verbosity-max-options`.


## Baseline assessment

### Scenario 1 — fixed three-choice depth control

#### Option-count behavior

- Claude compact: **PASS** — exactly three approaches, no additional distinct approach.
- Codex compact: **PASS** — exactly three approaches.
- Claude balanced: **FAIL option-cap composition** — after three approaches, it added a distinct hybrid combining bond-gating and stats.
- Codex balanced: **PASS** — closing summary compared the three surfaced approaches without introducing a clearly distinct fourth design.
- Claude detailed: **FAIL option-cap composition** — explicitly named a combined bond + stats + disciplines model as “a fourth distinct option,” despite saying it was being kept out.
- Codex detailed: **PASS** — remained within the three surfaced approaches.

The Claude detailed result is especially diagnostic: the model understood the semantic rule well enough to identify the combined design as a fourth option, but still surfaced the option by naming and describing it.

#### Verbosity differentiation

Observed response word counts:

| Harness | compact | balanced | detailed |
| --- | ---: | ---: | ---: |
| Claude | 214 | 398 | 279 |
| Codex | 185 | 262 | 374 |

Codex shows the expected directional depth increase.

Claude does **not** show a stable monotonic progression in this one matrix: the balanced response was substantially more developed than the detailed response. Word count alone is not the semantic target, but the balanced response also contained more explicit pros/cons detail than the detailed response.

**Assessment:** Claude verbosity differentiation requires repeat testing before any wording change. Do not harden from one stochastic ordering.

### Scenario 2 — open-ended cap pressure

- Claude compact: **FAIL option-cap composition** — surfaced three primary approaches, then named a hybrid as a fourth distinct approach.
- Codex compact: **FAIL option-cap composition** — surfaced three primary approaches, then proposed combining relationship progression, trait/habit, and skill-tree structure into another coherent design.
- Claude balanced: **FAIL option-cap composition** — surfaced three primary approaches, then described combining them into a bond-gated/narrative synthesis.
- Codex balanced: **FAIL option-cap composition** — surfaced three primary approaches, then proposed a combined routine + personality/trust + skill-tree design.
- Claude detailed: **PASS** — remained within three approaches and summarized only the tradeoff axis.
- Codex detailed: **FAIL option-cap composition** — surfaced three primary approaches, then suggested a combined design.

This is a repeatable cross-harness composition failure.

The existing Candidate D wording already says hybrids/syntheses count as options and should not be appended after the cap. The remaining loophole is **mentioning/describing the extra approach while claiming it is excluded or merely a closing synthesis**.

### Scenario 3 — informational-list boundary

- Claude compact: **PASS** — 12 informational attributes.
- Codex compact: **PASS** — 10 informational attributes.
- Claude detailed: **PASS** — 13 informational attributes.
- Codex detailed: **PASS** — 20 informational attributes.

The option cap did not become mechanical bullet-count control.

### Scenario 4A — explicit six-choice override

**PASS in both harnesses.**

Both Claude and Codex returned exactly six approaches under persistent `max_options: 3`.

### Scenario 4B — explicit detail override

**PASS in both harnesses.**

Under persistent `verbosity: compact`, both harnesses honored the explicit current request for detailed explanations, tradeoffs, and implementation implications while retaining exactly three primary approaches.

## Composition Candidate A — closing-choice leakage repair

The evidence supports a narrow strengthening of `max_options`, not a semantic change.

Add this enforcement boundary after the existing hybrid/synthesis rule:

```text
Once the configured or explicitly requested choice count has been reached, do not introduce, name, suggest, describe, or offer another distinct option anywhere else in the response — including as a hybrid, combined approach, excluded alternative, aside, caveat, honorable mention, follow-up invitation, or closing synthesis. A closing summary may compare or synthesize the already-surfaced choices, but it must not construct another selectable approach from them.
```

Why this is needed:

- the current wording correctly teaches the model that a hybrid counts as an option;
- multiple runs still mention or construct that extra option after the cap;
- one Claude run explicitly called its own closing synthesis a “fourth distinct option” and surfaced it anyway.

This is enforcement hardening of the existing semantic meaning.

## Next validation

1. Apply Composition Candidate A unchanged to both harnesses.
2. Replay Scenario 2 under compact / balanced / detailed.
3. Replay Scenario 1 under at least balanced and detailed.
4. Preserve Scenario 3 informational boundary.
5. Repeat Claude fixed-control balanced/detailed runs separately to determine whether the observed verbosity inversion is stable or stochastic before changing verbosity wording.
