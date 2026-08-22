# HAIL Project Instructions

@AGENTS.md

## Claude notes

You are working on HAIL itself, not consuming a user's HAIL profile.

Follow `AGENTS.md` for project purpose, scope discipline, design rules, validation, working style, roadmap hygiene, and the required profile-field development workflow.

When adding or changing a profile field, do not infer the expected structure from memory or implement schema first. Follow `evals/adding-profile-fields.md`, create/update the field's `spec/capabilities/<field-name>.md` behavioral contract, use the templates under `evals/templates/`, and update all relevant semantic, evaluation, roadmap, example/schema, and integration documentation. A field is not supported merely because configuration or prompt projection exists.

Before finishing any substantial experiment, milestone, spec change, or implementation phase, verify that `spec/roadmap.md` still accurately reflects:

- what is complete;
- what is currently active;
- what evidence changed;
- what the next meaningful step is; and
- what remains parked or explicitly out of scope.

If the work materially changes any of those, update `spec/roadmap.md` in the same branch/change set. Do not wait for a later documentation-cleanup pass. Do not churn the roadmap for trivial implementation details that do not change project status or direction.

The active product-facing Claude work lives under `integrations/claude/`. Keep Claude-specific behavior and persistence mechanics there. Do not leak Claude concepts into the vendor-neutral profile semantics.

The code under `reference/dotnet/` is reference/conformance tooling. New native profile-management capabilities must not depend on invoking it.

Do not confuse this repository-level `CLAUDE.md` with a user's `~/.claude/CLAUDE.md`. The repository file guides development of HAIL; the native HAIL skill may manage only its marked HAIL block in the user's file.
