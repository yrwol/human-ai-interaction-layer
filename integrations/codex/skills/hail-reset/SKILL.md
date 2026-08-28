---
name: hail-reset
description: Reset the user's persistent HAIL interaction profile in Codex to supported defaults. Use only when the user explicitly wants to reset HAIL configuration.
---

# Reset HAIL profile in Codex

Reset persistent HAIL configuration intentionally without damaging unrelated Codex configuration.

## Shared profile resources

Load the bundled root HAIL resources before profile work:

- `../hail/references/profile-schema.md`;
- `../hail/references/default-profile.yaml`;
- `../hail/references/profile-normalization.md`.

Do not redefine schema, defaults, migration, or compatibility behavior in this skill.

## Reset behavior

- If HAIL is not configured, explain that there is nothing persistent to reset rather than creating a profile unnecessarily.
- Normalize and validate the stored profile before applying a reset.
- Confirm whether the user means the whole profile or only one preference when scope is ambiguous.
- For a whole-profile reset, use the bundled `default-profile.yaml` as the resulting profile.
- For a single-preference reset, copy that field's value from the bundled default profile and preserve every other normalized valid preference.
- Validate the resulting profile against the bundled schema before writing `~/.hail/profile.yaml`.
- Regenerate HAIL's managed block in `$CODEX_HOME/AGENTS.md` using the root `$hail` projection mappings.
- Preserve all content outside the HAIL markers exactly. Replace one complete block or append one if absent; never create duplicates. If markers are malformed or multiple blocks exist, stop and ask permission to repair rather than guessing.
- Never infer diagnoses or neurotypes.
- Explain the resulting behavior in plain language.

This skill must produce the same persistent profile and Codex projection that equivalent reset intent handled through the root `$hail` skill would produce.
