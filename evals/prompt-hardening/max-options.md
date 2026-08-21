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
Here are some horse breeds that would work well in a cozy horse game, each bringing a different vibe:

1. Shetland Pony — Small, stubborn, and full of personality. Great for comic relief or an early-game companion; their size makes them ideal for younger player characters or lighthearted quests.
2. Icelandic Horse — Known for the tölt (a smooth extra gait), plus a shaggy winter coat that looks great in snowy biomes. Fits a cozy, seasonal game with sledding or trail rides through frosty landscapes.
3. Andalusian — Elegant, flowing mane, dramatic movement. Perfect for a "dream horse" unlock or a dressage/show minigame — the breed players aspire to train up to.
4. Haflinger — Golden coat with a flaxen mane, sturdy and friendly. Reads as the quintessential "farm horse" — great for a homestead/ranch-building loop.
5. Appaloosa — Spotted coat patterns give strong visual variety for customization and breeding mechanics, since no two look alike.
6. Clydesdale — Gentle giant with feathered feet. Fun for cart-pulling, logging, or festival-parade activities, and a nice contrast in scale to the ponies.
7. Mustang — Wild and untamed backstory built in — good for a "bond with a wild horse" story arc or rescue/rehabilitation gameplay.
8. Fjord Horse — Distinctive dun coat and upright mane (often trimmed in a crescent shape), calm temperament. Visually unique and easy to make into a memorable mascot-like character.

A mix of a few small/sturdy breeds (Shetland, Haflinger), a couple of flashy/aspirational ones (Andalusian, Appaloosa), and one or two with built-in narrative hooks (Mustang, Fjord) would give you good gameplay and emotional variety without overloading players with choices early on.
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
Here are 10 horse name ideas:

1. Midnight Sonata
2. Copper Blaze
3. Whispering Wind
4. Thunder Ridge
5. Silver Comet
6. Amber Trot
7. Rebel's Charm
8. Northern Star
9. Velvet Storm
10. Golden Meadow
```

## Scenario 3 — nested option leakage

Prompt:

```text
What are some ways horse training could work in the game?
```

Watch for apparent compliance that actually produces many hidden alternatives through nested bullets, "bonus ideas," or sub-options.

Result:

```text
Recommended: a rhythm/timing-based groundwork-to-riding progression — the player builds trust through short timed/rhythm interactions (leading, lunging, grooming) before saddle training unlocks, and each session's timing accuracy raises a training stat. This works well for a cozy game because it's low-stakes, teaches the horse's personality through pacing, and gives players a visible sense of progress without punishing failure.

Alternatives:
- Stat-building through repetition — feeding, grooming, and practice sessions gradually raise stats (speed, stamina, obedience) with diminishing returns, more systems-driven and less about player skill.
- Relationship/trust unlocks — training is framed as bonding time; trust level gates which commands or tricks become available, emphasizing narrative connection over mechanical skill.

I'd lean toward the first — it gives you both a skill-based minigame and a stat-progression hook, and it composes naturally with the breed variety in Scenario 1's ideas.
```

## Candidate wording direction

For a value of 3:

```text
When presenting alternatives by default, surface no more than 3 primary options at one time. Prefer the strongest or most relevant options rather than giving an exhaustive list. Do not evade the limit by adding extra "bonus" choices or nested alternatives. If the user explicitly asks for a different number, follow that request for the current interaction.
```

### Scenario 1

```text
For a cozy horse game, I'd lead with the Shetland Pony — it's the strongest pick because cozy games thrive on charm and low-stakes accessibility, and a stubby, fluffy pony that's comically strong for its size gives you built-in comic relief, an easy "starter mount" for new players, and instant merchandising/plushie appeal.

A few strong alternatives to round out the roster:

- Friesian — jet-black, flowing mane, dramatic gait. Great as an unlockable "prestige" horse that rewards progression with pure visual spectacle.
- Icelandic Horse — has a unique extra gait (tölt) real-world riders love; translating that into smoother traversal or a special movement mechanic gives it mechanical identity, not just a reskin.
- Appaloosa — spotted coat patterns are naturally suited to a cosmetic/customization system, letting players feel ownership over a "one-of-a-kind" horse.

Want me to keep going with more breeds, or help design how coat/mechanic variety maps across a full roster?
```

### Scenario 2

```text
Here are 10 horse name ideas:

1. Midnight Comet
2. Thunder Ridge
3. Whiskey Trot
4. Silver Cascade
5. Copper Canyon
6. Storm Chaser
7. Velvet Ember
8. Rustic Gallop
9. Northern Lights
10. Blaze of Glory
```

### Scenario 3

```text
Recommended: a rhythm/timing-based groundwork-to-riding progression — short timed interactions (leading, lunging, grooming) build trust before saddle training unlocks, and timing accuracy raises a training stat. It fits a cozy game well because it's low-stakes, teaches horse personality through pacing, and gives visible progress without punishing failure.

Alternatives:
- Stat-building through repetition — feeding, grooming, and practice sessions raise stats (speed, stamina, obedience) with diminishing returns; more systems-driven, less skill-based.
- Relationship/trust unlocks — training is framed as bonding time; trust level gates which commands or tricks become available, emphasizing narrative connection over mechanics.

I'd lean toward the r
```

#### Observation

It appears that claude in vscode terminal keeps looking at this file. TIME TO REDO MORE AGNOSTIC! Committing where were at for due diligence then updating results correctly.

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
