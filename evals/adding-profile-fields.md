# Adding HAIL profile fields

This guide defines the standard for introducing a new configurable behavior to a HAIL profile.

HAIL evaluates **behavior, not configuration**. A new YAML property is not a supported HAIL capability merely because the schema accepts it. A profile field is supported only when its intended behavioral difference is defined, implemented, evaluated, and documented.

## Terminology

A **profile field** is a named, independently configurable dimension of human-AI interaction behavior represented in a HAIL profile.

Examples include `verbosity`, `task_chunking`, `step_pacing`, `decision_mode`, and `tangent_policy`.

- **Field** — the behavioral dimension being configured, such as `task_chunking`.
- **Value** — one supported behavior for that field, such as `adaptive`.
- **Behavior** — the observable interaction change the field/value is intended to produce.
- **Skill** — a HAIL capability or user-invoked operation. Skills are not profile fields.

## 1. Decide whether the behavior belongs in the profile

Before implementation, establish that the candidate represents a meaningful interaction preference rather than simply a useful configuration option.

A profile field should:

- represent a distinct human-AI interaction behavior;
- support preferences that can reasonably differ between users;
- produce observable behavioral differences between its values;
- be evaluable without relying only on subjective answer quality;
- remain meaningful across harnesses rather than encode a harness implementation detail; and
- not substantially duplicate an existing field.

If the behavior cannot yet be distinguished from an existing field, define that boundary before adding configuration.

## 2. Define the behavioral contract first

Every proposed field must have a behavioral contract before it is considered implemented.

The contract should define:

```text
Field: <field_name>

Purpose:
What human-AI interaction behavior does this control?

Values:
What values are supported and what does each mean?

Expected behavioral differences:
What observable change should occur when the value changes?

Interactions:
Which existing profile fields may affect the same behavior?

Non-goals:
What similar-looking behavior does this field explicitly not control?
```

For example, `task_chunking` may control how much work is grouped into one interaction step, while `step_pacing` controls whether execution continues or pauses between steps. Those behaviors can interact, but they are not interchangeable.

The contract exists to prevent semantic drift: a field must continue to mean the same thing across schema definitions, prompt compilation, adapters, tests, and documentation.

## 3. Review interactions with existing fields

Before adding the field, identify existing fields that can influence the same observable response.

Classify meaningful interactions as:

- **independent** — the fields affect different behavior;
- **complementary** — the fields affect different parts of the same interaction and should work together;
- **overlapping** — the fields can produce similar observable effects and require explicit boundaries; or
- **conflicting** — some value combinations can request incompatible behavior and require precedence or resolution semantics.

An interaction review determines where combination tests are required. Do not wait for hardening to discover that two profile fields accidentally override one another.

## 4. Implement the field

Once the behavioral contract is defined, update every layer required to make that behavior portable and usable.

Depending on the field, this can include:

- profile schema and validation;
- supported values;
- default behavior;
- semantic profile examples;
- prompt/compiler behavior;
- harness adapters;
- profile configuration or editing flows; and
- compatibility handling for unsupported values or harnesses.

Harness-specific prompt wording belongs in the adapter. Do not change the semantic meaning of the profile field to improve results in one harness.

## 5. Evaluate behavior

Evaluation is part of implementing a profile field, not follow-up cleanup.

At minimum, evaluation coverage should consider four kinds of cases.

### Positive cases

Demonstrate that each value produces the behavior its contract describes.

### Contrast cases

Use the same task or prompt with different field values and verify an observable directional difference.

Whenever possible, change only the field under evaluation. This preserves the A/B discipline described in [`manual-testing.md`](manual-testing.md).

### Boundary cases

Test situations where the field should **not** alter behavior. These protect the field's non-goals and help prevent semantic expansion over time.

### Interaction cases

Test meaningful combinations identified during the interaction review, especially overlapping and conflicting fields.

For example, `task_chunking × step_pacing` should be evaluated as an interaction because both affect how a multi-step task unfolds even though they control different dimensions.

Evaluation should compare the **direction of behavior**, not whether one answer is subjectively better.

## 6. Harden the behavior

Once the baseline behavioral difference is demonstrated, add hardening coverage for prompts and contexts likely to expose ambiguity, weak adherence, or interaction failures.

Hardening should answer questions such as:

- Does the field still work when the user's request strongly invites the opposite behavior?
- Does it remain distinct from neighboring fields?
- Do important field combinations preserve both behavioral contracts?
- Does behavior remain directionally consistent across supported harnesses?

A field that works only for its easiest demonstration prompt is not yet robust.

## 7. Document the field

Documentation should make the behavioral contract discoverable without requiring contributors to reverse-engineer tests or prompt templates.

Update the relevant project documentation with:

- the field and supported values;
- semantics for each value;
- default behavior;
- important boundaries/non-goals;
- known interactions;
- evaluation coverage;
- harness compatibility or limitations; and
- roadmap/status references when applicable.

## Definition of done

**Schema support is not HAIL support.**

A profile field is considered supported only when:

- [ ] its behavioral purpose is defined;
- [ ] each supported value has explicit semantics;
- [ ] boundaries and non-goals are documented;
- [ ] interactions with existing fields have been reviewed;
- [ ] required schema/compiler/adapter behavior is implemented;
- [ ] baseline behavioral evaluation exists;
- [ ] meaningful value contrasts are evaluated;
- [ ] boundary cases are covered where applicable;
- [ ] meaningful field interactions are evaluated;
- [ ] hardening coverage exists for known weak points;
- [ ] user/contributor documentation is updated; and
- [ ] roadmap or implementation status is updated when applicable.

## Governing principle

> **HAIL evaluates behavior, not configuration.**
>
> A profile field exists to produce a meaningful and observable difference in human-AI interaction. Adding configuration without demonstrating that behavioral difference does not extend HAIL; it only extends its schema.
