# Milestone 5 working notes — contextual overrides

## Hypothesis

Can HAIL support an explicit temporary interaction override for the current task/session without mutating the user's persistent HAIL profile?

The motivating evidence is ordinary language such as:

> Stop waiting on me for this.

A human collaborator would normally interpret that as contextual, not as a permanent preference change.

## First experiment

Keep the persistent profile unchanged and allow an explicit temporary override to alter current behavior only.

Initial scenario:

1. Persistent profile has `step_pacing: wait_for_user`.
2. User explicitly requests a temporary override for the current task: "for this task, stop waiting on me."
3. Harness continues through the current task without waiting between steps.
4. Persistent `~/.hail/profile.yaml` remains unchanged.
5. A fresh interaction returns to `step_pacing: wait_for_user`.

## Scope

This milestone is intentionally narrower than adaptive state.

In scope:

- explicit temporary/contextual overrides;
- clear separation from persistent HAIL configuration;
- precedence between persistent defaults and an active temporary override;
- observable expiration behavior.

Out of scope for the first experiment:

- automatic overwhelm/mood detection;
- inferring temporary state from ordinary conversation without explicit intent;
- diagnosis-based behavior;
- timers or complex expiration policies;
- cross-device/session synchronization;
- MCP/shared runtime unless a native harness experiment proves it is required;
- new persistent profile fields.

## Design constraint

Persistent defaults remain human-owned and unchanged unless the user explicitly enters persistent HAIL configuration.

Conceptually:

```text
persistent HAIL profile
        ↓
explicit temporary override
        ↓
current interaction behavior
```

The temporary layer must not rewrite the base profile.

## Open questions to learn from testing

- What explicit language or command should enter temporary HAIL mode?
- Is task scope sufficient for the first implementation, or is session scope easier/more natural?
- How should the user inspect or clear an active override?
- Does `/clear` or the harness equivalent naturally provide the right expiration boundary?
- Can Claude and Codex implement this natively without shared runtime state?
- Does temporary pacing expose a separate autonomy dimension, or can that remain ordinary harness behavior for now?

## Exit condition

Milestone 5 passes its first gate when one harness can:

1. start from a known persistent HAIL profile;
2. intentionally apply a temporary override;
3. show observable behavior consistent with that override;
4. prove the persistent profile did not change; and
5. return to the persistent behavior after the override expires.

Do not generalize the storage/runtime design until this loop is demonstrated.