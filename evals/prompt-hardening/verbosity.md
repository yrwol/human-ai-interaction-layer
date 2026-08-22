# Prompt-Hardening Experiment — `verbosity`

## Semantic intent

Control the **depth of explanation** the user receives while preserving correctness, necessary context, and the user's explicit current request.

Verbosity should change how much supporting detail is surfaced, not who owns decisions, how many options are offered, how work is chunked, or whether the AI pauses.

## Values to exercise

```yaml
verbosity: compact | balanced | detailed
```

## Behavioral contract

The values should be distinguishable by behavior alone:

```text
compact
→ core answer + only the context needed to understand/use it

balanced
→ core answer + important reasoning/tradeoffs needed for confident action

detailed
→ core answer + fuller reasoning, examples, implications, tradeoffs, and relevant edge cases when the task benefits from depth
```

The distinction is **information depth**, not arbitrary word count.

## Known concern

Earlier evidence showed weak-to-moderate enforcement and inconsistent interpretation across harnesses. Main risks:

- all three modes converge on the harness/model's native response length;
- `compact` removes necessary reasoning rather than optional elaboration;
- `detailed` becomes indiscriminately long;
- changing verbosity accidentally changes option count, task chunking, or decision behavior.

## Test method

Use the smallest repair loop that answers the current question:

1. run the targeted differentiation scenario;
2. change only `verbosity` projection wording;
3. rerun the same prompt in a fresh interaction;
4. if differentiation improves, run boundary/regression scenarios;
5. only then test composition with another stable semantic.

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

### Scenario 1 — explanation with real room for depth

Prompt:

```text
Explain how a horse temperament system could affect gameplay in a cozy horse game.
```

Expected observable behavior:

#### `compact`

- directly states a usable core model;
- includes only enough rationale/detail to understand how it affects gameplay;
- omits optional examples, secondary implications, and edge cases unless necessary.

#### `balanced`

- explains the core model and the most important gameplay consequences;
- includes enough reasoning/tradeoff context to make the design understandable and actionable;
- does not attempt to cover every implication.

#### `detailed`

- retains the same core answer but meaningfully expands relevant reasoning;
- may include examples, interactions, tradeoffs, implementation implications, or important edge cases;
- added detail should deepen understanding rather than merely repeat the answer.

### Differentiation pass condition

A reviewer should be able to distinguish the three values by **what explanatory layers are included**, not merely by counting words.

Failure examples:

- `compact` and `balanced` communicate essentially identical depth;
- `detailed` is only longer because it repeats or restates;
- one mode changes the actual recommendation/system rather than just explanation depth.

## Phase 2 — boundary / regression

Run only after the primary differentiation improves.

### Scenario 2 — simple factual request

Prompt:

```text
What is a palomino?
```

Expected:

- all modes remain proportionate to the simplicity of the question;
- `compact` may answer in one or two concise sentences;
- `balanced` may add one useful distinction or contextual detail;
- `detailed` may add relevant nuance, but must not manufacture an essay merely to satisfy the preference.

Failure pattern to watch:

> interpreting `detailed` as a command to maximize length regardless of informational value.

### Scenario 3 — explicit current override

Prompt:

```text
Give me a very detailed breakdown of how horse training progression could work, including examples and edge cases.
```

Expected:

The explicit request for detail should override `verbosity: compact` for this interaction. The persistent profile remains unchanged.

### Scenario 4 — necessary brevity / direct answer

Prompt:

```text
In one sentence, what's the biggest design risk of making every horse stat independently trainable?
```

Expected:

The explicit one-sentence request should constrain `verbosity: detailed`. Persistent detail preference must not override a direct current instruction.

## Candidate wording direction

Use these as current candidates, not schema definitions.

### `compact`

```text
Prefer concise responses that deliver the answer, recommendation, or next action with only the context needed to understand and use it. Preserve necessary reasoning, caveats, and accuracy, but omit optional elaboration, repetition, and low-value examples unless requested.
```

### `balanced`

```text
Provide enough detail to make the answer clear, actionable, and well-supported. Include the important reasoning and tradeoffs that materially help understanding, while omitting exhaustive examples, secondary edge cases, repetition, and low-value elaboration.
```

### `detailed`

```text
When the task benefits from depth, provide fuller reasoning and the relevant examples, tradeoffs, implications, and important edge cases that deepen understanding. Stay organized and purposeful, and keep simple requests proportionate rather than expanding them for length alone.
```

## What not to optimize for

Do not judge success by:

- exact word count;
- number of bullets/headings;
- number of options;
- number of task steps;
- whether the answer is conversational versus structured.

The observable question is whether the **amount of useful explanatory context** changes while the underlying task behavior stays stable.

## Semantic boundaries

Verbosity is not the same as:

```text
max_options
→ how much simultaneous choice is surfaced

task_chunking
→ how work is partitioned

step_pacing
→ whether the AI continues or waits

decision_mode
→ who owns a decision / whether the AI recommends or chooses
```

A compact response may still contain several necessary steps. A detailed response may still offer only one recommendation. A detailed response should not invent more alternatives just to create more content.

## Phase 3 — composition

Only after `verbosity` is reasonably distinct alone.

### Composition A — `max_options`

Use:

```yaml
max_options: 3
```

Prompt:

```text
What are some good approaches for horse training in a cozy game?
```

Expected:

- all verbosity values honor the same choice-set limit;
- `compact` explains each option minimally;
- `balanced` gives useful tradeoffs;
- `detailed` may deepen each option/tradeoff but must not escape the cap with bonus/nested alternatives.

### Composition B — `task_chunking`

Use a fixed `task_chunking` value and a multi-step design prompt.

Expected:

- changing verbosity changes the depth inside chunks;
- it does not materially change the number/size of chunks;
- detailed output must not be mistaken for more chunking.

## Promotion criteria

Promote wording only if:

- `compact`, `balanced`, and `detailed` show recognizable information-depth differences;
- necessary reasoning survives `compact`;
- `detailed` remains proportionate on simple tasks;
- explicit current instructions override the persistent default;
- changing verbosity does not materially change decision ownership, option count, chunking, or pacing;
- added detail is informative rather than repetitive;
- no semantic schema change is required.

## Results

Store raw runs and assessments under:

```text
results/verbosity-<model>-<effort>.md
```

Use [`template.md`](template.md) for the result record.
