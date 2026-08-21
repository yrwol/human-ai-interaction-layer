# Milestone 3 — Native persistent profile management (working notes)

Milestone 3 is validating HAIL configuration inside a harness without requiring users to edit YAML or run the reference compiler.

## Working product decision

**[DECIDED] Persistent profile mutation requires explicit HAIL intent.**

Ordinary conversational instructions are contextual and MUST NOT silently rewrite the user's persistent HAIL profile.

Examples:

- During normal work, `stop waiting on me` is interpreted as a request for the current context/task. It does not persist `step_pacing: continuous`.
- Inside explicit HAIL configuration, `stop waiting between steps by default` may persist a new `step_pacing` value.
- `I'm overwhelmed today and need things dumbed down` raises a potentially useful temporary/session adaptation scenario, but persistence duration, scope, expiry, and precedence are not defined yet and are intentionally parked.

This boundary favors user agency over automatic preference inference.

## Explicit Claude entry point

For the current Claude experiment, persistent management begins through the HAIL skill, for example:

```text
/hail:hail
/hail:hail show
/hail:hail setup
/hail:hail change
/hail:hail reset
```

The interaction after invocation remains natural language. The user does not need to know schema names or YAML values.

Separate implementations for each sub-action are not required yet; the single HAIL skill can interpret the requested operation.

## New semantic dimension validated by testing

The native experiment demonstrated that `task_chunking` alone cannot represent a user's request to receive one small step and then pause.

`step_pacing` therefore captures a separate persistent concern:

- `continuous` — continue through steps naturally;
- `check_in` — check readiness between meaningful steps when useful;
- `wait_for_user` — provide one small actionable step and wait for explicit readiness before continuing.

`task_chunking` describes **how work is divided**. `step_pacing` describes **how progression through those pieces is paced**.

## Native Claude findings so far

**[VALIDATED]** An explicit conversational HAIL interaction can create or update the canonical semantic profile without the user editing YAML.

**[VALIDATED]** The Claude-native skill can regenerate `~/.hail/claude-code.md` and preserve the user-level `CLAUDE.md` import without invoking the .NET reference implementation.

**[VALIDATED]** Persisted preferences survive `/clear` and influence a fresh conversation.

**[VALIDATED]** `step_pacing: wait_for_user` produced one-decision-at-a-time planning behavior in a fresh conversation.

**[VALIDATED]** `step_pacing: wait_for_user` composed successfully with `tangent_policy: capture_and_return`: Claude answered a tangent briefly, returned to the exact pending planning decision, and continued waiting for the user.

**[FIXED]** Migration from the older reference-generated Claude projection initially preserved contradictory old instructions. The native integration now treats `~/.hail/claude-code.md` as HAIL-owned generated output and replaces it completely from the canonical profile.

## Parked: temporary/contextual adaptation

Testing also demonstrated a legitimate distinction between persistent defaults and current-task instructions.

A future milestone may explore overlays such as session-, task-, or day-scoped behavior, but this is not part of the current persistent profile-management milestone.

Questions that must be answered before building temporary overlays include:

- what scopes exist (turn, task, conversation, day, explicit-until-cleared);
- what survives `/clear` or a new session;
- how temporary values override persistent defaults;
- whether HAIL ever infers a temporary state versus requiring explicit user intent;
- how and when an override expires; and
- how the user inspects or clears active overrides.

Do not implement this layer opportunistically inside persistent profile management.

## Current exit condition

Milestone 3 is complete when persistent HAIL configuration is intentionally invoked, human-friendly, mutable, inspectable, and effective across a fresh conversation without requiring the reference compiler.
