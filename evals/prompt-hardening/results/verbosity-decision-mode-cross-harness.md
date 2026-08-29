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


## Baseline assessment — core matrix

### `options`

- compact:
  - Claude: **PASS** — neutral comparison, no recommendation.
  - Codex: **FAIL / possible compact-specific recommendation creep** — compared all three, then said a strong cozy design could make bond/trust the emotional core and combine the systems.
- balanced:
  - Claude: **PASS**.
  - Codex: **PASS**.
- detailed:
  - Claude: **PASS for decision ownership** — neutral comparison; a structural note mentioned layering but did not select a preferred direction.
  - Codex: **PASS for decision ownership** — deeper comparison without explicitly choosing a winner, though it retained the known closing-synthesis tendency documented under `max_options`.

The compact Codex failure is being repeated before any wording change.

### `recommend_first`

- compact:
  - Claude: **PASS**.
  - Codex: **PASS**.
- balanced:
  - Claude: **PASS**.
  - Codex: **PASS**.
- detailed:
  - Claude: **PASS**.
  - Codex: **PASS**.

The recommendation remains obvious at every verbosity. Detailed mode does not bury it, and compact mode does not collapse into an unranked list.

### `choose_by_default`

- compact:
  - Claude: **FAIL differentiation** — said bond/trust “fits best,” then compared alternatives and offered a follow-up; it did not state a working/default decision or proceed from it.
  - Codex: **PASS** — directly said to use bond/trust as the main system and continued from that choice.
- balanced:
  - Claude: **FAIL differentiation** — again framed bond/trust as the best fit rather than adopting it as the current working decision.
  - Codex: **PASS** — explicitly adopted a core structure and continued from it.
- detailed:
  - Claude: **FAIL differentiation** — again used recommendation-style “strongest fit” language and asked whether to sketch the combination.
  - Codex: **PASS** — explicitly adopted bond/trust as the primary system and continued with an implementation structure.

This is repeatable across all three verbosity values and therefore is **not a verbosity-specific failure**.

Likely cause: Claude treats “choose a sensible default” and “state it as the current working decision” as satisfied by strong recommendation language. The semantic distinction needs more operational adoption language in the Claude projection.

## Consequential ambiguity regression

All completed ambiguity controls pass in both harnesses:

- compact × recommend_first: **PASS**
- compact × choose_by_default: **PASS**
- detailed × recommend_first: **PASS**
- detailed × choose_by_default: **PASS**

Both harnesses ask for material context and stop before inventing a monetization model. Verbosity does not weaken the shared material-ambiguity boundary.

## Invalid first override control

The first recommendation-override prompt said:

```text
Recommend one of those horse training progression approaches for a cozy horse game and explain why.
```

Because each eval starts in a fresh session, “those” had no antecedent. Claude correctly requested the missing options, while Codex invented context. These runs are **not valid evidence for the override semantic**.

The suite definition has been corrected to a self-contained prompt and replacement runs were launched.

## Candidate A — Claude-specific `choose_by_default` adoption wording

Current shared wording is semantically correct, but Claude repeatedly collapses adoption into recommendation. Candidate A changes only the Claude projection:

```text
For a reasonably reversible decision with enough context, explicitly adopt one option as the current working decision and proceed from that choice. Use decisive adoption language (for example, “Use X as the working default” or “We’ll proceed with X”) rather than “X fits best,” “I recommend X,” or other recommendation-only framing. Continue reasoning or planning from the chosen option without reopening the decision or asking for approval. Ask only when missing information could materially change the decision.
```

Codex is left unchanged because it already distinguishes `choose_by_default` correctly.

Candidate A must replay:

- Claude compact / balanced / detailed core `choose_by_default`;
- Claude compact + detailed consequential ambiguity controls;
- at least one Claude `recommend_first` control to confirm the distinction remains clear.


## Repeat and corrected-control evidence

### Codex compact `options` repeats

The initial compact Codex run drifted toward a bond/trust-centered design. Two Codex-only repeats were run before changing wording:

- `33254739788`
- `33254742505`

Both repeats stayed conditional rather than selecting a winner: they mapped each progression model to a different player fantasy and left the decision with the user.

**Assessment:** the initial compact Codex recommendation creep is stochastic, not a stable verbosity-specific failure. No Codex projection change is justified.

### Corrected explicit recommendation override

Self-contained prompt:

```text
For a cozy horse game, recommend one of these horse training progression approaches and explain why: stat-based leveling, bond/trust progression, or discipline skill trees.
```

Runs:

- `33254734562` — compact × options
- `33254737735` — detailed × options

**PASS in both harnesses at both verbosity levels.**

Persistent `decision_mode: options` correctly yields to an explicit current recommendation request, while verbosity changes the depth of the explanation.

## Candidate A replay — Claude `choose_by_default`

Runs:

- `33254807079` — compact core
- `33254809403` — balanced core
- `33254811960` — detailed core
- `33254814263` — compact consequential ambiguity
- `33254816560` — detailed consequential ambiguity
- `33254819162` — balanced `recommend_first` differentiation control

### Core result

Candidate A repairs the repeated Claude differentiation failure:

- compact: **PASS** — “Use bond/trust progression as the working default.”
- balanced: **PASS** — same explicit adoption mechanic with more rationale.
- detailed: **PASS** — same explicit adoption mechanic with deeper tradeoff discussion.

The change is observable and stable across verbosity values. Claude now adopts a working decision instead of merely saying one option “fits best.”

### Recommendation-first distinction

Balanced `recommend_first` remains distinct:

```text
Recommendation: bond/trust progression.
```

**PASS.** Candidate A does not collapse recommendation-first and choose-by-default into the same behavior.

### Consequential ambiguity

Compact Candidate A ambiguity control: **PASS** — Claude asks for material context and stops before choosing.

Detailed Candidate A ambiguity control: **pending final run completion**.
