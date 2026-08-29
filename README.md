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

- [`evals/prompt-hardening/`](evals/prompt-hardening/) — current semantic prompt-hardening experiments, focused composition boundaries, and current result records
- [`evals/historical/`](evals/historical/) — historical fixtures, manual procedures, milestone summaries, raw transcripts, and portability evidence
- [`evals/prompt-context-efficiency.md`](evals/prompt-context-efficiency.md) — standalone prompt-context efficiency experiment

Completed evidence includes:

- Static semantic profile → Claude: **PASS**
- Same semantic profile → Codex: **PASS for portability evidence**
- Claude native persistent profile management: **PASS / manually validated**
- Codex native persistent profile management: **PASS / manually validated**
- `verbosity: detailed` cross-harness projection hardening: **PASS / promoted for Claude and Codex**

The current semantic-hardening checkpoint remains `task_chunking` cross-harness replay and supported single-turn boundary checks. Separately, the discoverable-skills management surface is implemented and its deterministic Claude/Codex management behavior has been validated; only interactive autocomplete/search discoverability remains a manual harness-UI check. See [`spec/roadmap.md`](spec/roadmap.md).

## Discoverable HAIL management

HAIL keeps a root management skill for orientation and compatibility while exposing common persistent-profile actions as separately discoverable skills.

Conceptually:

```text
hail   → orient, inspect, or route conversational HAIL intent
show   → inspect the persistent profile (read-only)
setup  → initialize HAIL
change → change persistent defaults
reset  → reset all or part of the profile
```

Invocation syntax is harness-native rather than artificially identical across products. `review` is intentionally not advertised yet because that separate capability is not implemented.

## Native Claude integration

The Claude implementation lives at [`integrations/claude/`](integrations/claude/).

With the plugin loaded, the discoverable management surface is:

```text
/hail:hail
/hail:show
/hail:setup
/hail:change
/hail:reset
```

The root `/hail:hail` skill remains compatible with natural-language configuration intent and the legacy subcommand-shaped requests during migration.

The Claude skills read/write the canonical `~/.hail/profile.yaml`, regenerate HAIL's Claude projection at `~/.hail/claude-code.md`, and preserve the single HAIL import in `~/.claude/CLAUDE.md`.

See [`integrations/claude/README.md`](integrations/claude/README.md).

### Claude developer test

```bash
claude --plugin-dir ./integrations/claude
```

Then verify `/hail:hail`, `/hail:show`, `/hail:setup`, `/hail:change`, and `/hail:reset` are discoverable.

## Native Codex integration

The Codex port lives at [`integrations/codex/`](integrations/codex/).

Codex uses collision-safe HAIL-prefixed skill names because skill names are effectively global:

```text
$hail
$hail-show
$hail-setup
$hail-change
$hail-reset
```

The root `$hail` skill remains available for orientation, compatibility, and conversational routing.

The Codex skills use the same canonical `~/.hail/profile.yaml` and manage only HAIL's marked block in `$CODEX_HOME/AGENTS.md` (normally `~/.codex/AGENTS.md`).

The original native Codex profile-management path was manually validated before this skill split. The discoverable surface now also passes deterministic package, callability, normalization, state-mutation, and projection-integrity validation in both Claude and Codex. Interactive autocomplete/search presentation remains a separate manual harness-UI check.

See [`integrations/codex/README.md`](integrations/codex/README.md) and [`evals/historical/results/milestone-4-codex-native-raw.md`](evals/historical/results/milestone-4-codex-native-raw.md).

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
  claude/             native Claude profile management + discoverable skills
  codex/              native Codex profile management + discoverable skills
reference/
  dotnet/             reference compiler/conformance tooling
profiles/             reference semantic profile examples
evals/
  prompt-hardening/   active field hardening + focused composition experiments/results
  historical/         superseded eval fixtures/procedures + milestone evidence
    results/           historical milestone summaries/raw transcripts
spec/                 product truth, semantics, taxonomy, roadmap, historical decisions
  capabilities/       focused capability contracts
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
- Make meaningful user actions discoverable when the harness supports it; do not hide every capability behind private subcommand vocabulary.
- Add semantic fields only after a real scenario proves the current vocabulary insufficient.
- Give each tracked composition boundary its own focused eval definition.
- Do not make multi-harness synchronization a shipping requirement.
- Do not add MCP, shared runtime, cloud sync, diagnosis presets, or polished UI until a concrete experiment requires them.
