# HAIL Agent Guide

## Project purpose

HAIL is a human-first, vendor-neutral interaction layer. A human-owned semantic profile describes how a person wants AI to work with them. Harness integrations translate and apply that profile using the mechanisms native to each harness.

The product is the semantic model + harness integrations. The .NET implementation under `reference/dotnet/` is reference/conformance tooling, not the user experience.

## Current project checkpoint

The active work is **projection hardening of the existing validated semantics**.

Current priorities are tracked in `spec/roadmap.md`. At this checkpoint, the immediate sequence is:

1. harden and characterize existing semantics one at a time;
2. keep experiment evidence separate from durable semantic definitions;
3. replay stable candidates across harnesses before introducing harness-specific wording;
4. avoid adding new semantics or infrastructure unless experiments demonstrate a real vocabulary or delivery gap.

Start with `spec/README.md`.

Current authoritative docs:

- `spec/product.md` — durable product definition and architecture boundary;
- `spec/semantics.md` — current validated persistent semantic model;
- `spec/interaction-taxonomy.md` — broader candidate interaction dimensions;
- `spec/roadmap.md` — current project state and next candidate experiments.

Historical milestone documents are evidence records, not competing current specs.

## Roadmap hygiene

`spec/roadmap.md` is a living project-status document, not a periodic cleanup artifact.

Whenever work materially changes any of the following, review and update the roadmap in the same branch/change set:

- a checkpoint becomes complete or is superseded;
- an experiment reaches a meaningful conclusion;
- a previously weak/unknown behavior becomes validated, rejected, or reclassified;
- the active next experiment changes;
- a roadmap candidate becomes parked, promoted, or deprioritized;
- new evidence changes what is required before moving forward.

Do not update the roadmap for trivial implementation details or every commit. Update it when the **project's current state, evidence, or next meaningful step** has changed.

Before considering a milestone/checkpoint or substantial experiment complete, explicitly ask:

> Does `spec/roadmap.md` still accurately describe what is complete, what is active now, and what comes next?

If not, update it before finishing the work.

## Current implementation state

- Static Claude behavior proof: complete.
- Claude→Codex semantic portability proof: complete.
- Claude native persistent profile management: complete / manually validated.
- Codex native persistent profile management: complete / manually validated.
- Projection hardening: active; strongest current evidence is for `decision_mode` and `max_options` under documented Claude test conditions.
- `task_chunking` and `verbosity`: next hardening targets.
- Temporary interaction state: parked until the current hardening work earns completion and the roadmap deliberately promotes it.

Multi-harness projection refresh is a parked optional enhancement, not a shipping requirement or a milestone blocker.

No `dotnet run` dependency is allowed in normal-user product paths.

## Scope discipline

Keep changes small and evidence-driven.

Do not add MCP, a shared runtime, desktop UI, cloud sync, diagnosis presets, automatic emotional-state detection, or broad abstractions unless a concrete experiment demonstrates a need.

Prefer harness-native capabilities when they can preserve HAIL semantics and user ownership without extra plumbing.

Do not treat cross-harness projection synchronization as required product plumbing. Many users may use only one harness. If multi-harness users later show meaningful friction from stale projections, prefer the smallest refresh-at-use mechanism before considering synchronization infrastructure.

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
- Multi-harness synchronization is an optional enhancement, not a portability requirement.

## Adding or changing profile fields

For any new profile field or semantic change to an existing field, follow the required workflow in `evals/adding-profile-fields.md`.

That document is the single authoritative procedure for capability contracts, evaluation, implementation, hardening, documentation, and support criteria. Do not duplicate or invent a parallel field-development process elsewhere.

## Validation

The reference .NET implementation should continue to build and preserve its existing smoke tests under `reference/dotnet/`.

Native integration changes must be validated independently of the reference compiler.

Behavioral experiments and evidence belong in `evals/`.

Completed experiments should point to their raw behavioral evidence and record material test conditions such as harness, model, reasoning/effort mode, and whether the interaction was fresh when those conditions could affect interpretation.

Parked enhancements such as multi-harness projection refresh should remain explicitly separate from milestone pass/fail status.

## Important files

- `spec/README.md` — specification map and authority rules
- `spec/product.md` — current product specification
- `spec/semantics.md` — current validated semantic model
- `spec/interaction-taxonomy.md` — broad interaction-dimension reference
- `spec/roadmap.md` — living current roadmap/checkpoint; keep synchronized with material project progress
- `spec/capabilities/` — focused capability specifications and behavioral contracts
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
- `evals/adding-profile-fields.md` — authoritative profile-field development/evaluation workflow
- `evals/templates/` — reusable evaluation artifact templates

`spec/draft-guidance.md`, where present after branch reconciliation with `main`, is a legacy source for the canonical `spec/capabilities/review-guidance.md` capability specification and should not be repurposed as the core roadmap/schema source.

## Working style

Keep the user out of decision paralysis. Maintain one active next step, park unrelated ideas, and distinguish assumptions that are cheap to reverse from decisions that genuinely need discussion.
