---
name: setup
description: Set up the user's persistent HAIL interaction profile. Use when the user explicitly wants to initialize HAIL or create their persistent HAIL defaults.
---

# Set up HAIL

Create or initialize the user's persistent HAIL profile intentionally and conversationally.

## Persistence boundary

This skill is an explicit HAIL configuration surface. Ordinary conversation outside explicit HAIL configuration must not silently mutate persistent defaults.

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
- Prefer mapping clear natural-language needs directly instead of forcing a questionnaire.
- Preserve human intent; do not invent fields or infer diagnoses/neurotypes.
- Validate the resulting profile against the bundled schema before writing `~/.hail/profile.yaml`.
- Regenerate `~/.hail/claude-code.md` using the root `hail` projection mappings and ensure `~/.claude/CLAUDE.md` contains exactly one standalone `@~/.hail/claude-code.md` import while preserving unrelated content.
- Summarize the resulting behavior in plain language.

This skill must produce the same persistent profile and Claude projection that equivalent setup intent handled through the root `hail` skill would produce.
