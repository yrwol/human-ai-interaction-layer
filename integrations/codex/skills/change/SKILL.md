---
name: change
description: Change one or more persistent HAIL interaction defaults in Codex. Use only when the user explicitly wants to modify HAIL or persistent HAIL preferences.
---

# Change HAIL profile in Codex

Modify only the persistent HAIL preferences the user intentionally wants changed.

## Canonical profile

Read and write `~/.hail/profile.yaml` using these v0.1 fields and allowed values:

- `verbosity`: `compact`, `balanced`, `detailed`
- `decision_mode`: `options`, `recommend_first`, `choose_by_default`
- `max_options`: positive integer
- `task_chunking`: `off`, `adaptive`, `always`
- `step_pacing`: `continuous`, `check_in`, `wait_for_user`
- `tangent_policy`: `follow`, `capture_and_return`, `redirect`

## Change behavior

- Read the existing profile first.
- If no profile exists, start from v0.1 defaults, apply the requested change, and make clear that this initializes HAIL as part of the explicit change request.
- Treat older valid profiles without `step_pacing` as `continuous` unless the current explicit request specifies otherwise.
- Preserve all valid preferences the user did not ask to change.
- Map clear natural-language intent directly; ask only the minimum clarification when materially different mappings are plausible.
- Never infer diagnoses or neurotypes.
- Validate the complete profile before writing.
- Regenerate HAIL's managed block in `$CODEX_HOME/AGENTS.md` (normally `~/.codex/AGENTS.md`) using the same semantic projection mappings as the root `hail` skill.
- Preserve all content outside the HAIL markers exactly. Replace one complete block or append one if absent; never create duplicates. If markers are malformed or multiple blocks exist, stop and ask permission to repair rather than guessing.
- Explain what changed in behavioral terms.

This skill must produce the same persistent profile and Codex projection that equivalent change intent handled through the root `hail` skill would produce.