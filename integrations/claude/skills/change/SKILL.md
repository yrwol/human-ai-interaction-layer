---
name: change
description: Change one or more persistent HAIL interaction defaults. Use only when the user explicitly wants to modify HAIL or their persistent HAIL preferences.
---

# Change HAIL profile

Modify only the persistent HAIL preferences the user intentionally wants changed.

## Persistence boundary

This skill is an explicit persistent-configuration surface. Do not treat ordinary conversational instructions outside HAIL configuration as permission to persist changes.

## Canonical profile

Read and write `~/.hail/profile.yaml` using the current v0.1 fields:

- `verbosity`: `compact`, `balanced`, `detailed`
- `decision_mode`: `options`, `recommend_first`, `choose_by_default`
- `max_options`: positive integer
- `task_chunking`: `off`, `adaptive`, `always`
- `step_pacing`: `continuous`, `check_in`, `wait_for_user`
- `tangent_policy`: `follow`, `capture_and_return`, `redirect`

## Change behavior

- Read the existing profile first.
- If no profile exists, start from the v0.1 defaults, apply the requested change, and make clear that this initializes HAIL as part of the explicit change request.
- When reading an older valid profile without `step_pacing`, treat it as `continuous` unless the current explicit request specifies another value.
- Preserve every valid preference the user did not ask to change.
- Map clear natural-language requests directly; ask only the minimum clarification when two materially different mappings are plausible.
- Never infer diagnoses or neurotypes.
- Validate the complete profile before writing it.
- Regenerate `~/.hail/claude-code.md` using the same semantic projection mappings as the root `hail` skill.
- Ensure `~/.claude/CLAUDE.md` contains exactly one standalone `@~/.hail/claude-code.md` import and preserve all unrelated content exactly.
- Explain what changed in behavioral terms rather than implementation terms.

This skill must produce the same persistent profile and Claude projection that equivalent change intent handled through the root `hail` skill would produce.