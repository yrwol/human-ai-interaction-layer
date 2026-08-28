---
name: hail-setup
description: Set up the user's persistent HAIL interaction profile in Codex. Use when the user explicitly wants to initialize HAIL or create persistent HAIL defaults.
---

# Set up HAIL in Codex

Create or initialize the user's persistent HAIL profile intentionally and conversationally.

## Canonical profile

Write `~/.hail/profile.yaml` using the current v0.1 schema:

```yaml
version: 0.1
profile:
  verbosity: balanced
  decision_mode: recommend_first
  max_options: 3
  task_chunking: adaptive
  step_pacing: continuous
  tangent_policy: capture_and_return
```

Allowed values:
- `verbosity`: `compact`, `balanced`, `detailed`
- `decision_mode`: `options`, `recommend_first`, `choose_by_default`
- `max_options`: positive integer
- `task_chunking`: `off`, `adaptive`, `always`
- `step_pacing`: `continuous`, `check_in`, `wait_for_user`
- `tangent_policy`: `follow`, `capture_and_return`, `redirect`

## Setup behavior

- If a stored profile exists, normalize it using HAIL's canonical profile-normalization behavior before deciding whether it is already configured.
- Do not define migration or compatibility behavior in this skill; normalization semantics are authoritative in `spec/semantics.md` and executed by the root `$hail` profile-management contract.
- If a valid normalized profile already exists, explain that HAIL is already configured and ask whether the user wants to change it instead of silently replacing it.
- If no profile exists, start from defaults and apply explicit preferences the user gives during setup.
- Prefer direct natural-language mapping over a forced questionnaire.
- Never infer diagnoses or neurotypes.
- Validate the resulting current-schema profile before writing.
- After writing, regenerate HAIL's managed block in `$CODEX_HOME/AGENTS.md` (normally `~/.codex/AGENTS.md`) using the same semantic projection mappings as the root `$hail` skill.
- Preserve all content outside `<!-- HAIL:START -->` / `<!-- HAIL:END -->` exactly. Replace one complete block or append one if absent; never create duplicates. If markers are malformed or multiple blocks exist, stop and ask permission to repair rather than guessing.
- Summarize the resulting behavior in plain language.

This skill must produce the same persistent profile and Codex projection that equivalent setup intent handled through the root `$hail` skill would produce.