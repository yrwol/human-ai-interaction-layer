# HAIL Agent Guide

## Project purpose

HAIL is a human-first, vendor-neutral interaction layer. A human-owned semantic profile describes how a person wants AI to work with them. Harness integrations translate and apply that profile using the mechanisms native to each harness.

The product is the semantic model + harness integrations. The .NET implementation under `reference/dotnet/` is reference/conformance tooling, not the user experience.

## Current active milestone

Prove that a normal user can manage HAIL conversationally inside a harness without editing YAML, running a compiler, or understanding instruction-file plumbing.

The first native experiment is the Claude plugin under `integrations/claude/`.

Current target flow:

`natural-language preference → HAIL semantic profile → native Claude projection`

No `dotnet run` dependency is allowed in this product path.

## Scope discipline

Keep changes small and evidence-driven.

Do not add MCP, a shared runtime, desktop UI, cloud sync, diagnosis presets, more profile fields, or broad abstractions unless this milestone demonstrates a concrete need.

Prefer harness-native capabilities when they can preserve HAIL semantics and user ownership without extra plumbing.

## Design rules

- The profile is semantic and vendor-neutral.
- Harness integrations own harness-specific wording, persistence mechanics, and delivery behavior.
- The canonical profile is the source of truth; generated harness instructions are projections.
- Normal users should not need to see or edit YAML unless they explicitly want to.
- Human preference data remains user-owned and should not need to be committed into project repositories.
- Existing harness configuration must be preserved.
- Preferences map to observable behavior, not diagnosis labels or vague personality categories.
- Weak behavior enforcement in one harness is an adapter/compatibility concern, not a reason to mutate human intent.

## Validation

The reference .NET implementation should continue to build and preserve its existing smoke tests under `reference/dotnet/`.

Native integration changes should also be validated independently of the reference compiler. Do not treat passing .NET tests as proof that a native integration works.

Behavioral experiments and evidence belong in `evals/`.

## Important files

- `spec/draft.md` — broader product and architecture direction
- `spec/milestone-1-addendum.md` — first-harness learnings
- `spec/milestone-2-addendum.md` — portability learnings
- `profiles/example.yaml` — reference semantic profile representation
- `integrations/claude/` — current product-facing native Claude experiment
- `reference/dotnet/` — reference compiler and previous bootstrap implementations
- `evals/` — behavioral expectations and evidence

## Working style

Keep the user out of decision paralysis. Maintain one active next step, park unrelated ideas, and distinguish assumptions that are cheap to reverse from decisions that genuinely need discussion.
