# HAIL portability comparison — Claude vs Codex

This document compares the first two HAIL behavioral experiments using the same semantic profiles and evaluation scenarios.

## Cross-harness results

| Preference | Claude | Codex | Portability observation |
|---|---|---|---|
| `decision_mode` | Strong | Weak | Semantic dimension is meaningful, but observed differentiation is currently much stronger in Claude. |
| `max_options` | Weak / partial | Very strong | Codex demonstrates that the numeric semantic intent can be enforced even though Claude did not reliably do so. |
| `task_chunking` | Strong | Weak / inverted | Claude responded directionally; Codex strongly chunked both profiles despite `off` in Profile B. |
| `verbosity` | Moderate | Weak | Both harnesses showed limited separation, especially Codex. |
| `tangent_policy` | Very strong | Directional | Both harnesses moved in the intended direction; Claude produced the cleaner separation. |

## What Milestone 2 validates

The experiments support three claims:

1. **The semantic profile can remain vendor-neutral.** The same profile fields were used unchanged for Claude and Codex.
2. **Harness-specific translation is necessary.** The two harnesses did not enforce the same dimensions with equal strength.
3. **Compatibility is not binary.** "Supports HAIL" is too coarse to describe actual behavior. Compatibility needs to represent preference-level enforcement quality.

The results do **not** establish deterministic compliance, identical responses, or universal support for every semantic preference.

## Emerging compatibility model

The evidence suggests a future compatibility report should be able to express something like:

| Enforcement | Meaning |
|---|---|
| strong | Repeatedly produces a clear directional difference consistent with the semantic preference. |
| moderate | Produces a visible but less reliable or less pronounced difference. |
| weak | Preference can be expressed, but observed behavior does not reliably distinguish profile values. |
| unsupported | The harness lacks a viable mechanism for expressing the semantic intent. |
| untested | No behavioral evidence has been collected yet. |

These labels are an evidence-driven direction, not yet a frozen HAIL schema.

## Key architectural implication

The compatibility characteristics belong to the **harness + adapter**, not to the human profile.

```text
human-owned HAIL profile
          |
          +-------------------+
          v                   v
     Claude adapter       Codex adapter
          |                   |
   strong decisions       weak decisions
   weak max-options       strong max-options
          |                   |
          +---------+---------+
                    v
          same semantic intent
```

A HAIL implementation should therefore avoid mutating the user's semantic preferences merely because one harness has weaker enforcement. Instead, adapters should express the intent as effectively as possible and compatibility reporting should communicate the observed limitations.

## Milestone status

- Milestone 1 — Static profile → Claude: **PASS**
- Milestone 2 — Same profile → second harness: **PASS for portability evidence**

The next architecture decision should use these results rather than assuming that every harness needs the same delivery mechanism or a shared standalone runtime.
