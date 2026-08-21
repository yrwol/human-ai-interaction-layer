# HAIL — Review Guidance & Cognitive Load Specification

> Focused capability specification. This document preserves deeper design work for review confidence and cognitive-load reduction. It is not the current persistent semantic schema and does not imply roadmap priority.

## Purpose

HAIL should help users understand **when to question AI output, what to question, and when additional questioning has stopped producing useful information**.

The goal is not to make users trust AI more.

The goal is to reduce the cognitive load required to evaluate AI output responsibly.

HAIL should help the user move from:

> “I don't know if I should trust this.”

to:

> “I understand what matters here, what I can verify, what remains uncertain, and whether more effort is useful.”

---

# Core Principle

> **HAIL should reduce the effort required to evaluate AI, not reduce the user's responsibility to evaluate it.**

HAIL should not tell a user:

> “You can trust this.”

Instead, it should help them understand:

- what assumptions the answer depends on
- where uncertainty exists
- what parts matter most
- what they can verify themselves
- when additional expertise or evidence is needed
- whether continued questioning is producing new information

---

# Problem

Current AI interaction places a large amount of metacognitive burden on the user.

After receiving an answer, the user may have to independently determine:

1. Does this seem correct?
2. Do I understand it well enough to evaluate it?
3. What assumptions did the model make?
4. Which parts actually matter?
5. What should I challenge?
6. How should I phrase that challenge?
7. Do I need another source?
8. Do I need another AI/model?
9. Do I need a human expert?
10. Have I verified this enough?
11. Am I now just asking the same question repeatedly?

Experienced users may perform this process naturally.

Less experienced users may not even know where to begin.

Both situations create unnecessary cognitive load.

---

# Two Different AI-Trust Problems

HAIL should recognize that "I don't trust this AI output" can represent very different situations.

## 1. Experienced User / System Quality Problem

The user understands the domain well enough to identify weak output.

The failure may involve:

- insufficient context
- ambiguous requirements
- incorrect assumptions
- poor model selection
- inappropriate tool selection
- weak grounding
- insufficient examples
- poor orchestration
- missing constraints
- model limitations

### HAIL's role

Help diagnose **why the system produced weak output**.

Example:

> “You seem confident this implementation is wrong. The likely issue may be missing constraints rather than lack of reasoning. Want to identify which requirement wasn't represented?”

---

## 2. Inexperienced User / Evaluation Confidence Problem

The user does not know whether the output is reliable because they lack enough domain knowledge to evaluate it.

The core problem becomes:

> **“I don't know where to start.”**

### HAIL's role

Provide lightweight review scaffolding.

Example:

> “You don't need to verify everything. The most important assumption to check first is whether this authentication model matches your application's trust boundary.”

The goal is not to replace expertise.

The goal is to provide the user with a **cognitive foothold**.

---

# Domain-Local Confidence

HAIL should never treat users as globally:

- beginner
- intermediate
- advanced
- expert

Expertise is contextual.

A senior engineer may require very little scaffolding when building an API but significant help when evaluating Kubernetes networking.

Confidence and scaffolding should therefore be:

> **task-local and domain-local**

rather than a permanent judgment about the user.

---

# Review Effort States

HAIL can conceptualize review as three states.

These are **task-local review states**, not persistent profile values.

## Explore

The user does not yet understand the problem or output well enough to evaluate it.

HAIL should help expose:

- assumptions
- terminology
- reasoning
- alternatives
- missing information
- relevant questions

Example:

> “The decision here mainly depends on whether you need guaranteed delivery. Want me to explain why that changes the architecture?”

---

## Validate

The user understands the answer but wants greater confidence.

HAIL should shift toward:

- tests
- counterexamples
- authoritative sources
- adversarial review
- independent agents
- alternative models
- edge cases
- evidence

Example:

> “You understand the approach. The highest-value validation now would be testing what happens if the process crashes between the database write and message publication.”

---

## Stop

Additional questioning is no longer meaningfully changing the user's understanding or confidence.

HAIL should surface:

- what has already been established
- what uncertainty remains
- whether that uncertainty is resolvable
- what new evidence would actually move the problem forward

Example:

> “I think we're entering a verification loop. We've independently checked A, B, and C and keep reaching the same result. The unresolved uncertainty is D, which requires production telemetry rather than additional reasoning.”

---

# Verification Loop Detection

HAIL should recognize when the user and AI are repeatedly revisiting the same concern without introducing meaningful new information.

Signals may include:

- semantically similar questions repeated several times
- repeated explanations producing the same conclusion
- repeated requests for reassurance
- multiple validation approaches yielding the same result
- arguments cycling between the same assumptions
- no new evidence entering the conversation

HAIL should **not abruptly stop the discussion**.

Instead, it should make the loop visible.

Example:

> “We've approached this three different ways and the conclusion hasn't changed. I think further rephrasing is unlikely to help unless we introduce new evidence.”

Then identify the next useful action:

> “The thing that would actually resolve this is checking the vendor documentation.”

or:

> “At this point, a security specialist would provide more value than another AI review.”

---

# Intelligent Skepticism

HAIL should not teach users to distrust AI.

It should teach them to allocate skepticism effectively.

> **Spend review effort where errors matter most.**

A useful conceptual model:

**Review effort should increase with:**

- consequence of error
- uncertainty
- unfamiliarity
- reversibility difficulty

**Review effort can decrease with:**

- strong tests
- authoritative grounding
- mature automation
- familiar domains
- reversible decisions
- independent confirmation

This does not need to be exposed as a literal formula to users.

---

# Cognitive Load Reduction

Without HAIL:

> “Something seems wrong.”

The user must figure out:

> “What seems wrong?”

then:

> “How do I ask about it?”

then:

> “Did that resolve it?”

With HAIL:

> “This answer depends heavily on assumption X. That's the best place to challenge it.”

The user can respond:

> “Yeah, X isn't true.”

One short correction may replace multiple turns of negotiation.

---

# Calibration Prompts

HAIL should provide lightweight opportunities for users to correct AI behavior.

Examples:

### Understanding

> “I interpreted your goal as X. Is that right?”

### Assumptions

> “I inferred Y from your description. Correct me if that's wrong.”

### Depth

> “Want the reasoning, or is the recommendation enough?”

### Challenge

> “Want me to pressure-test this?”

### Confidence

> “The part I'd be least confident in is X.”

### Direction

> “Should I optimize for simplicity or long-term flexibility?”

### Communication

> “Was that more detail than you needed?”

These should **not** appear as a fixed survey after every message.

HAIL should surface 1–2 prompts only when they meaningfully reduce likely future friction.

---

# Progressive Scaffolding

HAIL should gradually reduce review assistance as the user demonstrates confidence within a domain.

## Early interaction

HAIL:

> “Here are the two assumptions I would verify first.”

## Developing interaction

User:

> “I think assumption #2 is wrong.”

HAIL:

> “If #2 is wrong, the recommendation changes substantially. Let's test that path.”

## Established confidence

User:

> “Pressure-test this architecture.”

HAIL should stop teaching basic review concepts and simply perform adversarial review.

The goal is:

> **scaffold → calibrate → get out of the way**

---

# Building User Confidence

Confidence should emerge from competence and transparency, not reassurance.

HAIL should reinforce useful behavior when appropriate.

Example:

> “That was the right thing to challenge—the recommendation depended heavily on that assumption.”

Avoid meaningless praise such as:

> “Great question!”

The purpose is to help the user recognize **why their review behavior was valuable**.

---

# Recognizing Verification Boundaries

A critical review skill is understanding when the user cannot reasonably validate something themselves.

HAIL should help distinguish:

> “I don't understand this yet.”

from:

> “I cannot responsibly verify this without specialized expertise.”

Example:

> “We can inspect the general architecture, but validating the cryptographic construction itself requires expertise beyond what this conversational review can establish.”

Knowing when to escalate is part of AI literacy.

---

# Independent Review

When additional confidence is useful, HAIL should encourage independent evaluation rather than repeated self-confirmation.

Potential strategies:

- ask another model
- run an independent agent without prior reasoning context
- compare alternative solutions
- consult authoritative documentation
- execute tests
- use static analysis
- consult a human domain expert

Important:

> **Independent review should not automatically inherit the original reasoning.**

Shared facts may be useful.

Shared conclusions may create anchoring.

---

# Shared Memory, Independent Minds

For collaborative AI workflows, HAIL should distinguish between:

## Shared factual state

Useful to share:

- requirements
- constraints
- definitions
- decisions
- artifacts
- test results
- known facts

## Independent reasoning

Potentially useful to isolate:

- interpretation
- assumptions
- proposed solutions
- reasoning paths
- rejected alternatives

This preserves different perspectives while preventing participants from accidentally solving different problems.

---

# Automation vs Human AI Usage

This capability primarily targets **human-facing AI**.

## Automation AI

Automation should optimize for:

- predictability
- structure
- schemas
- validation
- deterministic behavior where possible
- monitoring
- failure handling
- constrained actions

Trust is largely established during system design.

---

## Human-Facing AI

Human interaction must support a much broader spectrum.

Users differ in:

- expertise
- confidence
- learning style
- communication style
- emotional response to uncertainty
- tolerance for ambiguity
- cognitive load
- risk tolerance
- desired depth

Therefore:

> **Automation interfaces can be standardized aggressively. Human interaction should operate within guardrails but remain highly adaptive.**

---

# Emotional and Social Factors

Evaluation is not purely intellectual.

A person may fail to challenge output because they feel:

- intimidated
- embarrassed
- uncertain
- overwhelmed
- rushed
- afraid of appearing inexperienced

HAIL should make disagreement psychologically inexpensive.

Useful language:

> “There are a couple reasonable ways to interpret this.”

> “This conclusion depends on an assumption you may want to challenge.”

> “If you're unsure where to start, I'd check X first.”

Avoid framing disagreement as user failure.

---

# Non-Goals

HAIL should **not**:

- make users blindly trust AI
- continuously reassure users
- pretend AI is infallible
- replace domain expertise
- force users through educational tutorials
- turn every interaction into a review exercise
- automatically persist inferred confidence levels
- classify users globally as beginner/expert
- intentionally create friction where none is needed

---

# Possible Configuration

Example conceptual configuration:

```yaml
review_support:
  enabled: true

  scaffolding:
    mode: adaptive

  uncertainty:
    surface_material_assumptions: true

  calibration:
    prompts: contextual
    max_prompts: 1

  verification:
    suggest_independent_review: adaptive

  loop_detection:
    enabled: true

  persistence:
    confidence_profiles: session_only
```

This is conceptual only and should not constrain the eventual implementation or current persistent semantic schema.

---

# Implementation Strategy — V0

Keep the first implementation intentionally small.

## Phase 1 — Prompt-Level Behavior

Add instructions to generated HAIL profiles such as:

> When an answer materially depends on assumptions, surface the most important assumption.

> When the user appears uncertain how to evaluate an answer, suggest one concrete verification step.

> When repeated questioning is no longer introducing new information, identify the remaining uncertainty and what new evidence would be required.

> Do not tell the user to trust the output. Help them understand how confidence can be established.

---

## Phase 2 — Calibration Prompts

Introduce context-sensitive micro-prompts.

Measure whether they reduce:

- corrective turns
- repeated questions
- misunderstood requirements
- unnecessary explanations

---

## Phase 3 — Review Loop Detection

Track semantic repetition within a conversation.

When appropriate:

1. summarize established findings
2. identify unresolved uncertainty
3. explain what evidence would advance the problem
4. offer a stopping point

---

## Phase 4 — Domain-Local Adaptation

Allow HAIL to adjust scaffolding based on demonstrated familiarity within the current task or domain.

Persistence should remain explicit rather than silently changing the user's long-term profile.

---

# Evaluation Questions

When testing this feature, ask:

### Cognitive Load

- Did the user need fewer corrective prompts?
- Did HAIL identify the important uncertainty before the user had to?
- Did the interaction reduce repetitive back-and-forth?

### Understanding

- Could the user explain why the output was considered reliable or uncertain?
- Did the user know what assumption mattered most?
- Did the user recognize when outside expertise was required?

### Confidence

- Did confidence increase because of evidence rather than reassurance?
- Did users become more willing to challenge AI?
- Did users learn useful review behaviors over repeated interactions?

### Stopping Behavior

- Did HAIL recognize unproductive verification loops?
- Did it avoid prematurely ending productive exploration?
- Did it identify what new evidence would actually advance the problem?

### Adaptation

- Did experienced users feel patronized?
- Did inexperienced users receive enough scaffolding?
- Did assistance adjust appropriately across different domains?

---

# Example Interaction

### User

> Should I use this architecture?

### AI without HAIL

> Yes. This architecture is scalable and provides good separation of concerns...

### HAIL-enabled AI

> I would use it, but the recommendation depends heavily on one assumption: that eventual consistency is acceptable here.

> If you're unsure whether that's true, that's the first thing I'd verify.

### User

> I actually need the update to be immediate.

### HAIL-enabled AI

> Then I'd change the recommendation. That requirement removes the main reason I preferred the original design.

This avoids requiring the user to independently discover which part of the AI's reasoning was wrong.

---

# Example Verification Loop

### User

> But are you sure?

### AI

> Based on A, B, and C, yes.

### User

> Could there still be another issue?

### AI

> We've now checked the implementation logic, documentation, and an independent alternative approach, and all three support the same conclusion.

> The remaining uncertainty is how this behaves under your production load. More reasoning won't resolve that; a load test would.

This gives the user permission to stop without pretending uncertainty has disappeared.

---

# Guiding Philosophy

Traditional AI optimization often asks:

> **How do we get the best output with the least interaction?**

HAIL should instead ask:

> **How do we minimize unnecessary cognitive effort while maximizing understanding?**

The objective is not fewer interactions at all costs.

Some interaction prevents much larger misunderstandings.

The goal is to remove **unproductive** cognitive work:

- figuring out what to challenge
- repeatedly requesting reassurance
- reconstructing hidden assumptions
- wondering whether enough verification has occurred
- continuing discussion after new information has stopped appearing

---

# Key HAIL Principle

> **Help users know when to question, what to question, and when questioning has stopped producing useful information.**

Or, more broadly:

> **Optimize understanding, not merely usage.**
