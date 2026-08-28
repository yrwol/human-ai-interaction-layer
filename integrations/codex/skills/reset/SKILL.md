---
name: reset
description: Reset the user's persistent HAIL interaction profile in Codex to supported defaults. Use only when the user explicitly wants to reset HAIL configuration.
---

# Reset HAIL profile in Codex

Reset persistent HAIL configuration intentionally without damaging unrelated Codex configuration.

## Reset behavior

- Read `~/.hail/profile.yaml` if it exists.
- Confirm whether the user means the whole profile or only one preference when scope is ambiguous.
- Whole-profile defaults are:
  - `verbosity: balanced`
  - `decision_mode: recommend_first`
  - `max_options: 3`
  - `task_chunking: adaptive`
  - `step_pacing: continuous`
  - `tangent_policy: capture_and_return`
- For a single-preference reset, restore only that field's default and preserve every other valid preference.
- Validate the resulting complete profile before writing.
- Regenerate HAIL's managed block in `$CODEX_HOME/AGENTS.md` (normally `~/.codex/AGENTS.md`) using the same semantic projection mappings as the root `hail` skill.
- Preserve all content outside `<!-- HAIL:START -->` / `<!-- HAIL:END -->` exactly. Replace one complete block or append one if absent; never create duplicates. If markers are malformed or multiple blocks exist, stop and ask permission to repair rather than guessing.
- Never infer diagnoses or neurotypes.
- Explain the resulting behavior in plain language.

If HAIL is not configured, explain that there is nothing persistent to reset rather than creating a profile unnecessarily.

This skill must produce the same persistent profile and Codex projection that equivalent reset intent handled through the root `hail` skill would produce.