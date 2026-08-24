# HAIL Roadmap

This is the authoritative current roadmap. Historical milestone numbers in older documents describe when experiments occurred; they do not override this document.

The roadmap tracks **questions and evidence**, not release versions.

## Completed

### Static semantic profile → Claude

Status: **complete**

Established that a small vendor-neutral semantic profile can produce observable, directional behavior changes in Claude.

Key outputs:

- reference semantic profile;
- Claude adapter/compiler experiment;
- repeatable behavioral scenarios;
- first evidence that enforcement strength differs by preference.

See [`milestone-1-addendum.md`](milestone-1-addendum.md).

### Cross-harness portability → Codex

Status: **complete**

Established that the same semantic profile can retain its intent across Claude and Codex using harness-specific delivery, while enforcement strength differs by preference and harness.

Key result:

> Portability means preservation of semantic intent, not identical output or identical compliance.

See [`milestone-2-addendum.md`](milestone-2-addendum.md).

### Claude native persistent profile management

Status: **complete / manually validated**

Established that a user can intentionally configure persistent HAIL defaults conversationally inside Claude without manually editing YAML or invoking the reference compiler.

Validated behaviors include:

- setup/read/change/reset;
- persistence across `/clear`;
- migration from older generated instructions;
- `step_pacing` as a new evidence-backed semantic field;
- pacing + tangent-policy composition;
- explicit HAIL configuration as the persistence boundary.

Historical working notes: [`milestone-3-working-notes.md`](milestone-3-working-notes.md).

### Codex native persistent profile management

Status: **complete / manually validated**

Established that the same explicit persistent HAIL profile-management experience works in Codex using the shared semantic profile and Codex-native `AGENTS.md` projection.

Validated behaviors include:

- `$hail` inspection of the existing shared profile;
- natural-language persistent change;
- profile change to `task_chunking: always` + `step_pacing: wait_for_user`;
- changed behavior in a fresh Codex interaction;
- pacing + tangent-policy composition;
- ordinary conversational adaptation without persistent mutation;
- `$hail show` confirming the persistent profile remained unchanged by contextual requests; and
- reset to v0.1 defaults.

Cross-harness projection synchronization is **not a shipping requirement**. Users may use only one harness, and the validated product model does not require active synchronization between harness-specific projections. A smoother multi-harness refresh experience may be explored later as an optional enhancement if real usage shows it is valuable.

Historical working notes: [`milestone-4-working-notes.md`](milestone-4-working-notes.md).

### Documentation reconciliation

Status: **complete enough to support active development**

The repository now distinguishes current product truth, validated semantics, broader taxonomy, capability explorations, and historical evidence clearly enough that contributors do not need to reconstruct the project chronologically before working on it.

Documentation should continue to be reconciled when experiments produce meaningful evidence, but documentation cleanup is no longer the active product checkpoint.

### Claude projection hardening — current semantic set

Status: **strong checkpoint reached for current tested semantics**

Claude hardening now has strong recorded evidence for the currently tested semantic preferences:

- `decision_mode` — neighboring values produce recognizable differences in decision ownership;
- `max_options` — projection better limits simultaneous choice-like burden rather than only literal alternatives;
- `task_chunking` — Candidate A repairs the previously inverted/weak differentiation and produces the intended `off < adaptive < always` collaboration structure in the real Claude plugin path;
- `verbosity` — the existing Claude wording already produces recognizable `compact < balanced < detailed` information depth and passes proportionality plus explicit-override checks.

The `task_chunking` Candidate A wording is promoted into the Claude integration. Its remaining trivial-task boundary and `task_chunking × step_pacing` composition checks are follow-up regression/composition work rather than blockers to adopting the repaired projection wording.

Evidence:

- [`../evals/prompt-hardening/results/task-chunking-claude-sonnet.md`](../evals/prompt-hardening/results/task-chunking-claude-sonnet.md)
- [`../evals/prompt-hardening/results/verbosity-claude-sonnet.md`](../evals/prompt-hardening/results/verbosity-claude-sonnet.md)

## Current project checkpoint

### Cross-harness replay and focused composition

Status: **next**

The current question is:

> Do the stable Claude projection candidates preserve the same semantic intent in Codex, and do neighboring semantics remain independently controllable when composed?

The hardening method remains:

```text
semantic intent
→ observable behavioral distinction
→ targeted failure scenario
→ smallest projection-wording change
→ regression / boundary checks
→ composition only after individual behavior is stable
```

Evidence should record the harness, model, effort/reasoning mode, profile values, session conditions, and exact projection wording. A successful result under one configuration is evidence for that configuration, not a universal model/harness claim.

## Near-term work

### Cross-harness replay

Replay the stable Claude candidates in Codex before introducing harness-specific wording unnecessarily.

Priority targets:

1. `task_chunking` — verify the repaired semantic distinction survives in Codex; introduce Codex-specific wording only if evidence requires it.
2. `verbosity` — verify information-depth differentiation, proportionality, and explicit overrides in Codex.
3. preserve previously hardened `decision_mode` and `max_options` behavior while replaying neighboring semantics.

### Focused composition and remaining boundaries

Composition matters, but do not tune multiple semantics simultaneously.

Known next checks include:

- `task_chunking` trivial/single-step boundary;
- `task_chunking` × `step_pacing`;
- `verbosity` × `max_options`;
- `verbosity` × a fixed `task_chunking` value;
- `max_options` × `decision_mode` × ambiguity;
- tangent handling while pacing is paused.

Use composition to verify semantic independence, not to rescue weak individual projections.

## Next product-expansion candidate

### Temporary interaction state

Temporary state remains a strong future product experiment, but it is **not the immediate next step while cross-harness replay and focused composition are still producing useful evidence**.

Smallest useful question:

> Can a temporary interaction state change observable behavior without changing the persistent semantic profile?

The original candidate state was `overloaded`, with expected behavior such as:

- reduce unnecessary choices;
- prioritize one concrete next action;
- avoid nonessential tangents;
- preserve necessary safety/accuracy information.

Do **not** assume the implementation should be a runtime, MCP server, `state.json`, automatic mood inference, or a general override engine. Those are implementation hypotheses to test only if needed.

Relevant unresolved design questions:

- explicit versus inferred state;
- task/session/day scope;
- expiration;
- precedence over persistent preferences;
- inspection/clearing UX;
- behavior across `/clear` or new harness sessions.

## Later candidates

### Runtime / MCP — only if justified

A shared runtime is not automatically the next step after temporary state.

Explore it only if a validated capability cannot be delivered adequately with harness-native mechanisms. Multi-harness projection synchronization alone is not a requirement for shipping HAIL and should not be used as justification for a shared runtime without demonstrated user need.

### Real-user onboarding

Goal:

> Can people produce a useful HAIL profile without understanding the schema?

Potential experiments:

- guided conversational setup;
- starter configurations based on functional needs rather than diagnoses;
- 2–5 test users;
- observation of misunderstood, missing, or redundant dimensions.

### Distribution

Only after the interaction model and management experience are sufficiently stable should the project optimize marketplace/plugin packaging, installers, hosted sync, or broader distribution.

### Multi-harness profile refresh

Status: **parked optional enhancement**

For users who actively use multiple HAIL-enabled harnesses, a future enhancement may reduce stale generated projections after the canonical profile changes elsewhere.

Possible low-complexity direction:

> Refresh the harness-local disposable projection from the canonical profile at an appropriate point of use.

This is an enhancement to multi-harness convenience, not a requirement for semantic portability, single-harness use, milestone completion, or shipping.

Do not introduce a daemon, shared runtime, cloud service, or MCP synchronization layer unless later evidence demonstrates that simpler refresh behavior is insufficient.

## Parked / evidence required

- automatic persistent preference learning;
- diagnosis-based presets;
- cloud accounts/profile synchronization;
- polished settings UI;
- third+ harness expansion solely for coverage;
- universal protocol/standardization work;
- automatic state/mood/neurotype inference;
- autonomy as a new persistent semantic dimension;
- capability manifests as a frozen protocol schema;
- shared runtime/MCP without a concrete failing native experiment;
- automated eval infrastructure beyond the current lightweight HAIL testing harness unless additional complexity is justified by evidence.

## Roadmap discipline

- Prefer one active experimental question at a time.
- New idea does not automatically become the next milestone.
- Prefer behavioral experiments over architecture arguments.
- Define the observable behavioral contract before tuning projection wording.
- Add semantic fields only after concrete scenarios demonstrate a vocabulary gap.
- Treat a projection failure as a projection problem unless evidence demonstrates a semantic gap.
- Preserve historical evidence, but do not allow old milestone numbering to define current priority.
