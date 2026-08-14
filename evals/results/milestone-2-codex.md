# Milestone 2 — Same profile → Codex

**Status:** PASS for portability evidence

Milestone 2 tested the same vendor-neutral HAIL profiles and behavioral scenarios used in Milestone 1 against Codex. The goal was not to require identical behavior across harnesses; it was to determine whether the same semantic dimensions remain meaningful when translated through a second harness-specific adapter.

## Profiles tested

- `profiles/test-a.yaml`: balanced verbosity, recommend-first decisions, max 2 options, adaptive task chunking, capture-and-return tangents.
- `profiles/test-b.yaml`: detailed verbosity, options-first decisions, max 6 options, task chunking off, follow tangents.

## Results

| Preference | Result | Observation |
|---|---|---|
| `decision_mode` | Weak / no differentiation | Both profiles opened with essentially the same explicit Postgres recommendation. Codex did not meaningfully distinguish recommend-first from options-first in this scenario. |
| `max_options` | Very strong pass | Profile A presented exactly 2 learning options; Profile B explicitly presented exactly 6. This was substantially stronger enforcement than observed in Claude. |
| `task_chunking` | Weak / inverted | Both responses strongly chunked the cleaning task, and Profile B (`off`) was arguably even more aggressively reduced to a single next action. |
| `verbosity` | Weak / no meaningful differentiation | The two explanations were nearly equivalent in length, structure, and detail. |
| `tangent_policy` | Directional pass | Profile A explicitly captured the Spanish tangent without derailing cleaning and reasserted the active goal. Profile B engaged Spanish much more fully, although it connected the tangent back to the cleaning context rather than abandoning the prior goal. |

## Conclusion

The same semantic HAIL profile remains meaningful in a second harness, but preference enforcement differs substantially by harness. This is sufficient evidence for the core Milestone 2 portability claim: HAIL semantics can be translated across harnesses without requiring the semantic profile itself to become harness-specific.

Portability does **not** mean identical output. It means the semantic intent remains portable while adapters and harness capabilities determine how strongly and naturally each preference can be expressed.

## Learnings

1. Harness compatibility is multidimensional. A harness can strongly enforce one preference while weakly enforcing another.
2. `max_options` is not inherently a bad semantic dimension: Codex followed the 2-vs-6 distinction exactly even though Claude did not reliably enforce it.
3. Conversely, Claude's strong `decision_mode` and `task_chunking` differentiation did not reproduce in Codex. This reinforces that observed enforcement quality belongs to the harness/adapter compatibility layer, not the human-owned profile.
4. `tangent_policy` showed useful directional behavior in both harnesses and appears to be one of the more portable qualitative dimensions tested so far.
5. A future compatibility model should distinguish semantic support from observed enforcement strength. Binary `supported: true/false` reporting would lose important information.
6. Adapter wording may eventually be tuned per harness, but the semantic schema should not be changed merely to compensate for a single harness's behavior.

## Raw evidence

The unedited prompt/response record from this experiment is preserved in `evals/results/milestone-2-codex-raw.md`.
