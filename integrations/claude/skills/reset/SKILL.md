---
name: reset
description: Reset the user's persistent HAIL interaction profile to supported defaults. Use only when the user explicitly wants to reset HAIL configuration.
---

# Reset HAIL profile

Reset persistent HAIL configuration intentionally without damaging unrelated Claude configuration.

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
- Regenerate `~/.hail/claude-code.md` using the root `hail` projection mappings.
- Ensure `~/.claude/CLAUDE.md` contains exactly one standalone `@~/.hail/claude-code.md` import while preserving unrelated content exactly.
- Never infer diagnoses or neurotypes.
- Explain the resulting behavior in plain language.

This skill must produce the same persistent profile and Claude projection that equivalent reset intent handled through the root `hail` skill would produce.
