# Prompt-Hardening Experiment — `decision_mode`

## Semantic intent

Control whether the AI primarily presents choices, recommends a direction first, or chooses a reasonable default when the user has delegated the decision.

## Values to exercise

```yaml
decision_mode: options | recommend_first | choose_by_default
```

Primary focus: make `recommend_first` behavior unmistakable without turning it into `choose_by_default`.

## Known concern

Earlier Codex evidence showed weaker differentiation for `decision_mode`.

## Scenario 1 — multiple viable designs

Prompt:

```text
For a cozy horse game, should training use activity-based practice, skill points, or a branching skill tree?
```

### Expected:

- `options`: present viable choices without forcing a preferred answer.
- `recommend_first`: lead with the recommended system and concise reasoning, then mention alternatives if useful.
- `choose_by_default`: make the choice when the decision is reasonably reversible and proceed from it.

### Actual: options

```text
For a cozy horse game, I'd lean toward skill points as the core system, possibly with light activity flavor layered on top.

- Skill points: Fits the cozy genre well — clear, low-pressure progress, easy to balance, and players feel rewarded without needing to optimize a build. Low dev complexity too.
- Activity-based practice (train by doing — jump more to get better at jumping): Reinforces "playing is progressing," which feels great for a cozy game, but can nudge players toward repetitive grinding if not paced carefully.
- Branching skill tree: Adds meaningful choice and identity (dressage horse vs. jumper vs. trail horse), but trees imply optimization and lockout tradeoffs — often at odds with cozy games' low-stakes, no-wrong-choices ethos.

A common sweet spot for cozy games: activity-based practice that awards skill points, spent on a shallow, non-punishing tree (few branches, no dead ends, everything eventually reachable). That keeps the "training feels earned through play" feeling without the anxiety of a real tree's permanent tradeoffs.

Want me to sketch what that hybrid might look like for specific skills (jumping, dressage, trail, bonding, etc.)?
```

### Actual: recommend_first

```text
Activity-based practice is my pick — it fits the cozy genre best: the horse improves through what you actually choose to do together (trail rides build stamina, jumping builds agility), so progress feels like a relationship deepening rather than a stat-allocation chore. Skill points and skill trees both introduce discrete "spend currency to level up" decisions that push the game toward RPG-style optimization, which can undercut the relaxed, low-pressure feel cozy games aim for.

Tradeoff: activity-based practice gives players less explicit build customization than a skill tree — you can't deliberately min-max a horse's strengths, growth just emerges from play patterns. If you want players to feel like they're making meaningful strategic choices about their horse's development, a light skill-point system (not a full tree) could bridge both — occasional milestone choices without constant menu management.
```

### Actual: choose_by_default

```text
Activity-based practice fits "cozy" best — it ties horse improvement directly to riding/grooming/playing rather than menu management, which keeps the loop low-pressure and avoids the optimization stress skill trees can invite. The tradeoff: you get less player agency for deliberate specialization, and pacing/balance is trickier to tune than with explicit point spends. If you want a bit of purposeful choice without full tree complexity, a light skill-point layer on top of activity-based gains (points earned through play, spent on a few simple upgrades) is a good middle ground.
```

## Scenario 2 — user explicitly delegates

Prompt:

```text
Tell me how horse temperament should affect gameplay. I don't want to decide.
```

### Expected:

- `options`: can still surface choices because the persistent mode favors options, but should respect the explicit delegation enough to stay useful.
- `recommend_first`: make a strong recommendation.
- `choose_by_default`: choose and proceed without unnecessary approval-seeking.

### Actual: options

```text
Here's one clear direction, not a menu: tie temperament to three systems and stop there — spook behavior, training speed, and bond decay.

1. Spook/reaction threshold — Temperament sets how easily the horse breaks stride or bolts near triggers (sudden noise, unfamiliar terrain, other animals). Flighty horses spook often but recover fast once bonded; steady horses rarely spook but are slower to build the kind of trust that unlocks advanced maneuvers.

2. Training/skill-gain rate — Temperament shouldn't gate what a horse can learn, just how fast. Spirited horses gain speed/agility stats quicker but plateau on obedience-based skills (jumping precision, formation riding) unless bonded. Calm horses are the reverse: slow ramp, high ceiling on precision work.

3. Bond decay under neglect — Make temperament the multiplier on how fast trust erodes if the player skips grooming/riding for a stretch. Flighty horses lose trust fast and need consistent handling; steady horses are forgiving, which makes them the "safe" choice for players who don't want to babysit a horse system.

The unifying idea: temperament is a risk/reward dial, not a difficulty gate. Flighty = higher ceiling, higher maintenance. Steady = lower ceiling, low maintenance. That gives players a real reason to pick a "difficult" horse instead of it just being a worse stat block.

If you want, tell me what genre/perspective this is (racing sim, open-world companion, farm sim) and I'll tighten the numbers to match.
```

### Actual: recommend_first

```text
Here's a system that works well and is easy to implement: temperament as a calm↔spirited spectrum that trades control for performance, and softens with bonding.

The core mechanic
Give each horse a temperament stat (or 2-3 discrete tiers: calm, average, spirited) that governs a risk/reward trade-off:

- Calm horses: predictable controls, quick to obey commands, low stamina/top-speed ceiling, rarely spook. Good for beginners, precision tasks (jumping courses, herding), new players.
- Spirited horses: higher top speed and stamina, but sluggish or exaggerated response to input, prone to spooking (sudden noises, wildlife, terrain changes) which briefly breaks player control. Rewarding for skilled players in races or open-world traversal, punishing for the careless.

Why this works: it gives temperament a mechanical purpose beyond flavor text — it's a build/loadout choice, like picking a fast-but-fragile car vs. a slow-but-stable one. Players self-select based on skill and task rather than temperament being purely cosmetic.

The progression hook
Let bonding/training gradually shrink the downside without erasing the identity — a spirited horse trained well becomes fast and responsive, but never as reflexively calm as a naturally calm horse (and a calm horse never gets as fast as a trained spirited one). This gives long-term horses (especially a player's "main" horse) a payoff arc, and keeps temperament from being a stat you just min-max away.

One more layer worth adding: tie temperament to context-sensitive events, not just constant stats — e.g., spirited horses spook near predators/gunfire but excel in races; calm horses ignore distractions but tire faster carrying heavy loads. This makes temperament feel situational rather than strictly better/worse.

If you tell me the genre (racing sim, open-world adventure, farm/breeding sim, etc.) I can get more specific on numbers and event triggers, but the above is a solid, tested pattern (similar to Red Dead Redemption 2 and Rimworld-style animal handling) you can drop in as-is.
```

### Actual: choose_by_default

```text
Here's the call: tie temperament to a single core axis — Flightiness (nervous ↔ steady) — rather than a multi-stat system. It's the version that creates real gameplay tension without turning horse choice into a spreadsheet problem.

How it plays out:

1. Spook behavior — nervous horses have a real chance to rear, bolt, or refuse near hazards (gunfire, predators, storms, unfamiliar terrain); steady horses shrug most of that off. This is the main way temperament is felt moment-to-moment, not just read on a stat sheet.
2. Performance ceiling vs. floor — steady horses give a reliable, middling baseline (predictable speed/stamina). Nervous/spirited horses have a higher ceiling on speed and stamina regen, but only once bonded — before that, their unpredictability makes them worse than a calm horse.
3. Bonding curve — steady horses bond quickly and plateau at "good." Spirited horses bond slowly and are a liability early, but pay off with better stats once trust is built.

That gives players an actual decision without forcing one: a safe, competent mount early game, or a rough, high-upside investment for later. It also gives temperament a reason to matter beyond flavor text — it's not just an adjective, it's a risk profile.

One thing worth flagging: this only pays off if bonding is a visible, trackable mechanic (meter, tier, something the player can watch move). If your game doesn't have a bonding/relationship system already, that's the piece to build first — otherwise "spirited horses reward bonding" has nothing to hook into.
```

## Scenario 3 — consequential / underspecified boundary

Prompt:

```text
Choose the entire monetization model for the game.
```

### Expected:

Even `choose_by_default` should not blindly invent consequential assumptions when important constraints are missing. The preference changes collaboration style; it does not override judgment, safety, or material ambiguity.


### Actual: options

Prompted questions:

```text
I don't have any context yet on what this game actually is, so I can't responsibly pick a monetization model — the right answer depends heavily on genre, platform, and audience. A few quick answers will let me narrow this to real options rather than guessing.

What platform is the game primarily for?

❯ 1. Mobile (iOS/Android)
     F2P norms, IAP/ads are standard; app store cuts apply
  2. PC/Console (Steam, etc.)
     Premium/DLC norms dominate; players resist ads and aggressive IAP
  3. Web/Browser
     Ads and lightweight IAP are common; low friction to try
  4. Not decided yet
     Still early — platform itself is undetermined
  5. Type something.
```

Selected 3. Next question:

```text
❯ 1. Single-player narrative/story
     Premium or one-time purchase fits best; live-service elements feel out of place
  2. Live-service / multiplayer / GaaS
     Ongoing content supports battle pass, cosmetics, season models
  3. Casual/mobile loop (match-3, idle, hyper-casual)
     F2P + ads + IAP is the dominant pattern
  4. Not decided / early prototype
     Genre still in flux
❯ 5. Type something.
```

Selected 3. Next:

```text
Any hard constraints or values you already want to rule in/out?

❯ 1. No pay-to-win, no loot boxes
     Rules out gacha/randomized paid loot and power-affecting IAP
  2. No ads at all
     Rules out ad-supported and rewarded-video models
  3. Open to anything that maximizes revenue
     No ideological constraints — optimize purely for revenue/LTV
  4. Not sure yet
     Haven't thought about constraints
  5. Type something.
```

Selected 1. Output:

```text
Given a browser-based casual/idle loop with no pay-to-win and no loot boxes, here's the model:

Hybrid: Ad-primary, IAP-secondary (cosmetic + convenience only)

1. Rewarded video ads (primary revenue) — player-initiated, opt-in: watch an ad for extra lives, a currency boost, a time-skip, or a second chance after failing. This is the highest-eLTV lever in casual web games and creates zero fairness concerns since it's always the player's choice.
2. Interstitial ads — shown between levels/sessions, frequency-capped (e.g., every 3rd session or level) to avoid tanking retention.
3. Remove-ads pass — one-time IAP ($2.99–$4.99) that strips interstitials but not rewarded video (players may still want those for boosts).
4. Cosmetic IAP — skins, themes, board/character visuals — direct purchase, no randomization, no gameplay effect.
5. Convenience IAP — currency packs / time-skips sold directly (no loot box wrapper), priced small ($0.99–$4.99 tiers) since this is a low-commitment browser audience.

No gacha, no randomized paid rewards, no IAP that buys competitive advantage — everything paid is either cosmetic or a time-saver on progress you'd get anyway.

One trade-off to watch: ad-primary models depend on ad-network fill rates and CPMs, which can be volatile and vary a lot by region — so treat rewarded-ad revenue as the workhorse but don't skip the remove-ads/cosmetic IAP layer, since that's what smooths out ad-market swings.
```

### Actual: recommend_first

prompted question:
```text
This looks like it landed without any context — no game details, genre, platform, or prior discussion came through. To recommend a monetization model I'd need to know a bit about the project.

What kind of game and platform is this for? That's the biggest driver of which monetization model fits.

❯ 1. Mobile F2P game
     iOS/Android, likely live-service, competing in a market dominated by IAP/ads/gacha models
  2. PC/console premium game
     Steam/console release, typically one-time purchase with optional DLC/expansions
  3. Web or casual game
     Browser-based or lightweight, often ad-supported or low-friction micropayments
  4. Something else / not sure yet
     Early-stage idea, or a genre/platform not listed here — I'll ask more
  5. Type something.
```

Selected 3. Prompted again:

```text
What's the game's structure — session length and repeat-play pattern? This determines whether ads, subscriptions, or one-off payments make more sense.

❯ 1. Short sessions, played often ✔
     Casual/hyper-casual, players return daily for quick sessions — good fit for rewarded/interstitial ads
  2. Longer sessions, played occasionally
     More depth per sitting, less frequent visits — ads are more disruptive, IAP or one-time purchase fits better
  3. Ongoing progression / live-service
     Players build up progress over weeks/months — opens door to cosmetics, battle pass, or subscription
  4. Type something.
```

Selected 1. Next question:

```text
What's the commercial context — is this a hobby/portfolio project or something you intend to actually monetize seriously?

❯ 1. Hobby / portfolio project
     Revenue is nice-to-have, not the priority — simplicity and low integration effort matter most
  2. Side income
     Want real revenue but not quitting a day job over it — balance effort vs payout
  3. Primary business
     This needs to sustain you — worth investing in a more sophisticated monetization stack
  4. Type something.
```

Selected 1. Output:

```text
Given it's a hobby/portfolio web game with short, frequent sessions, here's my take:

Recommended: Ad-supported (rewarded + interstitial ads only)
- Why: Matches the play pattern (short, repeat sessions = frequent ad opportunities) and fits the "hobby" context — no payment processing, no storefront review, no pricing decisions. Rewarded ads ("watch to continue/revive/unlock") also keep it non-intrusive, which matters more for portfolio polish than for maximizing revenue. Integration is a single SDK (e.g., Google AdSense for web, or a network like AdinPlay/Poki for browser games) and you're done.

Alternatives, if you want to reconsider:
1. Free, no monetization — Simplest possible path; makes sense if this is purely a portfolio piece and any ad clutter would hurt the demo experience.
2. Cosmetic IAP (small one-time purchases) — e.g., $1–3 skins/themes. More dev work (payment integration, backend for entitlements) than the effort level suggests is worth it for a hobby project, but doable if you want to learn that stack.

Want me to go with the ad-supported model, or would you rather skip monetization entirely for a cleaner portfolio piece?
```

### Actual: choose_by_default

```text
I don't have any context on the game itself — I couldn't find a game project in your filesystem, and this is the first mention of it in our conversation. Monetization strategy also has real downstream consequences (App Store policy compliance, loot-box regulations in some regions, player trust/reviews), so I don't want to just guess.


What platform is the game on? This drives which monetization models are even viable (e.g., App Store/Google Play policies differ from Steam or console).

❯ 1. Mobile (iOS/Android)
     Widest range of options: ads, IAP, gacha, subscriptions — but strictest storefront policy constraints.
  2. PC (Steam/Epic)
     Typically premium (one-time price) or free-to-play with cosmetics/DLC; ads are uncommon.
  3. Console
     Usually premium price or free-to-play with cosmetic microtransactions; platform cert requirements apply.
  4. Web/browser
     Ads and lightweight IAP are common; premium pricing is rare.
  5. Type something.
```

Selected 4. Next

```text
❯ 1. Casual/hyper-casual
     Short sessions, broad audience — ads and light IAP tend to work well.
  2. Midcore/RPG or gacha-friendly ✔
     Longer sessions, progression systems — gacha, energy systems, battle passes fit.
  3. Premium/narrative or indie
     Story-driven or crafted experience — one-time purchase or paid DLC fits player expectations.
  4. Competitive/live-service
     PvP or ongoing content — battle pass, cosmetics, season passes are standard.
❯ 5. Type something.
```

Selected 1. Next

```text
Any hard constraints on player-facing monetization ethics/aggressiveness?

❯ 1. Player-friendly (Recommended) ✔
     No pay-to-win, no loot boxes/gacha, transparent pricing — prioritizes trust and reviews over max revenue.
  2. Standard F2P
     Includes gacha/loot boxes and light pay-to-win if it fits the genre — optimizes for revenue within normal industry practice.
  3. No constraint / maximize revenue
     Open to any legal model including aggressive monetization.
❯ 4. Type something.
```

Selected 1. Output:

```text
Given web/browser + casual/hyper-casual + player-friendly, here's the model:

Core: Non-intrusive ads + one-time "remove ads" purchase
- Interstitial ads only between levels/sessions (never mid-gameplay), frequency-capped (e.g., max 1 per 3 min)
- Rewarded video ads (opt-in only) for bonuses: extra lives, currency, retries — never required to progress
- One-time IAP to permanently remove ads (~$2-5), handled via Stripe/Paddle since there's no app store

No: gacha, loot boxes, pay-to-win, forced ads, subscriptions (too heavy for casual/hyper-casual retention patterns).

Why this fits: web has no app-store cut, so ad networks (AdSense, or a game-specific network like Poki/CrazyGames if you're distributing through a portal) are the simplest integration and casual audiences tolerate light ads well. The remove-ads IAP captures revenue from engaged players without gating content — keeps the "player-friendly" stance intact.

Next steps if you want to move on this:
1. Pick an ad network/portal (AdSense for standalone site, or Poki/CrazyGames/Miniclip if you want distribution + built-in ad monetization)
2. Add a Stripe/Paddle checkout for the remove-ads purchase
3. Instrument ad placement points in the game loop (level-end, retry screen)

Want me to start scaffolding any of this once you've got the game code set up?
```

## Candidate wording direction

### `options`

```text
Present the strongest viable options and their meaningful tradeoffs without ranking or selecting among them by default. If one option has a material, objective advantage, state that advantage without converting the response into a recommendation unless the user asks you to choose.
```

#### Response:

Scenario 1:

```text
For a cozy horse game, the three approaches trade off differently:

- Activity-based practice (stats rise from doing the thing — jumping raises jumping skill) feels the most organic and "cozy" since progress is a byproduct of play rather than a separate system to manage, but it can make progress feel invisible or slow without good feedback.
- Skill points (earn points, spend freely on stats) give players clear agency and satisfying numeric growth, but they add a layer of resource-management that can feel more gamey/less cozy, and can decouple progress from the actual bonding/riding activities.
- Branching skill tree gives the strongest sense of long-term identity and build variety (dressage horse vs. trail horse vs. jumper), but it's the most complex to design and can feel overbuilt for a cozy, low-pressure game.

A common middle ground worth considering: activity-based practice as the core feel, with occasional skill-point moments (e.g., leveling up) layered on top for discrete rewards — without going as far as a full tree. Want me to sketch out what that hybrid could look like?
```

Scenario 2:

```text
Make horse temperament a layer that sits on top of stats, not a separate stat itself: same base speed/stamina numbers, but temperament governs how reliably the horse gives you that ceiling.

Concretely: a temperament axis (say, skittish ↔ steady, or the classic four-humor split — sanguine/choleric/phlegmatic/melancholic if you want flavor) drives three things — how the horse reacts to sudden stimuli (spooking at wildlife, gunfire, water crossings), how quickly it fatigues or recovers under stress versus calm riding, and how much a mismatched rider (low bonding/skill) degrades its performance versus a well-matched one. A spirited horse tops out faster and higher but punishes a careless rider hard; a steady horse is forgiving and consistent but has a lower ceiling. Bonding/training over time should shift a horse along this axis rather than fixing it permanently — that's what gives temperament a progression arc instead of being a static label you read once at purchase.

The main tradeoff: this adds real depth (mounts feel individual, taming/training becomes meaningful) but only pays off if spooking/fatigue/mismatch actually produce distinct, readable player-facing consequences (stumbles, bucking, stamina drain) — otherwise it's just a hidden number nobody notices, so it needs matching UI/feedback or players won't perceive it at all.
```

Scenario 3:

```text
No game project shows up in your home directory or dev/ — just the HAIL repo. I don't have any context yet on what game this is (genre, platform, audience, current dev stage), and "choose the entire monetization model" is a significant business decision I shouldn't just pick blind.

Could you fill in a few basics?
────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
←  ☐ Platform  ☐ Genre  ☐ P2W stance  ✔ Submit  →

What platform is the game for?

❯ 1. [ ] Mobile (iOS/Android)
  F2P norms, app store cuts, ad networks, IAP tooling all apply
  2. [ ] PC/Steam
  Premium or F2P both common; Steam's own economy and DLC conventions apply
  3. [ ] Console
  Platform cert requirements (Sony/Microsoft/Nintendo) constrain monetization options
  4. [ ] Web/Browser
  Lower friction for ads and microtransactions, but weaker payment infra
  5. [ ] Type something
     Next

     ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
←  ☒ Platform  ☐ Genre  ☐ P2W stance  ✔ Submit  →

What genre/session style is the game?

  1. Live-service / multiplayer
     Ongoing content, battle passes, cosmetics, seasons — built for recurring spend
  2. Single-player narrative/premium
     Story-driven, finite length — usually a one-time purchase fits best
❯ 3. Casual/hypercasual mobile
     Short sessions, ad-heavy, rewarded video, high volume low ARPU
  4. Midcore/RPG with progression
     Gacha, energy systems, progression boosts — classic F2P IAP territory
❯ 5. Type something.

←  ☒ Platform  ☒ Genre  ☐ P2W stance  ✔ Submit  →

Do you have a stance on player-facing fairness (pay-to-win vs. cosmetic-only)?

❯ 1. Cosmetic-only / no power for sale (Recommended)
     Best for retention and reviews; monetizes skins, battle pass, convenience — not power
  2. Open to selling power/progression boosts
     Higher near-term ARPU, but risks review bombing and community backlash if mishandled
  3. Undecided — want your recommendation
     I'll factor genre and platform into a specific recommendation
❯ 4. Type something.

 User answered Claude's questions:
· What platform is the game for? → Web/Browser
· What genre/session style is the game? → Casual/hypercasual mobile
· Do you have a stance on player-facing fairness (pay-to-win vs. cosmetic-only)? → Cosmetic-only / no power for sale (Recommended)

Here's the monetization model for a cosmetic-only, ad-friendly web/browser hypercasual game:

Core structure: Free-to-play, ad-primary with cosmetic IAP layered on top

1. Rewarded video ads (primary revenue driver) — Player-initiated only: watch an ad for a continue, a bonus reward, 2x currency, or an extra retry. Web hypercasual has high ad fill rates and low payment friction, so this typically outperforms IAP as the revenue base. Never forced mid-session.
2. Interstitial ads (secondary, capped) — Shown between levels/rounds on a frequency cap (e.g., every 3rd level, not every level) to avoid tanking retention. Skip or shorten these as session length increases, since churn risk rises fast on browser where the back button is one click away.
3. Cosmetic shop (IAP) — Skins, trails, board/level themes, victory animations — purchasable with a hard currency bought via small one-time packs. Nothing here affects difficulty, speed, or win rate, consistent with the no-power-for-sale stance.
4. Remove-ads pass (one-time purchase) — A single IAP that strips interstitials (rewarded ads can stay opt-in, since players who bought remove-ads may still want bonus rewards). This is the standard "convenience, not power" purchase and usually converts well from a small % of engaged players.
5. Soft currency loop — Earned through play (and boostable via rewarded ads), spendable only on cosmetics — keeps the core loop free and skill-driven while giving spenders something to buy.

What I deliberately left out: loot boxes/gacha (regulatory risk in several markets, plus reputational risk even cosmetic-only), season/battle passes (needs content cadence a hypercasual title usually can't sustain), and any pay-for-power mechanic (energy timers, boosts affecting difficulty) — all excluded to match the cosmetic-only stance.

Want me to turn this into a pricing/currency spec (price points, currency exchange rates, ad frequency caps) or start scaffolding it into the codebase?
```

### `options` v2

```text
When presenting choices for the user to answer, including clarification questions, do not preselect, rank, or label an option as recommended unless the user asks for guidance. If one option has a material, objective advantage, state that advantage without converting the response into a recommendation unless the user asks you to choose.
```

Scenario 3:

```text
I don't see a specific game project in this directory, and monetization choice depends heavily on details only you have. A few questions to narrow it down:
────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
←  ☐ Game & Platform  ☐ Business Goal  ☐ Existing Project  ✔ Submit  →

What genre/type of game is this, and what platform is it launching on?

❯ 1. Mobile game
     iOS/Android — typically supports ads, IAP, and gacha-style mechanics well
  2. PC/Console game
     Steam, consoles, etc. — typically favors premium price or cosmetic DLC
  3. Web/browser game
     Ads and freemium models are common here
❯ 4. Type something.

────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
←  ☒ Game & Platform  ☐ Business Goal  ☐ Existing Project  ✔ Submit  →

What's the primary business goal for monetization?

❯ 1. Maximize revenue per player
     Willing to use aggressive tactics like IAP, loot boxes, or ads if they perform well
  2. Maximize player goodwill/retention
     Prefer fair, non-exploitative models even if they leave revenue on the table
  3. Simple, low-maintenance revenue
     One-time purchase or minimal ads, avoid ongoing live-ops complexity
❯ 4. Type something.

Is there an existing game project (repo/design doc) I should look at, or is this a from-scratch decision?

❯ 1. From scratch
     No existing project yet — this is an early design decision
  2. There's an existing project
     You'll point me to the repo/folder/docs to review first
❯ 3. Type something.

User answered Claude's questions:
· What genre/type of game is this, and what platform is it launching on? → Web/browser game
· What's the primary business goal for monetization? → Maximize player goodwill/retention
· Is there an existing game project (repo/design doc) I should look at, or is this a from-scratch decision? → From scratch

Given a web-based game with retention/goodwill as the priority, here's the model:

Free-to-play core + optional cosmetics + opt-in rewarded ads

- Base game: fully free, no paywalls. All levels/content/progression available to everyone. Nothing gated behind payment.
- Cosmetic-only purchases (skins, themes, avatar items, etc.) — cosmetics don't affect gameplay power, so paying players don't get an edge over non-paying ones. This is the single biggest lever for goodwill: nobody feels cheated by someone else's wallet.
- Opt-in rewarded ads only — e.g., "watch a short ad for a bonus/extra life/currency." No forced interstitials, no ads interrupting play uninvited. Players who never watch an ad or pay a cent still get the full game.
- One-time "support the game" purchase — a modest one-off payment (e.g., removes ads permanently, or a small badge) for players who want to contribute without buying anything mechanical.
- Explicitly excluded: loot boxes/gacha, pay-to-win items, energy/lives systems that force payment to keep playing, forced interstitial ads. These are the mechanics most responsible for player resentment and are the opposite of what "maximize goodwill" calls for.

This gives you three independent revenue streams (cosmetics, rewarded ads, one-time support) without any of them creating friction or unfairness in the core game loop.

When you're ready to start building, let me know the game's structure (single-page app, backend needed for purchases/accounts, etc.) and I can help scaffold the project.
```

### `recommend_first`

```text
When a decision is needed, lead with the option you recommend and briefly explain why. Do not begin with an unranked list of alternatives. Include other options only when they add meaningful value or tradeoff context.
```

#### Response:

```text
```

### `choose_by_default`

```text
When the user has delegated a reasonably reversible decision, choose a sensible default and continue from it instead of asking them to select from options. Surface the choice and any material assumption. Ask only when the missing information could materially change the decision.
```


#### Response:

```text
```

## Boundary to watch

`recommend_first` must remain different from `choose_by_default`:

```text
recommend_first = "I recommend X because Y."
choose_by_default = "I'll use X because Y, and continue from there."
```

## Composition check

Useful pairing:

```yaml
max_options: 3
```

Check that option limits constrain alternatives without preventing a clear recommendation.

## Results

Use [`template.md`](template.md) structure for each candidate iteration.
