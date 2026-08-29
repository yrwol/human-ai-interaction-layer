# HAIL Product Specification

## Product statement

HAIL (Human-AI Interaction Layer) is a portable, human-controlled interaction-accessibility layer that lets people define **how AI should collaborate with them**, then applies those preferences through the mechanisms native to the AI harness they are using.

Neurodivergent users are a motivating use case, but diagnosis is not the configuration model. HAIL models functional interaction needs.

## Problem

AI products ship with product-specific defaults and personalization mechanisms. Users who need different levels of structure, decision support, task chunking, pacing, tangent handling, detail, or other interaction support must repeatedly re-teach each tool how to work with them.

HAIL gives the user a portable semantic description of those needs while allowing each harness to deliver them differently.

## Durable product principles

1. **Needs over diagnoses.** Store functional interaction preferences, not rules such as `ADHD = do X`.
2. **Human-controlled.** Persistent preferences belong to the user and should be inspectable, editable, exportable, disableable, and deletable.
3. **Portable semantics, harness-specific delivery.** Human intent stays vendor-neutral. Harness implementations own native delivery details.
4. **Profile is not state.** Persistent defaults and temporary/contextual interaction state are separate concepts.
5. **Explicit persistence.** Ordinary conversation must not silently rewrite persistent HAIL preferences. Persistent mutation requires explicit HAIL configuration intent.
6. **Adapt the AI, not every downstream tool.** Cognitive/communication preferences belong primarily on the human-to-AI interaction path and should not be indiscriminately forwarded to external tools.
7. **Progressive compatibility.** A harness may support some semantic preferences more strongly than others; HAIL should expose limitations rather than flattening compatibility into yes/no.
8. **Observable behavior.** Semantic preferences should map to behavior that can be evaluated.
9. **Collaboration mechanics over output cosmetics.** Persistent semantics should describe meaningful interaction behavior — such as who owns a decision, how much simultaneous choice burden is presented, or how work is chunked — rather than merely prescribing superficial tone or formatting.
10. **No silent diagnosis.** HAIL may learn or apply explicitly stated interaction preferences; inferring medical or neurodevelopmental labels is not part of the product.
11. **Minimum useful complexity.** Do not add runtime, MCP, sync, cloud services, schema dimensions, or other machinery until a concrete experiment requires them.
12. **Zero-plumbing normal-user experience.** Normal users should not need to understand YAML, compilers, instruction files, MCP, or harness configuration to benefit from HAIL.
13. **Harness-native when sufficient.** Native skills/plugins/instruction mechanisms are preferred when they can preserve HAIL semantics and user control without shared runtime infrastructure.
14. **Optimize understanding, not merely usage.** HAIL should reduce unnecessary cognitive effort while helping users understand assumptions, uncertainty, tradeoffs, and what evidence would actually change a conclusion. Fewer interactions are not inherently better if additional interaction prevents misunderstanding.
15. **Context over global labels.** Expertise, confidence, cognitive load, and support needs can vary by task and domain. HAIL should avoid turning local interaction signals into permanent judgments such as globally classifying a user as beginner, expert, or low-confidence.
16. **Support evaluation without replacing judgment.** HAIL should reduce the effort required to evaluate AI output, not reduce the user's responsibility to evaluate it. When useful, it should help identify what matters, what can be verified, what remains uncertain, and when outside evidence or expertise is needed.
17. **Discoverable user actions.** When a harness supports skill discovery, meaningful HAIL actions should be independently discoverable rather than requiring prior knowledge of hidden subcommand vocabulary.

## Current architecture boundary

The current product model is:

```text
Human
  |
  +--> explicit HAIL management surface
  |            |
  |            +--> persistent semantic HAIL profile
  |                         |
  |                         +--> harness-native projection / behavior
  |
  +--> ordinary contextual instructions
               |
               +--> current interaction only
```

The `.NET` implementation under `reference/dotnet/` is reference/conformance tooling. It is not the intended normal-user runtime or product architecture.

A shared runtime remains optional and must be justified by a capability that harness-native mechanisms cannot provide adequately.

## Semantic ownership

The user-owned semantic profile is the source of truth for persistent interaction defaults.

Generated harness instructions are disposable projections. A weak harness implementation does not justify changing the user's semantic intent.

Recent prompt-hardening work reinforces an important architecture rule:

```text
semantic meaning
!= projection wording
```

A harness may need more explicit operational wording to recognize or enforce a semantic correctly. That is a projection problem unless evidence shows the human-facing semantic itself is incomplete.

## Persistent vs contextual behavior

A critical current boundary is:

```text
explicit HAIL configuration
→ persistent profile change

ordinary task conversation
→ contextual behavior
→ persistent profile unchanged
```

For example, `stop waiting on me` during a task may reasonably mean “continue for this task.” It must not automatically redefine the user's default pacing forever.

Temporary/session/task-specific state remains a distinct future design area.

## Configuration and discovery model

Persistent configuration happens through explicit HAIL interaction inside a harness.

HAIL keeps a root management skill for orientation, compatibility, and natural-language routing while exposing supported management actions as first-class discoverable skills where the harness allows it.

The current conceptual surface is:

```text
hail   → orient, inspect, or route conversational HAIL intent
show   → inspect the persistent profile (read-only)
setup  → initialize persistent HAIL configuration
change → intentionally change persistent HAIL defaults
reset  → reset all or part of the persistent profile
```

Invocation syntax is harness-native:

```text
Claude: /hail:hail, /hail:show, /hail:setup, /hail:change, /hail:reset
Codex:  $hail, $hail-show, $hail-setup, $hail-change, $hail-reset
```

The Codex action names carry a HAIL prefix because its skill namespace is effectively global; generic names such as `$show` and `$change` would create avoidable collision risk.

Users should still be able to configure HAIL conversationally. Discoverability adds affordance; it does not turn HAIL into a rigid command parser.

The discoverable-skill structure is currently experimental: the Claude and Codex implementations exist and deterministic management behavior has been validated in both harnesses. The remaining completion question is interactive autocomplete/search presentation, which requires a manual harness-UI check unless a reliable programmatic discovery interface becomes available.

## Evaluation model

HAIL should evaluate semantics as behavioral contracts rather than prompt text quality.

A useful hardening loop is:

```text
semantic intent
→ observable behavioral distinction
→ boundary / counterexample
→ projection wording
→ harness test
→ failure classification
→ smallest justified refinement
```

For each semantic, evidence should distinguish:

- positive behavior — did the requested interaction behavior occur?;
- boundary behavior — did the preference avoid over-applying?; and
- differentiation behavior — are neighboring values behaviorally distinct?

Composition testing should generally follow individual-semantic validation so interaction failures can be attributed cleanly.

Interaction-surface capabilities such as discoverable skills require their own evidence: structure/discovery, behavioral parity with validated flows, compatibility, and harness-specific invocation behavior.

## Compatibility

Compatibility is preference-level and evidence-based.

Useful descriptions include:

- strong observed enforcement;
- moderate observed enforcement;
- weak observed enforcement;
- unsupported capability; and
- untested behavior.

These labels are not yet a frozen protocol schema.

Portability means the same human-owned semantic intent can remain unchanged while harness-specific implementations express it differently. It does not promise deterministic compliance or identical behavior across models/harnesses.

Model, harness, and reasoning/effort mode are part of the evidence context. A successful result under one tested configuration should not silently become a universal compatibility claim.

## Privacy boundary

HAIL interaction preferences primarily belong on the human → AI harness/model path.

```text
Human interaction context
Human -----------------------> Harness / Model

Task/domain context
Tool / Plugin ----------------> Harness / Model
```

A downstream tool such as GitHub, Jira, or Drive should not receive a user's cognitive or communication profile merely because the AI uses that service.

Minimum rule:

> Profile data is not forwarded downstream unless a specific feature genuinely requires it and the user has a clear reason to expect that disclosure.

## Focused capability specifications

Some HAIL behaviors are coherent capability areas rather than current persistent-profile fields.

- [`capabilities/discoverable-skills.md`](capabilities/discoverable-skills.md) defines how meaningful HAIL management actions become first-class discoverable harness skills without changing profile semantics or persistence boundaries.
- [`capabilities/conversation-review.md`](capabilities/conversation-review.md) defines an evidence-based retrospective over the human–AI interaction itself, including collaboration friction/success, profile-vs-projection diagnosis, confidence, and non-mutating recommendations.
- [`capabilities/semantic-context-reflection.md`](capabilities/semantic-context-reflection.md) defines a proposed provider-neutral semantic interpretation layer for converting bounded conversation evidence into structured context proposals while preserving HAIL governance and explicit persistence boundaries.
- [`capabilities/review-guidance.md`](capabilities/review-guidance.md) explores how HAIL can help users evaluate AI output, allocate skepticism, surface material assumptions, recognize verification boundaries, and stop unproductive review loops without telling users simply to trust AI.

Capability specs do not automatically become current semantics. Their implementation and evidence status must be tracked separately.

## Non-goals for the current stage

Do not treat these as required merely because they may be useful later:

- automatic diagnosis or neurotype detection;
- automatic persistent preference learning;
- cloud profile sync;
- hosted accounts;
- polished settings UI;
- marketplace publication;
- universal harness support;
- shared runtime/MCP without demonstrated need;
- passing the interaction profile into downstream services by default;
- expanding every candidate interaction dimension into the current schema;
- automated eval infrastructure merely because manual evaluation exists.

## Product test

The core question remains:

> Can a person own one semantic interaction model and receive meaningfully aligned collaboration behavior across different AI harnesses without adapting themselves to each harness's configuration plumbing?

Current experiments have established semantic portability and native persistent-management flows. The current development phase is strengthening behavioral durability of existing semantics while selectively improving the harness-native management experience where a focused capability has been specified. See [`roadmap.md`](roadmap.md) for current project state.
