# HAIL Agent Guide

## Project purpose

HAIL is a human-first, vendor-neutral interaction layer. A human-owned semantic profile describes how a person wants AI to work with them. Harness adapters translate that profile into vendor-specific instructions.

The current v0 target is Claude Code. Do not let Claude-specific implementation details leak into the semantic profile.

## Current active milestone

Prove that the same semantic preferences produce observable behavior changes in a real AI harness.

The static Claude Code path already consists of:

`profile.yaml → ProfileLoader → HailProfile → ClaudeCodeAdapter → generated instructions → Claude Code installation`

## Scope discipline

Keep changes small and evidence-driven.

Do not add MCP, runtime state detection, marketplaces, plugin systems, UI, cloud sync, preset libraries, or broad abstractions unless the current milestone requires them.

Do not create architectural folders or abstractions merely because they may be useful later. Add structure when a concrete file or second implementation requires it.

Prefer one clear recommendation over presenting many architecture choices when the decision is reversible.

## Design rules

- The profile is semantic and vendor-neutral.
- Adapters own harness-specific wording and delivery behavior.
- Human preference data should remain user-owned and should not need to be committed into project repositories.
- Installation must preserve existing harness configuration.
- Repeated installation should be idempotent.
- Preferences should map to observable behavior, not vague personality labels.
- Behavior fixtures belong in `evals/` and should describe outcomes rather than implementation details.

## Validation

For implementation changes, run or rely on the `HAIL v0` GitHub Actions workflow. At minimum, changes should continue to build on .NET 10 and preserve the existing profile/compiler/install smoke tests.

When adding a preference, add an observable behavioral expectation for it.

## Important files

- `spec/draft.md` — broader product and architecture direction
- `profiles/example.yaml` — current minimal semantic profile
- `src/Hail/InteractionProfile.cs` — semantic profile model
- `src/Hail/ProfileLoader.cs` — YAML loading
- `src/Hail/ClaudeCodeAdapter.cs` — Claude-specific translation
- `src/Hail/ClaudeCodeInstaller.cs` — safe Claude Code delivery/bootstrap
- `evals/` — behavioral expectations

## Working style

Keep the user out of decision paralysis. Maintain one active next step, park unrelated ideas, and distinguish assumptions that are cheap to reverse from decisions that genuinely need discussion.
