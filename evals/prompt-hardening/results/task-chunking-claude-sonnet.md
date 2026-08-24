# Task Chunking Hardening — Claude Sonnet Differentiation Pass

## Outcome

**Result: fail — targeted hardening is required before composition testing.**

The three values were tested on fresh Claude Code runners with the same prompt and identical profile values except for `task_chunking`.

Observed ordering did not match the semantic contract:

```text
intended decomposition

off < adaptive < always

observed visible decomposition in this pass

always < adaptive < off
```

The ordering should not be interpreted as a precise scalar measurement, but it demonstrates that the current projection wording does not reliably control the intended axis.

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

## Results summary

### `off`

Source run: `32778778307`

Compiled instruction:

```text
Do not automatically break work into smaller steps unless the user asks.
```

Observed:

- Claude explicitly said it would "break the planning itself into pieces";
- produced five ordered planning chunks;
- therefore proactively decomposed the task despite `off`.

Assessment: **weak / fail**.

### `adaptive`

Source run: `32778793665`

Compiled instruction:

```text
When a task is complex, ambiguous, or likely to create cognitive overload, break it into a small number of concrete next steps. Do not over-structure simple work.
```

Observed:

- narrowed the problem to the core gameplay loop;
- asked three scoping questions;
- proposed three broad next steps;
- reasonable adaptive behavior in isolation, but less visibly decomposed than `off`.

Assessment: **moderate in isolation, weak comparatively / fail**.

### `always`

Source run: `32778799924`

Compiled instruction:

```text
Break multi-step work into small, concrete, executable steps.
```

Observed:

- recommended a vertical slice;
- asked two clarifying questions;
- deferred actual decomposition: "Once I know those, I'll break it into concrete steps";
- did not perform the requested persistent chunking behavior in the current response.

Assessment: **weak / fail**.

## Failure diagnosis

### 1. `off` is phrased as a prohibition, not a positive collaboration strategy

`Do not automatically break work into smaller steps unless the user asks` leaves substantial room for Claude to classify a numbered plan as ordinary organization rather than task decomposition.

The intended semantic is better expressed positively: keep the work whole by default, while still allowing presentation structure for clarity.

### 2. `always` does not say decomposition must happen in the current response

`Break multi-step work into small, concrete, executable steps` is semantically correct but underspecified operationally. Claude satisfied it weakly by promising future concrete steps after clarification.

The projection should make clear that genuinely multi-step work should be deliberately partitioned into actionable units in the response rather than merely discussed as a future plan.

### 3. `adaptive` lacks a strong comparative middle point

The current wording is reasonable, but because `off` and `always` are weakly enforced, the middle value cannot be identified reliably by behavior alone.

Do not overfit `adaptive` until the two endpoints are stronger.

## Candidate repair A

This repair keeps the semantic schema unchanged and strengthens only the Claude projection wording.

### `off`

```text
Keep the user's work whole by default rather than proactively decomposing it into execution steps. You may still use headings, bullets, or other presentation structure when useful for clarity or correctness. Break the work into smaller actionable steps only when the user explicitly asks or when decomposition is necessary to complete the task correctly.
```

### `adaptive`

```text
When a task is meaningfully complex, cognitively heavy, or easier to act on incrementally, decompose it into a small number of meaningful actionable chunks. For simple work, answer directly. Prefer broader chunks than an always-step-by-step approach; chunk only when doing so materially improves comprehension or actionability.
```

### `always`

```text
For genuinely multi-step work, deliberately partition the work in the current response into small, concrete, independently actionable chunks. Do not merely describe or promise a future step-by-step plan. Continue through those chunks according to the user's pacing preference. Do not manufacture a workflow for trivial or single-step requests.
```

## Why this candidate

- strengthens the endpoints without changing semantic meaning;
- explicitly distinguishes task decomposition from presentation formatting;
- prevents `always` from satisfying the instruction by promising future steps;
- preserves the `task_chunking` / `step_pacing` boundary by saying continuation follows the pacing preference;
- protects trivial requests from pathological over-structuring.

## Next test

Update only the Claude `task_chunking` projection to Candidate repair A and rerun the exact same three-way differentiation prompt.

Do **not** run `task_chunking × step_pacing` composition yet. Composition would add another variable before the base semantic is stable.

Promotion gate:

- `off` should no longer proactively produce an execution sequence;
- `adaptive` should produce a small number of meaningful chunks for this complex prompt;
- `always` should visibly produce smaller/more actionable units than `adaptive` in the same response;
- a reviewer should be able to infer the value from collaboration structure without relying only on numbered headings.
