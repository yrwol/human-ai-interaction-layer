# HAIL Agent Guide

## Project purpose

HAIL is a human-first, vendor-neutral interaction layer. A human-owned semantic profile describes how a person wants AI to work with them. Harness integrations translate and apply that profile using the mechanisms native to each harness.

The product is the semantic model + harness integrations. The .NET implementation under `reference/dotnet/` is reference/conformance tooling, not the user experience.

## Current active milestone

Milestone 3 proved explicit persistent HAIL profile management natively inside Claude. Milestone 4 ported that management surface to Codex; explicit Codex manual testing remains useful evidence but is not blocking the next experiment.

The active experiment is now **temporary/contextual overrides**:

`persistent HAIL defaults → explicit temporary override → current interaction behavior → expiration → persistent defaults restored`

The first target scenario is a user whose persistent profile says to wait between steps, then explicitly says something equivalent to "for this task, stop waiting on me." HAIL should adapt the current task without rewriting the persistent profile.

No `dotnet run` dependency is allowed in this product path.

## Scope discipline

Keep changes small and evidence-driven.

Do not add MCP, a shared runtime, desktop UI, cloud sync, diagnosis presets, automatic emotional-state detection, or broad abstractions unless a concrete experiment demonstrates a need.

Prefer harness-native capabilities when they can preserve HAIL semantics and user ownership without extra plumbing.

Do not turn this milestone into a general adaptive-state engine. Begin with explicit temporary overrides only.

Cross-harness projection synchronization remains intentionally unsolved. Record whether stale projections create a real user problem before designing synchronization infrastructure.

## Design rules

- The persistent profile is semantic, vendor-neutral, human-owned, and unchanged by temporary overrides.
- Harness integrations own harness-specific wording, persistence mechanics, and delivery behavior.
- The canonical profile is the source of truth for persistent defaults; generated harness instructions are disposable projections.
- **Persistent profile mutation requires explicit persistent HAIL intent.** Ordinary conversational instructions must not silently rewrite persistent defaults.
- Temporary overrides must have an observable scope/expiration boundary.
- Contextual statements such as `stop waiting on me`, `just decide`, or `I'm overwhelmed today` must not be promoted to permanent profile changes without explicit persistent intent.
- Normal users should not need to see or edit YAML unless they explicitly want to.
- Human preference data remains user-owned and should not need to be committed into project repositories.
- Existing harness configuration must be preserved.
- Preferences map to observable behavior, not diagnosis labels or vague personality categories.
- Weak behavior enforcement in one harness is an adapter/compatibility concern, not a reason to mutate human intent.
- Add a new persistent semantic field only when a concrete persistent user need cannot be expressed by the existing vocabulary.

## Validation

The reference .NET implementation should continue to build and preserve its existing smoke tests under `reference/dotnet/`.

Native integration changes must be validated independently of the reference compiler.

Behavioral experiments and evidence belong in `evals/`.

For temporary overrides, validate both the changed current behavior and that the persistent profile remains unchanged after the override expires.

## Important files

- `spec/draft.md` — broader product and architecture direction
- `spec/milestone-1-addendum.md` — first-harness learnings
- `spec/milestone-2-addendum.md` — portability learnings
- `spec/milestone-3-working-notes.md` — Claude native-management decisions
- `spec/milestone-4-working-notes.md` — Codex profile-management port
- `spec/milestone-5-working-notes.md` — active contextual-override experiment
- `profiles/example.yaml` — reference semantic profile representation
- `integrations/claude/` — native Claude persistent profile management
- `integrations/codex/` — native Codex persistent profile management
- `reference/dotnet/` — reference compiler and previous bootstrap implementations
- `evals/` — behavioral expectations and evidence

## Working style

Keep the user out of decision paralysis. Maintain one active next step, park unrelated ideas, and distinguish assumptions that are cheap to reverse from decisions that genuinely need discussion.
