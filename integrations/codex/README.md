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

Codex skills are user-level rather than plugin-namespaced, so HAIL-specific action skills use the `hail-` prefix to avoid collisions with unrelated generic skills.

The root `$hail` skill remains available for compatibility and conversational routing. `review` is not advertised because that capability is not yet implemented.

## Bundled runtime contracts

The root HAIL skill bundle owns shared runtime profile data:

```text
skills/hail/
  SKILL.md
  references/
    profile-schema.md
    default-profile.yaml
    profile-normalization.md
```

These resources must be installed with the root `$hail` skill because the independently invoked `$hail-*` action skills consume them rather than duplicating schema/default/migration behavior.

- `profile-schema.md` defines the current runtime profile shape and allowed values.
- `default-profile.yaml` is the authoritative runtime source for setup/reset defaults.
- `profile-normalization.md` defines backward compatibility, normalization precedence, and mutation boundaries.

Repository-level semantic authority remains `spec/semantics.md`. When semantics, defaults, or compatibility rules change, the bundled runtime resources must be updated to match before release.

## Local test setup

Install all HAIL skill directories into the user Codex skills directory:

```text
~/.codex/skills/hail/SKILL.md
~/.codex/skills/hail/references/profile-schema.md
~/.codex/skills/hail/references/default-profile.yaml
~/.codex/skills/hail/references/profile-normalization.md
~/.codex/skills/hail-show/SKILL.md
~/.codex/skills/hail-setup/SKILL.md
~/.codex/skills/hail-change/SKILL.md
~/.codex/skills/hail-reset/SKILL.md
```

For this repository experiment, copy each directory under `integrations/codex/skills/` into `~/.codex/skills/`.

Then start a fresh Codex session and verify the skills are discoverable and the root HAIL resource bundle is present before testing behavior.

The legacy conversational shape remains compatible through the root skill, for example `$hail show`, `$hail change`, and `$hail reset`.

## Expected storage

Canonical semantic profile:

```text
~/.hail/profile.yaml
```

Codex generated projection:

```text
$CODEX_HOME/AGENTS.md
```

normally `~/.codex/AGENTS.md`.

HAIL owns only the block between `<!-- HAIL:START -->` and `<!-- HAIL:END -->`; everything outside that block must be preserved.

## Important current limitation

The semantic profile is shared, but generated harness projections are not synchronized automatically. Do not add shared runtime or synchronization infrastructure solely to solve this; record portability behavior first.

## Validation status

The original native Codex profile-management flow was manually validated before the discoverable-skill split. The current skill surface and bundled-resource architecture now pass deterministic package, callability, normalization, state-mutation, and managed-block-integrity validation. Interactive autocomplete/search presentation remains a separate manual harness-UI check.
