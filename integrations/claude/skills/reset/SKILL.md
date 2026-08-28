---
name: reset
description: Reset the user's persistent HAIL interaction profile to supported defaults. Use only when the user explicitly wants to reset HAIL configuration.
---

# Reset HAIL profile

Reset persistent HAIL configuration intentionally without damaging unrelated Claude configuration.

## Reset behavior

- If a stored profile exists, first load and apply the bundled root HAIL normalization contract at `../hail/references/profile-normalization.md`.
- Do not define migration or compatibility behavior in this skill.
- Confirm whether the user means the whole profile or only one preference before changing anything when that scope is ambiguous.
- Whole-profile defaults are:
  - `verbosity: balanced`
  - `decision_mode: recommend_first`
  - `max_options: 3`
  - `task_chunking: adaptive`
  - `step_pacing: continuous`
  - `tangent_policy: capture_and_return`
- For a single-preference reset, restore only that field's default and preserve all other normalized valid preferences.
- Validate the resulting complete current-schema profile before writing.
- Regenerate `~/.hail/claude-code.md` using the same semantic projection mappings as the root `hail` skill.
- Ensure `~/.claude/CLAUDE.md` contains exactly one standalone `@~/.hail/claude-code.md` import while preserving unrelated content exactly.
- Never infer diagnoses or neurotypes.
- Explain the resulting behavior in plain language.

If HAIL is not configured, explain that there is nothing persistent to reset rather than creating a profile unnecessarily.

This skill must produce the same persistent profile and Claude projection that equivalent reset intent handled through the root `hail` skill would produce.
