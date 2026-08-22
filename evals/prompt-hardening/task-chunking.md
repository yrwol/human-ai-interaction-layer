# Prompt-Hardening Experiment — `task_chunking`

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

Earlier cross-harness evidence showed weak/inconsistent enforcement, including behavior that could appear inverted. The main risk is that all three values collapse into generic step-by-step formatting, or that `always` over-applies to trivial one-step requests.

## Test method

Use the smallest repair loop that answers the current question:

1. run the targeted differentiation scenario;
2. change only the projection wording for `task_chunking`;
3. rerun the same prompt in a fresh interaction;
4. if the distinction improves, run the boundary/regression scenarios;
5. only then run composition with `step_pacing`.

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

## Phase 2 — boundary / regression

Run only after the targeted distinction improves.

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

## Candidate wording direction

Use these as current candidates, not schema definitions.

### `adaptive`

```text
Break work into smaller steps when the task is complex, cognitively heavy, or easier to act on incrementally. For simple requests, answer directly without inventing unnecessary steps.
```

### `always`

```text
For genuinely multi-step work, deliberately divide the task into small, concrete, independently actionable pieces rather than presenting the whole task as one large block. Do not manufacture a step-by-step process for trivial or single-step requests.
```

### `off`

```text
Keep work whole by default rather than proactively decomposing it into execution steps. Use headings, bullets, or other structure when useful for clarity or correctness, and break work into steps when the user explicitly asks.
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

Only after `task_chunking` works reasonably alone.

Test at minimum:

```yaml
task_chunking: always
step_pacing: continuous
```

versus:

```yaml
task_chunking: always
step_pacing: wait_for_user
```

Use the same multi-step prompt.

Expected composition:

```text
chunking
→ determines the small pieces

pacing
→ determines whether the AI continues through those pieces or waits
```

Pass conditions:

- both configurations produce similarly sized pieces;
- `continuous` may proceed through several pieces in one interaction;
- `wait_for_user` should stop after the current actionable piece;
- changing pacing must not silently change chunk size.

## Promotion criteria

Promote wording only if:

- `off`, `adaptive`, and `always` are behaviorally distinguishable;
- `always` does not proceduralize trivial requests;
- `off` still permits structure needed for clarity/correctness;
- explicit current requests can override the persistent default;
- composition with `step_pacing` preserves the chunking/pacing boundary;
- no semantic schema change is required.

## Results

Store raw runs and assessments under:

```text
results/task-chunking-<model>-<effort>.md
```

Use [`template.md`](template.md) for the result record.
