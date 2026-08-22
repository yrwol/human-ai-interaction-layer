# HAIL Evaluations

This directory contains both current behavioral-development experiments and preserved historical evidence.

## Current behavioral development

[`prompt-hardening/`](prompt-hardening/) is the active evaluation methodology for strengthening how existing HAIL semantics are projected into AI harnesses.

Current experiments are organized by semantic or focused composition boundary, with model/harness-specific evidence stored under `prompt-hardening/results/`.

When developing current semantics, start there.

## Historical evaluation evidence

[`historical/`](historical/) contains the earlier evaluation methodology and evidence from HAIL's static-profile, portability, and native-management milestones.

- `historical/fixtures/` — lightweight YAML scenarios used during early testing;
- `historical/manual-testing.md` — the original manual A/B procedure;
- `historical/results/` — milestone summaries, raw transcripts, and the portability comparison.

These files are preserved as evidence of how the project was tested at that stage. They are **not** authoritative current eval definitions and should not be used to infer today's semantic organization or prompt-hardening methodology.

## Standalone experiments

[`prompt-context-efficiency.md`](prompt-context-efficiency.md) records the prompt/context efficiency experiment. It is a focused standalone evaluation rather than part of the current semantic hardening suite.

## Organization rule

```text
current semantic/capability development
→ organize by semantic or focused behavioral boundary
→ evals/prompt-hardening/

historical milestone evaluation
→ preserve the fixtures, procedure, and results together
→ evals/historical/
```

Do not add new root-level semantic fixture YAML files or a generic root-level `results/` directory. Current hardening evidence belongs with its experiment under `prompt-hardening/results/`.