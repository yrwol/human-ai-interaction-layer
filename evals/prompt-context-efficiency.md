# Prompt Context Efficiency Evaluation

## Purpose

This evaluation tests how the amount and timing of context provided to an AI affects the total interaction cost required to reach an acceptable result.

The primary question is:

> **How many total tokens are required to reach an acceptable result under different context-delivery strategies?**

This is not a test of which prompt is shortest. It is a test of **tokens-to-satisfactory-result**.

## Initial hypothesis

> Providing sufficient context earlier will reduce correction and rework tokens, while excessive upfront context may eventually produce diminishing returns.

A likely useful concept to investigate is **minimum sufficient context**: enough information to prevent avoidable misunderstanding without front-loading irrelevant detail.

---

## Test conditions

Run the same underlying task under three prompting styles.

### Condition A — Context up front

Provide nearly all relevant information in the first message.

Example:

> I want to design a cozy horse-management game. Players own horses with health, stamina, speed, temperament, and training stats. They can feed, groom, train, and compete in racing, jumping, and dressage. Different breeds should have meaningful strengths and weaknesses, and horses should improve based on care and training. It should be approachable for casual players, and I don't want to add multiplayer or a complicated economy yet.
>
> Help me design the core gameplay loop and progression system.

Allow normal follow-ups only when necessary.

### Condition B — Progressive context

Provide the goal plus some important context, then add the remaining known constraints across approximately 2–4 turns.

Initial prompt:

> I want to make a cozy horse-management game. Help me design the core gameplay loop.

Predetermined additions:

> I want players to feed, groom, and train their horses.

> Horses should have stats like health, stamina, speed, temperament, and training.

> I want racing, jumping, and dressage, with breeds having different strengths.

> Keep it casual. Don't build a giant multiplayer/economy system.

### Condition C — Minimal / conversational context

Start with very little information and reveal constraints only when the AI asks, makes an assumption that conflicts with the hidden brief, or reaches a point where the missing information becomes relevant.

Initial prompt:

> I want to make a horse game. Help me figure out the gameplay.

Example additions:

> I want you to actually manage the horses.

> Like feeding, grooming and training.

> They should have different stats.

> There should be competitions too.

> Oh, and I want breed differences to matter.

> This is supposed to be cozy though, not some giant competitive MMO.

---

## Source brief

Before running any condition, create a hidden source brief containing the complete information that may eventually be provided to the model.

All conditions for a task must use the same source brief. The experiment should manipulate **when and how context is delivered**, not which facts are available.

### Example source brief — Horse game

```text
Goal:
Design a small horse-management game app.

Known facts:
- Player owns and manages horses.
- Horses have stats such as health, stamina, speed, temperament, and training.
- Players can feed, groom, train, and compete with horses.
- Horses improve over time based on care and training.
- Different breeds should have meaningful strengths and weaknesses.
- Competitions include racing, jumping, and dressage.
- The game should feel cozy rather than highly competitive.
- It should be understandable for casual players.
- Avoid designing a giant economy/social/multiplayer system yet.
```

---

## Initial task set

Use several task types so results are not specific to one reasoning style.

Suggested first pass:

| Task | Example |
|---|---|
| Product/game design | Design the horse game's core loop |
| Explanation | Explain horse coat-color genetics to a beginner |
| Decision | Choose how training should work: points, activities, or skill trees |
| Writing | Rewrite the game's store description for casual players |
| Planning | Plan a small playable horse-game prototype |
| Ambiguous design | Design a horse personality/temperament system |

With three conditions, six tasks produce:

```text
6 tasks × 3 conditions = 18 sessions
```

This is enough for an initial directional test without turning the evaluation into a large study.

---

## Acceptance criteria

Define the completion criteria for each task before running any condition.

Stop the run as soon as the answer satisfies those criteria.

### Example — horse game core loop

A result is acceptable when it:

- defines a clear core gameplay loop;
- explains horse care activities;
- incorporates horse stats;
- gives training/progression meaning;
- handles breed differences;
- incorporates racing, jumping, and dressage;
- maintains the cozy/casual goal; and
- avoids unnecessary multiplayer/economy scope.

Do not continue refining a run after the acceptance criteria are met merely to make the answer nicer.

---

## Metrics

Capture at least:

- input tokens;
- output tokens;
- total tokens;
- number of user turns;
- number of assistant turns;
- number of clarification questions;
- number of corrective user turns;
- final quality score;
- interaction-effort score; and
- whether the result met acceptance criteria.

### Correction vs additional context

Distinguish between expected context delivery and rework.

Additional context:

> There should be competitions too.

Correction:

> No, that's not what I meant. The game is supposed to be cozy, not competitive.

Corrections are especially useful because they represent token cost caused by misunderstanding rather than ordinary progressive disclosure.

---

## Quality rubric

Score the final result from 1–5.

### 1 — Failed
Does not accomplish the task.

### 2 — Major revision required
Direction exists, but important requirements are wrong or missing.

### 3 — Acceptable
Meets the task requirements, though meaningful refinement could improve it.

### 4 — Strong
Correct and useful with only minor refinement needed.

### 5 — Target
Exactly or nearly exactly what was needed.

For simple analysis, define:

```text
acceptable = quality >= 3
```

The primary metric can then be expressed as:

> **Total tokens to acceptable result**

---

## Interaction-effort rubric

Token cost is not the only interaction cost.

Score human effort from 1–5:

### 1 — Almost no effort
Little or no clarification or correction required.

### 2 — Easy refinement
A small amount of natural interaction was useful.

### 3 — Moderate back-and-forth
Several turns were required, but the interaction remained comfortable.

### 4 — Annoying correction burden
The user had to repeatedly redirect or correct the model.

### 5 — Exhausting / repetitive
The interaction became meaningfully burdensome.

This allows the evaluation to distinguish token efficiency from interaction quality.

---

## Run record

Store each session using a consistent shape.

Example:

```yaml
task: horse_core_loop_01
condition: progressive

input_tokens: 3100
output_tokens: 2200
total_tokens: 5300

user_turns: 4
assistant_turns: 4

clarifications: 1
corrections: 0

quality: 5
interaction_effort: 2
acceptable: true
```

---

## Aggregate comparison

Summarize results by condition.

| Metric | Upfront | Progressive | Minimal |
|---|---:|---:|---:|
| Average total tokens | | | |
| Median total tokens | | | |
| Average user turns | | | |
| Average corrections | | | |
| Average quality | | | |
| Average interaction effort | | | |
| % acceptable | | | |

For a small initial sample, median token usage may be more informative than the mean because a single failed conversation can heavily skew averages.

---

## Controls

Use a fresh conversation for every run.

Keep constant where possible:

- model;
- model configuration;
- tool availability;
- source brief;
- acceptance criteria; and
- scoring rubric.

Rotate condition order across tasks to reduce learning/order effects.

Example:

```text
Task 1: B → C → A
Task 2: A → B → C
Task 3: C → A → B
```

Avoid always running A, then B, then C for the same task.

---

## Interpretation

Do not treat the experiment as an attempt to prove that long prompts are better.

Two different sources of inefficiency may exist.

### Insufficient context

```text
insufficient context
        ↓
wrong assumptions / clarification / correction
        ↓
additional token cost
```

### Excess context

```text
excess context
        ↓
irrelevant information / larger processing burden
        ↓
additional token cost
```

The most useful region may be:

```text
minimum sufficient context
        ↓
few misunderstandings
+ little irrelevant information
        ↓
efficient interaction
```

A useful initial conclusion may therefore be more nuanced than "short prompts" versus "long prompts."

---

## HAIL relevance

If the experiment shows a meaningful relationship between context delivery and interaction efficiency, a later HAIL question could be:

> **Can HAIL help users and AI reach minimum sufficient context without forcing users to front-load everything?**

This evaluation does not assume that such behavior should become a HAIL semantic preference or capability. It exists to gather evidence first.
