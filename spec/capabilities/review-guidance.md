# HAIL — Review Guidance & Cognitive Load Specification

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

## Explore

The user does not yet understand the problem or output well enough to evaluate it.

HAIL should help expose:

- assumptions
- terminology
- reasoning
- alternatives
- missing information
- relevant questions

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

## Stop

Additional questioning is no longer meaningfully changing the user's understanding or confidence.

HAIL should surface:

- what has already been established
- what uncertainty remains
- whether that uncertainty is resolvable
- what new evidence would actually move the problem forward

These are **task-local review states**, not persistent profile values.

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

Instead, it should make the loop visible and identify what evidence would actually move the problem forward.

---

# Intelligent Skepticism

HAIL should not teach users to distrust AI.

It should teach them to allocate skepticism effectively.

> **Spend review effort where errors matter most.**

Review effort should increase with consequence of error, uncertainty, unfamiliarity, and reversibility difficulty. It can decrease with strong tests, authoritative grounding, mature automation, familiar domains, reversible decisions, and independent confirmation.

---

# Cognitive Load Reduction

Without HAIL, a user may have to determine what seems wrong, how to challenge it, and whether the challenge resolved the issue.

With HAIL, the system should help expose the most important assumption or verification target so the user has a concrete cognitive foothold.

---

# Calibration Prompts

HAIL should provide lightweight opportunities for users to correct AI behavior.

Examples include checking understanding, assumptions, depth, challenge level, confidence, direction, or communication fit.

These should **not** appear as a fixed survey after every message. HAIL should surface 1–2 prompts only when they meaningfully reduce likely future friction.

---

# Progressive Scaffolding

HAIL should gradually reduce review assistance as the user demonstrates confidence within a domain.

The goal is:

> **scaffold → calibrate → get out of the way**

Confidence should emerge from competence and transparency, not reassurance.

---

# Recognizing Verification Boundaries

HAIL should help distinguish:

> “I don't understand this yet.”

from:

> “I cannot responsibly verify this without specialized expertise.”

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

Shared facts may be useful. Shared conclusions may create anchoring.

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

Automation should optimize more aggressively for predictability, structure, schemas, validation, monitoring, constrained actions, and deterministic behavior where possible.

Human-facing AI must adapt across expertise, confidence, learning style, communication style, ambiguity tolerance, cognitive load, risk tolerance, and desired depth.

> **Automation interfaces can be standardized aggressively. Human interaction should operate within guardrails but remain highly adaptive.**

---

# Emotional and Social Factors

Evaluation is not purely intellectual. A person may fail to challenge output because they feel intimidated, embarrassed, uncertain, overwhelmed, rushed, or afraid of appearing inexperienced.

HAIL should make disagreement psychologically inexpensive and avoid framing disagreement as user failure.

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

Surface important assumptions, suggest one high-value verification step when useful, expose verification-loop boundaries, and avoid telling users simply to trust the output.

## Phase 2 — Calibration Prompts

Introduce context-sensitive micro-prompts and measure whether they reduce corrective turns, repeated questions, misunderstood requirements, and unnecessary explanations.

## Phase 3 — Review Loop Detection

When appropriate:

1. summarize established findings
2. identify unresolved uncertainty
3. explain what evidence would advance the problem
4. offer a stopping point

## Phase 4 — Domain-Local Adaptation

Allow HAIL to adjust scaffolding based on demonstrated familiarity within the current task or domain.

Persistence should remain explicit rather than silently changing the user's long-term profile.

---

# Evaluation Questions

When testing this capability, evaluate cognitive load, understanding, confidence, stopping behavior, and adaptation quality.

Questions include:

- Did HAIL identify the important uncertainty before the user had to?
- Did the interaction reduce repetitive back-and-forth?
- Could the user explain why the output was considered reliable or uncertain?
- Did the user recognize when outside expertise was required?
- Did confidence increase because of evidence rather than reassurance?
- Did HAIL recognize unproductive verification loops without prematurely ending productive exploration?
- Did experienced users feel patronized?
- Did inexperienced users receive enough scaffolding?
- Did assistance adjust appropriately across different domains?

---

# Guiding Philosophy

Traditional AI optimization often asks:

> **How do we get the best output with the least interaction?**

HAIL should instead ask:

> **How do we minimize unnecessary cognitive effort while maximizing understanding?**

The objective is not fewer interactions at all costs. Some interaction prevents much larger misunderstandings.

The goal is to remove **unproductive** cognitive work: figuring out what to challenge, repeatedly requesting reassurance, reconstructing hidden assumptions, wondering whether enough verification has occurred, and continuing discussion after new information has stopped appearing.

---

# Key HAIL Principle

> **Help users know when to question, what to question, and when questioning has stopped producing useful information.**

Or, more broadly:

> **Optimize understanding, not merely usage.**
