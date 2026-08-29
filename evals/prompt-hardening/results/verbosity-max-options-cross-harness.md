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
