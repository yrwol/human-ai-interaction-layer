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