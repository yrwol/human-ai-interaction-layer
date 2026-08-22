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

## Quick HAIL setup

### 1. Switch to this experiment branch

```bash
git switch eval/projection-prompt-hardening
```

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

For repository-local testing, launch Claude Code with the plugin directly from the repo:

```bash
claude --plugin-dir ./integrations/claude
```

Then invoke:

```text
/hail:hail
```

Useful commands:

```text
/hail:hail show
/hail:hail change
/hail:hail reset
```

For prompt-hardening work, you may manually edit the generated Claude projection while experimenting:

```text
~/.hail/claude-code.md
```

Do not treat manual projection edits as final implementation. Once wording is proven useful, port the chosen wording back into the Claude integration.

### 4. Codex setup

Install the HAIL Codex skill locally:

```bash
mkdir -p ~/.codex/skills/hail
cp integrations/codex/skills/hail/SKILL.md ~/.codex/skills/hail/SKILL.md
```

Then start a fresh Codex session and invoke:

```text
$hail
```

Useful commands:

```text
$hail show
$hail change
$hail reset
```

Codex applies HAIL through the managed block in:

```text
$CODEX_HOME/AGENTS.md
```

normally:

```text
~/.codex/AGENTS.md
```

For prompt-hardening work, you may manually edit only the HAIL-managed wording while testing. Preserve unrelated `AGENTS.md` content.

### 5. Reset between meaningful comparisons

Use a fresh interaction/session whenever practical so previous conversation context does not mask whether the projection itself caused the behavior change.

When testing a different wording candidate:

1. update only the target projection wording;
2. leave the semantic profile unchanged;
3. start a fresh interaction;
4. run the same scenario prompts;
5. record raw results before judging them.

## Experiment method

Work on one semantic preference at a time.

For each preference:

1. State the semantic intent in harness-neutral language.
2. Record the current Claude and Codex projection wording.
3. Identify the observed failure mode from previous evidence or a fresh baseline run.
4. Draft one small wording change.
5. Test the same scenarios in fresh Claude and Codex interactions.
6. Record raw output before interpreting it.
7. Compare behavior against the expectation, not against stylistic preference alone.
8. Keep the candidate only if it improves enforcement without changing the semantic meaning.
9. Repeat if useful.
10. Port proven wording back into the harness integration only after the experiment is convincing.

## Result-file convention

Keep experiment definitions and observed evidence separate.

```text
<preference>.md
→ semantic intent, scenarios, current candidate wording, boundaries, composition questions

results/<preference>-<model>-<effort>.md
→ test conditions, raw/representative outputs, assessments, candidate history, decision
```

Start each result record from [`template.md`](template.md). Record model and effort level explicitly whenever those settings can affect instruction-following strength.

Do not append large raw test runs directly into the experiment definition file. A new harness, model, effort level, or materially different wording iteration should get a result record that can be compared without turning the definition into a chronological scratchpad.

## What this experiment is not

Do not:

- add new persistent semantic fields;
- change the meaning of an existing preference to make a harness comply;
- optimize one harness by weakening portability of the human intent;
- add runtime, MCP, sync, or other infrastructure;
- infer that more forceful wording is automatically better;
- tune multiple semantic dimensions at once unless testing composition explicitly.

## Starter targets

The first useful targets are based on earlier behavioral evidence:

| Preference | Known concern |
| --- | --- |
| `task_chunking` | Weak / inconsistent cross-harness enforcement |
| `decision_mode` | Codex showed weaker differentiation |
| `max_options` | Claude showed weaker enforcement |
| `verbosity` | Weak-to-moderate and interpreted inconsistently |

## Files

- [`template.md`](template.md) — reusable experiment/result record structure
- [`task-chunking.md`](task-chunking.md) — experiment definition for `task_chunking`
- [`decision-mode.md`](decision-mode.md) — experiment definition for `decision_mode`
- [`max-options.md`](max-options.md) — experiment definition for `max_options`
- [`max-options-decision-mode-composition.md`](max-options-decision-mode-composition.md) — focused composition evaluation
- [`verbosity.md`](verbosity.md) — experiment definition for `verbosity`
- [`results/decision-mode-sonnet-5-high.md`](results/decision-mode-sonnet-5-high.md) — Claude Sonnet 5 / high-effort decision-mode evidence
- [`results/max-options-sonnet-5-high.md`](results/max-options-sonnet-5-high.md) — Claude Sonnet 5 / high-effort max-options evidence

## Success criterion

The goal is not identical Claude and Codex output.

Success means:

> The same HAIL semantic intent produces reliably recognizable behavior in each harness using the smallest harness-specific wording necessary.
