# Prompt-Hardening Experiment — `verbosity`

## Current status

**Cross-harness `detailed` projection hardening: promoted.**

The original Claude projection produced acceptable `compact < balanced < detailed` differentiation, proportionality, and explicit-override behavior. Cross-harness replay then exposed weaker `detailed` differentiation in Codex: the response was only marginally deeper than `balanced` and did not reliably add another useful explanatory layer.

A stronger `detailed` projection was therefore tested in both Codex and Claude. It materially improved information depth in Codex, remained strong in Claude, and preserved proportionality on simple factual requests. The shared wording is now promoted in both native integrations.

No semantic schema change was required.

Evidence:

- [`results/verbosity-claude-sonnet.md`](results/verbosity-claude-sonnet.md) — original Claude baseline and boundary evidence;
- [`results/verbosity-cross-harness-detailed.md`](results/verbosity-cross-harness-detailed.md) — promoted cross-harness repair evidence.

## Semantic intent

Control the **depth of explanation** the user receives while preserving correctness, necessary context, and the user's explicit current request.

Verbosity should change how much supporting detail is surfaced, not who owns decisions, how many options are offered, how work is chunked, or whether the AI pauses.

## Values to exercise

```yaml
verbosity: compact | balanced | detailed
```

## Behavioral contract

```text
compact
→ core answer + only the context needed to understand/use it

balanced
→ core answer + important reasoning/tradeoffs needed for confident action

detailed
→ core answer + fuller reasoning, examples, implications, tradeoffs, and relevant edge cases when the task benefits from depth
```

The distinction is **information depth**, not arbitrary word count.

## Hardening finding

The original Claude evidence did not justify changing the projection in isolation. Codex replay provided the missing concrete failure: `detailed` was too close to `balanced` in information depth on a prompt with substantial room for explanation.

The repair deliberately clarifies that detail means adding useful explanatory layers rather than merely expanding wording, while explicitly retaining proportionality for simple requests. Replaying the repair through Claude as well as Codex prevented a Codex-specific fix from silently regressing the other supported harness.

## Test method

Use the smallest repair loop that answers the current question:

1. run the targeted differentiation scenario;
2. change only `verbosity` projection wording if a concrete failure appears;
3. rerun the same prompt in a fresh interaction;
4. run boundary/regression scenarios;
5. replay shared candidates across supported harnesses before introducing harness-specific wording unnecessarily;
6. test composition with another stable semantic only after individual behavior is stable.

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

Prompt:

```text
Explain how a horse temperament system could affect gameplay in a cozy horse game.
```

Expected observable behavior:

### `compact`

- directly states a usable core model;
- includes only enough rationale/detail to understand how it affects gameplay;
- omits optional examples, secondary implications, and edge cases unless necessary.

### `balanced`

- explains the core model and the most important gameplay consequences;
- includes enough reasoning/tradeoff context to make the design understandable and actionable;
- does not attempt to cover every implication.

### `detailed`

- retains the same core answer but meaningfully expands relevant reasoning;
- may include examples, interactions, tradeoffs, implementation implications, or important edge cases;
- added detail should deepen understanding rather than merely repeat the answer.

Pass condition: a reviewer can distinguish the values by **what explanatory layers are included**, not merely by counting words.

**Status:** passed with the promoted cross-harness `detailed` wording under the recorded Claude and Codex configurations.

## Phase 2 — boundary / regression

### Simple factual request

```text
What is a palomino?
```

Expected: all modes remain proportionate to the simplicity of the question. `detailed` may add relevant nuance but must not manufacture an essay merely to satisfy the preference.

**Status:** passed in the cross-harness repair replay. Codex remained concise; Claude added useful genetics/appearance context without disproportionate expansion.

### Explicit detail override

```text
Give me a very detailed breakdown of how horse training progression could work, including examples and edge cases.
```

Expected: the explicit request for detail overrides `verbosity: compact` for the current interaction without mutating the persistent profile.

**Baseline status:** passed in the recorded Claude/Codex boundary evidence.

### Explicit brevity override

```text
In one sentence, what's the biggest design risk of making every horse stat independently trainable?
```

Expected: the explicit one-sentence request constrains `verbosity: detailed`.

**Baseline status:** passed in the recorded boundary evidence.

## Current promoted projection wording

`compact` and `balanced` remain harness-native wording. The repaired `detailed` behavior is intentionally shared across Claude and Codex:

```text
Provide detailed responses when the task benefits from depth. Add useful explanatory layers such as reasoning, interactions, examples, tradeoffs, edge cases, or implementation implications rather than merely expanding wording. Remain proportionate to simple requests.
```

This is projection wording, not a schema definition. The semantic contract remains vendor-neutral.

## What not to optimize for

Do not judge success by exact word count, number of bullets/headings, number of options, number of task steps, or conversational versus structured presentation.

The observable question is whether the **amount of useful explanatory context** changes while the underlying task behavior stays stable.

## Semantic boundaries

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

Composition remains useful follow-up evidence for semantic independence, but it is not a blocker to the promoted individual projection wording.

### `verbosity × max_options`

With a fixed option cap, changing verbosity should deepen explanation inside the allowed choice set without escaping the cap through bonus or nested alternatives.

### `verbosity × task_chunking`

With a fixed task-chunking value, changing verbosity should alter depth inside chunks without materially changing the number or size of chunks.

These checks should be run only with evaluation infrastructure capable of observing the required behavior. Do not claim multi-turn persistence or pacing evidence from the current single-turn workflow.

## Promotion criteria

The promoted cross-harness result satisfies the current individual/boundary criteria:

- recognizable information-depth differentiation;
- necessary reasoning survives `compact`;
- `detailed` adds useful explanatory layers rather than repetition;
- `detailed` remains proportionate on simple tasks;
- explicit current instructions retain priority over the persistent default;
- the repair works in both tested harnesses without changing semantic meaning.

## Results

Current promoted evidence:

[`results/verbosity-cross-harness-detailed.md`](results/verbosity-cross-harness-detailed.md)

Historical/baseline Claude evidence:

[`results/verbosity-claude-sonnet.md`](results/verbosity-claude-sonnet.md)

Raw harness records are persisted by `hail-testing` for traceability.
