# HAIL Agent Guide

## Project purpose

HAIL is a human-first, vendor-neutral interaction layer. A human-owned semantic profile describes how a person wants AI to work with them. Harness integrations translate and apply that profile using the mechanisms native to each harness.

The product is the semantic model + harness integrations. The .NET implementation under `reference/dotnet/` is reference/conformance tooling, not the user experience.

## Current project checkpoint

The active work is **documentation reconciliation** after native profile-management work.

Do not start a new product feature until the repository clearly distinguishes:

- current product truth;
- current validated semantic model;
- broader interaction taxonomy/reference ideas;
- historical experiment evidence; and
- current roadmap/parked work.

Start with `spec/README.md`.

Current authoritative docs:

- `spec/product.md` — durable product definition and architecture boundary;
- `spec/semantics.md` — current validated persistent semantic model;
- `spec/interaction-taxonomy.md` — broader candidate interaction dimensions;
- `spec/roadmap.md` — current project state and next candidate experiments.

Historical milestone documents are evidence records, not competing current specs.

## Current implementation state

- Static Claude behavior proof: complete.
- Claude→Codex semantic portability proof: complete.
- Claude native persistent profile management: complete / manually validated.
- Codex native persistent profile management: complete / manually validated.
- Temporary interaction state: not yet implemented.

Cross-harness projection refresh remains intentionally unresolved; do not treat that as a failure of the Codex native-management milestone.

No `dotnet run` dependency is allowed in normal-user product paths.

## Scope discipline

Keep changes small and evidence-driven.

Do not add MCP, a shared runtime, desktop UI, cloud sync, diagnosis presets, automatic emotional-state detection, or broad abstractions unless a concrete experiment demonstrates a need.

Prefer harness-native capabilities when they can preserve HAIL semantics and user ownership without extra plumbing.

Cross-harness projection synchronization remains intentionally unsolved. Record whether stale projections create a real user problem before designing synchronization infrastructure.

## Design rules

- The persistent profile is semantic, vendor-neutral, and human-owned.
- Harness integrations own harness-specific wording, persistence mechanics, and delivery behavior.
- The canonical profile is the source of truth for persistent defaults; generated harness instructions are disposable projections.
- **Persistent profile mutation requires explicit persistent HAIL intent.** Ordinary conversational instructions must not silently rewrite persistent defaults.
- Persistent profile and temporary interaction state are separate concepts.
- Normal users should not need to see or edit YAML unless they explicitly want to.
- Human preference data remains user-owned and should not need to be committed into project repositories.
- Existing harness configuration must be preserved.
- Preferences map to observable behavior, not diagnosis labels or vague personality categories.
- Weak behavior enforcement in one harness is an adapter/compatibility concern, not a reason to mutate human intent.
- Add a new persistent semantic field only when a concrete persistent user need cannot be expressed by the existing vocabulary.
- Do not promote taxonomy candidates into schema without evidence.

## Validation

The reference .NET implementation should continue to build and preserve its existing smoke tests under `reference/dotnet/`.

Native integration changes must be validated independently of the reference compiler.

Behavioral experiments and evidence belong in `evals/`.

Completed native-management experiments should point to their raw behavioral evidence. Unresolved follow-up questions such as projection refresh should remain explicitly separate from milestone pass/fail status.

## Important files

- `spec/README.md` — specification map and authority rules
- `spec/product.md` — current product specification
- `spec/semantics.md` — current validated semantic model
- `spec/interaction-taxonomy.md` — broad interaction-dimension reference
- `spec/roadmap.md` — current roadmap/checkpoint
- `spec/capabilities/` — focused capability specifications that are not automatically schema or roadmap commitments
- `spec/draft.md` — original legacy working spec; useful historical context, not current authority
- `spec/milestone-1-addendum.md` — first-harness learnings
- `spec/milestone-2-addendum.md` — portability learnings
- `spec/milestone-3-working-notes.md` — completed Claude native-management evidence
- `spec/milestone-4-working-notes.md` — completed Codex native-management evidence
- `profiles/example.yaml` — reference semantic profile representation
- `integrations/claude/` — native Claude persistent profile management
- `integrations/codex/` — native Codex persistent profile management
- `reference/dotnet/` — reference compiler/conformance tooling
- `evals/` — behavioral expectations and evidence

`spec/draft-guidance.md`, where present after branch reconciliation with `main`, is a legacy source for the canonical `spec/capabilities/review-guidance.md` capability specification and should not be repurposed as the core roadmap/schema source.

## Working style

Keep the user out of decision paralysis. Maintain one active next step, park unrelated ideas, and distinguish assumptions that are cheap to reverse from decisions that genuinely need discussion.
