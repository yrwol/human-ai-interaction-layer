---
name: hail-setup
description: Set up the user's persistent HAIL interaction profile in Codex. Use when the user explicitly wants to initialize HAIL or create persistent HAIL defaults.
---

# Set up HAIL in Codex

Create or initialize the user's persistent HAIL profile intentionally and conversationally.

## Shared profile resources

Before profile work, load the bundled root HAIL resources:

- `../hail/references/profile-schema.md` — current profile shape and allowed values;
- `../hail/references/default-profile.yaml` — authoritative current defaults;
- `../hail/references/profile-normalization.md` — compatibility and normalization rules.

Do not redefine schema, defaults, migration, or compatibility behavior in this skill.

## Setup behavior

- If a stored profile exists, normalize and validate it using the shared resources before deciding whether it is already configured.
- If a valid normalized profile already exists, explain that HAIL is already configured and ask whether the user wants to change it instead of silently replacing it.
- If no profile exists, copy the bundled default profile as the working profile, then apply explicit preferences the user gives during setup.
- Prefer direct natural-language mapping over a forced questionnaire.
- Never infer diagnoses or neurotypes.
- Validate the resulting profile against the bundled schema before writing `~/.hail/profile.yaml`.
- Regenerate HAIL's managed block in `$CODEX_HOME/AGENTS.md` using the root `$hail` projection mappings.
- Preserve all content outside the HAIL markers exactly. Replace one complete block or append one if absent; never create duplicates. If markers are malformed or multiple blocks exist, stop and ask permission to repair rather than guessing.
- Summarize the resulting behavior in plain language.

This skill must produce the same persistent profile and Codex projection that equivalent setup intent handled through the root `$hail` skill would produce.
