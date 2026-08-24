# Prompt-Hardening Experiment — `task_chunking`

## Current status

**Claude projection: Candidate A promoted for base differentiation.**

The real-plugin Claude pass repaired the earlier inverted/weak behavior and produced the intended collaboration ordering:

```text
off < adaptive < always
```

The selected Claude wording is implemented in `integrations/claude/skills/hail/SKILL.md`. The remaining trivial-task boundary and `task_chunking × step_pacing` composition checks are follow-up regression/composition work; they are not evidence that a stronger Candidate B is currently needed.

See [`results/task-chunking-claude-sonnet.md`](results/task-chunking-claude-sonnet.md).

## Semantic intent

Control the **size and decomposition of work**: whether a task is kept whole or broken into smaller actionable pieces.

This preference is about **how work is partitioned**, not whether the AI pauses between pieces. Movement through pieces belongs to `step_pacing`.

## Values to exercise

```yaml
task_chunking: off | adaptive | always
```

## Behavioral contract

The values should be distinguishable by behavior alone:

```text
off
→ keep the task whole by default; use structure only when needed for correctness/readability

adaptive
→ decompose when complexity, cognitive load, or actionability materially benefits from smaller pieces

always
→ for genuinely multi-step work, deliberately structure the work into small actionable pieces even when a holistic response would be possible
```

Important boundary:

```text
task_chunking != step_pacing
```

A response may contain several chunks and still move through them continuously. Conversely, `wait_for_user` can pause after one chunk regardless of how that chunk was selected.

## Known concern

Earlier cross-harness evidence showed weak/inconsistent enforcement, including behavior that could appear inverted. The Claude Candidate A differentiation pass repaired that failure under the recorded test conditions. Cross-harness replay and composition remain useful because a strong Claude result is not a universal harness/model claim.

## Test method

Use the smallest repair loop that answers the current question:

1. run the targeted differentiation scenario;
2. change only the projection wording for `task_chunking`;
3. rerun the same prompt in a fresh interaction;
4. if the distinction improves, run the boundary/regression scenarios;
5. only then run composition with `step_pacing` using [`task-chunking-step-pacing-composition.md`](task-chunking-step-pacing-composition.md).

Record raw outputs and test conditions under `results/` using [`template.md`](template.md).

Minimum metadata:

- harness;
- model;
- effort/reasoning mode;
- profile values;
- projection wording;
- whether the interaction/session was fresh;
- date.

## Phase 1 — differentiation / targeted repair

### Scenario 1 — multi-step planning

Prompt:

```text
I want to build a cozy horse game with horse care, training, competitions, breeds, and progression, but I don't know where to start. Help me plan it.
```

Expected observable behavior:

#### `off`

- may organize the answer for readability;
- should not turn the response into a sequence of small execution chunks by default;
- may provide a holistic architecture/plan in one response.

#### `adaptive`

- should recognize that the task is complex enough to benefit from decomposition;
- should group the problem into a manageable number of meaningful pieces;
- chunks should reduce cognitive load rather than merely add headings.

#### `always`

- should clearly decompose the multi-step work into small actionable pieces;
- each piece should be independently understandable/actionable;
- should be visibly more chunked than `adaptive`, not merely differently worded.

### Differentiation pass condition

A reviewer should be able to identify which value produced the response from the collaboration structure, without relying on explicit labels such as "step 1" alone.

If `adaptive` and `always` look substantially identical, treat that as a failure even if both responses are reasonable.

**Claude status:** passed with Candidate A through the real HAIL plugin path.

## Phase 2 — boundary / regression

### Scenario 2 — trivial one-step request

Prompt:

```text
Give me a name for a chestnut mare who is stubborn but sweet.
```

Expected:

- all modes should remain proportionate to the task;
- `adaptive` should answer directly;
- `always` must **not** invent a procedural workflow for a task that is not meaningfully multi-step;
- `off` should answer normally.

Failure pattern to watch:

> interpreting `always` as "format every answer as steps" rather than "chunk multi-step work."

### Scenario 3 — structure needed for correctness/readability

Prompt:

```text
Explain the rules for calculating a horse's final competition score from base stats, bonuses, penalties, and tie-breaking rules.
```

Expected:

- `off` may still use bullets, equations, headings, or ordered structure where needed for clarity/correctness;
- `off` must not mean "never structure information";
- the preference should affect task decomposition, not prohibit useful presentation structure.

### Scenario 4 — explicit current override

Prompt:

```text
Break this into tiny steps for me: I need to design the horse breeding system, genetics, inheritance rules, UI, and tests.
```

Expected:

An explicit current request for small steps should override `task_chunking: off` for this interaction. Persistent semantics remain unchanged.

## Selected Claude wording — Candidate A

These are projection wording, not schema definitions.

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

## What not to optimize for

Do not judge success by:

- number of headings;
- presence of numbered lists;
- response length;
- whether the AI pauses;
- whether the answer sounds organized.

The observable question is whether the **unit of work presented to the user** changes appropriately.

## Phase 3 — composition with `step_pacing`

The dedicated composition suite is:

[`task-chunking-step-pacing-composition.md`](task-chunking-step-pacing-composition.md)

It tests `always` and `adaptive` under both `continuous` and `wait_for_user`, plus a trivial-task boundary, so chunk size and continuation behavior can be evaluated independently.

## Promotion criteria

For a fully closed harness-specific hardening cycle, verify:

- `off`, `adaptive`, and `always` are behaviorally distinguishable;
- `always` does not proceduralize trivial requests;
- `off` still permits structure needed for clarity/correctness;
- explicit current requests can override the persistent default;
- composition with `step_pacing` preserves the chunking/pacing boundary;
- no semantic schema change is required.

Current Claude evidence satisfies the first criterion strongly and provides no evidence of semantic drift; the remaining boundary/composition criteria are tracked as follow-up checks.

## Results

Authoritative Claude result:

[`results/task-chunking-claude-sonnet.md`](results/task-chunking-claude-sonnet.md)

Raw harness records are persisted by `hail-testing` for traceability.
