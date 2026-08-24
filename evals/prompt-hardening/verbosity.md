# Prompt-Hardening Experiment — `verbosity`

## Current status

**Claude projection: promoted unchanged.**

The existing Claude wording already produced recognizable `compact < balanced < detailed` information depth through the real HAIL plugin path. Boundary checks also showed that `detailed` remains proportionate on a simple factual request and that explicit current instructions override the persistent verbosity default in both directions.

No wording repair or semantic schema change was justified.

See [`results/verbosity-claude-sonnet.md`](results/verbosity-claude-sonnet.md).

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

Earlier evidence showed weak-to-moderate enforcement and inconsistent interpretation across harnesses. The Claude pass did not reproduce a hardening failure under the recorded conditions, so the current Claude wording is retained. Cross-harness replay remains necessary before treating that result as universal.

## Test method

Use the smallest repair loop that answers the current question:

1. run the targeted differentiation scenario;
2. change only `verbosity` projection wording if a concrete failure appears;
3. rerun the same prompt in a fresh interaction;
4. run boundary/regression scenarios;
5. test composition with another stable semantic only after individual behavior is stable.

Record raw outputs and test conditions under `results/` using [`template.md`](template.md).

Minimum metadata:

- harness;
- model;
- effort/reasoning mode;
- profile values;
- projection wording;
- whether the interaction/session was fresh;
- date.

## Phase 1 — differentiation

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

**Claude status:** passed with the existing projection wording.

## Phase 2 — boundary / regression

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

**Claude status:** passed. `detailed` added useful genetics/context without disproportionate expansion.

### Scenario 3 — explicit current override

Prompt:

```text
Give me a very detailed breakdown of how horse training progression could work, including examples and edge cases.
```

Expected:

The explicit request for detail should override `verbosity: compact` for this interaction. The persistent profile remains unchanged.

**Claude status:** passed. The compact profile yielded to the explicit detailed request and produced a substantially detailed response with examples and edge cases.

### Scenario 4 — necessary brevity / direct answer

Prompt:

```text
In one sentence, what's the biggest design risk of making every horse stat independently trainable?
```

Expected:

The explicit one-sentence request should constrain `verbosity: detailed`. Persistent detail preference must not override a direct current instruction.

**Claude status:** passed with a single substantive sentence.

## Current Claude projection wording

These are projection wording, not schema definitions. The existing wording is promoted unchanged.

### `compact`

```text
Keep responses compact. Include necessary detail, but avoid expanding beyond what is needed to make progress.
```

### `balanced`

```text
Use balanced verbosity: enough detail to be useful without overwhelming the user.
```

### `detailed`

```text
Provide detailed responses when useful, including relevant reasoning, context, and implementation detail.
```

Stronger candidate wording remains available as a future repair direction if another harness/model exposes a concrete failure, but should not replace wording that already satisfies the behavioral contract without evidence.

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

Composition is follow-up evidence for semantic independence, not a blocker to retaining the current Claude wording after the successful individual and boundary checks.

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

The Claude result satisfies the current individual/boundary promotion criteria:

- `compact`, `balanced`, and `detailed` show recognizable information-depth differences;
- necessary reasoning survives `compact`;
- `detailed` remains proportionate on simple tasks;
- explicit current instructions override the persistent default;
- added detail is informative rather than repetitive;
- no semantic schema change is required.

Composition remains useful as regression evidence that verbosity stays independent from neighboring semantics.

## Results

Authoritative Claude result:

[`results/verbosity-claude-sonnet.md`](results/verbosity-claude-sonnet.md)

Raw harness records are persisted by `hail-testing` for traceability.
