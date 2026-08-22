# Milestone 2 — Codex raw results

Use this file to paste the complete Codex responses for the same A/B scenarios used in Milestone 1.

# Decision behavior
Prompt:
> I could use Redis, Postgres, or an in-memory cache. What should I use?

## Test A
```
• Use Postgres by default. It gives you durable storage, transactions, querying, and fewer moving parts. Add
  another system only when a measured need justifies it.

  - Postgres: Best source of truth—application data, jobs, sessions requiring durability, and even moderate
    caching via materialized views or indexed tables.

  - In-memory cache: Best for a single-process app and disposable data. It’s simplest and fastest, but each
    instance has different contents, and restarts erase everything.

  - Redis: Best when multiple app instances need shared, low-latency state—distributed caching, rate limits,
    locks, counters, queues, or expiring sessions.

  A practical progression is: Postgres → local in-memory cache → Redis when you need shared cache or coordination.
  Don’t make Redis the only copy of important data.
```

## Test B
```
• Use Postgres by default. It gives you durable storage, transactions, querying, backups, and fewer moving parts. Add Redis only when measurements show you need capabilities Postgres or local
  memory cannot provide.

  - Postgres: Source of truth, durable data, shared state, moderate caching via materialized views or cached tables.
  - In-memory cache: Fastest and simplest for disposable, per-process data. Best for small deployments; unsuitable when instances must share cache state.
  - Redis: Shared low-latency cache, sessions, rate limits, counters, queues, locks, or TTL-heavy data. Adds infrastructure and consistency concerns.

  A practical progression is:

  1. Start with Postgres.
  2. Add a bounded in-memory cache for hot, safe-to-lose values.
  3. Introduce Redis when you scale across processes or need Redis-specific primitives.

  Never make a cache the only copy of important data. If you tell me what you’re storing, your traffic level, and whether you run multiple app instances, I can make the choice concrete.
```

# Option limiting
Prompt:
> I have Saturday free and want to learn something new. What could I do?

## Test A
```
• I’d recommend a one-day “make something” challenge—it’s more memorable than passively studying.

  - Build a tiny website or app: choose a personal problem, follow one short tutorial, then customize the result. Aim for a finished, imperfect project by dinner.
  - Learn a physical skill: try a beginner class in pottery, woodworking, cooking, climbing, photography, or dance.

  A good Saturday rhythm: learn for 90 minutes, practice for 2–3 hours, then create or demonstrate one finished thing.

  Tell me your interests, budget, and whether you’d rather stay home or go out, and I’ll design a specific Saturday plan.
```

## Test B
```
• Here are six satisfying one-day learning projects—each produces a visible result by evening:

  - Build a tiny website: Make a personal homepage, recipe page, or interactive quiz with HTML/CSS/JavaScript. Great if you enjoy creating and problem-solving.
  - Cook one unfamiliar cuisine: Choose a dish that teaches a technique—fresh pasta, handmade dumplings, Thai curry, or sourdough. Shop in the morning and invite someone to dinner.
  - Learn practical photography: Study composition and exposure for an hour, take a themed photo walk, then edit your best five images.
  - Try a hands-on craft: Pick up basic bookbinding, watercolor, crochet, wood carving, or visible mending. Aim to finish one small object.
  - Explore your local history: Research a neighborhood, building, or historical event, visit relevant locations, and create a short illustrated account of what you discover.
  - Learn basic electronics: Use an Arduino or microcontroller starter kit to make an LED react to a button, light, or temperature. This is especially fun if you like tangible experiments.

  A good Saturday rhythm is: learn the fundamentals before lunch, build or practice during the afternoon, and finish with something you can share.

  Tell me what sounds most like you—creative, technical, physical, culinary, or outdoorsy—and whether you want to stay home or go somewhere, and I’ll turn it into a specific Saturday plan.
```

# Task chunking
Prompt:
> I need to clean my room, my living room, kitchen, and 3 bathrooms. Help me get started.

## Test A
```
• Start with a quick 10-minute reset—momentum matters more than choosing the perfect room.

  1. Grab a trash bag and laundry basket.
  2. Set a 10-minute timer.
  3. Begin in your room:
      - Trash goes in the bag.
      - Clothes go in the basket.
      - Dishes go by the door.
      - Don’t organize yet.

  4. When the timer ends, take dishes to the kitchen and trash to the bin.

  Then tell me “done”, and I’ll give you the next short round.
```

## Test B
```
• Let’s make the start tiny: grab a trash bag and set a 10-minute timer.

  Begin in your room. During those 10 minutes, collect only obvious trash—no organizing, folding, or decision-making. When the timer ends, take the bag out and come back here. I’ll give you the
  next move.
```

# Verbosity
Prompt:
> Explain why leaves change color in the fall.

## Test A
```
• As autumn days get shorter and temperatures drop, trees stop producing chlorophyll—the green pigment leaves use to capture sunlight for photosynthesis. Existing chlorophyll breaks down,
  revealing pigments that were already present:

  - Carotenoids produce yellow and orange.
  - Anthocyanins, made by some trees in autumn, produce red and purple.
  - Tannins contribute brown.

  Weather affects the intensity: sunny days and cool—but not freezing—nights often create brighter reds, while drought or an early frost can make colors duller or shorten the display. Eventually,
  the tree seals off each leaf’s connection and sheds it, conserving water and energy through winter.
```

## Test B
```
• Leaves change color in fall because shorter days and cooler temperatures signal trees to prepare for winter.

  Trees stop producing chlorophyll—the green pigment that helps leaves capture sunlight for photosynthesis—and reclaim useful nutrients from it. As chlorophyll breaks down, other pigments already
  present become visible:

  - Carotenoids produce yellow and orange colors.
  - Anthocyanins, made by some leaves in autumn, produce red and purple colors.
  - Tannins contribute brown shades.

  Weather affects the display: sunny days and cool, frost-free nights often produce vivid reds, while drought, early frost, or warm weather can make colors duller or shorten the season.
  Eventually, the tree seals off each leaf’s connection to the branch, causing it to fall and helping the tree conserve water and energy through winter.
```

# Tangent Handling
Prompt (1):
> Help me plan what I need to clean today: kitchen, bedroom, living room, and bathrooms.

After response, additional prompt (2):
> Oh also I've been thinking about learning Spanish.

## Test A
Prompt (1)
```
• Recommended order: bathrooms → bedroom → living room → kitchen. Finish with the kitchen so you can clean up after breaks and meals.

  1. Bathrooms — 20–30 minutes
      - Apply toilet and shower cleaner first.
      - Clear counters and empty trash.
      - Clean mirrors, sinks, toilets, and showers/tubs.
      - Sweep and mop floors.

  2. Bedroom — 20 minutes
      - Make the bed.
      - Put away clothes and clutter.
      - Dust surfaces.
      - Vacuum or sweep.

  3. Living room — 20 minutes
      - Gather trash and return misplaced items.
      - Straighten cushions and blankets.
      - Dust surfaces and electronics.
      - Vacuum or sweep.

  4. Kitchen — 30–40 minutes
      - Load or wash dishes.
      - Put away food and clear counters.
      - Wipe counters, stovetop, sink, and appliance fronts.
      - Take out trash.
      - Sweep and mop.

  Work in focused 20-minute rounds with 5-minute breaks. Aim for “clean and functional” first; only deep-clean if you still have energy. If the “4” means four bathrooms, clean all toilets and
  sinks today, then deep-clean only the most-used bathroom.
```

Prompt (2)
```
• Great goal—let’s capture it without derailing today’s cleaning plan.

  After cleaning, spend just 10 minutes on Spanish:

  - Choose one beginner resource, such as Language Transfer or Duolingo.
  - Complete one short lesson.
  - Learn five useful phrases, starting with hola, gracias, por favor, ¿cómo estás?, and no entiendo.

  For consistency, aim for 10–15 minutes daily. Today’s priority remains the four cleaning areas; Spanish can be your small reward afterward.
```

## Test B
Prompt (1)
```
• Here’s a practical cleaning order that avoids redoing work:

  1. Bedroom — 20–30 minutes
      - Put away clothes and clutter
      - Change the bedding
      - Dust surfaces
      - Empty trash
      - Vacuum or sweep

  2. Bathrooms — 20 minutes each
      - Apply toilet and shower cleaner first
      - Clear and wipe counters and mirrors
      - Scrub the toilet, sink, tub, and shower
      - Replace towels and empty trash
      - Mop the floors

  3. Living room — 20–30 minutes
      - Gather items that belong elsewhere
      - Straighten cushions and blankets
      - Dust furniture and electronics
      - Wipe tables and other surfaces
      - Vacuum or sweep

  4. Kitchen — 30–45 minutes
      - Wash or load dishes
      - Put away food and clear counters
      - Wipe appliances, counters, and cabinet fronts
      - Clean the sink and stovetop
      - Take out trash
      - Sweep and mop last

  Start a laundry load before cleaning, carry a basket for misplaced items, and work top-to-bottom so dust lands on floors before you vacuum. For a quicker reset, set a 20-minute timer per room
  and prioritize clutter, surfaces, trash, and floors.
```

Prompt (2)
```
That’s a good tangent—and cleaning is surprisingly useful for beginner Spanish practice. Today, label what you’re doing aloud:

  - la cocina — kitchen
  - el dormitorio — bedroom
  - la sala — living room
  - el baño — bathroom
  - limpiar — to clean
  - lavar — to wash
  - barrer — to sweep
  - ordenar — to tidy
  - Estoy limpiando la cocina. — I’m cleaning the kitchen.
  - Voy a limpiar el baño. — I’m going to clean el baño.

  For learning beyond today, a manageable routine is 15–20 minutes daily: about 10 minutes of a structured course, 5 minutes listening to beginner Spanish, then a few minutes speaking or writing
  from memory. Consistency matters much more than long sessions.

  A fun goal for today: describe each completed chore with one Spanish sentence.
```
