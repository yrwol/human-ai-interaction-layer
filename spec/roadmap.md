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

## Implemented, validation deferred

### Codex native persistent profile management

Status: **implemented; explicit manual behavioral validation deferred**

The `$hail` skill port uses the same canonical semantic profile and Codex-native `AGENTS.md` projection.

Open validation questions include:

- behavior after a fresh Codex interaction;
- change/reset experience;
- preservation of unrelated Codex instructions;
- whether ordinary conversation leaves persistent preferences unchanged; and
- whether cross-harness projection staleness creates meaningful user friction.

Historical working notes: [`milestone-4-working-notes.md`](milestone-4-working-notes.md).

Deferred validation is not the same as a failed experiment. Do not block unrelated conceptual work merely because this manual test has not been run.

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

Explore it only if a validated capability cannot be delivered adequately with harness-native mechanisms—for example, if state must reliably synchronize across harnesses and simpler refresh mechanisms prove insufficient.

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
