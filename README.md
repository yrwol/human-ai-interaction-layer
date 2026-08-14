# HAIL

**Human-AI Interaction Layer**

> AI should HAIL to you, not the other way around.

HAIL is an experimental, human-first interaction layer for expressing how a person wants AI systems to collaborate with them, then applying those preferences through the mechanisms native to each AI harness.

Milestone 1 proved that a small semantic profile can change Claude behavior. Milestone 2 proved that the same profile remains meaningful across Claude and Codex even when enforcement strength differs by harness.

The current milestone asks:

> **Can a normal person intentionally configure persistent HAIL defaults without editing YAML, running a compiler, or understanding harness configuration?**

## Product direction

```text
explicit HAIL configuration
          ↓
persistent semantic HAIL profile
          ↓
harness-native integration
          ↓
default adaptive AI behavior
```

Ordinary conversation is **not** persistent configuration. A user saying “stop waiting on me” during a task may reasonably mean “for this task,” not “change my default forever.” HAIL therefore does not silently mutate persistent preferences from ordinary conversational instructions.

Temporary/session/task-specific adaptation is a separate future problem and remains intentionally parked.

## Current semantic profile

HAIL currently supports six persistent preferences:

```yaml
version: 0.1
profile:
  verbosity: balanced
  decision_mode: recommend_first
  max_options: 3
  task_chunking: adaptive
  step_pacing: continuous
  tangent_policy: capture_and_return
```

`task_chunking` controls how work is divided. `step_pacing` controls how quickly HAIL moves through those pieces, including whether it should wait for the user before continuing.

YAML is the current reference representation. Normal users should not need to see or edit it.

## Native Claude experiment

The first no-compiler persistent profile-management experiment lives at [`integrations/claude/`](integrations/claude/).

It is a Claude plugin with an explicit HAIL skill. Persistent configuration should start through HAIL, for example:

```text
/hail:hail
/hail:hail show
/hail:hail setup
/hail:hail change
/hail:hail reset
```

The interaction remains conversational after invocation. For example:

```text
/hail:hail change
> Give me one small step at a time and wait for me before moving on.
```

or:

```text
/hail:hail
> Show me how HAIL is configured.
```

Inside explicit HAIL configuration, the skill translates natural-language needs into the semantic profile, persists the profile at `~/.hail/profile.yaml`, regenerates HAIL's Claude projection at `~/.hail/claude-code.md`, and preserves the single import in the user's `~/.claude/CLAUDE.md`.

The native path does **not** invoke the .NET reference implementation.

### Persistent vs contextual behavior

```text
/hail:hail change
"Stop waiting between steps by default."
        ↓
persistent profile change

ordinary task conversation
"Stop waiting on me."
        ↓
contextual request only
        ↓
HAIL profile remains unchanged
```

If users later need reliable “today only,” session, task, or overload modes, that should be designed explicitly rather than smuggled into persistent profile management.

### Developer test

With Claude Code installed, load the plugin directly from the repository:

```bash
claude --plugin-dir ./integrations/claude
```

Then invoke `/hail:hail` and configure or inspect the persistent profile. After a change, use `/clear` and test normal behavior to verify the generated preferences survive a fresh conversation.

## Reference implementation

The original .NET compiler lives under [`reference/dotnet/`](reference/dotnet/).

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
- Milestone 3 — native persistent profile management: **in progress**

The first native tests have already demonstrated conversational profile creation, migration from the reference-generated Claude projection, persistence across `/clear`, one-step-at-a-time pacing, and composition between pacing and tangent handling.

They also demonstrated why persistent changes must be explicit: ordinary instructions such as “stop waiting on me” naturally operate as task-local instructions and should not silently redefine the user's defaults.

## Repository shape

```text
integrations/         product-facing harness-native experiments
  claude/             current explicit persistent profile-management experiment
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
- Persistent preference mutation requires explicit HAIL intent.
- Ordinary conversational adaptation must not silently rewrite the persistent profile.
- Do not mutate user preferences to compensate for weak harness enforcement.
- Do not expose implementation plumbing to normal users.
- Do not add MCP, a shared runtime, diagnosis presets, cloud sync, or a UI until a concrete validated need requires them.
- Treat temporary/session/task-specific behavior as a separate design problem from persistent defaults.

## Current milestone exit condition

The native persistent profile-management experiment succeeds when a user can:

1. intentionally enter HAIL configuration;
2. describe persistent preferences in ordinary language inside that explicit context;
3. have them mapped to the HAIL semantic profile;
4. persist them without manually touching YAML;
5. have Claude apply the updated behavior across a fresh conversation without invoking the reference compiler; and
6. inspect, change, or reset the profile again through explicit HAIL interaction.

Only after that should we decide how persistent profile management should be packaged or ported to additional harnesses. Temporary/session adaptation remains parked until it gets its own requirements and tests.
