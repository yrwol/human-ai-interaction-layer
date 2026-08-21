# HAIL Specification Map

This directory separates **current product truth**, **reference ideas**, **roadmap state**, and **historical evidence**.

Start here when you need to understand the project without reconstructing its history.

## Current truth

- [`product.md`](product.md) — what HAIL is, the durable product principles, and the current architecture boundary.
- [`semantics.md`](semantics.md) — the semantic preferences HAIL actually supports and has evidence for today.
- [`interaction-taxonomy.md`](interaction-taxonomy.md) — the larger catalog of interaction dimensions HAIL may eventually support. This is a reference/idea space, not the current schema.
- [`roadmap.md`](roadmap.md) — completed experiments, deferred validation, next candidate work, and parked areas.

## Historical evidence and decisions

Milestone documents record what was learned at a point in time. They are evidence/decision records, not competing current specs.

- [`milestone-1-addendum.md`](milestone-1-addendum.md) — first-harness findings and zero-plumbing direction.
- [`milestone-2-addendum.md`](milestone-2-addendum.md) — cross-harness portability and preference-level compatibility findings.
- [`milestone-3-working-notes.md`](milestone-3-working-notes.md) — Claude native persistent profile-management evidence. Despite the historical filename, the core experiment is complete.
- [`milestone-4-working-notes.md`](milestone-4-working-notes.md) — Codex native profile-management port. Implementation exists; explicit manual behavioral validation remains deferred.

## Other specification work

`draft-guidance.md` is a separate HAIL feature/specification about review confidence, skepticism, verification loops, and cognitive-load reduction. It is not the project roadmap or source of truth for the core semantic profile.

## Legacy draft

[`draft.md`](draft.md) is the original working spec and build guide. It remains valuable historical context, especially for the original problem framing, interaction taxonomy, and early architecture hypotheses. Where it conflicts with `product.md`, `semantics.md`, or `roadmap.md`, the newer focused documents are authoritative.

## Documentation rule

Keep these roles distinct:

```text
CURRENT TRUTH      product.md + semantics.md
REFERENCE IDEAS    interaction-taxonomy.md
CURRENT ROADMAP    roadmap.md
HISTORICAL EVIDENCE milestone/addendum documents
FEATURE SPECS      focused documents such as draft-guidance.md
```

Do not turn one document back into a giant catch-all spec.
