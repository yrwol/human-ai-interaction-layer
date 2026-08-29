# Composition Experiment — `verbosity` × `task_chunking`

## Purpose

Verify that explanatory depth changes independently from how work is partitioned.

```text
verbosity
→ controls depth within a chunk or response

task_chunking
→ controls the unit/size of work presented
```

This is a focused composition boundary, not part of either field's individual hardening contract.

## Evaluation support

**Multi-turn required for authoritative evaluation.**

The behavior HAIL cares about is not merely whether one answer contains more headings or bullets. `task_chunking` controls how work is partitioned and carried forward as collaboration proceeds across turns. A one-response comparison can be retained as a diagnostic snapshot, but it must not be promoted as authoritative composition evidence.

Use a manual or future multi-turn runner that preserves one conversation across the scenario. The evaluation should observe whether the same chunking strategy remains stable as the user completes, redirects, or resumes work while verbosity changes only the explanatory depth inside the active chunk.

Record harness, model, effort/reasoning mode, full profile, exact projection wording, conversation/session continuity, and date. Store the complete transcript and assessment under `results/` using `template.md`.

Suggested result filename:

```text
results/verbosity-task-chunking-<model>-<effort>.md
```

## Core scenario

Start one preserved conversation with:

```text
Help me plan a horse breeding system for a cozy horse game, including genetics, inheritance rules, breeding eligibility, foal outcomes, UI, and tests.
```

Then continue through at least two meaningful follow-up turns that advance the same work so the evaluator can observe whether chunk boundaries remain stable rather than judging decomposition from formatting alone.

Hold all other profile values constant. Run two fixed chunking bands separately:

```yaml
task_chunking: adaptive
```

and

```yaml
task_chunking: always
```

Within each band compare `verbosity: compact`, `balanced`, and `detailed`.

## Expected behavior

For a fixed `task_chunking` value:

- chunk count and chunk granularity remain broadly comparable across verbosity values;
- compact gives less explanation inside each chunk;
- balanced adds enough reasoning for confident action;
- detailed adds deeper reasoning, interactions, examples, implementation implications, tradeoffs, or edge cases;
- detailed must not simulate stronger chunking by splitting every explanation into additional execution units;
- compact must not simulate weaker chunking by collapsing distinct actionable chunks into one holistic answer.

## Cross-band check

After each verbosity band is internally stable, compare `adaptive` vs `always` at the same verbosity value.

Expected:

- `always` remains visibly more decomposed than `adaptive`;
- changing chunking changes work partitioning more than explanatory depth.

## Boundary control — simple task

Prompt:

```text
Give me a name for a chestnut mare who is stubborn but sweet.
```

Expected:

- neither detailed verbosity nor `task_chunking: always` manufactures a procedural workflow;
- verbosity may change nuance slightly, but the one-step nature of the request remains intact.

## Pass criteria

The composition passes when:

- verbosity changes information depth within the active chunk without materially changing the work partition;
- task chunking changes how work is divided/carried forward without becoming a proxy for response length;
- `adaptive` and `always` remain distinguishable across the preserved conversation, not merely by heading count;
- completed work is not re-chunked or restarted just because verbosity changes;
- simple requests remain proportionate;
- no schema change is required.

## Failure patterns

- detailed mode creates extra chunks solely to hold more content;
- compact mode merges chunks until the chunking distinction disappears;
- `always` becomes merely a formatting style while verbosity drives the actual decomposition;
- response length is mistaken for chunk count.

## Interpretation rule

If this composition fails, identify whether verbosity is leaking into decomposition or chunking is leaking into depth before changing either projection. Preserve the semantic boundary rather than tuning both fields simultaneously.