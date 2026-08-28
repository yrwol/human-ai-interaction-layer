---
name: show
description: Show the user's current persistent HAIL interaction profile in plain language. Use when the user explicitly wants to inspect their HAIL configuration. This skill is read-only.
---

# Show HAIL profile

Show the user's current persistent HAIL configuration without modifying it.

## Rules

- Before interpreting a stored profile, load and apply the bundled root HAIL normalization contract at `../hail/references/profile-normalization.md`.
- Do not define migration or compatibility behavior in this skill.
- Read `~/.hail/profile.yaml` through that normalization contract before interpreting it.
- Do not create, rewrite, migrate, reset, or otherwise mutate the profile from this skill. Compatibility-derived values are display-time interpretation only here.
- If no profile exists, explain that HAIL has not been configured yet and point the user toward the HAIL `setup` skill.
- Summarize the normalized configured behavior in plain language first. Do not require the user to understand schema names or YAML.
- Show raw stored YAML only when requested; distinguish stored values from normalized interpretation if that difference matters.
- Never infer diagnoses or neurotypes from the profile.

Current v0.1 fields are `verbosity`, `decision_mode`, `max_options`, `task_chunking`, `step_pacing`, and `tangent_policy`.

This skill is an independently discoverable read-only surface over HAIL's existing persistent profile-management behavior. The canonical profile remains the source of truth.
