---
name: setup
description: Set up the user's persistent HAIL interaction profile. Use when the user explicitly wants to initialize HAIL or create their persistent HAIL defaults.
---

# Set up HAIL

Create or initialize the user's persistent HAIL profile intentionally and conversationally.

## Persistence boundary

This skill is an explicit HAIL configuration surface. Ordinary conversation outside explicit HAIL configuration must not silently mutate persistent defaults.

## Canonical profile

Write the canonical profile to `~/.hail/profile.yaml` using the current v0.1 schema:

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

- If a valid profile already exists, explain that HAIL is already configured and ask whether the user wants to change it instead of silently replacing it.
- If no profile exists, start from the v0.1 defaults and apply any explicit preferences the user gives during setup.
- Prefer mapping clear natural-language needs directly instead of forcing a questionnaire.
- Preserve human intent; do not invent fields or infer diagnoses/neurotypes.
- Validate all values before writing.
- After writing the profile, regenerate `~/.hail/claude-code.md` using the same semantic projection mappings as the root `hail` skill and ensure `~/.claude/CLAUDE.md` contains exactly one standalone `@~/.hail/claude-code.md` import while preserving all unrelated content.
- Summarize the resulting behavior in plain language. Do not narrate file operations unless asked.

This skill must produce the same persistent profile and Claude projection that equivalent setup intent handled through the root `hail` skill would produce.