# Projection Prompt Hardening

This experiment strengthens harness-specific projection wording for HAIL's existing validated semantics without changing the semantic schema.

## Goal

Improve behavioral durability across harnesses by refining how the same semantic intent is expressed to each harness.

The experiment must preserve this boundary:

```text
human semantic intent
        ↓
harness-specific projection wording
        ↓
observable behavior
```

A stronger prompt is allowed to be more operational or explicit for a given harness. It must not change the meaning of the underlying HAIL preference.

## Current status

Claude has reached a strong hardening checkpoint for the currently exercised semantics, and the repaired `verbosity: detailed` projection has now been promoted after cross-harness Claude/Codex replay.

| Preference | Status | Decision |
| --- | --- | --- |
| `decision_mode` | strong Claude evidence | hardened candidate retained |
| `max_options` | strong Claude evidence | hardened candidate retained |
| `task_chunking` | strong Claude base differentiation | Candidate A promoted; boundary/cross-harness work remains |
| `verbosity` | strong cross-harness evidence | strengthened shared `detailed` wording promoted |

Authoritative current evidence:

- [`results/decision-mode-sonnet-5-high.md`](results/decision-mode-sonnet-5-high.md)
- [`results/max-options-sonnet-5-high.md`](results/max-options-sonnet-5-high.md)
- [`results/task-chunking-claude-sonnet.md`](results/task-chunking-claude-sonnet.md)
- [`results/verbosity-claude-sonnet.md`](results/verbosity-claude-sonnet.md)
- [`results/verbosity-cross-harness-detailed.md`](results/verbosity-cross-harness-detailed.md)

The native integration paths are authoritative for current hardening. Earlier reference-compiler runs remain useful historical diagnostics but should not override native integration evidence.

## Evaluation-method boundary

The current `hail-testing` workflow supplies one prompt in a fresh session. It is authoritative only for **single-turn observable behavior**.

Composition evals must explicitly state whether they are:

- **single-turn observable** — valid for the current workflow; or
- **multi-turn required** — requires a manual or future multi-turn runner that preserves conversation state.

Do not convert a multi-turn composition into independent one-turn prompts and claim equivalent evidence.

## Quick HAIL setup

### 1. Use the branch containing the candidate under test

Do not rely on an old hardening branch name. Record the exact HAIL ref used for every run.

### 2. Ensure the shared HAIL profile directory exists

```bash
mkdir -p ~/.hail
```

If you do not already have a profile, start from the repository example:

```bash
cp profiles/example.yaml ~/.hail/profile.yaml
```

The canonical persistent profile is:

```text
~/.hail/profile.yaml
```

Treat that file as the human-owned source of truth. Harness-specific generated instructions are disposable projections.

### 3. Claude setup

For repository-local testing:

```bash
claude --plugin-dir ./integrations/claude
```

Then invoke `/hail:hail` as needed.

### 4. Codex setup

Install the HAIL Codex skill locally when testing outside the automated harness:

```bash
mkdir -p ~/.codex/skills/hail
cp integrations/codex/skills/hail/SKILL.md ~/.codex/skills/hail/SKILL.md
```

Then invoke `$hail` as needed.

### 5. Reset between meaningful comparisons

Use a fresh interaction/session whenever practical so previous conversation context does not mask whether the projection itself caused the behavior change.

When testing a different wording candidate:

1. update only the target projection wording;
2. leave the semantic profile unchanged;
3. start a fresh interaction;
4. run the same scenario prompts;
5. record raw results before judging them.

## Experiment method

Work on one semantic preference at a time unless a dedicated composition file explicitly defines the variables being composed.

For each preference:

1. State the semantic intent in harness-neutral language.
2. Record the current Claude and Codex projection wording.
3. Identify the observed failure mode from previous evidence or a fresh baseline run.
4. Draft one small wording change.
5. Test the same scenarios in fresh interactions.
6. Record raw output before interpreting it.
7. Compare behavior against the expectation, not against stylistic preference alone.
8. Keep the candidate only if it improves enforcement without changing semantic meaning.
9. Replay shared candidates across supported harnesses before introducing harness-specific wording unnecessarily.
10. Port proven wording back into the harness integration only after the experiment is convincing.

## Result-file convention

Keep experiment definitions and observed evidence separate.

```text
<preference>.md
→ individual semantic intent, scenarios, candidate wording, and boundaries

<semantic-a>-<semantic-b>-composition.md
→ one focused composition boundary and its execution requirements

results/<experiment>-<model>-<effort>.md
→ test conditions, raw/representative outputs, assessments, candidate history, decision
```

Start each result record from [`template.md`](template.md). Record model and effort level explicitly whenever those settings can affect instruction-following strength.

Do not bury composition suites inside an individual semantic definition. If a composition is important enough to track as a roadmap/evidence question, give it its own focused file.

## What this experiment is not

Do not:

- add new persistent semantic fields;
- change the meaning of an existing preference to make a harness comply;
- optimize one harness by weakening portability of the human intent;
- add runtime, MCP, sync, or other infrastructure;
- infer that more forceful wording is automatically better;
- tune multiple semantic dimensions at once outside a dedicated composition experiment.

## Next work

Immediate work is the single-turn-observable [`max_options × decision_mode × ambiguity`](max-options-decision-mode-composition.md) suite. Base `task_chunking` replay and any composition involving `task_chunking` or `step_pacing` are blocked until a semantic runner can preserve conversation state (or a separate valid multi-turn manual method is used).

## Files

### Individual semantic hardening

- [`decision-mode.md`](decision-mode.md)
- [`max-options.md`](max-options.md)
- [`task-chunking.md`](task-chunking.md)
- [`verbosity.md`](verbosity.md)

### Focused composition boundaries

- [`max-options-decision-mode-composition.md`](max-options-decision-mode-composition.md) — `max_options × decision_mode × ambiguity`
- [`task-chunking-step-pacing-composition.md`](task-chunking-step-pacing-composition.md) — `task_chunking × step_pacing`
- [`verbosity-max-options-composition.md`](verbosity-max-options-composition.md) — `verbosity × max_options`
- [`verbosity-task-chunking-composition.md`](verbosity-task-chunking-composition.md) — `verbosity × task_chunking`
- [`tangent-policy-step-pacing-composition.md`](tangent-policy-step-pacing-composition.md) — `tangent_policy × step_pacing`

### Candidate / historical notes

- [`results/codex-decision-max-options-candidate-history.md`](results/codex-decision-max-options-candidate-history.md) — rescued unvalidated hypotheses from the retired Codex hardening branch; not evidence or promoted wording.

### Shared template

- [`template.md`](template.md)

## Success criterion

The goal is not identical Claude and Codex output.

Success means:

> The same HAIL semantic intent produces reliably recognizable behavior in each harness using the smallest harness-specific wording necessary, while neighboring semantics remain independently controllable when composed.
