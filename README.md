# HAIL

**Human-AI Interaction Layer**

> AI should HAIL to you, not the other way around.

HAIL is an experimental, human-first interaction layer for expressing how a person wants AI systems to collaborate with them, then translating those preferences into harness-specific instructions.

The current proof looks like this:

```text
human-owned profile
       ↓
semantic HAIL model
       ↓
harness adapter
       ↓
harness-specific interaction instructions
       ↓
AI harness
```

Milestone 1 validated the idea against Claude. Milestone 2 is testing portability by applying the **same semantic profiles and behavioral scenarios to Codex**.

## Current profile

HAIL currently supports five interaction preferences:

```yaml
version: 0.1

profile:
  verbosity: balanced
  decision_mode: recommend_first
  max_options: 3
  task_chunking: adaptive
  tangent_policy: capture_and_return
```

The profile remains vendor-neutral. Claude- and Codex-specific wording belongs in their adapters.

## Requirements for the reference implementation

- .NET 10 SDK
- the target AI harness only if you want to manually test behavior

The .NET project is currently a **reference compiler/test harness**, not a statement that normal HAIL users should eventually install or operate a standalone executable.

## Use HAIL

Claude remains the default target:

```bash
dotnet run --project src/Hail -- profiles/example.yaml
```

Compile explicitly for Codex:

```bash
dotnet run --project src/Hail -- profiles/example.yaml --target codex
```

Generate an artifact for either harness:

```bash
dotnet run --project src/Hail -- profiles/example.yaml \
  --target codex \
  --output artifacts/codex.md
```

### Install for Claude Code

```bash
dotnet run --project src/Hail -- profiles/example.yaml \
  --install claude-code
```

HAIL writes the generated contract to `~/.hail/claude-code.md` and adds a single import to `~/.claude/CLAUDE.md` without replacing unrelated user instructions.

### Install for Codex

```bash
dotnet run --project src/Hail -- profiles/example.yaml \
  --install codex
```

HAIL manages only a marked block inside the user-level Codex `AGENTS.md`, preserving unrelated instructions:

```text
<!-- HAIL:START -->
...
<!-- HAIL:END -->
```

Reinstalling replaces the HAIL block rather than duplicating it.

## Milestone 2: test Codex portability

The goal is **not** to decide whether Codex gives better answers than Claude. The question is whether the same HAIL semantics cause the same directional interaction changes in a second harness.

Use the existing comparison profiles:

- [`profiles/test-a.yaml`](profiles/test-a.yaml): recommendation-first, fewer options, adaptive chunking, capture-and-return
- [`profiles/test-b.yaml`](profiles/test-b.yaml): options-first, more options, no automatic chunking, follow tangents

### 1. Apply Profile A to Codex

```bash
dotnet run --project src/Hail -- profiles/test-a.yaml \
  --install codex
```

Start a fresh Codex session.

### 2. Run the five evaluation scenarios

Use the exact prompts and procedure in [`evals/manual-testing.md`](evals/manual-testing.md).

Paste the complete responses into:

[`evals/results/milestone-2-codex-raw.md`](evals/results/milestone-2-codex-raw.md)

### 3. Apply Profile B and repeat

```bash
dotnet run --project src/Hail -- profiles/test-b.yaml \
  --install codex
```

Start another fresh Codex session and run the **same prompts in the same order**.

### 4. Compare two things

For each semantic dimension, record:

1. Did Codex move from Profile A toward Profile B in the intended direction?
2. Was Codex's enforcement stronger, similar, or weaker than Claude's Milestone 1 result?

Milestone 1 findings and raw Claude responses are preserved under [`evals/results/`](evals/results/).

## Automated checks

GitHub Actions validates the reference pipeline, including:

- .NET 10 restore/build;
- profile loading;
- Claude and Codex compilation;
- harness-specific artifact generation;
- safe/idempotent Claude installation; and
- safe/idempotent Codex installation without overwriting unrelated `AGENTS.md` content.

The behavioral fixtures under [`evals/`](evals/) describe intended behavior. CI is not currently calling the AI harnesses and grading their responses.

## Repository shape

```text
.github/workflows/   CI for the reference implementation
profiles/            vendor-neutral semantic profiles
evals/               behavioral scenarios, test guide, and results
spec/                product/specification work
src/Hail/            reference parser, model, adapters, and bootstrap code
AGENTS.md             harness-neutral contributor/agent guidance
CLAUDE.md             Claude project guidance
```

## Current design rule

Keep the boundary intact:

```text
human intent                   harness implementation
-----------                    ----------------------
recommend first       ───────▶  Claude wording
                      └───────▶  Codex wording

limit choices         ───────▶  harness-specific enforcement
preserve active goal  ───────▶  harness-specific delivery
```

Do not change the semantic profile just because one harness needs different wording to express the same intent.

## Intentionally not solved yet

HAIL does not currently attempt dynamic state detection, automatic preference learning, MCP/runtime integration, marketplaces, cloud profile sync, polished onboarding, or a profile-builder UI.

Those remain evidence-driven future work. Normal users should ultimately not need to understand YAML, instruction files, compilers, runtimes, or harness configuration to benefit from HAIL.
