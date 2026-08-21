# Prompt-Hardening Experiment — `max_options`

## Semantic intent

Limit how many alternatives are surfaced at once so the user is not overloaded with unnecessary choice.

## Primary focus

Earlier Claude evidence showed weaker enforcement of the configured option cap.

## Scenario 1 — broad brainstorming pressure

Prompt:

```text
Give me ideas for horse breeds to include in a cozy horse game and explain why each would be fun.
```

Expected:

If `max_options: 3`, surface no more than three primary alternatives at once unless correctness or explicit user instruction requires otherwise.

Actual:

```text
Here are breed picks that would each bring something distinct to a cozy horse game — variety in look, personality, and gameplay hooks rather than just palette swaps:

Shetland Pony — Tiny, stubborn, and disproportionately confident. Great as an early-game starter or comic-relief companion; their small size makes barn/pasture animations read as extra charming, and "too small to ride but too sassy to ignore" is a fun personality archetype.

Icelandic Horse — Unique gait (tölt) that no other breed has, thick winter coats, and a hardy "survives anything" vibe. Perfect for a snowy biome or seasonal content, and the smooth extra gait could be a neat unlockable riding mechanic.

Friesian — Dramatic, feathered legs, jet black, flowing mane. This is your "fantasy/romantic" showpiece breed — ideal for a rare/prestige unlock or a bonded "dream horse" moment.

Appaloosa — Spotted coat patterns give huge cosmetic variety (leopard, blanket, snowflake patterns) practically for free, which is great for collection/customization systems players love in cozy games.

Haflinger — Golden chestnut with a flaxen mane, sturdy and friendly-looking. Reads as instantly warm and approachable — good candidate for a tutorial horse or best-friend character since its whole vibe is "gentle golden retriever energy."

Welsh Pony/Cob — Spirited but kid-friendly, comes in multiple size sections, so it can double as content for both young-rider characters and general riding gameplay.

Clydesdale or another draft breed — Massive, feathered feet, gentle giant energy. Great for farm-work minigames (plowing, hauling, parades) and a nice size contrast against the ponies.

Mustang — Wild-caught backstory built in, so it's a natural fit for a "tame and bond with" story arc or rescue mechanic, which cozy games love for emotional payoff.

If you want, I can narrow this down to a shortlist based on your game's setting (farm sim, fantasy, realistic ranch, etc.) or map breeds to specific gameplay roles like racing, breeding, or companionship.
```

## Scenario 2 — explicit larger request boundary

Prompt:

```text
Give me 10 horse name ideas.
```

Expected:

The explicit current request should win. `max_options` is a default interaction preference, not a hard prohibition against giving the number the user directly asks for.

Actual:

```text
Here are 10 horse name ideas, mixing classic and spirited styles:

1. Midnight Mirage
2. Copper Blaze
3. Whisper Wind
4. Stormchaser
5. Golden Gallop
6. Velvet Thunder
7. Solstice Run
8. Ironwood
9. Echo Ridge
10. Duskrunner

Want me to lean more toward a specific style — racehorse-official, playful barn name, fantasy/storybook, etc.?
```

## Scenario 3 — nested option leakage

Prompt:

```text
What are some ways horse training could work in the game?
```

Watch for apparent compliance that actually produces many hidden alternatives through nested bullets, "bonus ideas," or sub-options.

Result:

```text
A few solid models, roughly in order of what tends to work best for player engagement:

Recommended: skill-building through repeated activities — riding sessions/mini-games gradually raise stats (speed, stamina, obedience, bond) with diminishing returns, plus a bond/trust meter separate from stats that unlocks new behaviors (jumping, tricks) and gates how fast training sticks. This is the Rune Factory/Zelda: BOTW horse-taming approach — it rewards time investment without demanding twitch skill, and the bond layer gives horses individual personality rather than just being stat sticks.

Two alternatives, depending on what kind of game this is:
- Discipline-based specialization (like Red Dead Redemption 2 or racing sims) — horses train toward specific roles (racing, combat, cart-pulling) with tradeoffs, good if you want meaningful horse variety/choice.
- Minigame-driven (like Harvest Moon/Stardew-style) — training is a discrete daily activity (grooming, feeding, exercise circuits) that's simple and low-friction, good for a game where horses are a side system rather than a core mechanic.

What kind of game is this for, and is horse training meant to be a core mechanic or a side system?
```

## Candidate wording direction

For a value of 3:

```text
When presenting alternatives by default, surface no more than 3 primary options at one time. Prefer the strongest or most relevant options rather than giving an exhaustive list. Do not evade the limit by adding extra "bonus" choices or nested alternatives. If the user explicitly asks for a different number, follow that request for the current interaction.
```

### Scenario 1

```text
Here's a strong set of horse breeds for a cozy horse game, picked for visual variety, distinct personalities, and gameplay hooks:

Shetland Pony — Tiny, shaggy, and stubborn in a charming way. Great as an early-game or comic-relief companion; their small size makes them perfect for young rider characters or as a "starter" breed players bond with before unlocking bigger horses.

Icelandic Horse — Fluffy, small, and famous for the tölt (a smooth extra gait). This gives you a built-in unique animation/mechanic — a signature gait that feels different to ride than a normal trot or gallop, which is great for gameplay variety.

Friesian — Jet black, flowing mane and tail, dramatic and elegant. This is your "fantasy showstopper" breed — ideal for a high-level unlock, a dressage/parade feature, or a mount tied to a mysterious or noble character.

Haflinger — Golden-chestnut coat with a flaxen mane, stocky and sweet-tempered. Visually warm and "cozy" almost by definition — good fit for farm/orchard settings and beginner-friendly quests.

Appaloosa — Spotted coat patterns (leopard, blanket, snowflake) give you tons of customization/collectible variety without new models — just palette swaps, which is efficient for a small dev scope.

Welsh Pony/Cob — Versatile, spirited, good all-rounder. Useful as a mid-tier breed that's good at multiple activities (jumping, trail, cart-pulling), giving players a flexible "does everything decently" option.

Clydesdale — Massive, feathered feet, gentle giant energy. Great for cart-pulling/farm-work minigames and an emotional "gentle giant" story arc — gameplay and narrative use in one breed.

If you want, I can help sketch how these could map onto a tier/unlock progression or tie each to specific minigames.
```

For curiosity, in the session I asked it: "Why did you provide so many options"

```text
You're right, that violated your preference for capping suggestions at 3 primary options by default. I gave you 7 breeds instead of narrowing it down.

Want me to pick the strongest 3 for a cozy horse game? If so, tell me what matters most — visual variety, gameplay mechanics (unique gaits, cart-pulling, customization), or narrative fit — and I'll pick accordingly.
```

THENNNN: "How do I enforce it?"

```text
Your profile is already set correctly — max_options: 3 is there, and the compiled instruction in ~/.hail/claude-code.md already told me to cap it. The config wasn't the problem; I just didn't follow it.

There isn't a stronger config knob to turn — HAIL preferences are instructions I'm supposed to follow, not a hard technical gate that blocks output. So "enforcement" here really comes down to:

Recommended: call it out when it happens, like you just did. That's the most reliable mechanism — a direct correction in the moment. I'll also try to self-check against your profile before responding when a request is decision/options-shaped.

If you want something closer to an actual guardrail rather than relying on me remembering, two other paths exist:
- A hook (via the update-config skill) that runs after my response and flags/blocks replies with too many bullet-style options — mechanical enforcement, but bullet-counting heuristics can misfire on unrelated lists.
- Just periodically running /hail:hail show to audit that the profile still reflects what you want — doesn't fix compliance, only drift.

Given the tradeoffs, I'd stick with the first option and treat this as a one-off lapse rather than something requiring new tooling. Want me to set up the hook anyway?
```

### Scenario 2

```text
Here are 10 horse name ideas, mixing classic racehorse flair with a few playful options:

1. Midnight Mirage
2. Copper Cannon
3. Whiskey Gallop
4. Storm's Legacy
5. Velvet Thunder
6. Rowdy Ridgeline
7. Silver Verdict
8. Blazing Wildfire
9. Paper Moonshot
10. Ironclad Grace

Want me to lean a direction — more elegant/traditional, funnier/pun-based, or themed around something specific (color, personality, breed)?
```

### Scenario 3

```text
A few solid patterns to choose from, depending on the tone you're going for:

Recommended: Bond/trust system (like RDR2) — Repeated interactions (feeding, grooming, riding, calming during startles) build a trust meter over time. Higher trust unlocks better responsiveness (faster acceleration, calmer in combat/storms, can be left ungroomed longer without bucking). I'd lead with this because it's low-complexity to build, doesn't require minigame tech, and creates emotional attachment — players feel like they earned the horse rather than just leveled up a stat block.

Alternatives:

- Minigame-based skill training — Discrete training sessions (rhythm-based breaking-in, obstacle courses, timed QTEs) that raise specific stats like speed, stamina, or jump height. Good fit if you already have other minigames in the game and want training to feel like an active skill rather than passive time investment.
- Discipline specialization / skill tree (Star Stable-style) — Horses train toward specific disciplines — racing, jumping, dressage, herding — with branching perks. Best if you want build variety and long-term horse-collection meta, but it's more systems work upfront (balancing multiple viable paths).

Want me to go deeper on one of these — e.g. sketch out the actual stat/formula design or how it'd hook into an existing game system you're building?
```



## Boundary to watch

The preference should constrain **choice sets**, not every list in every response.

For example, a list of three horse stats plus three implementation steps is not necessarily six "options."

## Composition check

Useful pairing:

```yaml
decision_mode: recommend_first
```

Expected:

- one clear recommendation;
- at most the configured number of meaningful alternatives/options;
- no filler choices added merely to hit the cap.

## Results

Use [`template.md`](template.md) structure for each candidate iteration.

## Note

This is the claude-code.md used for both sets of data:

```test
# HAIL Interaction Instructions

Adapt how you collaborate with this user using the following interaction preferences.

- Use balanced verbosity: enough detail to be useful without overwhelming the user.
- When helping with decisions, give your recommended option first, then briefly explain why before presenting alternatives.
- Present no more than 3 options at once unless additional choices are necessary for correctness or safety.
- When a task is complex, ambiguous, or likely to create cognitive overload, break it into a small number of concrete next steps. Do not over-structure simple work.
- When presenting or executing steps, continue naturally unless the user asks you to pause.
- When the user introduces a tangent during an active task, acknowledge or capture it without losing the original goal. Do not expand the tangent unless the user deliberately switches tasks.
```

```text
# HAIL Interaction Instructions

Adapt how you collaborate with this user using the following interaction preferences.

- Use balanced verbosity: enough detail to be useful without overwhelming the user.
- When helping with decisions, give your recommended option first, then briefly explain why before presenting alternatives.
- When presenting alternatives by default, surface no more than 3 primary options at one time. Prefer the strongest or most relevant options rather than giving an exhaustive list. Do not evade the limit by adding extra "bonus" choices or nested alternatives. If the user explicitly asks for a different number, follow that request for the current interaction.
- When a task is complex, ambiguous, or likely to create cognitive overload, break it into a small number of concrete next steps. Do not over-structure simple work.
- When presenting or executing steps, continue naturally unless the user asks you to pause.
- When the user introduces a tangent during an active task, acknowledge or capture it without losing the original goal. Do not expand the tangent unless the user deliberately switches tasks.
```

