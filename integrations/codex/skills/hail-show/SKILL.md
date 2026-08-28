---
name: hail-show
description: Show the user's current persistent HAIL interaction profile in Codex. Use when the user explicitly wants to inspect their HAIL configuration. This skill is read-only.
---

# Show HAIL profile in Codex

Show the user's current persistent HAIL configuration without modifying it.

## Rules

- Read `~/.hail/profile.yaml` through HAIL's canonical profile-normalization behavior before interpreting it.
- Do not define migration or compatibility behavior in this skill; normalization semantics are authoritative in `spec/semantics.md` and executed by the root `$hail` profile-management contract.
- Do not create, rewrite, migrate, reset, or otherwise mutate the profile from this skill. Compatibility-derived values are display-time interpretation only here.
- If no profile exists, explain that HAIL has not been configured yet and point the user toward `$hail-setup`.
- Summarize the normalized configured behavior in plain language first. Show raw stored YAML only when requested; distinguish stored values from normalized interpretation if that difference matters.
- Never infer diagnoses or neurotypes from the profile.

Current v0.1 fields are `verbosity`, `decision_mode`, `max_options`, `task_chunking`, `step_pacing`, and `tangent_policy`.

The canonical profile remains the source of truth. This skill is an independently discoverable read-only surface over HAIL's existing Codex profile-management behavior.