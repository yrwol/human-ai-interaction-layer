# Profile field evaluation templates

These templates preserve the development and evaluation shape used for HAIL profile fields so contributors and AI agents do not have to infer the standard from existing files.

When adding a field:

1. create `spec/capabilities/<field-name>.md` from `spec/capabilities/template.md`;
2. create the baseline `evals/<field_name>.yaml` from `profile-field.yaml`;
3. run field-only contrast evaluation using the discipline in `../manual-testing.md`;
4. add boundary and interaction coverage identified by the capability contract;
5. create hardening coverage using `profile-field-hardening.md` and the existing `../prompt-hardening/template.md` conventions; and
6. update semantics, roadmap/status, examples/schema, and relevant adapter/user documentation.

Templates are starting structures, not substitutes for behavioral reasoning. Preserve the governing rule: HAIL evaluates behavior, not configuration.
