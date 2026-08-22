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

## Current project checkpoint

**Documentation reconciliation.**

Before selecting the next implementation experiment, ensure the repository clearly distinguishes:

- current product truth;
- current validated semantic model;
- broader interaction taxonomy;
- historical experiment evidence; and
- future/parked ideas.

This checkpoint exists because the original `draft.md` and later milestone addenda accumulated overlapping roadmap and architecture language.

Exit condition:

> A contributor can determine what HAIL is today, what semantics are actually supported, what has been proven, and what is still a candidate without reconstructing chronological history.

## Next candidate experiment

### Temporary interaction state

This remains the strongest next product experiment from the original HAIL model, but it should begin only after the documentation checkpoint is complete and the scope is deliberately chosen.

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
- shared runtime/MCP without a concrete failing native experiment.

## Roadmap discipline

- Prefer one active experimental question at a time.
- New idea does not automatically become the next milestone.
- Prefer behavioral experiments over architecture arguments.
- Add semantic fields only after concrete scenarios demonstrate a vocabulary gap.
- Preserve historical evidence, but do not allow old milestone numbering to define current priority.
