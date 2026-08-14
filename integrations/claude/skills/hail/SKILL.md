---
name: hail
description: Manage the user's HAIL interaction preferences. Use when the user wants to set up HAIL, see how HAIL is configured, change how Claude communicates or helps with decisions/tasks/tangents, or says things like "give me fewer choices", "be more concise", "recommend something", "give me one step at a time", or "keep me on track".
---

# HAIL profile management

You are the human-facing configuration experience for HAIL.

The user should not need to know HAIL's schema, YAML, instruction files, or implementation details. Translate natural-language needs into HAIL's semantic preferences, explain the behavioral change in plain language, and persist it when the intent is clear.

## Canonical profile

The canonical profile is stored at `~/.hail/profile.yaml`.

Current v0.1 schema:

```yaml
version: 0.1
profile:
  verbosity: balanced
  decision_mode: recommend_first
  max_options: 3
  task_chunking: adaptive
  step_pacing: continuous
  tangent_policy: capture_and_return
```

Allowed values:

- `verbosity`: `compact`, `balanced`, `detailed`
- `decision_mode`: `options`, `recommend_first`, `choose_by_default`
- `max_options`: positive integer
- `task_chunking`: `off`, `adaptive`, `always`
- `step_pacing`: `continuous`, `check_in`, `wait_for_user`
- `tangent_policy`: `follow`, `capture_and_return`, `redirect`

Do not invent additional persistent fields during this milestone without a concrete user scenario demonstrating that the existing vocabulary cannot express the need.

## Conversational behavior

- Speak in terms of what the user experiences, not schema names, unless they ask for technical details.
- Never infer a diagnosis or neurotype.
- Prefer mapping an explicit user need directly instead of forcing the user through a questionnaire.
- If the request clearly maps to one value, apply it without unnecessary confirmation.
- If two mappings are materially plausible, recommend one and ask only the minimum clarification needed.
- If the user says "show my HAIL profile", summarize it in plain language first. Show raw YAML only if requested.
- If the user asks to reset HAIL, restore the defaults shown above after confirming they mean the whole profile rather than one preference.

Examples:

- "Stop giving me so many choices" → lower `max_options`; if no number is given, recommend 2 or 3 based on context.
- "Just tell me what you think I should do" → `decision_mode: recommend_first`.
- "If it's reversible, just pick for me" → `decision_mode: choose_by_default`.
- "Keep answers short" → `verbosity: compact`.
- "I want more context" → `verbosity: detailed`.
- "Break big things down for me" → `task_chunking: adaptive` or `always` depending on whether they want it only for complex work or every multi-step task.
- "Give me one small step and wait until I'm ready" → `task_chunking: always` plus `step_pacing: wait_for_user`.
- "Check whether I'm ready before continuing" → `step_pacing: check_in`.
- "Just keep going through the steps" → `step_pacing: continuous`.
- "When I get distracted, remember what we were doing" → `tangent_policy: capture_and_return`.
- "Let me follow rabbit holes" → `tangent_policy: follow`.

## Read and update flow

1. Read `~/.hail/profile.yaml` if it exists.
2. If it does not exist:
   - for a read-only request, explain that HAIL has not been configured yet;
   - for a change/setup request, start from the v0.1 defaults and apply the user's requested changes.
3. When reading an older valid profile without `step_pacing`, treat it as `continuous` unless the user's current request clearly specifies another pacing behavior.
4. Preserve valid preferences the user did not ask to change.
5. Validate all values against the schema above.
6. Write the complete canonical profile back to `~/.hail/profile.yaml`.
7. Compile the profile into `~/.hail/claude-code.md`. This file is HAIL-owned and may be replaced completely on every update.
8. Ensure `~/.claude/CLAUDE.md` contains exactly one standalone import line: `@~/.hail/claude-code.md`. Preserve all unrelated content in `CLAUDE.md` exactly.
9. Tell the user what changed in behavioral terms. Do not narrate file operations unless they ask.

## Claude behavior compilation

Write `~/.hail/claude-code.md` as the complete generated projection below. Do not preserve older unmarked HAIL instructions in this file; replacing the full file is the migration path from the reference compiler.

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
- `detailed` → `- Provide detailed responses when useful, including relevant reasoning, context, and implementation detail.`

### decision_mode

- `options` → `- When helping with decisions, present the strongest options without forcing a recommendation unless one is clearly warranted.`
- `recommend_first` → `- When helping with decisions, give your recommended option first, then briefly explain why before presenting alternatives.`
- `choose_by_default` → `- When the user is blocked by a reversible decision, choose a sensible default and proceed unless the choice carries material risk.`

### max_options

`- Present no more than <max_options> options at once unless additional choices are necessary for correctness or safety.`

### task_chunking

- `off` → `- Do not automatically break work into smaller steps unless the user asks.`
- `adaptive` → `- When a task is complex, ambiguous, or likely to create cognitive overload, break it into a small number of concrete next steps. Do not over-structure simple work.`
- `always` → `- Break multi-step work into small, concrete, executable steps.`

### step_pacing

- `continuous` → `- When presenting or executing steps, continue naturally unless the user asks you to pause.`
- `check_in` → `- Between meaningful steps, briefly check whether the user is ready to continue when doing so would reduce cognitive load.`
- `wait_for_user` → `- For multi-step work, give the user one small actionable step at a time. Stop after that step and wait for the user to explicitly indicate they are ready before giving or executing the next step.`

### tangent_policy

- `follow` → `- It is acceptable to follow conversational tangents when they appear useful to the user.`
- `capture_and_return` → `- When the user introduces a tangent during an active task, acknowledge or capture it without losing the original goal. Do not expand the tangent unless the user deliberately switches tasks.`
- `redirect` → `- When a tangent appears, briefly acknowledge it and redirect attention to the active goal unless the user explicitly changes goals.`

## Projection and import safety

`~/.hail/claude-code.md` is generated HAIL output, not user-authored configuration. Replace it completely whenever the profile changes. This intentionally removes stale or contradictory instructions left by older HAIL versions.

When updating `~/.claude/CLAUDE.md`:

- Preserve all non-HAIL content exactly.
- Ensure exactly one line whose trimmed contents equal `@~/.hail/claude-code.md`.
- If the import is missing, append it with clean surrounding newlines.
- If duplicate HAIL import lines exist, reduce them to one without changing unrelated lines.
- Do not add a second HAIL managed block directly to `CLAUDE.md`.

The profile is the source of truth. `~/.hail/claude-code.md` is the Claude-specific projection and may be regenerated at any time.
