# HAIL Evaluations

This directory contains both current behavioral-development experiments and preserved historical evidence.

## Current behavioral development

[`prompt-hardening/`](prompt-hardening/) is the active evaluation methodology for strengthening how existing HAIL semantics are projected into AI harnesses.

Current experiments are organized by semantic or focused composition boundary, with model/harness-specific evidence stored under `prompt-hardening/results/`.

When developing current semantics, start there.

## Historical fixtures

[`historical/`](historical/) contains the lightweight profile/scenario fixtures and manual A/B procedure used during HAIL's earlier static-profile and native-management milestones.

These files are preserved as evidence of how the project was tested at that stage. They are **not** authoritative current eval definitions and should not be used to infer today's semantic organization or prompt-hardening methodology.

## Historical milestone results

[`results/`](results/) contains the original milestone result summaries and raw transcripts from the early Claude/Codex portability and native-management experiments.

These remain in place as historical evidence. New prompt-hardening results belong under `prompt-hardening/results/`, not here.

## Standalone experiments

[`prompt-context-efficiency.md`](prompt-context-efficiency.md) records the prompt/context efficiency experiment. It is a focused standalone evaluation rather than part of the current semantic hardening suite.

## Organization rule

```text
current semantic/capability development
→ organize by semantic or focused behavioral boundary
→ evals/prompt-hardening/

historical evidence
→ preserve according to the experiment/milestone that produced it
→ evals/historical/ and evals/results/
```

Do not add new root-level semantic fixture YAML files. If a current behavior needs evaluation, add or extend the relevant experiment under `prompt-hardening/`.