# HAIL Roadmap

This is the authoritative current roadmap. Historical milestone numbers in older documents describe when experiments occurred; they do not override this document.

The roadmap tracks **questions and evidence**, not release versions.

## Completed

### Static semantic profile → Claude

Status: **complete**

Established that a small vendor-neutral semantic profile can produce observable, directional behavior changes in Claude.

See [`milestone-1-addendum.md`](milestone-1-addendum.md).

### Cross-harness portability → Codex

Status: **complete**

Established that the same semantic profile can retain its intent across Claude and Codex using harness-specific delivery, while enforcement strength differs by preference and harness.

> Portability means preservation of semantic intent, not identical output or identical compliance.

See [`milestone-2-addendum.md`](milestone-2-addendum.md).

### Claude native persistent profile management

Status: **complete / manually validated**

Validated setup/read/change/reset, persistence across `/clear`, migration, `step_pacing`, pacing + tangent composition, and explicit HAIL configuration as the persistence boundary.

Historical working notes: [`milestone-3-working-notes.md`](milestone-3-working-notes.md).

### Codex native persistent profile management

Status: **complete / manually validated**

Validated the same explicit persistent HAIL profile-management experience in Codex using the shared semantic profile and Codex-native `AGENTS.md` projection.

Cross-harness projection synchronization is **not a shipping requirement**. A smoother multi-harness refresh experience remains an optional future enhancement if real usage justifies it.

Historical working notes: [`milestone-4-working-notes.md`](milestone-4-working-notes.md).

### Documentation reconciliation

Status: **complete enough to support active development**

The repository distinguishes current product truth, validated semantics, broader taxonomy, capability explorations, and historical evidence. Documentation should continue to be reconciled whenever experiments materially change current evidence or promoted projection behavior.

### Claude projection hardening — current tested semantics

Status: **strong checkpoint reached**

Recorded Claude evidence supports:

- `decision_mode` — recognizable differences in decision ownership;
- `max_options` — limits simultaneous choice-like burden rather than only literal alternatives;
- `task_chunking` — Candidate A repairs inverted/weak differentiation and produces intended `off < adaptive < always` collaboration structure;
- `verbosity` — baseline Claude evidence showed recognizable information-depth differentiation and passed proportionality plus explicit-override checks.

Evidence:

- [`../evals/prompt-hardening/results/task-chunking-claude-sonnet.md`](../evals/prompt-hardening/results/task-chunking-claude-sonnet.md)
- [`../evals/prompt-hardening/results/verbosity-claude-sonnet.md`](../evals/prompt-hardening/results/verbosity-claude-sonnet.md)

### Cross-harness `verbosity: detailed` hardening

Status: **complete for the tested single-turn repair scope / promoted**

Codex replay exposed weak differentiation between `balanced` and the previous `detailed` projection. A stronger candidate defined detail as additional useful explanatory layers rather than additional wording and explicitly preserved proportionality for simple requests.

The candidate was replayed through both Codex and Claude. It materially improved Codex information depth, remained strong in Claude, and preserved the simple-request boundary in both tested configurations. The same strengthened `detailed` wording is promoted in both native integrations.

Evidence:

- [`../evals/prompt-hardening/results/verbosity-cross-harness-detailed.md`](../evals/prompt-hardening/results/verbosity-cross-harness-detailed.md)
- [`../evals/prompt-hardening/verbosity.md`](../evals/prompt-hardening/verbosity.md)

## Current project checkpoint

### Cross-harness `task_chunking` replay and supported boundaries

Status: **next**

The immediate semantic-hardening question is:

> Does the repaired `task_chunking` distinction survive in Codex under single-turn observable scenarios without requiring harness-specific semantic meaning?

The hardening method remains:

```text
semantic intent
→ observable behavioral distinction
→ targeted failure scenario
→ smallest projection-wording change
→ regression / boundary checks
→ cross-harness replay
→ composition only when the evaluation runner can validly observe it
```

Evidence should record harness, model, effort/reasoning mode, profile values, session conditions, and exact projection wording. A successful result under one configuration is evidence for that configuration, not a universal model/harness claim.

The current `hail-testing` workflow supplies one prompt in a fresh session. Treat it as authoritative only for **single-turn observable behavior**. Do not claim multi-turn persistence, follow-up behavior, or wait/resume composition from that runner until explicit multi-turn support exists.

## Near-term work

### `task_chunking` cross-harness replay

Replay the stable Claude Candidate A behavior in Codex before introducing Codex-specific wording unnecessarily.

Single-turn targets include:

- base differentiation among `off`, `adaptive`, and `always` on genuinely multi-step work;
- trivial/single-step boundary behavior where decomposition should not be manufactured;
- preservation of neighboring stable semantics while replaying the field.

### Focused composition and remaining boundaries

Each tracked composition boundary has its own focused experiment definition. Do not bury composition scenarios inside an individual field hardening file.

Single-turn-observable suites:

- [`verbosity × max_options`](../evals/prompt-hardening/verbosity-max-options-composition.md);
- [`verbosity × task_chunking`](../evals/prompt-hardening/verbosity-task-chunking-composition.md);
- [`max_options × decision_mode × ambiguity`](../evals/prompt-hardening/max-options-decision-mode-composition.md).

Multi-turn-dependent suites:

- [`task_chunking × step_pacing`](../evals/prompt-hardening/task-chunking-step-pacing-composition.md);
- [`tangent_policy × step_pacing`](../evals/prompt-hardening/tangent-policy-step-pacing-composition.md).

The multi-turn cases must wait for runner support or use a separate valid manual methodology that preserves conversation state. Composition should verify semantic independence, not rescue weak individual projections.

### Discoverable skills interaction surface

Status: **implementation in progress**

The capability contract is defined in [`capabilities/discoverable-skills.md`](capabilities/discoverable-skills.md), and implementation has started on a dedicated branch.

Current implementation direction:

- preserve the root `hail` orientation/routing skill for compatibility and conversational HAIL intent;
- expose `show`, `setup`, `change`, and `reset` as independently discoverable Claude plugin skills;
- expose the same conceptual actions in Codex using collision-resistant namespaced skills (`hail-show`, `hail-setup`, `hail-change`, `hail-reset`);
- preserve natural-language compatibility and existing persistence boundaries;
- preserve harness-specific projection behavior rather than forcing identical internal mechanics;
- validate discovery and behavioral parity separately in Claude and Codex before marking the capability complete;
- do not advertise speculative skills such as `review` until their own capability is sufficiently implemented.

Implementation does not change the persistent HAIL schema or semantic meanings.

## Next product-expansion candidate

### Temporary interaction state

Temporary state remains a strong future product experiment, but it is **not the immediate next step while current semantic hardening is still producing useful evidence**.

Smallest useful question:

> Can a temporary interaction state change observable behavior without changing the persistent semantic profile?

The original candidate state was `overloaded`, with expected behavior such as reducing unnecessary choices, prioritizing one concrete next action, avoiding nonessential tangents, and preserving necessary safety/accuracy information.

Do **not** assume the implementation should be a runtime, MCP server, `state.json`, automatic mood inference, or a general override engine. Those are implementation hypotheses to test only if needed.

Relevant unresolved design questions include explicit versus inferred state, scope, expiration, precedence, inspection/clearing UX, and behavior across `/clear` or new harness sessions.

**Persistence-contract dependency:** this capability is allowed to challenge or qualify today's rule that ordinary conversation cannot mutate persistent HAIL configuration, but any accepted change to temporary/contextual override or persistence semantics MUST update the authoritative semantic contract and the bundled runtime contracts in the same implementation. At minimum, review and update both harness copies of `skills/hail/references/profile-normalization.md`, plus any affected root/action skill instructions, schema/default resources, tests, and integration docs. Do not ship a temporary-state or contextual-persistence behavior change while leaving the bundled management contracts describing the old boundary.

## Later candidates

### Runtime / MCP — only if justified

Explore a shared runtime only if a validated capability cannot be delivered adequately with harness-native mechanisms. Multi-harness projection synchronization alone is not justification for one.

### Real-user onboarding

Goal:

> Can people produce a useful HAIL profile without understanding the schema?

Potential experiments include guided conversational setup, starter configurations based on functional needs rather than diagnoses, a small number of test users, and observation of misunderstood/missing/redundant dimensions.

### Distribution

Only after the interaction model and management experience are sufficiently stable should the project optimize marketplace/plugin packaging, installers, hosted sync, or broader distribution.

### Multi-harness profile refresh

Status: **parked optional enhancement**

For users who actively use multiple HAIL-enabled harnesses, a future enhancement may reduce stale generated projections after the canonical profile changes elsewhere. Prefer simple refresh-at-use behavior if this becomes valuable; do not introduce a daemon, shared runtime, cloud service, or MCP synchronization layer without demonstrated need.

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
- Match claims to what the evaluation method can actually observe.
- Give every tracked composition boundary its own focused eval definition.
- Preserve historical evidence, but do not allow old milestone numbering to define current priority.
