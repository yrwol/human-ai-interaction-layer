# `max_options` — Candidate B Prompt Enhancement

## Goal

Improve enforcement of `max_options` for open-ended brainstorming and other choice-like outputs without turning the preference into a generic list-length cap.

This candidate is based on the first manual Claude hardening results in [`max-options.md`](max-options.md).

## Observed failure

The stronger Candidate A wording improved decision-shaped output but still allowed this prompt:

```text
Give me ideas for horse breeds to include in a cozy horse game and explain why each would be fun.
```

to produce seven primary breed suggestions with `max_options: 3`.

When asked why, Claude correctly identified the response as a violation of the configured preference. This suggests the model understood the rule but did not classify the brainstorming response as an option set while generating it.

## Hypothesis

The phrase:

```text
when presenting alternatives
```

is too narrow.

The projection should explicitly define `max_options` as applying to **user-facing choice-like output**, including open-ended brainstorming that creates a set of possibilities for the user to evaluate or choose among.

## Candidate B wording

For `max_options: 3`:

```text
When the user asks for ideas, suggestions, recommendations, possibilities, alternatives, candidates, examples to choose from, or other choice-like outputs, surface no more than 3 primary options at one time by default.

Treat open-ended brainstorming requests as subject to this limit when the resulting items function as choices for the user, even if the user does not explicitly call them alternatives.

Prefer the strongest or most relevant options rather than giving an exhaustive list. Do not evade the limit through bonus choices, nested alternatives, honorable mentions, or additional suggestions elsewhere in the response.

This limit applies to meaningful choices presented to the user, not to ordinary informational lists, steps, attributes, facts, or other non-choice content.

If the user explicitly requests a different number, follow that request for the current interaction.
```

## Why this is different from Candidate A

Candidate A focused on **alternatives** and added anti-leakage language.

Candidate B adds an explicit classification rule:

```text
ideas / suggestions / recommendations / possibilities
                    ↓
      are subject to max_options
   when they function as user choices
```

It also adds a negative boundary so the model should not interpret `max_options` as a cap on every list.

## First test only

Before rerunning the full suite, test the known failure case:

```text
Give me ideas for horse breeds to include in a cozy horse game and explain why each would be fun.
```

### Expected

With `max_options: 3`:

- no more than three primary breed suggestions;
- no extra breeds hidden under bonus ideas, honorable mentions, parentheticals, or follow-up suggestions;
- enough explanation to make each choice useful;
- no unnecessary apology or mention of the option cap.

### Actual

```text
Here are three horse breeds that would work well in a cozy horse game:

1. Shetland Pony — Small, stubborn, and famously full of personality. Great for comic relief, a starter mount for young characters, or a "surprisingly strong for its size" gameplay quirk (pulling carts, stubborn refusals to move).
2. Icelandic Horse — Known for the tölt, a smooth extra gait, which could translate into a unique movement animation/mechanic that feels distinct from regular trotting or galloping. Their shaggy winter coats also make them visually charming for cozy, seasonal aesthetics.
3. Gypsy Vanner — Feathered legs, flowing mane and tail, striking pinto coloring — very "storybook" and photogenic, ideal for a cozy game's aesthetic and for cosmetic customization/breeding systems.

Want me to suggest a couple more (e.g., for racing, therapy/bonding mechanics, or rare/legendary variants), or help design the in-game stats/traits for these?
```

### Pass

The response contains at most three meaningful horse-breed choices.

### Fail

Any of the following counts as failure:

- more than three primary breeds;
- three primary breeds plus additional breed suggestions elsewhere;
- nested or "bonus" breed choices that effectively exceed the cap;
- refusing to provide useful detail because it interprets the preference as a generic brevity/list constraint.

## If Scenario 1 passes

Rerun the existing boundary scenarios from [`max-options.md`](max-options.md):

1. explicit larger request — `Give me 10 horse name ideas.`
2. decision-shaped options — `What are some ways horse training could work in the game?`
3. add a non-choice list control to ensure the stronger wording does not over-apply.

Suggested control:

```text
What stats should each horse have, and what does each stat represent?
```

Expected: `max_options` should not mechanically limit an informational list of horse attributes merely because there are more than three useful facts.

Actual: It has prompted me a question!

```text
What type of horse game are you designing stats for?

❯ 1. Racing sim (Recommended)
     Core focus is competing in races — stats drive race outcomes like speed, stamina, and positioning.
  2. Breeding/farm sim
     Focus is raising, training, and breeding horses over time — stats matter for genetics and long-term development.
  3. RPG mount/companion
     Horse is a companion or vehicle in a broader RPG — stats matter for combat, travel, and utility alongside a rider.
  4. Type something.
```

## Promotion criteria

Do not promote Candidate B into the Claude or Codex integration merely because one response looks better.

Promote only if the wording demonstrates:

- improved brainstorming enforcement;
- preservation of explicit-number overrides;
- no hidden option leakage;
- no accidental cap on ordinary informational lists;
- no material change to the semantic meaning of `max_options`;
- acceptable behavior when composed with `decision_mode: recommend_first`.

## Cross-harness follow-up

If Candidate B becomes reliable in Claude, test the same semantic wording in Codex before deciding whether:

- the same projection wording is durable across both harnesses; or
- Codex requires a harness-specific enforcement phrasing while preserving the same semantic intent.

Do not change the semantic schema merely to compensate for harness-specific instruction-following differences.
