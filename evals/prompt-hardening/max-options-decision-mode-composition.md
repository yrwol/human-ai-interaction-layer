# Composition Eval — `max_options` × `decision_mode` × ambiguity

## Purpose

Determine why an informational prompt under Candidate B caused Claude to ask a decision-shaped clarification instead of answering the informational list directly.

This eval is intentionally narrow. Do not change Candidate B yet. First isolate whether the behavior comes from:

1. `max_options` over-classifying ambiguous informational prompts as choice-like output;
2. `decision_mode: recommend_first` amplifying ambiguity into a decision interaction; or
3. the combination of both fields.

## Triggering prompt

```text
What stats should each horse have, and what does each stat represent?
```

Observed under Candidate B + `decision_mode: recommend_first`:

Claude asked what type of horse game was being designed and presented several game-type choices instead of directly providing an informational stat list.

That is not a direct `max_options` violation. The concern is that the model may be manufacturing a choice interaction to resolve ambiguity that did not materially block a useful answer.

## Question under test

> Is Candidate B causing an informational-list classification problem, or is `decision_mode` interacting with ambiguity and pushing the model into unnecessary decision-making?

## Test controls

Keep all other HAIL values identical across runs.

Recommended baseline:

```yaml
verbosity: balanced
max_options: 3
task_chunking: adaptive
step_pacing: continuous
tangent_policy: capture_and_return
```

Change only the variables named in each run.

Use a fresh session for every run.

## Run A — Candidate B + `recommend_first`

```yaml
max_options: 3
decision_mode: recommend_first
```

Prompt:

```text
What stats should each horse have, and what does each stat represent?
```

### Observe

- Does it answer directly?
- Does it ask for game-type clarification?
- Does it present choices before answering?
- If it answers directly, does it incorrectly cap the informational list at three stats?

### Result

```text
PASTE RAW RESULT HERE
```

## Run B — Candidate B + `options`

```yaml
max_options: 3
decision_mode: options
```

Use the exact same prompt.

### Interpretation

If the unnecessary clarification disappears here, `decision_mode: recommend_first` is likely contributing materially to the behavior.

### Result

```text
PASTE RAW RESULT HERE
```

## Run C — Candidate B + `choose_by_default`

```yaml
max_options: 3
decision_mode: choose_by_default
```

Use the exact same prompt.

### Interpretation

Useful for distinguishing whether the issue is specific to recommendation behavior or whether any decision-oriented mode causes the model to turn ambiguity into a choice point.

### Result

```text
PASTE RAW RESULT HERE
```

## Run D — Previous `max_options` wording + `recommend_first`

Restore the pre-Candidate-B `max_options` projection wording while keeping:

```yaml
decision_mode: recommend_first
```

Use the exact same prompt.

### Interpretation

If the unnecessary clarification occurs even without Candidate B, the behavior is probably not caused by the new `max_options` classification language.

### Result

```text
PASTE RAW RESULT HERE
```

## Run E — Direct informational control

Use Candidate B + `recommend_first`.

Prompt:

```text
List the useful horse stats for a cozy horse game and briefly explain what each stat represents. This is an informational list, not a decision between alternatives.
```

### Expected

- answer directly;
- more than three stats are allowed when useful;
- no manufactured game-type decision;
- no mention of the HAIL option cap.

### Result

```text
PASTE RAW RESULT HERE
```

## Run F — Genuine ambiguity control

Use Candidate B + `recommend_first`.

Prompt:

```text
What stats should each horse have?
```

### Observe

This prompt intentionally removes the explanatory framing and leaves more room for interpretation.

A clarification is acceptable only if the missing context would materially change the usefulness of the answer. Otherwise the model should make a reasonable assumption and answer.

### Result

```text
PASTE RAW RESULT HERE
```

## Classification

After running A–F, classify the failure mode.

### A — `max_options` classification problem

Evidence pattern:

- Candidate B consistently transforms informational prompts into choice interactions regardless of `decision_mode`;
- previous wording does not;
- direct informational controls still trigger unnecessary choice framing.

Action:

Refine Candidate B's boundary language so it distinguishes *choices the user must evaluate* from *ordinary content generation under ambiguity*.

### B — `decision_mode` composition problem

Evidence pattern:

- behavior appears primarily under `recommend_first` or another decision mode;
- Candidate B behaves normally when decision mode is neutralized/changed;
- previous `max_options` wording shows similar behavior under the same decision mode.

Action:

Harden `decision_mode` so it governs how the model handles real decisions without converting ordinary ambiguity into a decision point.

### C — general model ambiguity behavior

Evidence pattern:

- behavior persists across wording and decision-mode changes;
- Candidate B is not a meaningful causal factor.

Action:

Do not weaken `max_options` merely to compensate. Decide whether HAIL needs a separate clarification/assumption behavior later, or accept this as harness-native behavior if it is not materially disruptive.

### D — composition-specific interaction

Evidence pattern:

- neither field causes the behavior reliably alone;
- the combination does.

Action:

Document the composition rule and test the smallest wording change that prevents amplification without changing either semantic's meaning.

## Pass criteria

Candidate B is still viable if:

- brainstorming remains limited to the configured number of meaningful choices;
- ordinary informational lists are not capped merely because they contain more than `max_options` items;
- ambiguity does not routinely become an unnecessary choice interaction;
- `decision_mode` remains behaviorally distinct;
- no semantic meaning needs to change to get reliable composition.

## Important constraint

Do not solve this by making the prompt say "never ask clarifying questions." Clarification can be correct when missing information materially affects the answer.

The desired behavior is:

> clarify when materially necessary; otherwise make a reasonable assumption and answer.
