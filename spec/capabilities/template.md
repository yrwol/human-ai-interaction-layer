# <Capability name>

> Copy this file when adding a new HAIL profile field. Replace every placeholder and remove guidance that does not apply.

## Status

- **Profile field:** `<field_name>`
- **Status:** proposed | experimental | supported
- **Default:** `<default_value>`
- **Supported values:** `<value_a>`, `<value_b>`

## Purpose

Describe the human-AI interaction problem this capability addresses and why it belongs in the HAIL profile.

## Behavioral contract

### `<value_a>`

Define the observable behavior requested by this value.

### `<value_b>`

Define the observable behavior requested by this value.

## Expected contrast

Using the same user task, explain how behavior should differ when only this field changes between values. Describe direction of behavior rather than preferred answer wording.

## Boundaries and non-goals

Document similar-looking behavior this field does not control. Name neighboring fields where useful.

## Interactions

| Field | Relationship | Expected composition |
| --- | --- | --- |
| `<other_field>` | independent / complementary / overlapping / conflicting | `<expected behavior>` |

For overlapping or conflicting relationships, define precedence or composition semantics explicitly.

## Semantic representation

```yaml
profile:
  <field_name>: <value>
```

Document schema/default implications and any migration or compatibility considerations.

## Prompt/compiler behavior

Describe the semantic instruction the compiler should produce. Keep harness-specific wording out of this capability contract.

## Harness behavior and compatibility

Document supported harnesses, known limitations, and any adapter-specific considerations without changing field semantics.

## Evaluation plan

- **Baseline/positive:** `<what proves each value works>`
- **Contrast:** `<same prompt, field-only change>`
- **Boundary:** `<where this field should not affect behavior>`
- **Interaction:** `<important combinations with other fields>`
- **Hardening:** `<pressure cases / weak points>`

Link the corresponding files under `evals/` as they are added.

## Evidence and status

Summarize what evaluation has demonstrated, what remains uncertain, and why the current status is justified. Link raw or summarized results rather than copying them here.

## Documentation impact

List other documents that must be updated for this capability, normally including `spec/semantics.md`, examples/schema references, `spec/roadmap.md`, and relevant adapter or user-facing documentation.
