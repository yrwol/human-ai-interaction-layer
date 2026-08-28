---
name: hail-change
description: Change one or more persistent HAIL interaction defaults in Codex. Use only when the user explicitly wants to modify HAIL or persistent HAIL preferences.
---

# Change HAIL profile in Codex

Modify only the persistent HAIL preferences the user intentionally wants changed.

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
- Map clear natural-language intent directly; ask only the minimum clarification when materially different mappings are plausible.
- Never infer diagnoses or neurotypes.
- Validate the resulting profile against the bundled schema before writing `~/.hail/profile.yaml`.
- Regenerate HAIL's managed block in `$CODEX_HOME/AGENTS.md` using the root `$hail` projection mappings.
- Preserve all content outside the HAIL markers exactly. Replace one complete block or append one if absent; never create duplicates. If markers are malformed or multiple blocks exist, stop and ask permission to repair rather than guessing.
- Explain what changed in behavioral terms.

This skill must produce the same persistent profile and Codex projection that equivalent change intent handled through the root `$hail` skill would produce.
