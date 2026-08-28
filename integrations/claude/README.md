# HAIL for Claude

This directory contains the harness-native Claude integration for explicit persistent HAIL profile management.

## Persistence boundary

Persistent HAIL preferences change only through explicit HAIL configuration.

Ordinary conversation such as "stop waiting on me" or "be more concise" may affect the current interaction, but must not silently rewrite the persistent HAIL profile.

## Discoverable skills

When loaded as a Claude plugin, HAIL exposes these skills:

```text
/hail:hail   → orient, inspect, or route conversational HAIL intent
/hail:show   → show the current persistent HAIL profile (read-only)
/hail:setup  → initialize persistent HAIL configuration
/hail:change → change one or more persistent HAIL defaults
/hail:reset  → reset the whole profile or a selected preference
```

The root `/hail:hail` skill remains available for compatibility and conversational routing. The specific skills make supported actions discoverable without requiring the user to already know hidden `/hail:hail <subcommand>` vocabulary.

`review` is not advertised here because the separate review capability is not yet implemented as a supported skill.

## Local developer test

Load the plugin directly from this repository:

```bash
claude --plugin-dir ./integrations/claude
```

Then verify the HAIL skill list exposes the five skills above.

Recommended behavior checks:

```text
/hail:show
/hail:setup
/hail:change
/hail:reset
```

The legacy conversational shape remains compatible through the root skill, for example invoking `/hail:hail` and asking to show, change, set up, or reset the persistent HAIL configuration in natural language.

## Expected storage

The canonical semantic profile is stored at:

```text
~/.hail/profile.yaml
```

Claude's generated HAIL projection is stored at:

```text
~/.hail/claude-code.md
```

The user-level Claude instructions file should contain exactly one standalone import:

```text
@~/.hail/claude-code.md
```

HAIL owns the generated projection file, but all unrelated content in `~/.claude/CLAUDE.md` must be preserved.

## Validation status

The original native Claude profile-management flow was manually validated before the discoverable-skill split.

The new first-class skill surface is implemented on the current feature branch but still requires explicit discovery and behavioral-parity validation before the discoverable-skills capability should be marked complete.
