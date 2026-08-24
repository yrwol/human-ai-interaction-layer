# Verbosity Hardening — Claude Sonnet

## Outcome

**Result: pass — current Claude verbosity projection is promoted unchanged.**

The real Claude plugin integration was exercised with isolated profiles where only `verbosity` changed. `task_chunking` was held at `off` to avoid conflating response depth with decomposition.

## Phase 1 — differentiation

Prompt:

```text
Explain how a horse temperament system could affect gameplay in a cozy horse game.
```

### compact

Run: `32783294121`

Observed behavior:

- gave a usable core model through three gameplay effects;
- preserved necessary rationale;
- omitted secondary implementation detail and edge cases.

Assessment: **pass**.

### balanced

Run: `32783299577`

Observed behavior:

- added a core model plus four gameplay consequences;
- added useful cozy-design tradeoffs and implementation guidance;
- remained selective rather than exhaustive.

Assessment: **pass**.

### detailed

Run: `32783309089`

Observed behavior:

- added an explicit multi-axis temperament model;
- explained interactions between traits and multiple downstream systems;
- included examples, implementation implications, and design tradeoffs;
- added useful explanatory layers rather than repetition.

Assessment: **pass**.

Observed information-depth ordering:

```text
compact < balanced < detailed
```

The distinction was visible through explanatory layers, not just raw length.

## Phase 2 — boundary / regression

### Simple factual request

Prompt:

```text
What is a palomino?
```

Runs:

- compact: `32783685124`
- balanced: `32783702117`
- detailed: `32783700108`

Observed:

- compact answered in two concise sentences with the essential genetic distinction;
- balanced added useful genetics, not-a-breed context, and color range;
- detailed added some additional genetics and breed/registry context but stayed proportionate to the simple question.

Assessment: **pass**. `detailed` did not expand indiscriminately.

### Explicit detail override against `compact`

Prompt:

```text
Give me a very detailed breakdown of how horse training progression could work, including examples and edge cases.
```

Run: `32783940701`

Observed:

- despite persistent `verbosity: compact`, Claude produced six training phases;
- included examples for each phase, discipline-specific details, and multiple edge cases;
- included additional cross-cutting failure modes and tradeoffs.

Assessment: **pass**. The explicit current request correctly overrode the persistent compact default.

### Explicit brevity override against `detailed`

Prompt:

```text
In one sentence, what's the biggest design risk of making every horse stat independently trainable?
```

Run: `32783726845`

Observed:

- Claude returned exactly one substantive sentence;
- persistent `verbosity: detailed` did not override the direct current constraint.

Assessment: **pass**.

## Projection decision

Keep the current Claude projection wording:

```text
compact
Keep responses compact. Include necessary detail, but avoid expanding beyond what is needed to make progress.

balanced
Use balanced verbosity: enough detail to be useful without overwhelming the user.

detailed
Provide detailed responses when useful, including relevant reasoning, context, and implementation detail.
```

The stronger candidate wording in `verbosity.md` is not needed based on current evidence. Avoid adding instruction complexity without a demonstrated failure mode.

## Promotion criteria

- recognizable information-depth differences: **pass**
- necessary reasoning survives compact: **pass**
- detailed remains proportionate on simple tasks: **pass**
- explicit current instructions override persistent defaults: **pass**
- no observed semantic schema change required: **pass**

## Verdict

**Verbosity is hardened for Claude Sonnet on the tested scenarios. Current wording is promoted unchanged.**
