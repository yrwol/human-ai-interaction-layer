# HAIL Specification Map

This directory separates **current product truth**, **reference ideas**, **roadmap state**, **focused capability design**, and **historical evidence**.

Start here when you need to understand the project without reconstructing its history.

## Current truth

- [`product.md`](product.md) — what HAIL is, the durable product principles, and the current architecture boundary.
- [`semantics.md`](semantics.md) — the semantic preferences HAIL actually supports and has evidence for today.
- [`interaction-taxonomy.md`](interaction-taxonomy.md) — the larger catalog of interaction dimensions HAIL may eventually support. This is a reference/idea space, not the current schema.
- [`roadmap.md`](roadmap.md) — completed experiments, current checkpoint, next candidate work, and parked areas.

## Focused capability specifications

Capability specs preserve deeper design work for coherent HAIL behavior areas without implying that the capability is implemented, part of the persistent schema, or next on the roadmap.

- [`capabilities/review-guidance.md`](capabilities/review-guidance.md) — review confidence, intelligent skepticism, verification support, review-loop detection, domain-local scaffolding, and cognitive-load reduction while evaluating AI output.

A capability spec may contribute durable product principles or taxonomy concepts while remaining separate from the current semantic model.

## Historical evidence and decisions

Milestone documents record what was learned at a point in time. They are evidence/decision records, not competing current specs.

- [`milestone-1-addendum.md`](milestone-1-addendum.md) — first-harness findings and zero-plumbing direction.
- [`milestone-2-addendum.md`](milestone-2-addendum.md) — cross-harness portability and preference-level compatibility findings.
- [`milestone-3-working-notes.md`](milestone-3-working-notes.md) — Claude native persistent profile-management evidence. Despite the historical filename, the core experiment is complete.
- [`milestone-4-working-notes.md`](milestone-4-working-notes.md) — Codex native persistent profile-management evidence. The native management flow is complete and manually validated; cross-harness projection refresh remains a separate unresolved question.

Behavioral milestone transcripts and summaries are preserved under [`../evals/results/`](../evals/results/). Superseded lightweight eval fixtures and the original A/B procedure are under [`../evals/historical/`](../evals/historical/).

## Legacy source documents

[`draft/draft.md`](draft/draft.md) is the original working spec and build guide. It remains valuable historical context, especially for the original problem framing, interaction taxonomy, and early architecture hypotheses. Where it conflicts with `product.md`, `semantics.md`, or `roadmap.md`, the newer focused documents are authoritative.

[`draft/draft-guidance.md`](draft/draft-guidance.md) is the superseded source from which the canonical [`capabilities/review-guidance.md`](capabilities/review-guidance.md) capability specification was derived. Keep it as historical source material rather than treating it as current product guidance.

## Documentation rule

Keep these roles distinct:

```text
CURRENT TRUTH       product.md + semantics.md
REFERENCE IDEAS     interaction-taxonomy.md
CURRENT ROADMAP     roadmap.md
CAPABILITY SPECS    capabilities/*
HISTORICAL EVIDENCE milestone/addendum documents + evals/results
LEGACY CONTEXT      draft/* and superseded source docs
```

Do not turn one document back into a giant catch-all spec.
