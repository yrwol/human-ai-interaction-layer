# Adding HAIL profile fields

This guide defines the required development path for introducing a new configurable behavior to a HAIL profile. It intentionally mirrors the way existing fields are being defined, evaluated, hardened, and documented so future contributors and AI agents extend HAIL in a comparable, evidence-driven structure.

HAIL evaluates **behavior, not configuration**. A new YAML property is not a supported HAIL capability merely because the schema accepts it.

## Terminology

A **profile field** is a named, independently configurable dimension of human-AI interaction behavior represented in a HAIL profile.

Examples include `verbosity`, `task_chunking`, `step_pacing`, `decision_mode`, and `tangent_policy`.

- **Field** — the behavioral dimension being configured, such as `task_chunking`.
- **Value** — one supported behavior for that field, such as `adaptive`.
- **Behavior** — the observable interaction change the field/value is intended to produce.
- **Capability document** — the durable behavioral contract for one profile field, stored in `spec/capabilities/`.
- **Skill** — a HAIL capability or user-invoked operation. Skills are not profile fields.

## Required field-addition artifacts

Every new profile field must produce the following artifacts as part of the same development path:

1. `spec/capabilities/<field-name>.md` — the field's behavioral contract, created from [`spec/capabilities/template.md`](../spec/capabilities/template.md).
2. `evals/<field_name>.yaml` — a focused baseline/contrast evaluation, created from [`evals/templates/profile-field.yaml`](templates/profile-field.yaml).
3. hardening coverage under `evals/prompt-hardening/` when baseline behavior is established, using [`evals/templates/profile-field-hardening.md`](templates/profile-field-hardening.md) and the existing hardening conventions.
4. interaction/composition evaluations for meaningful overlapping or conflicting fields.
5. updates to the relevant durable documentation, including semantics, examples/schema, roadmap/status, and affected adapter or user-facing documentation.

The templates are starting structures. They do not remove the requirement to reason about the behavior or provide evidence.

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

## 2. Create the capability document first

**A new profile field requires a capability document.** Do not begin by adding the field to the schema.

Copy `spec/capabilities/template.md` to `spec/capabilities/<field-name>.md` and define:

- purpose and user need;
- supported values and their semantics;
- expected observable contrast between values;
- boundaries and non-goals;
- interactions with existing fields;
- semantic representation and default implications;
- prompt/compiler intent;
- harness compatibility considerations;
- evaluation plan; and
- evidence/status as work progresses.

The capability document is the durable behavioral contract. Evaluation files demonstrate whether implementations satisfy it.

For example, `task_chunking` may control how much work is grouped into one interaction step, while `step_pacing` controls whether execution continues or pauses between steps. Those behaviors can interact, but they are not interchangeable.

## 3. Review interactions with existing fields

Before implementation, identify existing fields that can influence the same observable response.

Classify meaningful interactions as:

- **independent** — the fields affect different behavior;
- **complementary** — the fields affect different parts of the same interaction and should work together;
- **overlapping** — the fields can produce similar observable effects and require explicit boundaries; or
- **conflicting** — some value combinations can request incompatible behavior and require precedence or resolution semantics.

Record this review in the capability document. Overlapping and conflicting relationships require explicit composition semantics and evaluation coverage. Do not wait for hardening to discover accidental overrides.

## 4. Add baseline evaluation before broad implementation

Create `evals/<field_name>.yaml` from `evals/templates/profile-field.yaml`.

Keep the first evaluation deliberately small. It should answer the most basic validity question:

> Does changing this semantic field produce the observable directional difference described by its capability contract?

Follow the A/B discipline in [`manual-testing.md`](manual-testing.md): use the same task across values whenever possible and change only the field under evaluation.

Baseline evaluation should establish:

- positive behavior for each meaningful value;
- contrast between values;
- at least the most important boundary/non-goal; and
- known interactions that will require later composition coverage.

Do not grade prose quality or exact wording. Evaluate whether behavior moved in the requested direction.

## 5. Implement the field

Once the contract and evaluation target are clear, update every layer required to make that behavior portable and usable.

Depending on the field, this can include:

- profile schema and validation;
- supported values;
- default behavior;
- semantic profile examples;
- prompt/compiler behavior;
- harness adapters;
- profile configuration or editing flows; and
- compatibility handling for unsupported values or harnesses.

Harness-specific prompt wording belongs in the adapter. Do not mutate the semantic meaning of the field to improve results in one harness.

## 6. Evaluate boundaries and composition

Baseline success is not enough when another field can influence the same response.

Add evaluation for:

### Boundary cases

Test situations where the field should **not** alter behavior. These protect the field's non-goals and prevent semantic expansion over time.

### Interaction cases

Test meaningful combinations identified in the capability document, especially overlapping and conflicting fields.

For example, `task_chunking × step_pacing` requires composition coverage because both affect how a multi-step task unfolds even though they own different dimensions.

An interaction test should make it possible to tell whether both behavioral contracts remain intact rather than simply whether the final response looks acceptable.

## 7. Harden the behavior

After the basic directional behavior is demonstrated, create hardening coverage using `evals/templates/profile-field-hardening.md` together with the conventions in `evals/prompt-hardening/template.md`.

Hardening should ask:

- Does the field still work when the user's request strongly invites the opposite behavior?
- Does it remain distinct from neighboring fields?
- Do important field combinations preserve both behavioral contracts?
- Does behavior remain directionally consistent across supported harnesses?

A field that works only for its easiest demonstration prompt is not robust.

## 8. Update relevant documentation

Adding a field changes more than its capability document. Review and update every durable source that should know the field exists.

Normally this includes:

- `spec/semantics.md` when the field becomes part of validated current semantics;
- schema/reference implementation and `profiles/example.yaml` as applicable;
- `spec/roadmap.md` when evidence or project status changes;
- relevant integration/adapter documentation;
- user-facing configuration/profile documentation; and
- the capability document's evidence/status section as evaluation progresses.

Do not promote an experimental field into `spec/semantics.md` merely because implementation exists. Promotion should follow evidence.

## Definition of done

**Schema support is not HAIL support.**

A profile field is considered supported only when:

- [ ] `spec/capabilities/<field-name>.md` exists and describes the behavioral contract;
- [ ] each supported value has explicit semantics;
- [ ] boundaries and non-goals are documented;
- [ ] interactions with existing fields have been reviewed;
- [ ] `evals/<field_name>.yaml` provides focused baseline/contrast coverage;
- [ ] required schema/compiler/adapter behavior is implemented;
- [ ] meaningful value contrasts have behavioral evidence;
- [ ] boundary cases are covered where applicable;
- [ ] meaningful field interactions are evaluated;
- [ ] hardening coverage exists for known weak points;
- [ ] relevant harness evidence and limitations are recorded;
- [ ] durable semantic/example/integration documentation is updated as appropriate; and
- [ ] roadmap or implementation status is updated when the evidence changes project state.

## Governing principle

> **HAIL evaluates behavior, not configuration.**
>
> A profile field exists to produce a meaningful and observable difference in human-AI interaction. Adding configuration without demonstrating that behavioral difference does not extend HAIL; it only extends its schema.
