# `<field_name>` prompt hardening

> Start from the existing hardening conventions in `evals/prompt-hardening/`. This template supplements `evals/prompt-hardening/template.md` with the requirements for a newly introduced profile field.

## Capability contract

Link: `../../spec/capabilities/<field-name>.md`

## Field semantics under test

- `<value_a>` — `<behavior>`
- `<value_b>` — `<behavior>`

## Baseline evidence

Link the baseline eval and summarize the directional behavior already demonstrated before hardening.

## Pressure cases

### Case 1 — explicit pressure toward opposite behavior

**Profile value:** `<value>`

**Prompt:**

```text
<prompt>
```

**Pass condition:** `<observable directional behavior>`

**Failure signal:** `<semantic failure, not wording preference>`

### Case 2 — boundary protection

**Profile value:** `<value>`

**Prompt:**

```text
<prompt>
```

**Pass condition:** `<field remains inside its defined scope>`

**Failure signal:** `<field changes neighboring behavior it does not own>`

## Interaction coverage

For each overlapping/conflicting interaction identified in the capability document, add or link a composition evaluation.

| Other field | Relationship | Eval | Expected result |
| --- | --- | --- | --- |
| `<other_field>` | `<relationship>` | `<path>` | `<composition contract>` |

## Cross-harness evidence

Record directional adherence by harness. Harness-specific wording may vary; field semantics may not.

## Result

- **Status:** pass | partial | fail
- **Known weak points:** `<...>`
- **Follow-up:** `<...>`
