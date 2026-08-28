# HAIL for Codex

This directory contains the harness-native Codex integration for explicit persistent HAIL profile management.

## Persistence boundary

Persistent HAIL preferences change only through explicit HAIL configuration.

Ordinary conversation such as "stop waiting on me" or "be more concise" may affect the current interaction, but must not silently rewrite the persistent HAIL profile.

## Discoverable skills

HAIL exposes persistent profile-management actions as independently discoverable Codex skills:

```text
$hail         → orient, inspect, or route conversational HAIL intent
$hail-show    → show the current persistent HAIL profile (read-only)
$hail-setup   → initialize persistent HAIL configuration
$hail-change  → change one or more persistent HAIL defaults
$hail-reset   → reset the whole profile or a selected preference
```

Codex skills are user-level rather than plugin-namespaced, so HAIL-specific action skills use the `hail-` prefix to avoid collisions with unrelated generic skills such as `show` or `change`.

The root `$hail` skill remains available for compatibility and conversational routing. The specific skills make supported actions discoverable without requiring the user to already know hidden `$hail <subcommand>` vocabulary.

`review` is not advertised here because the separate review capability is not yet implemented as a supported skill.

## Local test setup

Install all HAIL skill directories into the user Codex skills directory so Codex can discover the complete surface:

```text
~/.codex/skills/hail/SKILL.md
~/.codex/skills/hail-show/SKILL.md
~/.codex/skills/hail-setup/SKILL.md
~/.codex/skills/hail-change/SKILL.md
~/.codex/skills/hail-reset/SKILL.md
```

For this repository experiment, copy each directory under `integrations/codex/skills/` into `~/.codex/skills/` using your normal local file workflow.

Then start a fresh Codex session and verify the skills are discoverable before testing behavior.

The legacy conversational shape remains compatible through the root skill, for example:

```text
$hail
$hail show
$hail change
$hail reset
```

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

For example, if `$hail-change` modifies the canonical profile in Codex, Codex's `AGENTS.md` projection is regenerated immediately, but Claude's previously generated projection is not refreshed until the Claude HAIL integration runs again.

Do not add shared runtime or synchronization infrastructure solely to solve this. Record portability behavior first.
