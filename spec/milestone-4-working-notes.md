# Milestone 4 — Native Codex profile management

**Status: complete / manually validated.**

This document records the Codex-native profile-management port and its manual behavioral validation. Current project priority is tracked in [`roadmap.md`](roadmap.md).

## Hypothesis

Can the explicit persistent HAIL profile-management experience be ported from Claude to Codex without changing the vendor-neutral semantic profile or introducing a shared runtime?

**Result: PASS.**

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

## Manual validation results

The native Codex flow was manually exercised end-to-end.

Validated:

1. `$hail` inspected and summarized the existing canonical profile in plain language without mutating it.
2. `$hail change` accepted a natural-language request for one-small-step-at-a-time pacing.
3. The canonical profile changed to:

   ```yaml
   task_chunking: always
   step_pacing: wait_for_user
   ```

4. A fresh Codex interaction produced one small meaningful planning step and waited rather than advancing through the entire plan.
5. `tangent_policy: capture_and_return` composed with `step_pacing: wait_for_user`: a tangent was answered, the pending planning point was preserved, and Codex continued waiting.
6. An ordinary conversational instruction to stop waiting adapted the current task without mutating the persistent profile.
7. `$hail show` confirmed that the persistent profile still retained `step_pacing: wait_for_user` after the contextual instruction.
8. `$hail reset` restored the v0.1 defaults.

Raw evidence lives in [`../evals/historical/results/milestone-4-codex-native-raw.md`](../evals/historical/results/milestone-4-codex-native-raw.md).

## Optional enhancement: multi-harness projection refresh

The semantic profile is shared across harnesses, but each harness currently regenerates only its own projection.

For a user who actively uses multiple harnesses, that can create a temporary stale projection after the canonical profile is changed elsewhere. For example:

1. User changes `step_pacing` through `$hail` in Codex.
2. `~/.hail/profile.yaml` changes.
3. Codex `AGENTS.md` is regenerated.
4. Claude's existing `~/.hail/claude-code.md` projection may still express the old value until refreshed.

This does **not** affect the validity of the Codex experiment and is **not a requirement for shipping HAIL**. Many users may use only one harness, and semantic portability does not require all harness projections to be continuously synchronized.

If multi-harness usage later demonstrates meaningful friction, the preferred first direction is a simple refresh-at-use strategy from the canonical profile. Do not jump directly to a daemon, shared runtime, cloud service, or MCP synchronization layer.

## Exit conditions

The original exit conditions are now satisfied by manual testing:

1. explicitly enter HAIL configuration — **validated**;
2. inspect the same canonical profile used by Claude — **validated**;
3. change and reset persistent preferences conversationally — **validated**;
4. safely update HAIL-owned Codex instructions — **validated by the native flow**;
5. preserve unrelated `AGENTS.md` content — **part of the managed-block delivery contract**;
6. demonstrate changed behavior in a fresh interaction — **validated**; and
7. preserve the rule that ordinary conversation does not silently mutate persistent preferences — **validated**.

Milestone 4 is complete. Multi-harness projection refresh is parked as a possible future convenience enhancement, not an exit condition or release blocker.
