---
name: change
description: Change one or more persistent HAIL interaction defaults. Use only when the user explicitly wants to modify HAIL or their persistent HAIL preferences.
---

# Change HAIL profile

Modify only the persistent HAIL preferences the user intentionally wants changed.

## Persistence boundary

This skill is an explicit persistent-configuration surface. Do not treat ordinary conversational instructions outside HAIL configuration as permission to persist changes.

## Shared profile resources

Load the bundled root HAIL resources before profile work:

- `../hail/references/profile-schema.md`;
- `../hail/references/default-profile.yaml`;
- `../hail/references/profile-normalization.md`.

Do not redefine schema, defaults, migration, or compatibility behavior in this skill.

## Change behavior

- Normalize and validate the existing profile before applying the requested change.
- If no profile exists, copy the bundled default profile as the working profile, apply the requested change, and make clear that this initializes HAIL as part of the explicit request.
- Apply explicit current configuration intent after normalization so the user's requested value wins over stored or compatibility-derived values.
- Preserve every valid preference the user did not ask to change.
- Map clear natural-language requests directly; ask only the minimum clarification when two materially different mappings are plausible.
- Never infer diagnoses or neurotypes.
- Validate the resulting profile against the bundled schema before writing `~/.hail/profile.yaml`.
- Regenerate `~/.hail/claude-code.md` using the root `hail` projection mappings.
- Ensure `~/.claude/CLAUDE.md` contains exactly one standalone `@~/.hail/claude-code.md` import and preserve all unrelated content exactly.
- Explain what changed in behavioral terms rather than implementation terms.

This skill must produce the same persistent profile and Claude projection that equivalent change intent handled through the root `hail` skill would produce.
