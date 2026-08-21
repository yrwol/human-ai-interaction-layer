# HAIL for Codex

This directory contains the harness-native Codex experiment for explicit persistent HAIL profile management.

## Persistence boundary

Persistent HAIL preferences change only through explicit HAIL configuration.

In Codex, the intended entry point is the `hail` skill, invoked explicitly as `$hail`.

Ordinary conversation such as "stop waiting on me" or "be more concise" may affect the current interaction, but must not silently rewrite the persistent HAIL profile.

## Local test setup

Install the skill into your user Codex skills directory so Codex can discover it:

```text
~/.codex/skills/hail/SKILL.md
```

For this repository experiment, copy `skills/hail/` to that location using your normal local file workflow.

Then start a fresh Codex session and invoke:

```text
$hail
```

Useful test interactions:

```text
$hail show
$hail change
$hail reset
```

or invoke `$hail` and continue conversationally.

## Expected storage

The canonical semantic profile remains shared across harnesses:

```text
~/.hail/profile.yaml
```

Codex's generated projection is managed inside the user-level Codex instructions:

```text
$CODEX_HOME/AGENTS.md
```

normally:

```text
~/.codex/AGENTS.md
```

HAIL owns only the block between:

```text
<!-- HAIL:START -->
<!-- HAIL:END -->
```

Everything outside that block must be preserved.

## Important current limitation

The semantic profile is shared, but generated harness projections are not synchronized automatically.

For example, if `$hail change` modifies the canonical profile in Codex, Codex's `AGENTS.md` projection is regenerated immediately, but Claude's previously generated projection is not refreshed until the Claude HAIL integration runs again.

Do not add shared runtime or synchronization infrastructure to solve this during the current experiment. Record it as portability evidence first.
