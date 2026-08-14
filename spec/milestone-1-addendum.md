# Milestone 1 Spec Addendum

This addendum records changes in understanding that were validated during Milestone 1. It supplements `spec/draft.md`; it does not replace the broader draft.

## Validated

**[VALIDATED]** A small vendor-neutral semantic profile can produce observable, directional behavior changes in an AI harness.

**[VALIDATED]** Preference support is not binary. Different semantic dimensions may have different enforcement quality within the same harness.

**[OBSERVED]** In the first Claude experiment, qualitative behaviors such as recommendation style, task chunking, and tangent handling were followed more reliably than a hard numerical `max_options` constraint.

## Implementation role

**[DECIDED]** The current .NET implementation is a reference compiler and test harness. HAIL does not require normal users to install or run a standalone executable.

**[PRINCIPLE] Zero-plumbing user experience.** Normal HAIL users should not need to understand or directly operate YAML, instruction files, compilers, runtimes, MCP, or harness configuration. Where a harness can provide profile configuration and application natively, HAIL should prefer that experience.

**[PRINCIPLE] Harness-native when sufficient.** A harness-native skill, plugin, instruction system, or equivalent may implement HAIL directly when it can preserve the semantic profile and expected behavior. A shared runtime is optional, not assumed.

**[PARKED]** Whether HAIL ever requires a local runtime, MCP server, or standalone executable remains evidence-driven.

## Milestone ordering clarification

Temporary interaction state remains a later milestone after portability. The initial portability proof does not require an `overloaded` state even though the broader MVP section of the draft currently lists one. The build sequence is authoritative for current execution:

1. Static semantic profile → first harness.
2. Same semantics → second harness.
3. Temporary state overlays.
4. Runtime/MCP only if justified by a concrete capability gap.

## Next validation

Milestone 2 should reuse the same semantic profiles and behavioral scenarios in a meaningfully different second harness. The goal is to determine whether the same intent can be delivered through a different harness mechanism, not to increase profile complexity first.
