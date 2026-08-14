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

```text

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

## Test 3 — behavior after fresh interaction

Start a fresh Codex interaction/session and prompt:

```text
I want to plan a fancy AI-supported todo app but I don't know where to start.
```

Paste raw result below:

```text

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

Let Codex adapt for the current task. Then inspect the persistent profile with:

```text
$hail show
```

Paste raw result below:

```text

```

Expected:
- current-task behavior may adapt;
- persistent `step_pacing` remains unchanged.

## Test 6 — reset

Prompt:

```text
$hail reset
```

Paste raw result below:

```text

```

Expected:
- Codex clarifies whole-profile vs one-preference reset if ambiguous;
- whole-profile reset restores v0.1 defaults;
- generated Codex projection is updated safely.

## Cross-harness observation

After changing the canonical profile in Codex, note whether Claude's previously generated projection is now stale and whether that creates a meaningful UX problem in practice.

```text
Observation:

```
