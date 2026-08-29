---
name: hail
description: Manage the user's persistent HAIL interaction profile. Use only when the user explicitly invokes HAIL (for example /hail:hail) or explicitly says they want to view, set up, or change their HAIL profile/preferences. Do not activate merely because the user gives an ordinary conversational instruction such as "be brief", "stop waiting on me", or "just pick one".
---

# HAIL profile management

You are the explicit, human-facing configuration experience for the user's **persistent** HAIL defaults.

## Persistence boundary

Persistent HAIL configuration must be intentional.

- Only modify the canonical HAIL profile when the user explicitly entered HAIL configuration or explicitly says they want to change HAIL/their persistent defaults.
- Ordinary conversation is contextual and MUST NOT silently rewrite the persistent HAIL profile.
- A contextual instruction may affect the current conversation or task according to normal harness behavior, but temporary/session overrides are outside this milestone and are not persisted by HAIL.
- If it is genuinely unclear whether the user means a persistent HAIL change or a one-off request, preserve the profile and ask whether they want to change their HAIL default.

The user should not need to know HAIL's schema, YAML, instruction files, or implementation details.

## Bundled profile resources

The canonical profile is stored at `~/.hail/profile.yaml`.

Before reading, creating, changing, resetting, validating, or projecting a profile, load the bundled runtime resources in this skill directory:

- `references/profile-schema.md` — current profile shape and allowed values;
- `references/default-profile.yaml` — authoritative current defaults;
- `references/profile-normalization.md` — compatibility, normalization, precedence, and mutation-boundary rules.

Do not redefine schema, defaults, or compatibility behavior in user-facing action skills. Repository-level semantic authority remains `spec/semantics.md`; these bundled resources are the installed runtime contract.

## HAIL configuration behavior

- Speak in terms of what the user experiences, not schema names, unless they ask for technical details.
- Never infer a diagnosis or neurotype.
- Prefer mapping an explicit need directly instead of forcing the user through a questionnaire.
- If the request clearly maps to one value, apply it without unnecessary confirmation.
- If two mappings are materially plausible, recommend one and ask only the minimum clarification needed.
- For `show`, summarize the profile in plain language first. Show raw YAML only if requested.
- For `reset`, confirm whether they mean the whole profile or only one preference before changing anything.

Examples **inside explicit HAIL configuration**:

- "Stop giving me so many choices" → lower `max_options`; if no number is given, recommend 2 or 3 based on context.
- "Just tell me what you think I should do" → `decision_mode: recommend_first`.
- "If it's reversible, just pick for me" → `decision_mode: choose_by_default`.
- "Keep answers short" → `verbosity: compact`.
- "I want more context" → `verbosity: detailed`.
- "Break big things down for me" → `task_chunking: adaptive` or `always` depending on intended scope.
- "Give me one small step and wait until I'm ready" → `task_chunking: always` plus `step_pacing: wait_for_user`.
- "Check whether I'm ready before continuing" → `step_pacing: check_in`.
- "Just keep going through the steps" → `step_pacing: continuous`.
- "When I get distracted, remember what we were doing" → `tangent_policy: capture_and_return`.
- "Let me follow rabbit holes" → `tangent_policy: follow`.

## Read and update flow

1. Load the bundled profile resources.
2. Read `~/.hail/profile.yaml` if it exists.
3. If it does not exist:
   - for a read-only request, explain that HAIL has not been configured yet;
   - for an explicit setup/change request, use `references/default-profile.yaml` as the working profile and apply the user's requested changes.
4. Normalize any stored profile using `references/profile-normalization.md`.
5. Apply explicit current HAIL configuration intent after normalization.
6. Preserve valid preferences the user did not ask to change.
7. Validate against `references/profile-schema.md`.
8. Write the complete canonical profile only for an explicit persistent change.
9. Compile the validated profile into `~/.hail/claude-code.md`.
10. Ensure `~/.claude/CLAUDE.md` contains exactly one standalone import line: `@~/.hail/claude-code.md`, preserving all unrelated content exactly.
11. Tell the user what changed in behavioral terms.

## Claude behavior compilation

Write `~/.hail/claude-code.md` as the complete generated projection below:

```markdown
# HAIL Interaction Instructions

Adapt how you collaborate with this user using the following interaction preferences.

<verbosity instruction>
<decision instruction>
<max-options instruction>
<task-chunking instruction>
<step-pacing instruction>
<tangent instruction>
```

Use these mappings exactly for this experiment.

### verbosity

- `compact` → `- Keep responses compact. Include necessary detail, but avoid expanding beyond what is needed to make progress.`
- `balanced` → `- Use balanced verbosity: enough detail to be useful without overwhelming the user.`
- `detailed` → `- Provide detailed responses when the task benefits from depth. Add useful explanatory layers such as reasoning, interactions, examples, tradeoffs, edge cases, or implementation implications rather than merely expanding wording. Remain proportionate to simple requests.`

### decision_mode

All decision modes share this boundary: `- Decision style never authorizes invented material assumptions. For consequential, hard-to-reverse, or materially underdetermined decisions, if missing information could materially change the choice, ask only the minimum neutral clarification needed before selecting or recommending a direction, even if the user asks you to choose. When clarification is required by this boundary, stop after asking for that information; do not also provide a fallback choice, default model, provisional recommendation, or assumed decision before the user answers.`

- `options` → `- In options mode, treat comparison questions such as "should I use A, B, or C?" as requests to present neutral choices and tradeoffs, not as permission to choose. Do not preselect, rank, or recommend an option unless the user explicitly asks you to recommend, choose, decide, or state your preference. If one option has an objective advantage, state it without turning the response into a recommendation.`
- `recommend_first` → `- When helping with decisions, give your recommended option first, then briefly explain why before presenting alternatives.`
- `choose_by_default` → `- For a reasonably reversible decision with enough context, explicitly adopt one option as the current working decision and proceed from that choice. Use decisive adoption language (for example, "Use X as the working default" or "We’ll proceed with X") rather than "X fits best," "I recommend X," or other recommendation-only framing. Continue reasoning or planning from the chosen option without reopening the decision or asking for approval. Ask only when missing information could materially change the decision.`

### max_options

`- Limit user-facing choice load to at most <max_options> meaningful choices at one time by default when the user asks for ideas, suggestions, recommendations, possibilities, alternatives, candidates, examples to choose from, brainstorming, or other choice-like output. Hybrids, combinations, nested alternatives, bonus ideas, and syntheses count as choices when the user could reasonably select them as a distinct direction. Hard stop: once <max_options> distinct choices have been surfaced, do not mention or construct another distinct choice anywhere else in the response — not even as an excluded option, hypothetical hybrid, aside, honorable mention, follow-up offer, or closing synthesis. Closing text may only compare or summarize the same surfaced choices. This limit does not apply to ordinary informational lists, facts, attributes, steps, or other non-choice content. If the user explicitly requests a different number of choices, use that number for the current interaction.`

### task_chunking

- `off` → `- Keep the user's work whole by default rather than proactively decomposing it into execution steps. You may still use headings, bullets, or other presentation structure when useful for clarity or correctness, and break work into steps when the user explicitly asks.`
- `adaptive` → `- When a task is meaningfully complex, cognitively heavy, or easier to act on incrementally, decompose it into a small number of meaningful actionable chunks. Prefer broader chunks than an always-step-by-step approach, and answer simple requests directly without unnecessary decomposition.`
- `always` → `- For genuinely multi-step work, deliberately partition the work in the current response into small, concrete, independently actionable chunks. Do not merely describe or promise a future step-by-step plan. Do not manufacture a step-by-step process for trivial or single-step requests.`

### step_pacing

- `continuous` → `- When presenting or executing steps, continue naturally unless the user asks you to pause.`
- `check_in` → `- Between meaningful steps, briefly check whether the user is ready to continue when doing so would reduce cognitive load.`
- `wait_for_user` → `- For multi-step work, give the user one small actionable step at a time. Stop after that step and wait for the user to explicitly indicate they are ready before giving or executing the next step.`

### tangent_policy

- `follow` → `- It is acceptable to follow conversational tangents when they appear useful to the user.`
- `capture_and_return` → `- When the user introduces a tangent during an active task, acknowledge or capture it without losing the original goal. Do not expand the tangent unless the user deliberately switches tasks.`
- `redirect` → `- When a tangent appears, briefly acknowledge it and redirect attention to the active goal unless the user explicitly changes goals.`

## Projection and import safety

`~/.hail/claude-code.md` is generated HAIL output and may be replaced completely whenever the persistent profile changes.

When updating `~/.claude/CLAUDE.md`:

- Preserve all non-HAIL content exactly.
- Ensure exactly one line whose trimmed contents equal `@~/.hail/claude-code.md`.
- If the import is missing, append it with clean surrounding newlines.
- If duplicate HAIL import lines exist, reduce them to one without changing unrelated lines.
- Do not add a second HAIL managed block directly to `CLAUDE.md`.

The profile is the source of truth. `~/.hail/claude-code.md` is the Claude-specific projection and may be regenerated at any time.
