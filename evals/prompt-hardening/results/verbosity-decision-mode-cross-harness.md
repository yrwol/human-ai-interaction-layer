# Cross-Harness Composition Eval — `verbosity × decision_mode`

## Status

- **Harnesses:** Claude Code + Codex
- **Claude model:** Sonnet
- **Codex model:** GPT-5.5, high reasoning effort
- **HAIL ref:** `eval/verbosity-decision-mode`
- **Method:** one prompt in a fresh session per profile/harness
- **Current status:** in progress
- **Starting projection:** current promoted verbosity + decision-mode wording from merged `main`

## Core matrix

Prompt:

```text
For a cozy horse game, should horse training progression use stat-based leveling, bond/trust progression, or discipline skill trees?
```

Matrix:

| Verbosity | options | recommend_first | choose_by_default |
| --- | --- | --- | --- |
| compact | pending | pending | pending |
| balanced | pending | pending | pending |
| detailed | pending | pending | pending |

## Focused controls

### Explicit recommendation override

Run `options` at compact and detailed:

```text
Recommend one of those horse training progression approaches for a cozy horse game and explain why.
```

### Consequential ambiguity regression

Run recommend-first and choose-by-default at compact and detailed:

```text
Choose the entire monetization model for the game.
```

Expected: minimum necessary clarification and stop; no invented fallback/provisional model.

## Evidence discipline

Raw runner records remain private in `yrwol/hail-testing`. Record run IDs and reviewed findings here. Do not change projection wording from a single stochastic output.


## Launched runs

### Core 3 × 3 matrix

| Verbosity | Decision mode | Run |
| --- | --- | --- |
| compact | options | `33254512521` |
| compact | recommend_first | `33254516594` |
| compact | choose_by_default | `33254558495` |
| balanced | options | `33254561469` |
| balanced | recommend_first | `33254565482` |
| balanced | choose_by_default | `33254569488` |
| detailed | options | `33254572096` |
| detailed | recommend_first | `33254574287` |
| detailed | choose_by_default | `33254576589` |

### Explicit recommendation override

- `33254584557` — compact × options
- `33254587453` — detailed × options

### Consequential ambiguity regression

- `33254590170` — compact × recommend_first
- `33254592522` — compact × choose_by_default
- `33254595783` — detailed × recommend_first
- `33254600137` — detailed × choose_by_default

All runs target Claude Sonnet + Codex GPT-5.5/high and use fresh sessions against `eval/verbosity-decision-mode`.
