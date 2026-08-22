# HAIL

**Human-AI Interaction Layer**

> AI should HAIL to you, not the other way around.

HAIL is an experimental, human-first interaction layer for expressing how a person wants AI systems to collaborate with them, then applying those preferences through the mechanisms native to each AI harness.

## Start here

The specification has been split so current truth is easier to find:

- [`spec/README.md`](spec/README.md) — map of the specification and which documents are authoritative
- [`spec/product.md`](spec/product.md) — current product definition and architecture boundary
- [`spec/semantics.md`](spec/semantics.md) — current validated semantic profile
- [`spec/interaction-taxonomy.md`](spec/interaction-taxonomy.md) — broader catalog of candidate interaction dimensions
- [`spec/roadmap.md`](spec/roadmap.md) — completed work, current checkpoint, next experiments, and parked areas

The original [`spec/draft/draft.md`](spec/draft/draft.md) remains useful historical context, but it is no longer the authoritative source for current schema, architecture, or milestone order.

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

Ordinary conversation is **not** persistent configuration. A statement such as “stop waiting on me” during a task may reasonably be contextual. HAIL therefore does not silently mutate persistent preferences from ordinary conversational instructions.

Persistent profile and temporary interaction state are separate concepts.

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

See [`spec/semantics.md`](spec/semantics.md) for the behavioral contract and evidence status of each field.

The larger original interaction taxonomy has **not** been discarded. It now lives in [`spec/interaction-taxonomy.md`](spec/interaction-taxonomy.md), where candidate dimensions can remain visible without being mistaken for implemented schema.

## Evidence and active evaluation

Start with [`evals/README.md`](evals/README.md) for the evaluation map.

- [`evals/prompt-hardening/`](evals/prompt-hardening/) — current semantic prompt-hardening experiments and current result records
- [`evals/historical/`](evals/historical/) — superseded lightweight fixtures and the original manual A/B procedure
- [`evals/results/`](evals/results/) — preserved historical milestone transcripts and summaries
- [`evals/prompt-context-efficiency.md`](evals/prompt-context-efficiency.md) — standalone prompt-context efficiency experiment

Completed evidence includes:

- Static semantic profile → Claude: **PASS**
- Same semantic profile → Codex: **PASS for portability evidence**
- Claude native persistent profile management: **PASS / manually validated**
- Codex native persistent profile management: **PASS / manually validated**

The current project checkpoint is **projection hardening of the existing validated semantics**. Current targets are `task_chunking` and `verbosity`, followed by cross-harness replay of stable wording. See [`spec/roadmap.md`](spec/roadmap.md).

## Native Claude integration

The Claude implementation lives at [`integrations/claude/`](integrations/claude/).

Persistent configuration starts through explicit HAIL interaction:

```text
/hail:hail
/hail:hail show
/hail:hail setup
/hail:hail change
/hail:hail reset
```

The skill reads/writes the canonical `~/.hail/profile.yaml`, regenerates HAIL's Claude projection at `~/.hail/claude-code.md`, and preserves the single HAIL import in `~/.claude/CLAUDE.md`.

Normal use does **not** invoke the .NET reference implementation.

### Claude developer test

```bash
claude --plugin-dir ./integrations/claude
```

Then invoke `/hail:hail`.

## Native Codex integration

The Codex port lives at [`integrations/codex/`](integrations/codex/).

Its explicit persistent-configuration entry point is `$hail`:

```text
$hail
$hail show
$hail setup
$hail change
$hail reset
```

The Codex skill uses the same canonical `~/.hail/profile.yaml` and manages only HAIL's marked block in `$CODEX_HOME/AGENTS.md` (normally `~/.codex/AGENTS.md`).

The native Codex path has been manually validated for profile inspection, natural-language change, fresh-session pacing behavior, tangent composition, contextual requests that do not mutate persistent defaults, and reset.

See [`integrations/codex/README.md`](integrations/codex/README.md) and [`evals/results/milestone-4-codex-native-raw.md`](evals/results/milestone-4-codex-native-raw.md).

## Multi-harness refresh is optional

HAIL does not require a user to use multiple harnesses. Each harness can independently apply the same canonical semantic profile.

If a user does use multiple harnesses, one harness may temporarily have an older generated projection after the canonical profile is changed elsewhere. Improving that refresh experience is a **parked enhancement**, not a requirement for shipping or for HAIL's portability model.

Prefer simple refresh-at-use behavior if this becomes worth improving. Do not add a daemon, shared runtime, cloud service, or MCP synchronization layer without demonstrated user need.

## Reference implementation

The original .NET compiler lives under [`reference/dotnet/`](reference/dotnet/).

It remains useful as reference/conformance tooling for deterministic compilation and historical tests. It is **not** the intended normal-user product architecture.

## Repository shape

```text
integrations/         harness-native product experiments
  claude/             native Claude persistent profile management
  codex/              native Codex persistent profile management
reference/
  dotnet/             reference compiler/conformance tooling
profiles/             reference semantic profile examples
evals/
  prompt-hardening/   active behavioral hardening experiments/results
  historical/         superseded eval fixtures/procedures
  results/            preserved milestone evidence
spec/                 product truth, semantics, taxonomy, roadmap, historical decisions
  draft/              legacy source documents
.github/workflows/    repository validation
AGENTS.md              contributor/agent guidance
CLAUDE.md              Claude project guidance
```

## Current design rules

- Keep human intent vendor-neutral.
- Needs over diagnoses.
- Prefer harness-native capabilities when they are sufficient.
- Persistent preference mutation requires explicit HAIL intent.
- Ordinary conversational adaptation must not silently rewrite persistent defaults.
- Profile is not temporary state.
- Treat generated harness artifacts as disposable projections of the human-owned semantic profile.
- Do not mutate user intent to compensate for weak harness enforcement.
- Do not expose implementation plumbing to normal users.
- Add semantic fields only after a real scenario proves the current vocabulary insufficient.
- Do not make multi-harness synchronization a shipping requirement.
- Do not add MCP, shared runtime, cloud sync, diagnosis presets, or polished UI until a concrete experiment requires them.
