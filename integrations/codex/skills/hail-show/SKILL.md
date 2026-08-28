---
name: hail-show
description: Show the user's current persistent HAIL interaction profile in Codex. Use when the user explicitly wants to inspect their HAIL configuration. This skill is read-only.
---

# Show HAIL profile in Codex

Show the user's current persistent HAIL configuration without modifying it.

## Shared profile resources

Load the bundled root HAIL resources before interpreting a profile:

- `../hail/references/profile-schema.md`;
- `../hail/references/profile-normalization.md`.

Do not redefine schema, defaults, migration, or compatibility behavior in this skill.

## Rules

- Read and normalize `~/.hail/profile.yaml` before interpreting it.
- Do not create, rewrite, migrate, reset, or otherwise mutate the stored profile from this skill. Compatibility-derived values are display-time interpretation only here.
- If no profile exists, explain that HAIL has not been configured yet and point the user toward `$hail-setup`.
- Summarize the normalized configured behavior in plain language first.
- Show raw stored YAML only when requested; distinguish stored values from normalized interpretation if that difference matters.
- Never infer diagnoses or neurotypes from the profile.

This skill is an independently discoverable read-only surface over HAIL's Codex profile-management behavior. The canonical profile remains the source of truth.
