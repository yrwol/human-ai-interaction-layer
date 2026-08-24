# Task Chunking Hardening — Claude Sonnet Differentiation Pass

## Outcome

**Result: pass for base differentiation — Candidate A is strong enough to proceed to boundary and composition testing.**

This authoritative pass used the real Claude HAIL plugin (`integrations/claude`) rather than the reference .NET compiler. Each run used the same prompt and identical profile values except for `task_chunking`.

```text
intended decomposition

off < adaptive < always

observed collaboration structure

off < adaptive < always
```

## Test prompt

```text
I want to build a cozy horse game with horse care, training, competitions, breeds, and progression, but I don't know where to start. Help me plan it.
```

## Controlled profile values

```yaml
verbosity: balanced
decision_mode: recommend_first
max_options: 3
step_pacing: continuous
tangent_policy: capture_and_return
```

Only `task_chunking` changed between runs.

## Candidate A projection

### `off`

```text
Keep the user's work whole by default rather than proactively decomposing it into execution steps. You may still use headings, bullets, or other presentation structure when useful for clarity or correctness, and break work into steps when the user explicitly asks.
```

### `adaptive`

```text
When a task is meaningfully complex, cognitively heavy, or easier to act on incrementally, decompose it into a small number of meaningful actionable chunks. Prefer broader chunks than an always-step-by-step approach, and answer simple requests directly without unnecessary decomposition.
```

### `always`

```text
For genuinely multi-step work, deliberately partition the work in the current response into small, concrete, independently actionable chunks. Do not merely describe or promise a future step-by-step plan. Do not manufacture a step-by-step process for trivial or single-step requests.
```

## Results

### `off`

Source plugin run: `32782790443`

Observed behavior:

- gave one primary recommendation: start with the core loop;
- explained why and identified a few scoping questions;
- offered two alternative entry points as options, but did not turn the requested work into an execution sequence;
- preserved presentation structure without confusing formatting with task decomposition.

Assessment: **strong pass**.

### `adaptive`

Source plugin run: `32782817167`

Observed behavior:

- explicitly said, “So I'd break planning into these chunks”;
- produced five broad planning areas: vision/scope, core loop, systems design, tech foundation, MVP cut;
- chunks were meaningfully larger conceptual units rather than granular execution steps;
- clearly more decomposed than `off`, while remaining broader than `always`.

Assessment: **strong pass**, with a minor note that five broad chunks is near the upper edge of “small number.” This is not currently a hardening failure because the chunks remain broad and cognitively meaningful.

### `always`

Source plugin run: `32782833690`

Observed behavior:

- immediately partitioned the current response into numbered sections;
- included a build-order sequence of seven concrete system steps;
- ended with a concrete first actionable chunk for immediate implementation;
- did not defer decomposition to a future reply;
- visibly more granular/actionable than `adaptive`.

Assessment: **strong pass**.

## Comparison against baseline

Baseline failed because visible decomposition was effectively inverted (`always < adaptive < off`). Candidate A repaired both endpoints and made the middle value behaviorally identifiable.

The distinction is now observable without relying only on headings:

- `off`: discusses and recommends while keeping the work mostly whole;
- `adaptive`: decomposes complex work into broad meaningful work areas;
- `always`: deliberately creates smaller, execution-oriented units in the current response.

## Decision

**Keep Candidate A. Do not introduce Candidate B yet.**

There is no concrete endpoint failure remaining that justifies stronger wording. Additional strengthening risks semantic drift or pathological over-structuring.

## Next tests

1. Run the simple-request boundary case to verify `adaptive` and `always` do not over-apply to trivial/single-step work.
2. If the boundary passes, proceed to `task_chunking × step_pacing` composition.
3. Keep the real-plugin HAIL eval path as the authoritative Claude hardening harness; the earlier .NET-compiler runs are historical diagnostics only.

## Outcome status

- Claude base differentiation: **strong**
- Candidate A: **selected**
- Semantic drift observed: **no**
- Ready for boundary test: **yes**
- Ready for composition after boundary: **yes**
