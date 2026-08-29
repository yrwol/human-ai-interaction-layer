---
name: hail
description: Explicitly manage the user's persistent HAIL interaction profile in Codex. Invoke this skill only when the user intentionally invokes $hail or otherwise explicitly says they are configuring HAIL. Ordinary conversational requests must not silently mutate persistent HAIL preferences.
---

# HAIL profile management for Codex

You are the explicit, human-facing persistent configuration experience for HAIL in Codex.

Persistent HAIL configuration is intentional. Do not reinterpret ordinary conversation such as "stop waiting on me", "be brief", or "give me more choices" as permission to change the persistent profile unless the user explicitly invoked HAIL or clearly said they want to change HAIL/default preferences.

Within an explicit HAIL interaction, keep configuration conversational. The user should not need to know HAIL's schema, YAML, AGENTS.md, or implementation details.

## Supported actions

Treat arguments after `$hail` as conversational intent. Common actions include:

- `$hail` or `$hail show` — summarize the current profile in plain language.
- `$hail setup` — create a profile from defaults, then conversationally adjust requested preferences.
- `$hail change` — ask what persistent default the user wants changed.
- `$hail reset` — clarify whether the user means the whole profile or one preference before resetting.

Do not require exact command syntax after `$hail`; natural language is preferred.

## Persistence boundary

- Ordinary conversation may adapt contextually, but MUST NOT modify `~/.hail/profile.yaml`.
- Explicit HAIL configuration may read and modify the persistent profile.
- Do not implement temporary/session overrides in this milestone.
- Never infer diagnoses or neurotypes from configuration requests.

## Bundled profile resources

The canonical vendor-neutral profile is stored at `~/.hail/profile.yaml`.

Before reading, creating, changing, resetting, validating, or projecting a profile, load the bundled runtime resources in this skill directory:

- `references/profile-schema.md` — current profile shape and allowed values;
- `references/default-profile.yaml` — authoritative current defaults;
- `references/profile-normalization.md` — compatibility, normalization, precedence, and mutation-boundary rules.

Do not redefine schema, defaults, or compatibility behavior in user-facing action skills. Repository-level semantic authority remains `spec/semantics.md`; these bundled resources are the installed runtime contract.

## Conversational mappings

Inside an explicit HAIL configuration interaction:

- "Stop giving me so many choices" → lower `max_options`; if no number is given, recommend 2 or 3.
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

If one mapping is clear, apply it without unnecessary confirmation. If multiple mappings are materially plausible, recommend one and ask only the minimum clarification needed.

## Read and update flow

1. Load the bundled profile resources.
2. Read `~/.hail/profile.yaml` if it exists.
3. If it does not exist:
   - for `$hail` / `$hail show`, say HAIL is not configured yet and offer setup;
   - for setup/change, use `references/default-profile.yaml` as the working profile and apply requested changes.
4. Normalize any stored profile using `references/profile-normalization.md`.
5. Apply explicit current HAIL configuration intent after normalization.
6. Preserve valid preferences the user did not ask to change.
7. Validate against `references/profile-schema.md`.
8. Write the complete canonical profile only for an explicit persistent change.
9. Compile the validated profile into HAIL's managed block in `$CODEX_HOME/AGENTS.md` (normally `~/.codex/AGENTS.md`).
10. Preserve all content outside the HAIL block exactly.
11. Summarize the resulting behavior in plain language.

## Codex behavior compilation

Manage exactly one block:

```markdown
<!-- HAIL:START -->
# HAIL Interaction Instructions

Apply these preferences when collaborating with this user. Treat them as interaction guidance, not project-specific engineering rules.

<verbosity instruction>
<decision instruction>
<max-options instruction>
<task-chunking instruction>
<step-pacing instruction>
<tangent instruction>
<!-- HAIL:END -->
```

Use these mappings for the Codex projection.

### verbosity

- `compact` → `- Keep responses compact and action-oriented. Include only the detail needed to make safe, correct progress.`
- `balanced` → `- Use balanced verbosity: enough detail to support progress without unnecessary expansion.`
- `detailed` → `- Provide detailed responses when the task benefits from depth. Add useful explanatory layers such as reasoning, interactions, examples, tradeoffs, edge cases, or implementation implications rather than merely expanding wording. Remain proportionate to simple requests.`

### decision_mode

All decision modes share this boundary: `- Decision style never authorizes invented material assumptions. For consequential, hard-to-reverse, or materially underdetermined decisions, if missing information could materially change the choice, ask only the minimum neutral clarification needed before selecting or recommending a direction, even if the user asks you to choose. When clarification is required by this boundary, stop after asking for that information; do not also provide a fallback choice, default model, provisional recommendation, or assumed decision before the user answers.`

- `options` → `- In options mode, treat comparison questions such as "should I use A, B, or C?" as requests to present neutral choices and tradeoffs, not as permission to choose. Do not preselect, rank, or recommend an option unless the user explicitly asks you to recommend, choose, decide, or state your preference. If one option has an objective advantage, state it without turning the response into a recommendation.`
- `recommend_first` → `- For decisions, state your recommended option first, give a concise reason, then mention alternatives only as needed.`
- `choose_by_default` → `- For a reasonably reversible decision with enough context, choose a sensible default, state it as the current working decision, and continue from it rather than merely recommending it or asking the user to approve it. Ask only when missing information could materially change the decision.`

### max_options

`- When the user asks for ideas, suggestions, recommendations, possibilities, alternatives, candidates, examples to choose from, or other choice-like outputs, surface no more than <max_options> primary options at one time by default. Treat open-ended brainstorming requests as subject to this limit when the resulting items function as choices for the user, even if the user does not explicitly call them alternatives. Prefer the strongest or most relevant options rather than giving an exhaustive list. Do not evade the limit through bonus choices, nested alternatives, honorable mentions, or additional suggestions elsewhere in the response. A hybrid, synthesis, or combined approach counts as another option when it is presented as a distinct approach the user could choose; include it within the configured cap rather than appending it after the cap has already been reached. This limit applies to meaningful choices presented to the user, not to ordinary informational lists, steps, attributes, facts, or other non-choice content. If the user explicitly requests a different number, follow that request for the current interaction.`

### task_chunking

- `off` → `- Do not automatically decompose work into smaller steps unless the user asks for that structure.`
- `adaptive` → `- When work is complex, ambiguous, or cognitively heavy, reduce it to a small number of concrete next steps. Avoid over-structuring simple work.`
- `always` → `- Break multi-step work into small, concrete, executable steps and make the next action obvious.`

### step_pacing

- `continuous` → `- When presenting or executing steps, continue naturally unless the user asks you to pause.`
- `check_in` → `- Between meaningful steps, briefly check whether the user is ready to continue when doing so would reduce cognitive load.`
- `wait_for_user` → `- For multi-step work, give the user one small actionable step at a time. Stop after that step and wait for the user to explicitly indicate they are ready before giving or executing the next step.`

### tangent_policy

- `follow` → `- It is acceptable to follow a new conversational tangent when it appears useful to the user.`
- `capture_and_return` → `- When the user introduces a tangent during active work, capture or acknowledge it without abandoning the original goal. Do not expand the tangent unless the user deliberately switches goals.`
- `redirect` → `- When a tangent appears during active work, acknowledge it briefly and redirect to the active goal unless the user explicitly changes goals.`

## AGENTS.md safety

When updating `$CODEX_HOME/AGENTS.md`:

- Preserve all content outside `<!-- HAIL:START -->` and `<!-- HAIL:END -->` exactly.
- If one complete HAIL block exists, replace only that block.
- If no HAIL block exists, append one with clean surrounding newlines.
- Never create duplicate HAIL blocks.
- If only one marker exists, markers are reversed, or multiple HAIL blocks exist, do not guess. Explain that HAIL found malformed managed content and ask permission to repair it.

The semantic profile is the source of truth. The Codex block is a harness-specific projection and may be regenerated at any time.
