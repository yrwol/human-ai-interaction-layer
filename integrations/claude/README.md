# HAIL for Claude

This directory contains the harness-native Claude integration for explicit persistent HAIL profile management.

## Persistence boundary

Persistent HAIL preferences change only through explicit HAIL configuration.

Ordinary conversation such as "stop waiting on me" or "be more concise" may affect the current interaction, but must not silently rewrite the persistent HAIL profile.

## Discoverable skills

When loaded as a Claude plugin, HAIL exposes:

```text
/hail:hail   → orient, inspect, or route conversational HAIL intent
/hail:show   → show the current persistent HAIL profile (read-only)
/hail:setup  → initialize persistent HAIL configuration
/hail:change → change one or more persistent HAIL defaults
/hail:reset  → reset the whole profile or a selected preference
```

The root `/hail:hail` skill remains available for compatibility and conversational routing. `review` is not advertised because that capability is not yet implemented.

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

These files ship with the plugin so independently invoked action skills do not depend on repository-only specs or duplicate schema/default/migration rules.

- `profile-schema.md` defines the current runtime profile shape and allowed values.
- `default-profile.yaml` is the authoritative runtime source for setup/reset defaults.
- `profile-normalization.md` defines backward compatibility, normalization precedence, and mutation boundaries.

Repository-level semantic authority remains `spec/semantics.md`. When semantics, defaults, or compatibility rules change, the bundled runtime resources must be updated to match before release.

## Local developer test

```bash
claude --plugin-dir ./integrations/claude
```

Then verify `/hail:hail`, `/hail:show`, `/hail:setup`, `/hail:change`, and `/hail:reset` are discoverable and that the root HAIL bundle includes all three reference files.

The legacy conversational shape remains compatible through the root skill.

## Expected storage

Canonical semantic profile:

```text
~/.hail/profile.yaml
```

Generated Claude projection:

```text
~/.hail/claude-code.md
```

User-level Claude instructions should contain exactly one standalone import:

```text
@~/.hail/claude-code.md
```

HAIL owns the generated projection file, but unrelated content in `~/.claude/CLAUDE.md` must be preserved.

## Validation status

The original native Claude profile-management flow was manually validated before the discoverable-skill split. The new skill surface and bundled-resource architecture still require discovery, normalization, and behavioral-parity validation before the capability is marked complete.
