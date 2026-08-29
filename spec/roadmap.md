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

### Cross-harness `max_options × decision_mode × ambiguity` hardening

Status: **complete for the tested single-turn scope / promoted**

The cross-harness composition replay exposed and repaired several interaction failures:

- `options` treated explicit comparisons as permission to recommend;
- `choose_by_default` was not reliably distinct from recommendation-first behavior;
- open-ended brainstorming bypassed `max_options: 3` in both harnesses;
- hybrids/syntheses could behave as hidden extra choices;
- Codex invented consequential monetization assumptions across all decision modes;
- Claude `recommend_first` could ask for necessary context and still append a fallback recommendation.

Candidate D repaired those failures while preserving:

- explicit current-request count overrides;
- ordinary informational lists outside the option cap; and
- recognizable decision ownership across `options`, `recommend_first`, and `choose_by_default`.

Claude showed some run-to-run ambiguity variance on the informational control, but repeat runs did not reproduce a stable regression.

Evidence:

- [`../evals/prompt-hardening/results/decision-max-options-ambiguity-cross-harness.md`](../evals/prompt-hardening/results/decision-max-options-ambiguity-cross-harness.md)
- [`../evals/prompt-hardening/max-options-decision-mode-composition.md`](../evals/prompt-hardening/max-options-decision-mode-composition.md)

### Cross-harness `verbosity × max_options` composition

Status: **complete for the tested single-turn scope / Candidate B selected**

The composition replay established that verbosity and simultaneous choice load remain semantically independent under the recorded Claude/Codex conditions:

- compact responses retained enough tradeoff information to distinguish choices;
- detailed responses could deepen explanation without requiring more choices;
- an initial Claude balanced/detailed inversion did not reproduce on repeat and did not justify a verbosity wording change;
- ordinary informational lists remained outside the option cap;
- explicit current requests overrode persistent choice-count/detail defaults as intended.

The experiment also exposed a prompt-only enforcement ceiling for `max_options`: both harnesses, especially Claude, can still append a distinct helpful hybrid/synthesis after the configured cap. Candidate B materially improved compliance and preserved semantic boundaries; stronger Candidate A/C formulations did not outperform it.

Evidence:

- [`../evals/prompt-hardening/results/verbosity-max-options-cross-harness.md`](../evals/prompt-hardening/results/verbosity-max-options-cross-harness.md)
- [`../evals/prompt-hardening/verbosity-max-options-composition.md`](../evals/prompt-hardening/verbosity-max-options-composition.md)
### Cross-harness `verbosity × decision_mode` composition

Status: **complete for the tested single-turn scope / Claude Candidate A promoted**

The final planned single-turn composition established that explanatory depth and decision ownership remain independently meaningful under the recorded Claude/Codex conditions.

Key findings:

- `options` remains neutral overall across verbosity; one compact Codex recommendation drift did not reproduce on two repeats;
- `recommend_first` remains explicit and early at compact, balanced, and detailed;
- Codex `choose_by_default` already adopted a working decision with existing wording;
- Claude `choose_by_default` collapsed into recommendation-style “best fit” language at all three verbosity levels;
- a Claude-specific projection requiring explicit working-default adoption repaired the distinction across compact, balanced, and detailed;
- explicit current recommendation requests override persistent `options` for that interaction;
- consequential ambiguity remains governed by the shared material-context boundary.

Evidence:

- [`../evals/prompt-hardening/results/verbosity-decision-mode-cross-harness.md`](../evals/prompt-hardening/results/verbosity-decision-mode-cross-harness.md)
- [`../evals/prompt-hardening/verbosity-decision-mode-composition.md`](../evals/prompt-hardening/verbosity-decision-mode-composition.md)
## Current project checkpoint

### Multi-turn semantic eval support

Status: **next**

The defined single-turn composition backlog is complete. The remaining tracked semantics require preserved conversation state.

The immediate infrastructure question is:

> Can `hail-testing` preserve one harness conversation across a scripted sequence of user turns while capturing the complete interaction record and keeping HAIL profile/projection state stable?

Minimum runner contract:

- start from a fresh harness session;
- apply one recorded HAIL ref/profile before the scenario begins;
- send an ordered sequence of user turns through the same conversation/session;
- preserve assistant context naturally between turns;
- capture every user/assistant turn plus harness/model/effort/profile/ref metadata;
- do not mutate the persistent profile between turns unless the scenario explicitly tests profile management;
- keep Claude and Codex records comparable without requiring identical harness internals;
- retain the existing single-turn runner for one-response semantics.

First semantics unlocked:

1. base `task_chunking` cross-harness replay;
2. [`verbosity × task_chunking`](../evals/prompt-hardening/verbosity-task-chunking-composition.md);
3. [`task_chunking × step_pacing`](../evals/prompt-hardening/task-chunking-step-pacing-composition.md);
4. [`tangent_policy × step_pacing`](../evals/prompt-hardening/tangent-policy-step-pacing-composition.md).

The corrected `verbosity × task_chunking` spec now explicitly requires multi-turn evidence rather than treating single-response headings as authoritative chunking behavior.

## Near-term work

### Build the smallest multi-turn runner extension

Prefer extending private `yrwol/hail-testing`. The first validation should prove a wait/resume sequence that the current one-prompt runner cannot observe.
### Discoverable skills interaction surface

Status: **implemented; deterministic management behavior validated; interactive discovery UI check pending**

The capability contract is defined in [`capabilities/discoverable-skills.md`](capabilities/discoverable-skills.md).

Current evidence supports:

- the root `hail` orientation/routing skill remains packaged for compatibility and conversational HAIL intent;
- `show`, `setup`, `change`, and `reset` exist as first-class Claude plugin skills;
- the same conceptual actions exist in Codex using collision-resistant namespaced skills (`hail-show`, `hail-setup`, `hail-change`, `hail-reset`);
- shared schema/default/normalization contracts are bundled with the root HAIL skill rather than duplicated across action skills;
- `show` normalizes legacy profiles without mutating storage;
- `setup`, `change`, and `reset` produce validated canonical state and preserve unrelated harness configuration;
- Claude and Codex produce equivalent canonical profile outcomes in the tested management scenarios;
- unimplemented `review` skills are not packaged;
- YAML-safe writes quote semantic `task_chunking: "off"` while existing unquoted profiles remain valid and read-only access remains non-mutating.

Dedicated skill-surface evidence:

- `yrwol/hail-testing` run `33141790176` — Claude + Codex pass against the YAML serialization repair branch;
- `yrwol/hail-testing` run `33142056465` — Claude + Codex pass against merged HAIL `main`.

The remaining discoverability question is narrower than behavioral validation:

> Does each harness's actual autocomplete/search UI present the installed HAIL actions clearly enough that a user can discover them without prior command knowledge?

That presentation has not been proven by the deterministic runner and should be checked manually in each harness unless a reliable programmatic discovery interface becomes available.

Implementation does not add a persistent HAIL field or change the semantic meaning of existing preferences.

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

### Conversation review

Status: **specified; implementation pending**

The capability contract is defined in [`capabilities/conversation-review.md`](capabilities/conversation-review.md).

Conversation review asks:

> Given this conversation, what interaction patterns helped or hindered the user, what evidence supports that conclusion, and what is the smallest appropriate response?

The first implementation should stay intentionally narrow: review the current conversation from available transcript context, identify explicit corrections/repeated overrides and positive alignment signals, cluster them into evidence-backed findings, compare relevant findings against the active HAIL profile when available, and distinguish profile mismatch from projection/enforcement failure, situational behavior, unmodeled interaction signals, task/domain failures, or insufficient evidence.

Review must remain read-only with respect to persistent HAIL configuration unless the user separately and explicitly requests a supported profile change. Historical-session adapters, automatic eval persistence, broad history indexing, shared runtime/MCP infrastructure, and automatic preference learning are not prerequisites for the first experiment.

The discoverable-skills capability reserves a future `review` skill, but Claude/Codex must not package or advertise it until this capability is implemented and validated.

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
