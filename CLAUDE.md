# HAIL Project Instructions

@AGENTS.md

## Claude notes

You are working on HAIL itself, not consuming a user's HAIL profile.

Follow `AGENTS.md` for project purpose, scope discipline, design rules, validation, and working style.

The active product-facing Claude work lives under `integrations/claude/`. Keep Claude-specific behavior and persistence mechanics there. Do not leak Claude concepts into the vendor-neutral profile semantics.

The code under `reference/dotnet/` is reference/conformance tooling. New native profile-management capabilities must not depend on invoking it.

Do not confuse this repository-level `CLAUDE.md` with a user's `~/.claude/CLAUDE.md`. The repository file guides development of HAIL; the native HAIL skill may manage only its marked HAIL block in the user's file.
