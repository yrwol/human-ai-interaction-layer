# HAIL

**Human-AI Interaction Layer**

> AI should HAIL to you, not the other way around.

HAIL is an experimental, human-first interaction layer for expressing how a person wants AI systems to collaborate with them, then applying those preferences through the mechanisms native to each AI harness.

Milestone 1 proved that a small semantic profile can change Claude behavior. Milestone 2 proved that the same profile remains meaningful across Claude and Codex even when enforcement strength differs by harness.

The current milestone asks a different question:

> **Can a normal person configure and change HAIL conversationally without editing YAML, running a compiler, or understanding harness configuration?**

## Product direction

The intended product boundary is now:

```text
human language
     ↓
semantic HAIL profile
     ↓
harness-native integration
     ↓
adaptive AI behavior
```

The semantic profile remains vendor-neutral. Harness integrations own harness-specific wording, persistence mechanics, and delivery behavior.

## Current semantic profile

HAIL currently supports five persistent preferences:

```yaml
version: 0.1
profile:
  verbosity: balanced
  decision_mode: recommend_first
  max_options: 3
  task_chunking: adaptive
  tangent_policy: capture_and_return
```

YAML is the current reference representation. Normal users should not need to see or edit it.

## Native Claude experiment

The first no-compiler profile-management experiment lives at [`integrations/claude/`](integrations/claude/).

It is a Claude plugin with a `/hail` skill. The skill is intended to handle requests such as:

```text
Help me set up HAIL.
Show me how HAIL is configured.
Stop giving me so many choices.
Just recommend what you think I should do.
Keep answers shorter.
When I get distracted, remember what we were doing.
```

The skill translates those requests into the existing semantic profile, persists the profile at `~/.hail/profile.yaml`, and updates only HAIL's managed section in the user's Claude instructions.

The native path does **not** invoke the .NET reference implementation.

### Developer test

With Claude Code installed, load the plugin directly from the repository:

```bash
claude --plugin-dir ./integrations/claude
```

Then invoke:

```text
/hail
```

or ask naturally for one of the preference changes above. Claude Code plugins support skills as `skills/<name>/SKILL.md`, and local plugins can be tested with `--plugin-dir` before marketplace packaging.

## Reference implementation

The original .NET compiler has been moved to [`reference/dotnet/`](reference/dotnet/).

It remains useful as reference/conformance tooling for:

- parsing the reference YAML representation;
- deterministic Claude and Codex instruction compilation;
- preserving the Milestone 1 and 2 bootstrap tests; and
- comparing native implementations against previously validated behavior.

It is **not** the intended normal-user experience, and new product capabilities should not depend on `dotnet run` unless a later experiment demonstrates a real need for shared runtime code.

## Evidence so far

Detailed behavioral evidence lives under [`evals/results/`](evals/results/).

- Milestone 1 — static profile → Claude: **PASS**
- Milestone 2 — same profile → Codex: **PASS for portability evidence**

One of the strongest findings so far is that compatibility is preference-level rather than binary. Claude and Codex followed different semantic dimensions with different strengths, while the human-owned profile remained unchanged.

## Repository shape

```text
integrations/         product-facing harness-native experiments
  claude/             current conversational profile-management experiment
reference/
  dotnet/             reference compiler/conformance tooling
profiles/             reference semantic profile examples
evals/                behavioral scenarios, guides, and evidence
spec/                 product/specification work
.github/workflows/    validation for reference + native integration shape
AGENTS.md              harness-neutral contributor/agent guidance
CLAUDE.md              Claude project guidance
```

## Current design rules

- Keep human intent vendor-neutral.
- Prefer harness-native capabilities when they are sufficient.
- Do not mutate user preferences to compensate for weak harness enforcement.
- Do not expose implementation plumbing to normal users.
- Do not add MCP, a shared runtime, more profile fields, diagnosis presets, cloud sync, or a UI until a concrete validated need requires them.

## Current milestone exit condition

The native profile-management experiment succeeds when a user can:

1. express a preference change in ordinary language;
2. have it mapped to the existing HAIL semantic profile;
3. persist that preference without manually touching YAML;
4. have Claude apply the updated behavior without invoking the reference compiler; and
5. inspect or change the preference again conversationally.

Only after that should we decide what parts of profile management need to become portable across additional harnesses.
