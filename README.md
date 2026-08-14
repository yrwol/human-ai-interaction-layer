# HAIL

**Human-AI Interaction Layer**

> AI should HAIL to you, not the other way around.

HAIL is an experimental, human-first interaction layer for expressing how a person wants AI systems to collaborate with them, then translating those preferences into harness-specific instructions.

The current v0 proves the smallest useful slice of that idea:

```text
human-owned profile
       ↓
semantic HAIL model
       ↓
harness adapter
       ↓
generated interaction instructions
       ↓
AI harness
```

Today, the implemented harness is **Claude Code**.

## Current v0 scope

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

The example profile lives at [`profiles/example.yaml`](profiles/example.yaml).

The current Claude Code adapter translates those semantic preferences into concrete behavioral instructions. The profile itself is intentionally vendor-neutral; Claude-specific wording lives in the adapter.

## Requirements

- .NET 10 SDK
- Claude Code, only if you want to install/test the generated Claude instructions

This repository is still a v0 proof of concept. HAIL is not packaged as a global `hail` executable yet, so commands currently use `dotnet run --project src/Hail -- ...`.

## Use HAIL

### Print the generated Claude Code instructions

```bash
dotnet run --project src/Hail -- profiles/example.yaml
```

This loads the profile and prints the compiled Claude Code interaction instructions to stdout.

### Generate a Markdown artifact

```bash
dotnet run --project src/Hail -- profiles/example.yaml \
  --output artifacts/claude-code.md
```

HAIL will create the output directory if necessary and write the generated interaction contract to the requested file.

### Install the profile for Claude Code

```bash
dotnet run --project src/Hail -- profiles/example.yaml \
  --install claude-code
```

The Claude Code installer is intentionally conservative. It:

1. writes the generated contract to `~/.hail/claude-code.md`;
2. ensures `~/.claude/CLAUDE.md` exists; and
3. adds `@~/.hail/claude-code.md` to the Claude file only if that import is not already present.

Existing `~/.claude/CLAUDE.md` content is preserved, and running the installer repeatedly should not duplicate the HAIL import.

After installation, start a **new Claude Code session** when comparing behavior so the instructions are loaded cleanly.

## Test the profiles manually

The point of the current experiment is not merely to prove that YAML can become Markdown. We want to determine whether changing the **same semantic HAIL profile** produces meaningfully different AI behavior.

### 1. Create two deliberately different profiles

Keep the experiment obvious. Do not test tiny preference differences first.

For example:

```yaml
# profiles/test-a.yaml
version: 0.1

profile:
  verbosity: balanced
  decision_mode: recommend_first
  max_options: 2
  task_chunking: adaptive
  tangent_policy: capture_and_return
```

```yaml
# profiles/test-b.yaml
version: 0.1

profile:
  verbosity: detailed
  decision_mode: options
  max_options: 6
  task_chunking: off
  tangent_policy: follow
```

### 2. Install Profile A

```bash
dotnet run --project src/Hail -- profiles/test-a.yaml \
  --install claude-code
```

Start a fresh Claude Code session.

### 3. Run the same prompts

Use identical prompts for both profiles. Suggested scenarios are represented in [`evals/`](evals/).

A useful manual set is:

**Decision behavior**

```text
I could use Redis, Postgres, or an in-memory cache. What should I use?
```

Look for whether Claude recommends first or primarily presents options.

**Option limiting**

```text
Give me every reasonable approach I could use to add caching to this service.
```

Look at how many choices are presented at once.

**Task chunking**

```text
I need to add authentication, migrate the database, update the deployment pipeline, write tests, and document everything. Help me get started.
```

Look for whether complex work is converted into a small set of concrete next steps.

**Tangent handling**

Begin working with Claude toward an explicit implementation goal. Then introduce a separate side question.

Look for whether Claude captures the tangent while preserving the active goal, or follows the tangent instead.

**Verbosity**

```text
Explain what this code change does and what I need to know before I continue.
```

Compare response depth, expansion, and whether the amount of detail feels materially different.

### 4. Record what happened

For each scenario, capture:

- profile used;
- exact prompt;
- response or relevant excerpt;
- whether the expected behavior was clearly present, partially present, or absent; and
- anything surprising.

Do not grade whether one profile is universally "better." The question is whether the behavior **changed in the direction requested by the profile**.

### 5. Install Profile B and repeat

```bash
dotnet run --project src/Hail -- profiles/test-b.yaml \
  --install claude-code
```

Start another fresh Claude Code session and use the **exact same prompts in the same order**.

The strongest v0 result would look roughly like this:

| Behavior | Profile A | Profile B |
| --- | --- | --- |
| Decisions | recommends first | presents options |
| Options | no more than 2 at once | allows up to 6 |
| Complex tasks | chunks into concrete next steps | leaves work less structured |
| Tangents | captures and returns | follows the tangent |
| Verbosity | balanced | more detailed |

If those differences are visible and repeatable, HAIL's core static-profile hypothesis is working.

## Automated checks

GitHub Actions currently validates the v0 pipeline on `feat/hail-v0`, including:

- restore and .NET 10 build;
- loading the example profile;
- compiling the Claude Code interaction contract;
- checking the `recommend_first` contract behavior;
- generating a Markdown delivery artifact; and
- exercising the Claude Code bootstrap against a temporary home directory to ensure existing Claude instructions are preserved and the HAIL import remains idempotent.

The files under [`evals/`](evals/) describe the intended observable behaviors. They are currently evaluation fixtures rather than a claim that CI is executing Claude and judging model responses.

## Repository shape

```text
.github/workflows/   CI for the v0 proof
profiles/            human-owned semantic interaction profiles
evals/               behavioral evaluation scenarios
spec/                product/specification work
src/Hail/            parser, semantic model, adapter, and CLI/bootstrap code
AGENTS.md             harness-neutral contributor/agent guidance
CLAUDE.md             Claude Code project guidance
```

## v0 design rule

HAIL should keep this boundary intact:

```text
human intent                   harness implementation
-----------                    ----------------------
recommend first       ───────▶  Claude instructions
limit choices         ───────▶  future harness instructions
preserve active goal  ───────▶  future adapter behavior
```

Do not put Claude-specific concepts into the semantic profile simply because Claude Code is the first harness.

## What is intentionally not built yet

The current v0 does **not** attempt to solve dynamic state detection, automatic learning, MCP integration, marketplaces, plugin distribution, cloud profile sync, a profile-builder UI, or every possible interaction preference.

Those should be added only when the validated behavior requires them.
