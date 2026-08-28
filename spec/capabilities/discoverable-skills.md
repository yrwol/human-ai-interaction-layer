# Discoverable HAIL Skills

## Status

- **Capability type:** harness interaction / command discovery
- **Status:** proposed
- **Profile field:** none
- **Implementation status:** not yet implemented

## Purpose

HAIL currently exposes multiple user actions through one harness entry point and interprets words such as `show`, `setup`, `change`, and `reset` as subcommands or conversational intent inside that single skill.

That works when the user already knows the actions exist, but it creates a discoverability problem: important HAIL capabilities are effectively hidden behind undocumented knowledge of command words.

This capability defines a discoverable skill model where meaningful HAIL actions are represented as first-class harness skills when the harness supports skill discovery.

The goal is:

> A user should be able to discover what HAIL can do from the harness itself without first knowing HAIL's private command vocabulary.

This is an interaction-surface capability, not a new persistent semantic preference.

## User problem

The current shape is conceptually:

```text
HAIL entry point
  └─ internally interprets show / setup / change / reset / review / ...
```

The user must already know which words are meaningful.

The target shape is conceptually:

```text
HAIL
  ├─ hail       inspect / orient / route
  ├─ show       show current persistent HAIL configuration
  ├─ setup      create or initialize a HAIL profile
  ├─ change     intentionally change persistent HAIL configuration
  ├─ reset      restore supported defaults
  ├─ review     evaluate an interaction/conversation when that capability is implemented
  └─ future capabilities become discoverable skills when appropriate
```

Exact invocation syntax remains harness-native. HAIL should not force Claude, Codex, or another harness to imitate another product's command syntax.

## Behavioral contract

### Discoverability

A user browsing or searching the harness's available HAIL skills should be able to identify the supported HAIL actions from names and descriptions alone.

Users should not have to know that a hidden `show`, `change`, or similar subcommand exists before they can find it.

### Single-purpose skills

Each first-class skill should represent one coherent user intent.

Examples:

- **show** — inspect the current persistent profile and effective HAIL configuration;
- **setup** — establish a usable initial profile;
- **change** — intentionally persist one or more preference changes;
- **reset** — restore the supported default profile;
- **review** — perform the separately specified review capability once implemented.

A skill may share implementation helpers with other skills, but its user-facing contract should remain narrow enough to understand from its name and description.

### Root HAIL skill

A root `hail` skill may remain as the general orientation and routing entry point.

It should:

- explain what HAIL is when useful;
- summarize available HAIL actions;
- route ambiguous HAIL configuration intent to the correct behavior;
- remain useful for users who invoke HAIL conversationally rather than selecting a more specific skill.

The root skill should not be the only place where users can discover all supported actions.

### Natural-language compatibility

First-class skills must not remove HAIL's ability to understand clear natural-language intent.

For example, a user saying:

```text
Change my default so you stop waiting between steps.
```

should not be rejected merely because they did not explicitly invoke a `change` skill.

Skill discovery improves affordance; it does not turn HAIL into a rigid command parser.

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
- remain semantically similar across harnesses where practical, even when invocation syntax differs.

Preferred conceptual names for the current management surface:

```text
hail
show
setup
change
reset
```

`review` is reserved for the separately defined review capability and should be added only when that capability is implemented sufficiently to expose it truthfully.

## Harness-native invocation

HAIL defines the capability names and intent, not universal invocation syntax.

Examples may differ by harness:

```text
Claude
→ harness-native plugin/skill invocation

Codex
→ Codex-native skill invocation
```

If a harness namespaces plugin skills, the visible invocation may include the HAIL namespace. That is acceptable as long as the individual skills are discoverable and their descriptions make their purpose clear.

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

## Shared implementation vs. duplicated behavior

First-class skills should not require duplicating profile parsing, validation, persistence, migration, or projection-generation logic.

Preferred implementation shape:

```text
user-facing skills
      ↓
shared HAIL management behavior/helpers
      ↓
canonical ~/.hail/profile.yaml
      ↓
harness-local projection refresh
```

The exact code/file organization may differ by harness. The durable requirement is behavioral consistency, not identical internal architecture.

## Migration from the current single-skill model

The current single HAIL skill should be treated as the compatibility starting point, not deleted blindly.

Migration should preserve existing successful user intents while making them independently discoverable.

At minimum:

- existing `show` behavior remains available through the new `show` skill;
- existing `setup` behavior remains available through the new `setup` skill;
- existing `change` behavior remains available through the new `change` skill;
- existing `reset` behavior remains available through the new `reset` skill;
- the root HAIL skill continues to understand or route equivalent natural-language/subcommand intent during migration unless evidence shows compatibility is unnecessary.

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

The review capability is a natural candidate for a separate discoverable skill because it represents a distinct user intent. It should only be exposed after its own contract and implementation are sufficient.

### Future capabilities

Future user-facing HAIL capabilities should explicitly decide whether they warrant their own discoverable skill. New functionality should not automatically become another hidden word interpreted by the root skill.

## Evaluation plan

### Structure / discovery

For each supported harness:

- install/load the HAIL integration;
- inspect the harness's discoverable skill list/search behavior;
- verify that each implemented HAIL action is independently discoverable by an understandable name/description;
- verify that unsupported/speculative actions are not presented as implemented.

### Behavioral parity

For each migrated management action, compare the new first-class skill against the existing behavior:

- `show` returns the same authoritative persistent configuration;
- `setup` creates a valid supported profile;
- `change` persists intentional supported changes and refreshes the harness-local projection appropriately;
- `reset` restores defaults without damaging unrelated harness configuration.

### Compatibility

Verify that reasonable existing invocation patterns continue to work during migration where compatibility is intended.

The root HAIL skill should still orient and route users rather than responding as though only exact skill invocation is valid.

### Cross-harness semantics

Claude and Codex do not need identical UI syntax. They do need conceptually equivalent discoverable capabilities and persistence outcomes.

## Implementation completion criteria

This capability should not be marked implemented for a harness until:

- supported HAIL actions exist as independently discoverable skills in that harness;
- each skill has a clear user-facing name and description;
- each skill delegates to or preserves the validated management behavior;
- the root HAIL skill remains a useful orientation/routing surface;
- persistent configuration behavior is regression-tested;
- README/integration docs show the actual skill surface rather than legacy hidden subcommands;
- unsupported future skills are not advertised as available.

Cross-harness completion requires evidence for each supported harness separately.

## Documentation impact

When implemented, update at minimum:

- root `README.md`;
- `integrations/claude/README.md` and Claude skill documentation;
- `integrations/codex/README.md` and Codex skill documentation;
- `spec/product.md` if the interaction-surface description needs refinement;
- `spec/roadmap.md` with implementation/evidence status;
- relevant evaluation documentation/results.

The specification map should list this capability while it remains proposed so contributors can distinguish the intended skill architecture from the currently implemented single-skill structure.
