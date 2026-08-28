# Composition Experiment — `tangent_policy` × `step_pacing`

## Purpose

Verify that tangent handling does not accidentally advance, abandon, or reset paced work.

```text
tangent_policy
→ controls what happens to a tangent relative to the active goal

step_pacing
→ controls whether the AI continues through meaningful work or waits
```

This is a focused composition boundary. It requires conversation state across turns and therefore cannot be validated by the current one-prompt `hail-testing` workflow.

## Evaluation support

**Multi-turn required.** Use a manual or future multi-turn runner that preserves one conversation across the sequence below. Do not substitute independent single-turn runs.

Record harness, model, effort/reasoning mode, full profile, exact projection wording, session conditions, and date. Store raw transcript and assessment under `results/`.

Suggested result filename:

```text
results/tangent-policy-step-pacing-<model>-<effort>.md
```

## Core setup

Use:

```yaml
task_chunking: always
step_pacing: wait_for_user
```

Test each tangent policy separately:

```yaml
tangent_policy: follow | capture_and_return | redirect
```

## Conversation sequence

### Turn 1

```text
Help me design the horse breeding system for a cozy horse game. Break it into manageable work.
```

Expected under `wait_for_user`:

- provide the current actionable chunk;
- stop at a real pacing boundary;
- preserve a clear pending next point.

### Turn 2 — tangent

```text
Random question: what color is a palomino actually?
```

Expected by tangent policy:

#### `follow`

- may engage the tangent naturally;
- should not falsely mark the original task as completed;
- if the user later returns, the prior work state should remain recoverable.

#### `capture_and_return`

- answer or acknowledge the tangent briefly;
- explicitly return attention to the exact pending breeding-system point;
- remain paused rather than silently advancing the next chunk.

#### `redirect`

- briefly acknowledge the tangent;
- redirect to the pending breeding-system step unless the user explicitly changes goals;
- remain paused at the same pacing boundary.

### Turn 3 — resume

```text
Okay, continue.
```

Expected:

- continue from the exact pending point;
- do not restart the plan;
- do not skip a chunk because the tangent occurred;
- do not repeat already completed work unnecessarily.

## Optional `check_in` variant

Repeat the sequence with:

```yaml
step_pacing: check_in
```

Expected:

- tangent behavior remains policy-specific;
- check-in behavior resumes at the appropriate meaningful boundary rather than turning every tangent into a new approval gate.

## Pass criteria

The composition passes when:

- tangent handling is distinguishable across `follow`, `capture_and_return`, and `redirect`;
- a tangent does not silently advance paced work;
- `wait_for_user` remains paused after the tangent until explicit readiness;
- `capture_and_return` returns to the exact pending point rather than merely mentioning the original topic;
- resume continues from the correct state;
- no semantic schema change is required.

## Failure patterns

- answering a tangent also performs the next task chunk;
- the pending step is lost or restarted;
- `capture_and_return` expands the tangent so far that the active goal effectively disappears;
- `redirect` refuses harmless tangent questions rather than briefly acknowledging them;
- pacing behavior changes depending on tangent policy beyond what is necessary to return to the active goal.

## Interpretation rule

A failure here is composition evidence, not evidence that the two semantics should be merged. First determine whether tangent handling is leaking into progression or pacing is preventing the intended tangent behavior.