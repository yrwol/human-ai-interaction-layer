# Prompt-Hardening Experiment — `task_chunking`

## Semantic intent

Break complex or cognitively heavy work into manageable pieces without unnecessarily fragmenting simple work.

## Values to exercise

```yaml
task_chunking: off | adaptive | always
```

Primary focus: distinguish `adaptive` from `always` clearly and prevent both from collapsing into generic step-by-step behavior.

## Known concern

Earlier evidence showed weak or inconsistent cross-harness enforcement, including behavior that could appear inverted.

## Baseline questions

1. Does `adaptive` chunk genuinely complex work while leaving trivial work intact?
2. Does `always` reliably produce small sequential pieces even when the model could otherwise give a full plan?
3. Does `off` avoid unsolicited decomposition while still allowing necessary structure?
4. Does stronger wording accidentally force excessive fragmentation?

## Scenario 1 — complex planning

Prompt:

```text
I want to build a cozy horse game with horse care, training, competitions, breeds, and progression, but I don't know where to start.
```

Expected:

- `off`: may answer holistically; no requirement to break the work into discrete chunks.
- `adaptive`: should recognize complexity and structure the work into manageable pieces.
- `always`: should strongly chunk the work into small concrete pieces.

## Scenario 2 — simple task

Prompt:

```text
Give me a name for a chestnut mare who is stubborn but sweet.
```

Expected:

- `off`: normal direct answer.
- `adaptive`: normal direct answer; should not manufacture a multi-step process.
- `always`: may still respect the configured chunking tendency, but should not become absurdly procedural for a trivial request.

## Scenario 3 — medium ambiguity

Prompt:

```text
Help me decide what stats a horse should have in a casual horse game.
```

Expected:

- `adaptive` should make a reasonable judgment about whether chunking helps.
- Observe whether Claude and Codex make materially different judgments.

## Candidate wording direction

Do not copy this blindly; use it as a starting hypothesis.

### `adaptive`

```text
Break work into smaller steps when the task is complex, cognitively heavy, or easier to act on incrementally. For simple requests, answer directly without inventing unnecessary steps.
```

### `always`

```text
For multi-step work, present the work in small, concrete pieces rather than delivering the entire plan at once. Keep each piece independently actionable. Do not over-fragment trivial one-step requests.
```

### `off`

```text
Do not proactively decompose work into a step-by-step sequence unless the user asks for it or structure is necessary for correctness.
```

## Composition check

Most important pairing:

```yaml
step_pacing: continuous | wait_for_user
```

Verify that:

- chunk size is controlled by `task_chunking`;
- movement through chunks is controlled by `step_pacing`;
- the two do not collapse into one behavior.

## Results

Use [`template.md`](template.md) structure for each candidate iteration.
