# Historical HAIL Evaluation Artifacts

This directory preserves evaluation artifacts from HAIL's earlier milestone work.

These files are intentionally **historical evidence**, not the current evaluation methodology.

## What lives here

- `fixtures/` — lightweight YAML scenarios used during early static-profile/reference testing and native profile-management experiments.
- `manual-testing.md` — the original manual A/B procedure used for the first Claude/Codex portability experiments.
- `results/` — milestone summaries, raw transcripts, and the Claude/Codex portability comparison produced by those experiments.

Keeping the procedure, fixtures, and resulting evidence together makes the historical boundary explicit and avoids making old milestone artifacts look like current prompt-hardening tests.

## Why these were moved

The early YAML fixtures were named around individual values or one-off scenarios (for example `recommend_first.yaml`) rather than around the full semantic contract. As HAIL matured, that naming became misleading beside the current semantic-level prompt-hardening suite.

Current behavioral development belongs in [`../prompt-hardening/`](../prompt-hardening/).

Do not update these historical fixtures or result records to match today's conventions unless correcting an archival error. Their value is that they preserve the experiment shape and evidence produced at the time.