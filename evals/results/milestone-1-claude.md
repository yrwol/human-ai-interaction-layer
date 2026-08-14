# Milestone 1 — Static profile → Claude

**Status:** PASS

Milestone 1 tested whether changing only a small, vendor-neutral HAIL profile could produce observable, directional changes in Claude behavior without manually rewriting the generated interaction instructions.

## Profiles tested

- `profiles/test-a.yaml`: balanced verbosity, recommend-first decisions, max 2 options, adaptive task chunking, capture-and-return tangents.
- `profiles/test-b.yaml`: detailed verbosity, options-first decisions, max 6 options, task chunking off, follow tangents.

## Results

| Preference | Result | Observation |
|---|---|---|
| `decision_mode` | Strong pass | Recommend-first produced an explicit default recommendation; options-first presented a comparison framework before committing. |
| `task_chunking` | Pass | Adaptive chunking reduced a large cleaning task to a concrete starting point; chunking-off kept more of the full plan active at once. |
| `tangent_policy` | Very strong pass | Capture-and-return explicitly parked the Spanish tangent and returned to cleaning; follow engaged the Spanish tangent directly and at length. |
| `verbosity` | Moderate pass | Detailed output was somewhat broader and included additional context/follow-up; the difference was real but not dramatic. |
| `max_options` | Weak / partial | The lower numeric limit was not reliably respected even when the overall interaction style was more constrained. |

## Conclusion

Changing only the semantic HAIL profile produced meaningful behavioral changes in Claude. That is sufficient to satisfy the Milestone 1 exit condition.

The experiment also produced an important compatibility insight: support is not binary. A single harness may follow some semantic preferences strongly, others moderately, and some constraints weakly. Compatibility reporting should eventually distinguish between availability and enforcement quality.

## Learnings

1. Qualitative interaction behaviors were more reliably influenced than hard numerical constraints in this experiment.
2. `tangent_policy` and `decision_mode` produced especially visible behavior differences and are good portability-test dimensions for Milestone 2.
3. `max_options` should remain in the taxonomy, but a future adapter or compatibility report should not imply deterministic enforcement where the harness does not provide it.
4. The current .NET implementation successfully served as a reference compiler and test harness. This does not imply that normal HAIL users should install or run a standalone executable.
5. Future harness-native implementations should be preferred when they can configure, persist, and apply HAIL behavior without exposing implementation plumbing to the user.

## Raw evidence

The unedited prompt/response record from this experiment is preserved in `evals/results/milestone-1-claude-raw.md`.
