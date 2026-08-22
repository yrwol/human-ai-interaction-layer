# Skill-Based Capability Structure

Status: **proposed capability architecture**

This specification defines how HAIL capabilities should be exposed in harnesses that support discoverable skills or similar first-class capability units.

The immediate driver is Claude Code, where the plugin is named `hail` and the current sole skill is also named `hail`. As a result, `/hail` resolves to the plugin's `hail` skill (effectively `/hail:hail`), and all HAIL behavior currently lives behind one skill entrypoint.

The proposed change is to make **skills the canonical capability boundary** and use the default `hail` skill as a lightweight discovery/router experience.

## Problem

The current single-skill structure makes HAIL functionality difficult to discover.

A command-oriented interaction such as:

```text
/hail show
/hail reset
/hail review
/hail compare
```

requires the user to already know that each action exists and to remember HAIL-specific command vocabulary.

This creates two problems:

1. **Discovery depends on prior knowledge.** A user searching the harness's available skills cannot find `review`, `compare`, or another HAIL capability if all functionality is hidden inside one `hail` skill.
2. **The default skill becomes a catch-all.** Profile management, review behavior, comparison behavior, future capabilities, and routing logic accumulate in one instruction file even though they represent different workflows.

HAIL should not require users to learn a private CLI before they can discover what it can do.

## Design principle

> Skills are the canonical HAIL capabilities. Command-like syntax is only a shortcut into a skill.

When the host harness supports first-class skills, every **new user-facing HAIL capability that would otherwise be introduced as a new command** SHOULD be implemented as a separate skill.

Examples:

```text
/hail:profile
/hail:review
/hail:compare
```

rather than adding new hidden subcommands such as:

```text
/hail profile
/hail review
/hail compare
```

The default `/hail` entrypoint remains useful, but its primary role becomes **discovery and routing**, not ownership of all HAIL behavior.

## Proposed Claude skill layout

```text
integrations/claude/skills/
  hail/
    SKILL.md        # default entrypoint, discovery, intent routing

  profile/
    SKILL.md        # persistent HAIL profile management

  review/
    SKILL.md        # retrospective review of one conversation/session

  compare/
    SKILL.md        # comparison of two conversations/sessions
```

Additional capabilities should follow the same pattern when they are justified by product evidence.

Do not create a new skill merely because an existing capability has multiple small actions. A skill should represent a coherent user goal, not every verb in the interaction.

## Default `hail` skill

The existing `hail` skill remains because Claude's plugin/skill naming model makes it the natural `/hail` entrypoint.

Its responsibility changes.

### Responsibilities

The default skill SHOULD:

- identify itself as the HAIL capability entrypoint;
- show the user the available HAIL skills in plain language;
- help route natural-language intent to the appropriate capability;
- preserve compatibility with reasonable legacy invocations during migration;
- avoid duplicating the full implementation instructions of child capabilities.

Example conceptual response to `/hail`:

```text
HAIL can currently help you:

- manage your persistent interaction profile
- review a conversation for interaction friction
- compare two conversations to understand why one worked better
```

The exact rendering may vary by harness. The requirement is discoverability, not a specific menu format.

### Non-responsibilities

The default skill SHOULD NOT become a second implementation of profile management, review, or comparison behavior.

Capability-specific rules belong in the corresponding skill.

## Existing profile-management behavior

The current `integrations/claude/skills/hail/SKILL.md` contains HAIL's validated persistent profile-management workflow.

That behavior SHOULD move to:

```text
integrations/claude/skills/profile/SKILL.md
```

The profile skill remains responsible for:

- initial setup;
- viewing the current persistent profile;
- changing persistent preferences;
- resetting all or part of the profile;
- validating the semantic schema;
- writing `~/.hail/profile.yaml`;
- compiling the Claude projection to `~/.hail/claude-code.md`;
- maintaining the HAIL import in `~/.claude/CLAUDE.md`;
- enforcing the existing explicit-persistence boundary.

### Actions remain actions, not separate skills

The following are actions inside the coherent **profile management** capability and SHOULD NOT become separate skills by default:

```text
show
setup
change
reset
```

For example, these are all valid expressions of the profile capability:

```text
/hail:profile
/hail:profile show
/hail:profile reset
```

or natural-language equivalents such as:

```text
/hail:profile make my answers more compact
```

The distinction is:

- **capability-level noun / user goal** → skill;
- **operation within that capability** → action handled by the skill.

This prevents replacing one oversized command surface with hundreds of tiny skills.

## Legacy compatibility

Existing users may already invoke the current default skill using patterns such as:

```text
/hail
/hail show
/hail reset
/hail make my answers shorter
```

Migration SHOULD avoid abruptly breaking those flows.

During a compatibility period, the default `hail` skill MAY recognize clear legacy profile-management intent and route or execute the equivalent profile workflow.

Examples:

```text
/hail show
→ equivalent intent: /hail:profile show

/hail reset
→ equivalent intent: /hail:profile reset
```

However, documentation and discovery surfaces SHOULD teach the new capability form:

```text
/hail:profile
```

Compatibility behavior must not become a reason to keep adding new `/hail <command>` branches to the router.

## Rule for all new command-like capabilities

Once this structure is adopted:

> Do not add a new top-level HAIL command when the behavior represents a distinct user-facing capability. Add a skill.

For example, if HAIL adds conversation review, implement:

```text
integrations/claude/skills/review/SKILL.md
```

and expose it as:

```text
/hail:review
```

Do **not** first implement it as a hidden branch inside the default skill such as `/hail review` and later duplicate it into a skill.

Likewise:

```text
compare conversations → /hail:compare
```

A shorthand `/hail compare ...` MAY be recognized for compatibility or routing, but it is not the canonical capability boundary.

## Conversation review skill

The proposed `review` capability analyzes interaction quality in one conversation or session.

Canonical entrypoint:

```text
/hail:review
```

### Target selection

The skill SHOULD support, when the harness makes the source available:

- the current conversation/session by default;
- a referenced previous session;
- a supplied session identifier;
- a transcript or exported conversation file;
- future harness-specific conversation selectors.

The review engine should operate on a normalized conversation representation rather than depending on one vendor's storage format.

Conceptually:

```text
current session --------┐
Claude session history -┤
Codex session history --┤
exported transcript ----┼→ normalized HAIL session → review
pasted transcript ------┘
```

### Review purpose

Review evaluates the **human–AI interaction**, not merely whether the answer was technically correct.

Useful signals include:

- repeated verbosity corrections;
- unnecessary confirmation or pacing friction;
- repeated restatement of context;
- poor decision ownership;
- excessive or insufficient option generation;
- tangent handling failures;
- repeated user redirection;
- mismatches between observed interaction and current HAIL preferences.

### Read-only default

Conversation review MUST be read-only by default.

A review MUST NOT automatically:

- rewrite the target conversation;
- resume or inject messages into a historical session;
- change the persistent HAIL profile;
- create an eval fixture;
- infer a permanent user trait from one conversation.

It may recommend follow-up actions such as:

- propose a profile change;
- save an observation;
- create an eval candidate;
- dismiss the finding as situational.

Persistent changes remain intentional.

## Conversation comparison skill

The proposed `compare` capability analyzes two conversations or sessions to identify why collaboration behavior differed.

Canonical entrypoint:

```text
/hail:compare
```

Example question:

> Why did collaboration feel smooth in conversation A but frustrating in conversation B?

The capability SHOULD compare interaction signals such as:

- interruption/redirection frequency;
- decision ownership;
- pacing behavior;
- option burden;
- verbosity;
- tangent handling;
- context loss/repetition;
- relevant profile/projection differences when known.

Like review, comparison is read-only by default and should produce evidence-backed observations rather than silently changing persistent preferences.

## Harness-neutral capability model

Claude slash syntax is one presentation of the architecture, not the architecture itself.

The durable model is:

```text
HAIL capability
    ↓
harness adapter / discovery surface
    ↓
Claude skill, Codex skill, menu action, plugin action, tool, or future equivalent
```

Therefore HAIL documentation SHOULD distinguish:

- the **capability name and behavioral contract**;
- the **harness-specific invocation mechanism**.

`review` should remain the same conceptual HAIL capability even if another harness exposes it as `$hail-review`, a menu item, or a tool rather than `/hail:review`.

## Discoverability requirements

A HAIL capability is not considered properly exposed in a skill-capable harness merely because the default router can execute it.

For each first-class capability:

1. it has its own skill definition;
2. its skill metadata contains plain-language terms a user might search for;
3. the default HAIL entrypoint lists or can describe it;
4. documentation names the capability directly;
5. users are not required to know an undocumented subcommand to find it.

The skill description should favor user intent over internal terminology.

For example, the review skill description should include concepts such as:

> review a conversation, analyze interaction friction, understand what could be improved

rather than only implementation terms such as "retrospective analyzer."

## Routing behavior

Natural-language routing is desirable, but routing must not erase capability boundaries.

If the user enters `/hail` and says:

> I want to see why my previous conversation kept frustrating me

HAIL can identify `review` as the relevant capability and tell the user what it is called or route into the equivalent workflow when the harness allows it.

The user should learn that the capability exists without needing to memorize it beforehand.

## File ownership

Under this structure:

```text
skills/hail/SKILL.md
```

owns:

- capability discovery;
- routing guidance;
- migration compatibility.

```text
skills/profile/SKILL.md
```

owns:

- persistent profile-management behavior;
- profile/projection file mutation rules.

```text
skills/review/SKILL.md
```

owns:

- single-conversation retrospective behavior;
- review evidence and recommendation format;
- conversation-source normalization requirements specific to review.

```text
skills/compare/SKILL.md
```

owns:

- two-conversation comparison behavior;
- comparative signal interpretation.

Shared semantic definitions remain in the current HAIL semantic specification rather than being redefined independently by every skill.

## Implementation sequence

The smallest safe migration is:

1. create `profile` as a new skill using the validated behavior currently in `hail`;
2. reduce the default `hail` skill to discovery/routing plus temporary legacy compatibility;
3. verify existing profile setup/show/change/reset behavior through `/hail:profile`;
4. verify `/hail` clearly exposes available capabilities;
5. add `review` as a first-class skill, not a new hidden router command;
6. add `compare` as a first-class skill when its behavior is ready to implement;
7. update user-facing docs and evals to use canonical skill invocations while retaining targeted legacy coverage during migration.

Do not combine this structural migration with changes to the persistent semantic schema.

## Validation criteria

The migration is successful when:

- `/hail` remains a useful entrypoint for a user who knows nothing beyond the HAIL name;
- profile management is independently discoverable as a HAIL skill;
- existing persistent profile behavior still works after being moved;
- users can discover a new capability through the harness's skill search without already knowing a `/hail <command>` phrase;
- new distinct capabilities are added as skills rather than branches in the default `hail` skill;
- the default `hail` skill does not duplicate every child skill's implementation;
- persistent profile mutation remains explicit and intentional.

## Non-goals

This specification does not require:

- a shared HAIL runtime;
- MCP;
- automatic discovery of every historical conversation across every harness;
- modification of undocumented Claude session files;
- automatic profile learning;
- a new persistent semantic field;
- identical command syntax across harnesses.

The goal is a discoverable, capability-oriented interaction surface with clean behavioral ownership.
