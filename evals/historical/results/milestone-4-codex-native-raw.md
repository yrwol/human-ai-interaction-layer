# Milestone 4 — Codex native HAIL raw results

Use this file to paste raw Codex interactions from the native `$hail` profile-management experiment.

## Setup

- Install `integrations/codex/skills/hail/SKILL.md` at `~/.codex/skills/hail/SKILL.md`.
- Start a fresh Codex session.
- Do not manually edit `~/.hail/profile.yaml` or `~/.codex/AGENTS.md` during the test.

## Test 1 — inspect existing shared profile

Prompt:

```text
$hail
```

Paste raw result below:

```text
Your persistent HAIL profile is configured to:

- Give balanced-detail responses.
- Recommend the best option first.
- Present at most 3 options at once.
- Break down complex or cognitively heavy work, but not simple tasks.
- Continue naturally through steps without waiting.
- Acknowledge tangents while keeping the original goal in view.

Nothing was changed. To adjust it, tell me naturally—for example, “HAIL, keep answers short.”
```

Expected:
- HAIL summarizes the existing canonical profile in plain language.
- It does not require YAML knowledge.
- It does not mutate the profile merely by showing it.

## Test 2 — explicit change

Prompt:

```text
$hail change
```

Then request:

```text
Give me one small step at a time and wait for me before moving on.
```

Paste raw result below:

change request:

```text
What persistent default would you like to change? Describe it naturally—for example, “keep answers shorter,” “make reversible decisions for me,” or “give me one step and wait.”
```

change response:

```text
Your persistent HAIL preference is updated.

For multi-step work, I’ll now give you one small, actionable step and wait for you to explicitly say you’re ready before continuing. All other preferences remain unchanged.
```


Expected semantic result:

```yaml
task_chunking: always
step_pacing: wait_for_user
```

Expected delivery result:
- canonical `~/.hail/profile.yaml` is updated;
- exactly one HAIL managed block exists in `$CODEX_HOME/AGENTS.md`;
- unrelated `AGENTS.md` content is preserved.

Actual Results:

```yaml
version: 0.1
profile:
  verbosity: balanced
  decision_mode: recommend_first
  max_options: 3
  task_chunking: always
  step_pacing: wait_for_user
  tangent_policy: capture_and_return
```

## Test 3 — behavior after fresh interaction

Start a fresh Codex interaction/session and prompt:

```text
I want to plan a fancy AI-supported todo app but I don't know where to start.
```

Paste raw result below:

```text
Start with the problem, not the AI features.

**Step 1:** Describe one real situation where existing todo apps fail you.

Complete this sentence:

> “When I’m trying to ___, I struggle because ___, and I wish the app would ___.”

Use a specific recent example. Send me that sentence, and we’ll turn it into the app’s core concept before considering features or technology.
```

Expected:
- one small meaningful planning step or question;
- Codex waits rather than advancing through the whole plan.

## Test 4 — tangent composition

During the paced planning interaction, prompt:

```text
but why is the sky blue?
```

Paste raw result below:

```text
Sunlight contains many colors. Air molecules scatter shorter blue wavelengths much more strongly than longer red wavelengths, so blue light reaches your eyes from across the sky. Violet scatters too, but our eyes are less sensitive to it, and some is absorbed higher in the atmosphere.

We can return to the todo-app sentence whenever you’re ready.

```

Expected:
- tangent is answered or acknowledged;
- Codex returns to the pending planning step;
- it still waits rather than advancing.

## Test 5 — ordinary conversation must not persist

Without invoking `$hail`, say:

```text
stop waiting on me, just keep going for this
```

Result:

```text
I’ll keep advancing the todo-app plan autonomously and treat this as a session instruction, not a persistent preference.

Assumption: the app is for busy knowledge workers whose task lists become cluttered and hard to prioritize.

Core concept:

> An AI-supported todo app that turns messy inputs into a realistic daily plan, explains its choices, and keeps the user in control.

A strong first version would support one workflow:

1. The user quickly dumps tasks in plain language.
2. AI extracts titles, deadlines, effort, and dependencies.
3. The app proposes a small, achievable daily plan.
4. The user accepts, edits, or rejects each suggestion.
5. When circumstances change, the app replans unfinished work.

Key product rule: AI recommends; the user decides. Avoid autonomous deletion, vague priority scores, and a chat interface that hides the actual task list.

For the MVP, include:

- Fast task capture
- Structured task extraction
- Today view with 3–5 recommended tasks
- Plain-language reasons for recommendations
- Simple reordering and approval
- End-of-day rollover
- Basic privacy controls

Defer calendar integration, team collaboration, voice capture, elaborate agents, and automatic emailing until the core planning loop proves useful.

The first prototype should test one question: **Does an AI-generated daily plan help users decide what to do faster than a conventional prioritized list?**
```

Let Codex adapt for the current task. Then inspect the persistent profile with:

```text
$hail show
```

Paste raw result below:

```text
Your persistent HAIL profile is:

- Balanced detail
- Recommendations first, with concise reasoning
- No more than 3 options at once
- Always break multi-step work into small, concrete steps
- Give one step at a time and wait for you to continue
- Acknowledge tangents, then return to the original goal

Your earlier “just keep going” instruction affected that conversation only; it did not change this profile.
```

Expected:
- current-task behavior may adapt;
- persistent `step_pacing` remains unchanged.

Actual yaml:

```yaml
version: 0.1
profile:
  verbosity: balanced
  decision_mode: recommend_first
  max_options: 3
  task_chunking: always
  step_pacing: wait_for_user
  tangent_policy: capture_and_return
```

## Test 6 — reset

Prompt:

```text
$hail reset
```

Paste raw result below:

```text
Do you want to reset your whole HAIL profile to its defaults, or reset only one preference?
```

Expected:
- Codex clarifies whole-profile vs one-preference reset if ambiguous;
- whole-profile reset restores v0.1 defaults;
- generated Codex projection is updated safely.

Additionally, I did responde:

me:

```text
defaults
```

response:

```text
Your HAIL profile is reset to defaults:

- Balanced detail
- Recommendations first
- Up to 3 options
- Adaptive task breakdown
- Continuous progress unless you ask to pause
- Tangents acknowledged, then return to the original goal
```

```yaml
version: 0.1
profile:
  verbosity: balanced
  decision_mode: recommend_first
  max_options: 3
  task_chunking: adaptive
  step_pacing: continuous
  tangent_policy: capture_and_return
```

## Cross-harness observation

After changing the canonical profile in Codex, note whether Claude's previously generated projection is now stale and whether that creates a meaningful UX problem in practice.

```text
Observation:

```
