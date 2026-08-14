I had to adjust prompts as I don't have code in my local computer.


# Decision behavior
Prompt:
> I could use Redis, Postgres, or an in-memory cache. What should I use?

## Test A
```
It depends mainly on one thing: **does the cached data need to survive a process restart and be shared across multiple app instances?**

**My default recommendation: start with an in-memory cache.** If you're running a single process (or can tolerate each instance having its own cache) and the data is cheap to recompute, an in-memory cache (a dict with TTLs, or a library like `lru_cache`/Caffeine/`node-cache`) is the simplest thing that works. No network hop, no extra infrastructure, no serialization. Don't add a service you don't yet need.

**Reach for Redis when you outgrow that** — specifically when:
- You have multiple app instances that must share the same cache
- The cache should survive restarts/deploys
- You need cache-specific features: TTL eviction, pub/sub, atomic counters, rate limiting, distributed locks

**Postgres is usually the wrong tool for a *cache*** — but it's the right answer if what you're really describing is durable state you'll query, not a disposable speed layer. If you already run Postgres and the data must be persistent and correct, keep it there rather than standing up Redis.

Quick rule of thumb:

| Need | Use |
|------|-----|
| Single instance, recomputable data | In-memory |
| Shared across instances, disposable | Redis |
| Durable source of truth | Postgres |

If you tell me what you're caching and whether you're running more than one instance, I can give you a firm answer instead of a framework.
```

## Test B
```
It depends on what you're caching and why. Here's how I'd decide:

## Quick decision guide

**In-memory cache** (e.g. a language-native map, Caffeine, `lru-cache`, `functools.lru_cache`)
- ✅ Single process/instance, or per-instance caching is fine
- ✅ You want the absolute lowest latency (nanoseconds, no network hop)
- ✅ Cache can be rebuilt cheaply on restart
- ❌ Doesn't survive restarts, doesn't share across instances, competes with your app for heap/RAM
- **Use when:** you're one service, data is derivable, and you just want to avoid recomputation.

**Redis**
- ✅ Multiple app instances need to *share* cache state
- ✅ You need TTLs, eviction policies, atomic counters, rate limiting, pub/sub, distributed locks, sessions, queues
- ✅ Sub-millisecond reads, survives app restarts (and can persist if configured)
- ❌ Another service to run/operate; network hop; not your source of truth
- **Use when:** you're horizontally scaled, or you need cache semantics beyond a plain map.

**Postgres**
- ✅ It's your **system of record** — durable, transactional, queryable
- ✅ You already run it and don't want another moving part
- ❌ Slower than Redis for pure key-value cache reads; caching *in* your primary DB can add load to the thing you're trying to protect
- **Use when:** the data needs durability/consistency/relational queries — i.e. it's not really a cache, it's data. (Postgres can serve as a "good enough" cache table for low-traffic cases, and `UNLOGGED` tables help, but it's rarely the right tool for a hot cache.)

## The usual answer

Most systems end up with **layered caching**: in-memory for the hottest per-instance lookups, Redis as the shared distributed cache, Postgres as the durable source of truth behind both.

To point you at the right one, the deciding factors are:

- **How many app instances?** One → in-memory may be enough. Many → Redis.
- **Must it survive restarts / be shared?** Yes → Redis (or Postgres if it's real data).
- **Is it actually a cache, or your source of truth?** Source of truth → Postgres.
- **Do you need cache-specific features** (TTL, eviction, atomic ops, pub/sub)? → Redis.

If you tell me what you're storing, your scale (instances/traffic), and whether losing it on restart is acceptable, I'll give you a single concrete recommendation.
```
# Option limiting
Prompt:
> Give me every reasonable option to learn something new

## Test A
```
Here's a broad menu, grouped so you can scan and pick a lane. I don't know your interests or how much time you have, so I've cast wide — tell me what pulls at you and I'll go deep on that one.

## Structured courses
- **MOOCs** — Coursera, edX, MIT OpenCourseWare, Stanford Online (often free to audit; pay for certificates)
- **Skill platforms** — Udemy, Khan Academy (free, K-12 through early college), Brilliant (math/science, interactive)
- **Tech-specific** — freeCodeCamp, The Odin Project, Codecademy, Frontend Masters
- **University credit** — community college classes, local university continuing-ed, audited lectures

## Learn by doing / making
- Pick a small project and build it (a website, a shelf, a spreadsheet tool, a garden bed)
- Kaggle (data science), Advent of Code (programming puzzles), CTF challenges (security)
- Cooking a cuisine you've never made; a new instrument; a craft (knitting, woodworking, pottery)

## Reading & self-paced
- A textbook worked cover-to-cover (surprisingly effective for a whole field)
- Nonfiction on one topic — go deep on 3-4 books rather than one
- Newsletters/blogs in a niche; Wikipedia rabbit holes with intent

## Watch & listen
- YouTube deep-dive channels (3Blue1Brown, Veritasium, CrashCourse, countless niche ones)
- Podcasts and audiobooks for commutes/chores
- Documentaries and recorded university lecture series (The Great Courses, YouTube)

## People & practice
- A local class or workshop (library, community center, makerspace)
- A club or meetup (language exchange, chess, hiking, book club)
- Find a mentor, or teach someone else (teaching forces mastery)
- Volunteer somewhere that builds a skill (habitat builds, animal shelters, nonprofits)

## Languages & travel
- Duolingo/Anki/Pimsleur, then conversation practice (iTalki, tandem apps)
- Immersion or a themed trip

## AI-assisted (right here)
- Ask me to teach you a topic in a structured arc, quiz you, or explain something you've always wondered about

---

To narrow this down, two quick questions would help me point you somewhere specific: **What's a subject you're curious about?** and **How much time per week?** Or just say "surprise me" and I'll suggest something.
```
## Test B

```
Happy to lay out a full menu. Since "learn something new" can mean a topic, a skill, or just a method, here's a broad spread organized by how you like to learn. Pick a lane and I can go deep on any of them.

## Practical skills (usable within days)
- **A programming language** — Python (most versatile), JavaScript (web), SQL (data), Rust/Go (systems)
- **Data & spreadsheets** — Excel/Sheets mastery, pivot tables, or a real data-analysis workflow
- **AI tooling** — prompt engineering, building with the Claude API, automating your own workflows
- **Design basics** — Figma, typography, color theory
- **Writing craft** — persuasive writing, editing, storytelling structure
- **A trade skill** — cooking technique, home repair, woodworking, gardening

## Knowledge domains (satisfying to understand)
- **How money works** — personal finance, how markets function, economics fundamentals
- **How your body works** — nutrition science, exercise physiology, sleep
- **How the world works** — history, geopolitics, how supply chains/electricity/the internet actually run
- **Science foundations** — physics intuition, chemistry, biology, astronomy
- **Philosophy & logic** — ethics, argument, how to reason well

## Creative & expressive
- **A musical instrument** or music theory
- **Drawing / painting / photography**
- **A new language** (spoken) — Spanish, French, Japanese, etc.
- **Chess, Go, or strategy games**

## Body & hands-on
- **A physical practice** — strength training, running, yoga, martial arts, swimming
- **A craft** — knitting, pottery, leatherwork, cooking a cuisine

## Methods for learning *anything* (meta-skills)
- **How to learn faster** — spaced repetition, active recall, the Feynman technique
- **Speed reading / better note-taking** (e.g. Zettelkasten)
- **Building a habit that sticks**

---

To point you at the *best* option rather than all of them, three quick things shape the answer:

1. **Why now** — career leverage, fun/curiosity, health, or making something?
2. **How you like to learn** — reading, watching, or doing hands-on?
3. **Time budget** — 15 min/day, or bigger weekend chunks?

Tell me those (or just name anything above that sparks) and I'll build you a concrete starting path.
```
# Task chunking
Prompt:
> I need to clean my room, my livingroom, kitche, and 3 bathrooms. Help me get started

## Test A
```
Cleaning multiple rooms can feel overwhelming, so here's the key to getting started: **pick one room and do a fast "reset" pass, not a deep clean.** Momentum matters more than perfection right now.

## Start here: your room (15–20 min)
Starting in your own room clears your personal space first, which usually feels the most relieving.

1. **Trash pass** — grab a bag, walk the room, toss anything that's garbage.
2. **Dishes/cups pass** — collect anything that belongs in the kitchen, set it by the door.
3. **Laundry pass** — clothes into the hamper or a pile.
4. **Surfaces** — clear the bed/nightstand/dresser top, then make the bed. A made bed makes the whole room feel done.

## Then the rest, in this order
- **Kitchen** → **Living room** → **3 bathrooms**

Kitchen and bathrooms take the most effort, so do the kitchen while your energy is high, and save bathrooms for a batch at the end (you can carry the same supplies room to room).

## Two tips that make it easier
- **Set a timer for 15 minutes per room.** When it goes off, you can stop or keep going — either way you've made progress.
- **Play music or a podcast.** It turns cleaning into background activity.

---

Want me to build you a **full room-by-room checklist** you can check off (I can make it a printable/clickable page), or would you rather I just coach you one room at a time as you go? I'd lean toward the checklist so you can see everything in one place.
```
## Test B
```
Cleaning six rooms in one go is a big lift, so the trick is to work in a smart order and not let yourself get stuck perfecting one spot. Here's a game plan.

## The overall strategy
1. **Do a fast "reset" lap of the whole house first** — before deep cleaning anything. Grab a laundry basket and walk every room just picking up trash (one bag) and anything that belongs in another room (put it in the basket, deliver on your way through). This alone makes everything feel 50% better and gives you momentum.
2. **Then deep-clean room by room, worst-to-first.** Do the room that's bugging you most while your energy is highest.
3. **Within each room, work top-to-bottom** (dust/surfaces before floors) so debris falls down and gets cleaned up last.

## Suggested room order
Kitchen and bathrooms are the "wet/germy" rooms and take the most effort, so I'd hit them when fresh:

1. **Kitchen** – dishes → wipe counters → stovetop → sink → floor
2. **Bathroom 1, 2, 3** – you can batch these: spray all three toilets/showers with cleaner, let it sit while you do mirrors and counters, then come back and scrub. Doing them back-to-back is way faster than one at a time.
3. **Living room** – clear surfaces → tidy → dust → vacuum
4. **Your room** – bed made first (instant win) → clothes (put away or hamper) → surfaces → floor

## Where to start *right now*
Two options depending on your mood:

- **Feeling overwhelmed?** Start with the 15-minute trash-and-basket lap. Low effort, big visual payoff.
- **Want to knock out the hard part?** Start dishes soaking in the kitchen and spray down all 3 bathrooms so the cleaners can work while you move.

A couple of practical tips: set a timer for ~25 min per room so you don't rabbit-hole, keep a trash bag and a "belongs elsewhere" basket on you at all times, and put on music or a podcast.

Want me to build you a **printable checklist** for each room, or a **timed schedule** (like a 2-hour or half-day plan) so you have a clear finish line? I can also tell you which supplies to grab so you're not hunting mid-task.
```

# Re-Test: Option limiting
Prompt:
> I have Saturday free and what to learn something new. What could I do?

## Test A
```
Fun question. A free Saturday is perfect for something you can start and get a small win from the same day. Here's what I'd point you toward.

**My pick: pick something with a fast feedback loop** — where you can go from zero to "hey, I made a thing" in a few hours. That momentum is what makes a new skill stick past day one. A few concrete options:

- **Cook one ambitious dish** — fresh pasta, a proper risotto, or homemade bread. Tactile, rewarding, and you eat the result.
- **Learn the basics of a musical instrument** — 3–4 hours on a keyboard or guitar gets you a real chord progression or simple song.
- **Build a tiny thing with code/AI** — e.g. a small personal website or a little automation. Low setup, visible payoff, and I can walk you through it live.
- **A hands-on craft** — watercolor, hand-lettering, or basic photo editing. Cheap to start, immediately visible progress.

To point you at the *right* one instead of a random list — what's the pull for you this Saturday?

- **Creative/artistic** (music, drawing, writing)
- **Hands-on/physical** (cooking, a craft, something tactile)
- **Techie** (coding, automation, a tool you've wanted to try)
- **Mind/knowledge** (a language, chess, a topic you're curious about)

Tell me the direction and I'll give you a specific Saturday plan — including free resources to start with.
```

## Test B
```
A free Saturday is a great canvas. Here are some directions depending on what kind of "new" you're in the mood for — hands-on, cerebral, physical, or creative:

**Make something with your hands**
- **Cooking a cuisine you've never attempted** — pick one dish (fresh pasta, a proper Thai curry, sourdough starter kickoff) and go deep. Immediate, tasty payoff.
- **A craft with a starter kit** — hand-lettering/calligraphy, woodworking a small object, embroidery, or leatherwork. Low barrier, satisfying result by dinnertime.

**Learn a skill that compounds**
- **A programming project** — if you've never coded, something like a "build your first web page" or a small automation in Python. If you already code, try a language or paradigm outside your comfort zone (Rust, functional programming, a game in Godot).
- **An instrument or a song** — a single day is enough to get a recognizable song out of a piano, ukulele, or harmonica.

**Move your body / go outside**
- **Bouldering** at an indoor gym — great intro-friendly, social, and a full-body puzzle.
- **A "learn to X" class** — pottery, dance, a knife-skills cooking class, archery. Saturdays are when these run.

**Feed your head**
- **A structured mini-course** — pick one topic (astronomy, negotiation, basic electronics, chess openings) and knock out a focused set of lessons or a good documentary series, then *do* one small exercise so it sticks.

A couple of questions that'd help me sharpen this:

- Do you want something you can **finish in a day** with a tangible result, or the **start of an ongoing hobby**?
- More **indoors/quiet** or **out-and-about/social**?
- Any rough **budget** (free-with-what-I-have vs. happy to buy a kit or class)?

Tell me the vibe and I'll build you an actual Saturday plan — timeline, what to get, and the best free resources to follow.
```

# Verbosity
Prompt:
> Explain why leaves change color in the fall

## Test A
```
Leaves change color in the fall through a combination of chemistry and the tree preparing for winter. Here's what's happening:

## The green fades: chlorophyll breaks down

During spring and summer, leaves are packed with **chlorophyll**, the pigment that makes them green and drives photosynthesis (converting sunlight into food). Chlorophyll is constantly being produced and broken down, so the green stays dominant.

As days shorten and temperatures drop in autumn, trees sense that winter is coming. They stop producing chlorophyll, and the existing supply breaks down. As the green disappears, other pigments that were there all along become visible.

## The colors that emerge

- **Yellows and oranges** come from **carotenoids**, pigments present in the leaf year-round but masked by the green. These are the same compounds that make carrots orange. Once chlorophyll fades, they show through.

- **Reds and purples** come from **anthocyanins**, which are mostly *newly produced* in fall. Trees make these from sugars trapped in the leaf. They're thought to act like a sunscreen, protecting the leaf as the tree reclaims nutrients before dropping it.

- **Browns** come from **tannins**, waste products left behind after the other pigments are gone.

## Why it happens at all

The tree is recycling. Before a leaf falls, the tree pulls valuable nutrients (like nitrogen) back into its branches and trunk to store for spring. A layer of cells called the **abscission layer** forms at the base of the leaf stem, sealing it off and eventually letting it drop.

## Why some years are more vivid

Weather tunes the show:
- **Bright, sunny days + cool (not freezing) nights** produce the most vivid reds, because sugars get trapped in the leaves and fuel anthocyanin production.
- **Warm, cloudy, or rainy** autumns tend to make duller colors.

So the yellows and oranges are a reveal, the reds are a fresh autumn creation, and the whole process is really the tree efficiently shutting down for winter.
```

## Test B
```
Leaves change color in the fall because of shifts in the pigments they contain, driven by changes in daylight and temperature. Here's what's actually happening:

## The green is always being replaced

During spring and summer, leaves are packed with **chlorophyll**, the pigment that captures sunlight for photosynthesis and gives leaves their green color. Chlorophyll is constantly broken down and regenerated, so as long as the tree keeps producing it, green dominates.

Chlorophyll also masks other pigments that are present in the leaf the whole time.

## The trigger: shorter days and cooler nights

As autumn approaches, the main cue is the **lengthening of nights** (temperature and moisture play supporting roles). In response, the tree begins preparing to drop its leaves:

- A layer of cells called the **abscission layer** forms at the base of each leaf stem, gradually cutting off the flow of water and nutrients.
- The tree **stops producing chlorophyll**, and the existing chlorophyll breaks down and isn't replaced.

As the green fades, the pigments that were there all along become visible.

## The pigments that get revealed (or created)

- **Carotenoids** — produce **yellows and oranges** (the same pigments in carrots and corn). These are present in the leaf all summer, just hidden by the green.
- **Anthocyanins** — produce **reds and purples**. Unlike carotenoids, these are mostly **produced fresh in the fall**, made from sugars trapped in the leaf after the abscission layer forms. This is why red color varies so much year to year.

## Why some years are more vivid

Because anthocyanins depend on sugar and sunlight, weather matters:

- **Warm, sunny days + cool (but not freezing) nights** produce the most brilliant reds — sunlight makes lots of sugar, and cool nights trap it in the leaf.
- **Cloudy, warm, or wet** autumns tend to produce duller, more muted colors.

## Why different trees differ

- **Oaks** often turn brown (tannins).
- **Maples** can turn brilliant red or orange.
- **Aspens and birches** tend toward gold.

Eventually the abscission layer fully severs the leaf, it falls, and the tree conserves resources through winter.

Would you like me to go deeper on any part — the chemistry of the pigments, or the ecological reasons trees bother doing this?
```

# Tangent Handling
Prompt (1):
> Help me plan what I need to clean today: kitchen, bedroom, living room, and bathrooms.

After response, additional prompt (2):
> Oh also I've been thinking about learning spanish

## Test A
Prompt (1)
```
Here's a room-by-room plan to tackle those four areas efficiently today.

## Suggested order
Work **top-down and dirtiest-last**: Bedroom → Living Room → Kitchen → Bathrooms. This keeps the grimiest jobs at the end so you're not tracking mess into freshly-cleaned rooms.

---

### 🛏️ Bedroom
- Strip and remake the bed (wash sheets if due)
- Clear surfaces — nightstands, dresser
- Put away clothes / start a laundry load
- Dust surfaces and any sills
- Vacuum floor, including under the bed

### 🛋️ Living Room
- Pick up clutter, fold throws, fluff cushions
- Dust surfaces, TV, shelves
- Wipe down remotes/high-touch spots
- Vacuum couch and floor (spot-clean stains if any)

### 🍽️ Kitchen
- Load/run dishwasher or wash dishes
- Clear and wipe counters + stovetop
- Wipe appliance fronts (microwave inside too)
- Take out trash/recycling
- Sweep and mop floor

### 🚽 Bathrooms
- Toilets (bowl + exterior)
- Sink and counter
- Mirror
- Tub/shower
- Empty trash, swap towels
- Sweep/mop floor last

---

**Two time-savers:**
1. Start a laundry load *first* so it runs in the background while you clean.
2. Do the "pick up clutter" pass in every room before you start deep-cleaning any single one — it makes the actual cleaning much faster.

Want me to turn this into a printable checklist, or add rough time estimates so you can fit it into your day?
```

Prompt (2)
```
That's a fun one to have on the horizon! 🌎 I'll park it here so we don't lose it.

Want me to circle back to it once the cleaning's sorted — maybe pull together a simple starting plan (apps, a daily routine, whatever fits how you like to learn)? For now, back to the task at hand:

Do you want that cleaning plan as a **printable checklist** or with **time estimates** added — or are you good to just dive in?
```

## Test B
Prompt (1)
```
Here's a practical plan to tackle all four areas today. I've ordered it to work efficiently — start with the room that needs to "rest" (kitchen surfaces, bathroom disinfectant sitting) and end with floors everywhere so you're not tracking through cleaned rooms.

## Suggested order & game plan

### 1. Kitchen (do first — let cleaners sit while you move on)
- Clear counters; put dishes in dishwasher or wash
- Spray counters, stovetop, and backsplash — let sit
- Wipe down appliance fronts (fridge, microwave, oven)
- Clean sink and faucet
- Wipe counters/stove clean
- Take out trash & recycling
- Sweep floor (mop later with other floors)

### 2. Bathrooms (apply cleaners early, let them work)
- Spray toilet bowl, tub/shower, and sink — let disinfectant dwell
- Wipe mirrors and counters
- Scrub tub/shower, then sink
- Scrub and wipe toilet (inside and out)
- Swap towels, refill soap/TP
- Empty trash
- Sweep/mop floor last

### 3. Bedroom
- Strip and remake bed (or launder sheets if due)
- Clear and put away clutter/clothes
- Dust surfaces, nightstands, dresser
- Empty any trash
- Sweep/vacuum floor

### 4. Living room
- Clear clutter, fold blankets, straighten cushions
- Dust surfaces, TV, shelves, electronics
- Wipe down remotes/high-touch spots
- Vacuum sofa (and under cushions)
- Vacuum/sweep floor

### 5. Floors — all rooms
Do a final pass: vacuum carpets, mop hard floors kitchen → bathrooms → bedroom → living room, working toward your exit.

---

**A few questions to tailor this:**
- Do you have a time limit, or is this an all-day deep clean?
- How many bathrooms?
- Any laundry (sheets/towels) to fold into the timing, since that runs in the background?

Want me to turn this into a printable checklist you can check off as you go?
```

Prompt (2)
```
Nice — that's a fun one to pick up, and honestly it pairs well with cleaning: you can run Spanish audio or a podcast while you scrub. Free hands, captive ears.

To point you in the right direction, the big fork is usually *why* you want it, since that changes the whole approach:

- **Travel/basics fast** → phrasebook + audio (Pimsleur, Language Transfer) to get speaking quickly
- **Long-term fluency** → a mix of an app for daily habit (Duolingo/Busuu), plus comprehensible input (Dreaming Spanish is excellent for beginners)
- **Conversation ASAP** → italki tutors for cheap 1-on-1 practice, even a few times a week
- **Just curious/casual** → podcasts like *Coffee Break Spanish* or *Notes in Spanish* while you do other things

A couple things that make a big difference regardless of method:
- **Consistency beats intensity** — 15 min daily crushes 2 hours once a week
- **Get to listening/speaking early** rather than drilling grammar in isolation
- **Latin American vs. Spain Spanish** — worth picking based on where you'd use it (pronunciation and some vocab differ)

What's drawing you to it? Travel, work, family/friends, or just something new to sink into? And do you have any past experience with it, or starting from zero? I can sketch out a realistic weekly plan that fits around your life if you want.
```

