# Milestone 3 — Claude-native persistent profile management

**Status:** IN PROGRESS — core persistent-management path validated

This milestone tests whether a user can intentionally manage persistent HAIL defaults inside Claude without editing YAML or invoking the .NET reference compiler.

## Native setup behavior observed

The user invoked the HAIL skill explicitly and configured preferences conversationally. Claude created and updated `~/.hail/profile.yaml`, regenerated the HAIL-owned Claude projection, and preserved the existing user-level import.

An older profile without `step_pacing` was read successfully and treated as the backward-compatible default (`continuous`) until the user explicitly changed it.

## New semantic finding: step pacing

The user asked for work to be broken into small pieces **and** for Claude to wait before moving on. `task_chunking` could express the first need but not the second.

The experiment introduced:

```yaml
step_pacing: continuous | check_in | wait_for_user
```

After the user explicitly changed pacing to `wait_for_user`, a fresh conversation (`/clear`) showed the intended behavior:

1. The user asked to plan an AI-supported todo app without knowing where to start.
2. Claude recommended one first planning decision and stopped.
3. After the user answered, Claude moved to one next design decision and stopped again.
4. When asked for an example, Claude provided one bounded example and continued waiting for the user's reaction.

This was materially different from the pre-pacing behavior, which generated an extensive multi-phase plan and delegated background planning work.

## Composition finding: pacing + tangents

During the paced planning flow, the user asked `but why is the sky blue?`.

Claude:

- answered the tangent briefly;
- explicitly returned to the todo-app task;
- remembered the exact pending design decision; and
- continued waiting for the user instead of advancing autonomously.

**Result:** strong directional evidence that `step_pacing: wait_for_user` composes with `tangent_policy: capture_and_return`.

## Migration bug found and fixed

The first native run encountered an older reference-generated setup where `~/.claude/CLAUDE.md` imported `~/.hail/claude-code.md`.

The initial native skill attempted a second managed-block strategy, which could preserve contradictory generated instructions. The implementation was changed so:

- `~/.hail/profile.yaml` is the canonical semantic profile;
- `~/.hail/claude-code.md` is fully HAIL-owned generated output and is replaced from the current profile; and
- `~/.claude/CLAUDE.md` preserves unrelated user content and exactly one HAIL import.

## Persistence-boundary finding

During ordinary task conversation, the user said `make a decision and stop waiting on me`.

Claude reasonably treated this as a current-task instruction and proceeded more autonomously. This produced an important product decision:

> Persistent HAIL profile changes must require explicit HAIL intent. Ordinary conversational instructions must not silently mutate persistent defaults.

For the current milestone, persistent management therefore begins through explicit HAIL configuration (for example `/hail:hail` or an explicit request to change the HAIL profile).

Temporary/task/session/day-specific adaptation is a separate future design problem and remains parked.

## Current scoring

| Capability | Result |
|---|---|
| explicit conversational profile inspection | pass |
| explicit conversational preference update | pass |
| backward-compatible profile migration | pass |
| native projection regeneration | pass |
| no reference-compiler dependency in user flow | pass |
| persistence across `/clear` | pass |
| `step_pacing: wait_for_user` | strong pass |
| pacing + tangent composition | strong pass |
| automatic persistent inference from ordinary conversation | intentionally not supported |
| temporary/session overrides | unimplemented / parked |

## Remaining Milestone 3 validation

Before closing the milestone, verify at least:

1. explicit `show` of the current profile in plain language;
2. another persistent preference change after initial setup;
3. reset behavior (whole profile versus one preference) without accidental data loss; and
4. a fresh conversation reflecting a changed persistent preference.
