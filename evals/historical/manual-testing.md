# Manual HAIL behavioral testing

> Historical procedure used for the first static-profile Claude/Codex portability experiments. Current prompt-hardening work lives under [`../prompt-hardening/`](../prompt-hardening/).

This guide defines the manual A/B procedure used to test whether changing only a semantic HAIL profile changes observable AI behavior.

The comparison profiles were:

```yaml
# profiles/test-a.yaml
version: 0.1

profile:
  verbosity: balanced
  decision_mode: recommend_first
  max_options: 2
  task_chunking: adaptive
  tangent_policy: capture_and_return
```

```yaml
# profiles/test-b.yaml
version: 0.1

profile:
  verbosity: detailed
  decision_mode: options
  max_options: 6
  task_chunking: off
  tangent_policy: follow
```

## Test discipline

For every harness:

1. Apply Profile A.
2. Start a fresh session.
3. Run the prompts below in the same order.
4. Save the complete responses.
5. Apply Profile B.
6. Start another fresh session.
7. Run the exact same prompts in the same order.
8. Compare direction of behavior, not subjective answer quality.

The question was:

> Did changing only the HAIL profile move the harness behavior in the direction requested by the profile?

## Scenarios

### Decision behavior

Prompt:

```text
I could use Redis, Postgres, or an in-memory cache. What should I use?
```

Look for whether the harness recommends first or primarily presents alternatives.

### Option limiting

Prompt:

```text
I have Saturday free and want to learn something new. What could I do?
```

Look at how many distinct choices are presented at once. Treat this as a weaker/less deterministic dimension: Milestone 1 showed that Claude followed the qualitative decision style more reliably than the strict numeric option limit.

### Task chunking

Prompt:

```text
I need to clean my room, my living room, kitchen, and 3 bathrooms. Help me get started.
```

Look for whether the response reduces the task to a small, concrete starting action versus presenting the whole plan at once.

### Verbosity

Prompt:

```text
Explain why leaves change color in the fall.
```

Compare response depth, expansion, contextual detail, and follow-up material.

### Tangent handling

This test must be multi-turn.

Prompt 1:

```text
Help me plan what I need to clean today: kitchen, bedroom, living room, and bathrooms.
```

After the harness responds, send Prompt 2:

```text
Oh also I've been thinking about learning Spanish.
```

For `capture_and_return`, look for the tangent being acknowledged or parked while cleaning remains the active goal. For `follow`, look for the harness engaging the Spanish topic directly.

## Claude / Milestone 1

Milestone 1 used the same semantic profiles against Claude. The summarized findings are in [`results/milestone-1-claude.md`](results/milestone-1-claude.md), and the full raw responses are preserved in [`results/milestone-1-claude-raw.md`](results/milestone-1-claude-raw.md).

## Codex / Milestone 2

The Codex record is preserved in [`results/milestone-2-codex-raw.md`](results/milestone-2-codex-raw.md).

When reviewing Codex, the original procedure scored both:

- whether Profile A and B produced the expected directional difference; and
- how strongly Codex enforced each dimension compared with Claude.

The semantic profiles were not changed merely to make one harness perform better. Harness-specific wording belonged in the adapter.