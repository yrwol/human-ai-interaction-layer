# Discoverable HAIL Skills

## Status

- **Capability type:** harness interaction / command discovery
- **Status:** experimental — management behavior validated; interactive discovery presentation still pending manual verification
- **Profile field:** none
- **Implementation status:** first-class skill structure is implemented for Claude and Codex; deterministic package/callability/profile-management validation passes in both harnesses

## Purpose

HAIL historically exposed multiple user actions through one harness entry point and interpreted words such as `show`, `setup`, `change`, and `reset` as subcommands or conversational intent inside that single skill.

That works when the user already knows the actions exist, but it creates a discoverability problem: important HAIL capabilities are effectively hidden behind prior knowledge of command words.

This capability defines a discoverable skill model where meaningful HAIL actions are represented as first-class harness skills when the harness supports skill discovery.

The goal is:

> A user should be able to discover what HAIL can do from the harness itself without first knowing HAIL's private command vocabulary.

This is an interaction-surface capability, not a new persistent semantic preference.

## Implemented shape

The current implementation preserves a root HAIL skill and adds independently invokable management skills.

Conceptually:

```text
HAIL
  ├─ hail       inspect / orient / route
  ├─ show       show current persistent HAIL configuration
  ├─ setup      create or initialize a HAIL profile
  ├─ change     intentionally change persistent HAIL configuration
  └─ reset      restore supported defaults
```

Harness-native invocation differs intentionally:

```text
Claude
/hail:hail
/hail:show
/hail:setup
/hail:change
/hail:reset

Codex
$hail
$hail-show
$hail-setup
$hail-change
$hail-reset
```

Codex uses HAIL-prefixed action names because skill names are effectively global and generic names such as `$show` or `$change` would create unnecessary collision risk. Claude action skill folders may use the short conceptual names because the HAIL plugin namespace exposes them as `/hail:<skill>`; they are not intended to be installed as generic standalone global skills.

`review` remains reserved for the separately defined [`conversation-review`](conversation-review.md) capability and is not exposed as implemented.

## Behavioral contract

### Discoverability

A user browsing or searching the harness's available HAIL skills should be able to identify the supported HAIL actions from names and descriptions alone.

Users should not have to know that a hidden `show`, `change`, or similar subcommand exists before they can find it.

Automated validation can prove that the expected skills are packaged and callable by their harness-native names. Visual autocomplete/search presentation remains a distinct harness-UI check and must not be claimed from filesystem/callability evidence alone.

### Single-purpose skills

Each first-class skill should represent one coherent user intent.

- **show** — inspect the current persistent profile and effective HAIL configuration;
- **setup** — establish a usable initial profile;
- **change** — intentionally persist one or more preference changes;
- **reset** — restore the supported default profile or a selected preference.

A skill may share implementation behavior with other skills, but its user-facing contract should remain narrow enough to understand from its name and description.

### Root HAIL skill

The root `hail` skill remains the general orientation and routing entry point.

It should:

- explain what HAIL is when useful;
- summarize available HAIL actions;
- route ambiguous HAIL configuration intent to the correct behavior;
- remain useful for users who invoke HAIL conversationally rather than selecting a more specific skill;
- preserve reasonable legacy subcommand-shaped intent during migration.

The root skill should not be the only place where users can discover all supported actions.

### Natural-language compatibility

First-class skills must not remove HAIL's ability to understand clear natural-language intent.

For example, a user saying:

```text
Change my default so you stop waiting between steps.
```

should not be rejected merely because they did not explicitly invoke a `change` skill.

Skill discovery improves affordance; it does not turn HAIL into a rigid command parser.

## Shared packaged runtime contracts

The management skills consume shared runtime profile contracts bundled with the root HAIL skill rather than independently re-declaring schema/default/migration behavior:

```text
hail/
  SKILL.md
  references/
    profile-schema.md
    default-profile.yaml
    profile-normalization.md
```

Repository-level semantic authority remains `spec/semantics.md`. The bundled references are the installed runtime representation and must mirror that semantic contract before release.

The bundled runtime split means:

- `show` reads schema + normalization and remains non-mutating;
- `setup` initializes from the bundled default profile;
- `change` normalizes first, applies explicit user intent, validates, and persists only the intended semantic change plus supported normalization;
- `reset` reads defaults from the bundled default profile instead of duplicating default values in the action skill;
- harness-specific projection mappings remain harness-local rather than being treated as vendor-neutral semantic data.

## Skill lifecycle

A new HAIL action should become a first-class skill when all of the following are true:

1. it represents a coherent user intent rather than an internal implementation step;
2. users benefit from discovering that action independently;
3. its behavior has a stable enough contract to describe accurately;
4. the target harness supports an appropriate skill/discovery mechanism.

Do not create separate skills for tiny implementation variants that would clutter discovery without giving the user a meaningful new capability.

## Naming rules

Skill names should:

- use short user-intent language;
- be understandable without reading repository architecture;
- avoid exposing implementation details such as compiler, adapter, projection, YAML mutation, or file paths;
- remain semantically similar across harnesses where practical, even when invocation syntax differs;
- include a HAIL namespace/prefix where the harness does not provide one and collision risk would otherwise be high.

Preferred conceptual action names remain:

```text
hail
show
setup
change
reset
```

`review` should be added only when that capability is implemented sufficiently to expose it truthfully.

## Harness-native invocation

HAIL defines capability names and intent, not universal invocation syntax.

Claude's plugin namespace naturally produces `/hail:<skill>` commands. Codex's global skill surface uses HAIL-prefixed action names.

Do not introduce a shared runtime or command router solely to make invocation syntax identical across harnesses.

## Persistence boundary

Discoverable skills do not change HAIL's existing persistence rule.

```text
ordinary conversational adaptation
→ may affect the current interaction
→ must not silently mutate the persistent profile

explicit HAIL configuration intent
→ may persist supported profile changes
```

The `change`, `setup`, and `reset` skills are explicit HAIL configuration surfaces.

The `show` skill is read-only.

The root `hail` skill should preserve the same distinction when routing natural-language requests.

Future temporary-interaction-state work may refine this boundary. Any such change must update the repository semantic contract and the bundled runtime normalization/persistence instructions together rather than leaving installed skills with stale persistence rules.

## Shared behavior and duplication boundary

First-class skills should preserve the same canonical profile schema, validation rules, persistence boundary, normalization behavior, and harness projection semantics as the root skill.

Shared schema/default/normalization behavior belongs in bundled runtime references rather than being copied into each action skill. Harness-specific operational projection behavior may remain harness-local where that is the simplest reliable execution contract.

Do not introduce a shared runtime merely to remove textual duplication.

## Migration from the original single-skill model

The original single HAIL skill remains as the compatibility starting point rather than being deleted.

Migration should preserve existing successful user intents while making them independently discoverable.

At minimum:

- existing `show` behavior remains available through the new show skill;
- existing `setup` behavior remains available through the new setup skill;
- existing `change` behavior remains available through the new change skill;
- existing `reset` behavior remains available through the new reset skill;
- the root HAIL skill continues to understand or route equivalent natural-language/subcommand intent during migration.

Do not break persistent profiles or require profile migration merely because the interaction surface is reorganized.

## Boundaries and non-goals

This capability does **not**:

- add a new field to the HAIL semantic profile;
- change the meaning of `verbosity`, `decision_mode`, `max_options`, `task_chunking`, `step_pacing`, or `tangent_policy`;
- require identical command syntax across harnesses;
- require a runtime, MCP server, daemon, cloud service, or central command registry;
- make every internal helper a user-facing skill;
- make unimplemented/speculative capabilities appear discoverable as though they already work;
- replace natural-language configuration with mandatory command syntax.

## Relationship to other capabilities

### Persistent profile management

`show`, `setup`, `change`, and `reset` are discoverable surfaces over the already validated persistent profile-management capability. The skill split reorganizes access to behavior; it does not redefine persistence semantics.

### Review capability

[`conversation-review.md`](conversation-review.md) defines the behavioral contract for reviewing the human–AI interaction itself. It is a natural candidate for a separate discoverable skill because it represents a distinct user intent. It should only be exposed after that contract has an implementation and validation evidence sufficient to advertise truthfully.

### Future capabilities

Future user-facing HAIL capabilities should explicitly decide whether they warrant their own discoverable skill. New functionality should not automatically become another hidden word interpreted by the root skill.

## Validation evidence

A dedicated deterministic skill-surface runner now exists in the private `yrwol/hail-testing` repository. It is intentionally separate from HAIL's single-turn semantic behavior eval workflow.

The runner validates the actual installed management surfaces for Claude and Codex using fresh invocations and then inspects persistent filesystem state. It checks:

- expected action skills and bundled root resources exist;
- `review` is not packaged as an implemented HAIL skill;
- the harness-native action names are callable;
- a legacy profile missing `step_pacing` is interpreted as `continuous`;
- `show` does not mutate the legacy profile;
- `setup` produces the bundled default profile and preserves unrelated harness configuration;
- `change` applies the requested delta, preserves untouched semantic values, and persists supported normalization;
- `reset` reads bundled defaults and preserves unrelated semantic values;
- Claude projection/import integrity is preserved;
- Codex managed-block integrity is preserved;
- both harnesses preserve unrelated user-authored configuration.

During the first run, the suite exposed a YAML portability issue: plain `task_chunking: off` may be interpreted as boolean `false` by YAML 1.1-style parsers. HAIL now preserves existing unquoted profiles as semantic `off`, keeps read-only access non-mutating, and serializes HAIL-owned writes as `task_chunking: "off"`.

Validation runs:

- `yrwol/hail-testing` run `33141790176` — complete Claude + Codex pass against the YAML serialization repair branch;
- `yrwol/hail-testing` run `33142056465` — complete Claude + Codex pass against merged HAIL `main`.

The second run is the authoritative current release evidence for the deterministic skill surface.

### What this evidence does not prove

The skill-surface runner does not validate:

- visual autocomplete/search rendering in Claude or Codex;
- whether a user naturally notices the skill in a harness UI;
- multi-turn conversational persistence or wait/resume behavior;
- tangent-return composition across multiple turns.

Those require separate observation/methodology. In particular, the existing single-prompt semantic runner must not be used to claim multi-turn behavior.

## Implementation completion criteria

For deterministic management behavior, the implemented Claude and Codex skill surfaces now satisfy the tested criteria:

- supported management actions exist as first-class harness skills;
- each skill has a clear user-facing name and description;
- the action skills preserve validated profile-management semantics in the tested scenarios;
- persistent configuration behavior is regression-tested;
- README/integration docs describe the actual skill surface;
- unsupported `review` skills are not packaged;
- bundled schema/default/normalization contracts are installed and exercised;
- equivalent canonical profile outcomes were observed in Claude and Codex.

The capability remains **experimental rather than complete** until the actual harness discovery/autocomplete presentation is manually verified in each harness and any remaining root-skill migration compatibility checks considered necessary for release are recorded.

## Documentation impact

Current implementation documentation includes:

- root `README.md`;
- `integrations/claude/README.md`;
- `integrations/codex/README.md`;
- `spec/product.md`;
- `spec/roadmap.md`;
- this capability contract.

The private testing repository documents the deterministic skill-surface methodology separately from the semantic eval methodology.
