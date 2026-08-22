# Profile field evaluation templates

These templates provide reusable artifact structures for HAIL profile-field evaluation. They intentionally do **not** define the development sequence.

The authoritative procedure for adding or changing a profile field is [`../adding-profile-fields.md`](../adding-profile-fields.md). If this directory and that guide ever appear to disagree about process, the guide is authoritative.

## Templates

- [`profile-field.yaml`](profile-field.yaml) — starting structure for focused baseline and value-contrast evaluation.
- [`profile-field-hardening.md`](profile-field-hardening.md) — field-specific hardening structure used alongside the existing [`../prompt-hardening/template.md`](../prompt-hardening/template.md) conventions.

Templates are starting structures, not substitutes for behavioral reasoning or evidence. HAIL evaluates behavior, not configuration.
