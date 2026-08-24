# Composition Experiment — `task_chunking` × `step_pacing`

## Purpose

Verify that `task_chunking` and `step_pacing` remain behaviorally independent when composed.

The semantic boundary is:

```text
task_chunking
→ controls the size / decomposition of work

step_pacing
→ controls whether the AI continues through those chunks or waits
```

This test should not be used to tune either semantic until each works reasonably well in isolation.

## Test conditions

Record for every run:

- harness;
- model;
- effort/reasoning mode;
- full profile values;
- exact projection wording for both preferences;
- whether the interaction/session was fresh;
- date.

Store raw outputs and assessment in a result file under `results/` using [`template.md`](template.md).

Suggested result filename:

```text
results/task-chunking-step-pacing-<model>-<effort>.md
```

## Core scenario

Prompt:

```text
Help me plan the horse breeding system for a cozy horse game. I need genetics, inheritance rules, breeding eligibility, foal outcomes, UI, and tests.
```

Use the same prompt for all configurations.

## Matrix

### A — `always` × `continuous`

```yaml
task_chunking: always
step_pacing: continuous
```

Expected:

- work is divided into small, concrete, independently actionable pieces;
- the AI may continue through several pieces in the same response;
- pacing should not cause the pieces themselves to become larger.

### B — `always` × `wait_for_user`

```yaml
task_chunking: always
step_pacing: wait_for_user
```

Expected:

- piece size is comparable to Configuration A;
- the AI presents only the current actionable piece and waits;
- it should not compensate for waiting by making the first piece much larger or by sneaking future chunks into the same response.

### C — `adaptive` × `continuous`

```yaml
task_chunking: adaptive
step_pacing: continuous
```

Expected:

- the task is complex enough that some decomposition is likely useful;
- chunks may be somewhat broader than `always`;
- the AI may continue naturally through them.

### D — `adaptive` × `wait_for_user`

```yaml
task_chunking: adaptive
step_pacing: wait_for_user
```

Expected:

- decomposition judgment should remain similar to Configuration C;
- the AI should stop after the current meaningful chunk;
- `wait_for_user` should change progression, not force finer decomposition by itself.

## Primary pass conditions

The test passes when both semantic effects are independently recognizable:

```text
changing task_chunking
→ changes chunk size/decomposition
→ does not primarily change whether the AI waits

changing step_pacing
→ changes continuation/waiting behavior
→ does not materially change chunk size
```

A reviewer should be able to compare A vs B and C vs D and see similar chunk sizing with different pacing.

A reviewer should also be able to compare A vs C and B vs D and see a chunking distinction while the pacing behavior remains stable.

## Failure patterns

### Pacing leaks into chunking

Examples:

- `wait_for_user` causes the AI to make a huge first chunk because it knows it must stop afterward;
- `continuous` causes the AI to collapse several logical chunks into one large plan;
- changing pacing materially changes how the task is decomposed.

### Chunking leaks into pacing

Examples:

- `task_chunking: always` causes the AI to wait even when `step_pacing: continuous`;
- `adaptive` keeps asking permission before proceeding despite continuous pacing;
- changing chunking alters approval-seeking/continuation behavior.

### Hidden future work

Under `wait_for_user`, failure includes presenting the first chunk and then appending:

- the rest of the plan;
- future implementation steps;
- a detailed preview that effectively performs later chunks;
- multiple independently actionable chunks disguised as one section.

A short orientation sentence about what will come later is fine; executing those later chunks is not.

## Boundary control — simple task

Prompt:

```text
Give me a name for a chestnut mare who is stubborn but sweet.
```

Expected:

- neither semantic should manufacture multi-step interaction for a truly one-step request;
- `wait_for_user` should not create an artificial approval checkpoint when there is nothing meaningful to pace;
- `always` should not proceduralize the request.

This confirms that composition does not amplify both preferences into pathological over-structuring.

## Optional `check_in` follow-up

After the `continuous` vs `wait_for_user` boundary is clear, add:

```yaml
task_chunking: always
step_pacing: check_in
```

Expected:

- same general chunk sizing as `always` in the other configurations;
- check-ins happen between meaningful chunks when useful;
- behavior should be distinguishable from both uninterrupted continuation and strict one-chunk waiting.

Do not use `check_in` to block promotion of the core two-way composition unless it exposes a genuine semantic collision.

## Promotion criteria

Consider this composition stable when:

- `task_chunking` controls decomposition without implicitly controlling continuation;
- `step_pacing` controls continuation without materially changing decomposition;
- `always × continuous` proceeds through small chunks without unnecessary pauses;
- `always × wait_for_user` stops after one comparable chunk without leaking future work;
- `adaptive` retains its own decomposition judgment under both pacing values;
- trivial requests remain proportional;
- no semantic-schema change is required to explain observed behavior.

## Interpretation rule

If composition fails, first identify which projection is leaking across the semantic boundary.

Do not merge the two fields or redefine their semantic meanings merely because one harness has difficulty following both instructions simultaneously.
