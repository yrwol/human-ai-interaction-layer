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
9. **No silent diagnosis.** HAIL may learn or apply explicitly stated interaction preferences; inferring medical or neurodevelopmental labels is not part of the product.
10. **Minimum useful complexity.** Do not add runtime, MCP, sync, cloud services, schema dimensions, or other machinery until a concrete experiment requires them.
11. **Zero-plumbing normal-user experience.** Normal users should not need to understand YAML, compilers, instruction files, MCP, or harness configuration to benefit from HAIL.
12. **Harness-native when sufficient.** Native skills/plugins/instruction mechanisms are preferred when they can preserve HAIL semantics and user control without shared runtime infrastructure.

## Current architecture boundary

The current product model is:

```text
Human
  |
  +--> persistent semantic HAIL profile
  |            |
  |            +--> harness-native integration
  |                       |
  |                       +--> harness-specific projection / behavior
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

## Current normal-user configuration model

Persistent configuration should happen through an explicit HAIL entry point inside a harness.

Claude currently demonstrates this through `/hail:hail`; the Codex port uses `$hail`.

After entering HAIL configuration, the interaction should remain natural language. Users should not need to know schema field names or YAML values.

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
- expanding every candidate interaction dimension into the current schema.

## Product test

The core question remains:

> Can a person own one semantic interaction model and receive meaningfully aligned collaboration behavior across different AI harnesses without adapting themselves to each harness's configuration plumbing?

Current experiments have established the static cross-harness portion of that question and a native persistent-management experience in Claude. See [`roadmap.md`](roadmap.md) for current project state.
