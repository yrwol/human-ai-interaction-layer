---
name: show
description: Show the user's current persistent HAIL interaction profile in Codex. Use when the user explicitly wants to inspect their HAIL configuration. This skill is read-only.
---

# Show HAIL profile in Codex

Show the user's current persistent HAIL configuration without modifying it.

## Rules

- Read `~/.hail/profile.yaml`.
- Do not create, rewrite, migrate, reset, or otherwise mutate the profile from this skill.
- If no profile exists, explain that HAIL has not been configured yet and point the user toward the HAIL `setup` skill.
- Summarize the configured behavior in plain language first. Show raw YAML only when requested.
- Never infer diagnoses or neurotypes from the profile.
- Treat older valid profiles without `step_pacing` as having the historical default `continuous` for display purposes; do not persist that migration from this read-only skill.

Current v0.1 fields are `verbosity`, `decision_mode`, `max_options`, `task_chunking`, `step_pacing`, and `tangent_policy`.

The canonical profile remains the source of truth. This skill is an independently discoverable read-only surface over HAIL's existing Codex profile-management behavior.