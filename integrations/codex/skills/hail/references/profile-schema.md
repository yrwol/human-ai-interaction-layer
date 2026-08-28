# HAIL canonical profile schema

This bundled reference defines the current runtime profile shape and allowed values for the Codex HAIL skill set. Repository-level semantic authority remains `spec/semantics.md`.

## Current schema

```yaml
version: 0.1
profile:
  verbosity: compact | balanced | detailed
  decision_mode: options | recommend_first | choose_by_default
  max_options: positive integer
  task_chunking: off | adaptive | always
  step_pacing: continuous | check_in | wait_for_user
  tangent_policy: follow | capture_and_return | redirect
```

Validate normalized profiles against this contract before persistence or projection.

## YAML serialization

When HAIL writes `task_chunking: off`, serialize the scalar as:

```yaml
task_chunking: "off"
```

This avoids YAML 1.1 parsers interpreting plain `off` as boolean `false`. Existing unquoted `task_chunking: off` profiles remain semantically valid and should be interpreted as the HAIL string value `off`.

Do not redefine allowed fields or values inside individual action skills.
