# Task Chunking Hardening Result — Claude Sonnet — `adaptive`

## Test conditions

- Date: 2026-08-24
- Harness: Claude Code
- Model: `sonnet`
- HAIL ref: `eval/task-chunking-verbosity-hardening`
- HAIL commit: `f976f9ea84ff9f516054bf25c9c1d4b07103d5b9`
- Profile: `profiles/eval-task-chunking-adaptive.yaml`
- `task_chunking`: `adaptive`
- `step_pacing`: `continuous`
- Interaction: fresh GitHub Actions runner
- Source run: `32778793665`

## Compiled projection

```text
- When a task is complex, ambiguous, or likely to create cognitive overload, break it into a small number of concrete next steps. Do not over-structure simple work.
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
Building a cozy horse sim (think Horse Reality / Howrse crossed with Stardew-style progression) touches a lot of systems, so the trap is trying to design all of them at once. I'd recommend starting narrower: nail down the **core gameplay loop** first — the one cycle of actions a player repeats every session (e.g., care → train → compete → earn → breed/upgrade) — before fleshing out breeds, competitions, or progression as separate systems. Everything else should hang off that loop rather than being designed in parallel, or you'll end up with five half-built systems that don't reinforce each other.

A few things that would help me give you a useful next step:

1. **What stage are you at?** Pure concept/paper design, or do you already have an engine/tools in mind (Unity, Godot, web-based, etc.)?
2. **What's the emotional hook?** Cozy horse games lean on different fantasies — some are about nurturing/bonding (Bella Sara-ish), others about competitive mastery (real equestrian sim), others about collection/breeding genetics (Howrse). Knowing which one you're chasing shapes what "core loop" even means.
3. **Solo or team, and rough timeline?** Affects how aggressively we should scope the MVP.

Once I know those, I'd suggest we do this in order: (1) sketch the core loop on paper, (2) cut it down to the smallest playable slice (probably: one horse, basic care stats, one training activity, one competition type), (3) then layer breeds/breeding/deeper progression on top once the loop feels good. Want to start with #1, or do you already have a loop in mind you want to react to?
```

## Assessment

- Expected: recognize the task as complex and decompose it into a manageable number of meaningful pieces.
- Actual: narrowed the task to the core loop, asked three scoping questions, and proposed three broad next steps.
- Failure pattern: the response is reasonable in isolation, but it is not behaviorally distinguishable enough from ordinary planning behavior and does not establish a reliable middle point between `off` and `always`.
- Differentiation contribution: broader and less decomposed than the `off` response, which is the wrong ordering for this experiment.
- Semantic drift: no major semantic drift in isolation; comparative differentiation failed.
- Strength: **moderate in isolation, weak comparatively**.
- Promotion status: **fail pending stronger differentiation**.
