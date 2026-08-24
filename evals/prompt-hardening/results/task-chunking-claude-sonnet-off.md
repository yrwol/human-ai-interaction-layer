# Task Chunking Hardening Result — Claude Sonnet — `off`

## Test conditions

- Date: 2026-08-24
- Harness: Claude Code
- Model: `sonnet`
- HAIL ref: `eval/task-chunking-verbosity-hardening`
- HAIL commit: `f976f9ea84ff9f516054bf25c9c1d4b07103d5b9`
- Profile: `profiles/eval-task-chunking-off.yaml`
- `task_chunking`: `off`
- `step_pacing`: `continuous`
- Interaction: fresh GitHub Actions runner
- Source run: `32778778307`

## Compiled projection

```text
- Do not automatically break work into smaller steps unless the user asks.
```

Other profile values were held constant across the differentiation runs:

```yaml
verbosity: balanced
decision_mode: recommend_first
max_options: 3
step_pacing: continuous
tangent_policy: capture_and_return
```

## Prompt

```text
I want to build a cozy horse game with horse care, training, competitions, breeds, and progression, but I don't know where to start. Help me plan it.
```

## Raw result

```text
Good starting point I'd recommend: **nail the core gameplay loop and MVP scope before touching breeds, competitions, or progression systems.** Cozy sims live or die on how satisfying the moment-to-moment loop feels (grooming, feeding, riding), and it's easy to burn months designing breed genetics or tournament brackets for a game whose basic "pet the horse" moment isn't fun yet. Getting that loop right first also tells you what your progression and content systems actually need to support.

Alternatives worth considering instead:
- **Start with world/narrative** (setting, town, characters) if the emotional hook matters more to you than mechanics — common for cozy games where vibe sells it.
- **Start with art direction** if you already have a strong visual identity in mind and want mechanics to serve that look.

If core loop first sounds right, here's how I'd break the planning itself into pieces:

1. **Pillars** — 3-4 words that define what makes it *this* game (e.g. "nurturing," "low-stakes," "collectible"). Keeps every later decision anchored.
2. **Core loop** — what does a player do in their first 5 minutes, and what do they do on day 30? (e.g. groom → bond → train → compete → earn → buy/breed → repeat)
3. **MVP scope** — one horse, one breed, minimal care stats, one training activity, one competition type. Everything else is post-MVP.
4. **System sketches** (lightweight, not full specs yet):
   - Horse care (needs, stats, decay/reward loop)
   - Training (skills, how they map to competition performance)
   - Competitions (formats, rewards, stakes)
   - Breeds (how many at launch, what differentiates them mechanically vs. cosmetically)
   - Progression (player level vs. horse level vs. unlocks — these are often separate tracks)
5. **Content/tech plan** — engine choice, art pipeline, save system, rough scope estimate.

Want to start with pillars and core loop together — I can help you draft those right now?
```

## Assessment

- Expected: keep the planning task comparatively whole; structure for readability is allowed, but do not proactively turn it into an execution sequence.
- Actual: explicitly announced that it would "break the planning itself into pieces" and produced five ordered planning chunks.
- Failure pattern: `off` still triggered substantial proactive decomposition despite the compiled instruction.
- Differentiation contribution: this response is at least as visibly chunked as the `adaptive` response and more visibly chunked than the `always` response.
- Semantic drift: yes, relative to the intended `off` behavior.
- Strength: **weak**.
- Promotion status: **fail**.
