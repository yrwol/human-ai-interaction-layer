# Prompt-Hardening Experiment — `verbosity`

## Semantic intent

Control response detail and length without sacrificing correctness, necessary context, or the user's explicit current request.

## Values to exercise

```yaml
verbosity: compact | balanced | detailed
```

Primary focus: make the three modes behaviorally distinct across harnesses without reducing them to arbitrary word counts.

## Known concern

Earlier evidence showed weak-to-moderate enforcement and inconsistent interpretation across harnesses.

## Scenario 1 — ordinary explanation

Prompt:

```text
Explain how a horse temperament system could affect gameplay in a cozy horse game.
```

Expected:

- `compact`: answer the core question directly with minimal supporting detail.
- `balanced`: provide enough explanation to understand the system and its main implications without exhaustiveness.
- `detailed`: include deeper mechanics, examples, edge cases, and tradeoffs when useful.

## Scenario 2 — simple factual request

Prompt:

```text
What is a palomino?
```

Expected:

All modes should remain proportionate to the simplicity of the task. `detailed` should not automatically turn a simple answer into an essay.

## Scenario 3 — explicit current override

Prompt:

```text
Give me a very detailed breakdown of how horse training progression could work.
```

Expected:

The explicit request for detail should override a compact persistent default for this interaction.

## Candidate wording direction

### `compact`

```text
Prefer concise responses that deliver the answer, recommendation, or next action with only the context needed to understand and use it. Avoid throat-clearing, repetition, and optional elaboration unless requested or necessary.
```

### `balanced`

```text
Use enough detail to make the answer clear, actionable, and well-supported without being exhaustive. Include important reasoning and tradeoffs, but omit repetition and low-value elaboration.
```

### `detailed`

```text
When the task benefits from depth, provide fuller reasoning, relevant examples, tradeoffs, and important edge cases. Stay organized and purposeful rather than expanding simple requests unnecessarily.
```

## Boundary to watch

Verbosity is not the same as:

- task chunking;
- number of options;
- explanation complexity;
- step pacing.

A compact response may still contain several necessary steps, and a detailed response may still recommend only one option.

## Composition check

Useful pairings:

```yaml
task_chunking: adaptive
max_options: 3
```

Verify that changing verbosity affects **depth of explanation**, not the semantic behavior of those other preferences.

## Results

Use [`template.md`](template.md) structure for each candidate iteration.
