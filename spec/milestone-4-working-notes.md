# Milestone 4 — Native Codex profile management

**Status: implemented; explicit manual behavioral validation deferred.**

This document records the Codex-native profile-management port. Current project priority is tracked in [`roadmap.md`](roadmap.md).

## Hypothesis

Can the explicit persistent HAIL profile-management experience be ported from Claude to Codex without changing the vendor-neutral semantic profile or introducing a shared runtime?

## Persistent configuration boundary

Codex uses explicit skill invocation as the boundary:

```text
$hail
$hail show
$hail setup
$hail change
$hail reset
```

Ordinary conversation is contextual behavior and must not silently mutate the persistent HAIL profile.

## Shared semantics

The port uses the same canonical profile as Claude:

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

Do not add Codex-specific fields to the semantic profile.

The authoritative semantic definitions live in [`semantics.md`](semantics.md).

## Native Codex delivery

The Codex skill lives at:

```text
integrations/codex/skills/hail/SKILL.md
```

The canonical profile remains:

```text
~/.hail/profile.yaml
```

Codex applies it through the HAIL-managed block in:

```text
$CODEX_HOME/AGENTS.md
```

normally `~/.codex/AGENTS.md`.

## Implementation state

The native skill, profile-management flow, Codex projection logic, repository documentation, and test scaffolding are implemented.

Manual behavioral validation was intentionally deferred rather than treated as complete evidence.

Still unvalidated manually:

1. explicit `$hail` configuration in a real Codex session;
2. profile change/reset behavior end-to-end;
3. resulting behavior in a fresh interaction;
4. preservation of unrelated real-world `AGENTS.md` content; and
5. the ordinary-conversation persistence boundary in Codex.

## Portability question: projection staleness

The semantic profile is shared across harnesses, but each harness currently regenerates only its own projection.

Example:

1. User changes `step_pacing` through `$hail` in Codex.
2. `~/.hail/profile.yaml` changes.
3. Codex `AGENTS.md` is regenerated.
4. Claude's existing `~/.hail/claude-code.md` projection may still express the old value.

This is a real architectural question, but it does not justify jumping directly to a daemon, shared runtime, cloud service, or MCP synchronization layer.

First determine whether the stale-projection window creates a meaningful user problem and whether a simpler harness-native refresh mechanism is sufficient.

## Original exit conditions

The experiment would fully pass when Codex can demonstrate that it can:

1. explicitly enter HAIL configuration;
2. inspect the same canonical profile used by Claude;
3. change and reset persistent preferences conversationally;
4. safely update only HAIL-owned Codex instructions;
5. preserve unrelated `AGENTS.md` content;
6. demonstrate changed behavior in a fresh interaction; and
7. preserve the rule that ordinary conversation does not silently mutate persistent preferences.

Until that explicit test is run, treat this as **implemented with validation deferred**, not as either pass or failure.
