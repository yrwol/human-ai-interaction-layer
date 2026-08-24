# Task Chunking Hardening Result — Claude Sonnet — `always`

## Test conditions

- Date: 2026-08-24
- Harness: Claude Code
- Model: `sonnet`
- HAIL ref: `eval/task-chunking-verbosity-hardening`
- HAIL commit: `f976f9ea84ff9f516054bf25c9c1d4b07103d5b9`
- Profile: `profiles/eval-task-chunking-always.yaml`
- `task_chunking`: `always`
- `step_pacing`: `continuous`
- Interaction: fresh GitHub Actions runner
- Source run: `32778799924`

## Compiled projection

```text
- Break multi-step work into small, concrete, executable steps.
```

Other profile values were held constant across the differentiation runs:

```yaml
verbosity: balanced
decision_mode: recommend_first
max_options: 3
step_pacing: continuous
tangent_policy: capture_and_return
```

## Prompt

```text
I want to build a cozy horse game with horse care, training, competitions, breeds, and progression, but I don't know where to start. Help me plan it.
```

## Raw result

```text
Good instinct to start with a plan rather than a system all at once — "horse game" is really five interlocking games (sim, training, competition, breeding, progression), and building them in parallel is how these projects stall.

My recommendation: start by scoping a tiny vertical slice — one horse, one care loop, one training stat, one competition type — and get that fully playable before touching breeds or progression. The reason is that care/training/competition are your core gameplay loop; breeds and progression are multipliers on top of it, and multipliers on a loop that isn't fun yet just create more work to throw away.

Before I sketch that slice out, two things will shape the plan a lot:

1. **Platform/engine** — web (browser, e.g. Phaser/PixiJS or even a UI-driven stack), or a game engine (Godot/Unity) for something more visual/3D?
2. **Solo or team, and rough time budget** — weekend project, or something you're committing months to?

Once I know those, I'll break it into concrete steps (core loop design → data model for stats/breeds → vertical slice build → expand).
```

## Assessment

- Expected: clearly decompose the multi-step planning work into small, concrete, independently actionable pieces and continue through them under `step_pacing: continuous`.
- Actual: recommended a vertical slice, asked two clarifying questions, and deferred the promised decomposition until after the user replies.
- Failure pattern: `always` describes future decomposition instead of performing it; the response is less visibly chunked than `off` and not stronger than `adaptive`.
- Differentiation contribution: ordering is inverted relative to the intended behavior (`off` was most explicitly decomposed in this pass).
- Semantic drift: yes, because the required decomposition did not occur in the current response.
- Strength: **weak**.
- Promotion status: **fail**.
