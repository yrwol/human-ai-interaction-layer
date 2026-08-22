# Conversation Review Capability

Status: **proposed capability design**

This specification defines what HAIL means by **reviewing a conversation**: how it examines a completed or active human–AI interaction, identifies evidence of collaboration friction or success, relates that evidence to HAIL's interaction model, and recommends what—if anything—could be improved.

This is the behavioral contract behind a future discoverable `review` skill. Skill naming, routing, and invocation are defined separately in [`skill-based-capability-structure.md`](skill-based-capability-structure.md).

The capability is intentionally broader than the current persistent HAIL schema. A useful review system must be able to notice interaction problems HAIL cannot yet configure, while remaining conservative about turning those observations into permanent user preferences or new semantic fields.

## Why this capability exists

Users are usually better at noticing that an interaction felt frustrating than identifying which interaction mechanic caused the frustration.

A user may know:

> I kept having to fight the AI in this conversation.

without knowing whether the underlying problem was:

- excessive verbosity;
- repeated confirmation;
- poor decision ownership;
- context loss;
- too many simultaneous choices;
- bad tangent handling;
- inappropriate task decomposition;
- a temporary situational need;
- weak enforcement of an already-correct HAIL preference; or
- an interaction dimension HAIL does not model yet.

HAIL should translate real conversational evidence into an understandable interaction retrospective without requiring the user to know HAIL's schema first.

The core question is:

> Given this conversation, what interaction patterns helped or hindered the user, what evidence supports that conclusion, and what is the smallest appropriate response?

## Capability boundary

Conversation review evaluates the **quality and mechanics of the human–AI collaboration**.

It is not primarily:

- a factual correctness grader;
- a code reviewer;
- a writing-quality scorer;
- a sentiment classifier;
- a personality profiler;
- an automatic preference learner; or
- a mechanism for rewriting conversation history.

Those issues may be relevant when they directly caused interaction friction, but the review should keep the distinction clear.

For example:

```text
The assistant gave an incorrect answer.
→ correctness issue

The user corrected the answer, but the assistant repeatedly ignored the correction.
→ interaction issue: correction uptake / context handling
```

The second is in HAIL's review scope even if the underlying subject matter is outside HAIL.

## Design principles

### Evidence before inference

Review should prefer observable interaction evidence over speculation about the user.

Strong evidence includes:

- explicit user corrections;
- repeated requests to change the assistant's behavior;
- repeated restatement of already-provided context;
- explicit positive or negative feedback about how the interaction is going;
- repeated assistant behavior that conflicts with an explicit instruction;
- user intervention required to make progress resume.

Weak evidence includes:

- tone alone;
- one short reply with multiple plausible meanings;
- assumptions about diagnosis, neurotype, personality, mood, or cognitive ability;
- a single assistant behavior that produced no observable friction.

### Review the interaction, not the person

Findings should be phrased as:

> In this conversation, repeated confirmation created friction after execution authority had already been delegated.

not:

> The user hates confirmation.

One conversation can justify an interaction observation. It does not automatically justify a permanent trait claim.

### Persistent change remains intentional

Review may recommend a persistent profile change, but MUST NOT perform one automatically.

The user remains the authority over durable HAIL configuration.

### Existing schema is not the review vocabulary ceiling

Review MUST be able to report useful interaction signals that do not map cleanly to current persistent fields.

The current semantic model is deliberately small. Restricting review to those fields would prevent real usage from exposing missing concepts.

An unsupported signal may be reported as an **unmodeled interaction finding** without becoming a new field.

### Smallest appropriate response

Not every finding should become a profile change, eval, or product feature.

The review process should distinguish among:

- no action needed;
- situational/contextual adjustment;
- persistent profile candidate;
- projection/enforcement problem;
- eval candidate;
- semantic-model gap candidate;
- capability/design issue;
- insufficient evidence.

## Inputs

The review capability accepts one conversation/session as its primary evidence source.

Depending on harness support, the target may be:

- the current active conversation;
- a previous conversation selected by the user;
- a session identifier;
- an exported transcript;
- a local conversation file;
- a pasted transcript; or
- another harness-specific conversation reference.

The review engine SHOULD operate on a normalized conversation representation rather than directly encoding assumptions about Claude, Codex, or another vendor's storage format.

Conceptually:

```text
harness/session source
        ↓
source adapter
        ↓
normalized conversation
        ↓
conversation review
```

A source adapter may use harness-specific mechanisms, including documented or locally available session history, but the review contract must not depend on one undocumented storage format.

## Minimal normalized conversation representation

A normalized conversation needs enough information to reconstruct interaction sequence and attribution.

At minimum, each turn should preserve:

```text
role
content
order
```

When available, adapters MAY also preserve:

```text
timestamp
message/session identifier
tool or harness event metadata
explicit user feedback markers
active HAIL profile/projection metadata
```

Review should not require optional metadata to function.

The original ordering and authorship MUST be preserved. Normalization must not paraphrase away corrections, repeated instructions, or other evidence before analysis.

## Review process

The capability should follow a staged retrospective rather than jumping directly from transcript to profile recommendation.

### Stage 1 — Establish review context

Determine what is actually known about the interaction.

Useful context includes:

- whether the conversation is active or historical;
- the user's stated reason for requesting review, if any;
- the persistent HAIL profile that was active, if available;
- the relevant harness/projection, if available; and
- whether the user has identified a specific frustrating or successful section.

The user's framing is evidence, not a conclusion to blindly confirm.

For example:

> "This conversation was frustrating because it kept asking me questions."

should make confirmation/pacing behavior a high-priority area to inspect, while the transcript still determines what actually happened.

Do not require the user to provide a complaint or goal. A general review remains valid.

### Stage 2 — Identify interaction events

Scan the conversation for concrete events that may indicate friction, adaptation, or successful alignment.

Candidate event types include:

#### Explicit correction

The user directly tells the assistant its behavior is wrong or unwanted.

Examples:

```text
"stop asking me"
"I already told you that"
"too much detail"
"just pick one"
"don't lose the original task"
```

#### Repeated override

The user gives substantially the same behavioral correction more than once because the prior correction did not hold.

Repeated override is stronger evidence than a single correction.

#### Restatement burden

The user repeats information, constraints, or intent that the assistant should reasonably have retained from the conversation.

#### Progress interruption

The assistant unnecessarily stops forward motion and requires the user to re-authorize, re-decide, or reconstruct the next step.

#### Choice burden

The assistant creates more simultaneous decisions than the interaction appears to require, especially when the user subsequently asks for narrowing, ranking, or direct recommendation.

#### Misapplied structure

Task decomposition, pacing, or formatting makes a simple task harder to follow or fails to break down genuinely complex work when the user is struggling to proceed.

#### Tangent handling event

The assistant either loses the active goal while following a tangent or redirects so aggressively that a useful tangent is suppressed.

#### Context uptake failure

The assistant acknowledges new information but continues reasoning or acting as if the information had not been supplied.

#### Positive alignment signal

Review should detect what worked, not only failures.

Examples include:

- explicit user approval;
- smoother progression after the assistant changes approach;
- a correction that is successfully incorporated and remains incorporated;
- the user delegating more responsibility after successful recommendations;
- a previously blocked task progressing without additional friction.

Positive evidence is useful when deciding whether a behavior should remain unchanged.

### Stage 3 — Cluster events into findings

Individual events should be grouped into coherent interaction findings.

For example:

```text
Turn 12: user says "just decide"
Turn 16: assistant asks which option the user prefers
Turn 17: user says "I said you can pick"
Turn 23: assistant asks for approval again
```

should usually produce one finding about **decision ownership / unnecessary confirmation**, not four separate findings.

A finding should contain:

- the observed pattern;
- representative evidence;
- frequency or recurrence when meaningful;
- the interaction consequence;
- the likely HAIL relationship, if any; and
- confidence.

### Stage 4 — Classify the likely cause

HAIL should not assume every interaction failure means the persistent profile is wrong.

Each meaningful finding should be classified into the smallest plausible cause category.

#### Profile mismatch

The user's apparent desired default differs from the active persistent HAIL setting, with enough evidence to justify proposing a change.

Example:

```text
active: decision_mode: options
observed: user repeatedly asks the assistant to recommend first
```

Result:

> possible persistent profile change

#### Projection/enforcement failure

The persistent semantic value already expresses the user's desired behavior, but the harness did not reliably follow it.

Example:

```text
active: step_pacing: continuous
observed: assistant repeatedly stops for unnecessary approval
```

Result:

> profile is probably correct; projection/harness enforcement may need hardening

This distinction is critical. HAIL must not "fix" weak enforcement by changing the user's semantic intent.

#### Situational override

The user needed different behavior in this conversation, but evidence does not support changing the persistent default.

Example:

> User normally prefers detailed explanation but explicitly requested a compact answer during a time-sensitive task.

Result:

> contextual behavior; no persistent change recommended

#### Unmodeled interaction signal

The interaction problem appears meaningful, but the current HAIL semantic model cannot express it cleanly.

Result:

> record as a capability/taxonomy/semantic-gap candidate; do not invent a profile field during review

#### Task/domain failure

The primary problem belongs to task execution or domain correctness rather than interaction adaptation.

Result:

> identify the distinction and avoid forcing it into HAIL semantics

#### Insufficient evidence

There is not enough evidence to distinguish among plausible causes.

Result:

> report uncertainty rather than guessing

### Stage 5 — Compare with current HAIL semantics

When the active profile is available, review SHOULD check relevant findings against the authoritative semantic model in [`../semantics.md`](../semantics.md).

Current fields include:

- `verbosity`;
- `decision_mode`;
- `max_options`;
- `task_chunking`;
- `step_pacing`;
- `tangent_policy`.

The comparison is diagnostic, not prescriptive.

A finding may map to:

```text
one current field
multiple interacting fields
no current field
```

Do not force a one-to-one mapping where the evidence suggests composition or an unmodeled concept.

### Stage 6 — Assign confidence

Review should communicate uncertainty in plain language.

A simple three-level model is sufficient initially:

#### High confidence

Use when evidence is explicit and repeated or directly contradicted by subsequent assistant behavior.

Typical evidence:

- repeated user correction;
- explicit user statement of desired behavior plus repeated violation;
- clear mismatch between active profile and observed behavior.

#### Medium confidence

Use when a pattern is recurrent but the preferred alternative is partly inferred.

#### Low confidence

Use when evidence is sparse, ambiguous, or has multiple plausible explanations.

Low-confidence observations SHOULD NOT produce strong persistent-change recommendations.

Confidence applies to the **finding/cause interpretation**, not to a claim about the user's identity or diagnosis.

## Review output contract

The default review output should be understandable without exposing internal schema unless that detail is useful.

A useful structure is:

```text
Conversation review

What worked
- ...

Friction found
1. [finding]
   Evidence: ...
   Likely cause: ...
   Confidence: high / medium / low

What I would change
- ...

What I would leave alone
- ...
```

Exact formatting is harness-specific. The behavioral requirements are more important than the headings.

### Evidence should be inspectable

Findings SHOULD point back to representative turns or short excerpts when the source format allows it.

The user should be able to understand why HAIL reached the conclusion without being given an opaque score.

Do not flood the output with every turn that supports a repeated pattern. Prefer a few representative examples plus recurrence summary.

### Separate observation from recommendation

A review should clearly distinguish:

```text
Observed
→ what happened

Interpretation
→ why it may have happened

Recommendation
→ what, if anything, should change
```

This prevents a speculative recommendation from being presented as transcript fact.

## Recommendation types

A review may conclude with one or more of the following recommendations.

### No change

Use when the interaction was aligned or when isolated friction does not justify further action.

### Contextual strategy

Recommend a situational behavior for similar tasks without changing the persistent profile.

### Profile change candidate

Recommend a specific existing semantic change only when the evidence supports a likely durable preference mismatch.

The change remains proposed until the user explicitly chooses to persist it.

### Projection hardening candidate

Use when the active semantic preference already appears correct but the harness failed to express it reliably.

This is especially valuable to HAIL development because real conversation failures can become prompt-hardening evidence.

### Eval candidate

A concrete failure pattern may be suitable for a regression/evaluation scenario.

Review MAY explain what the candidate would test, but MUST NOT create or persist the eval automatically unless the user explicitly requests it.

A useful candidate captures:

```text
scenario
relevant context
observed failure
expected interaction behavior
related semantics or capability
```

### Semantic-gap candidate

Use when repeated or important friction cannot be represented by the current semantic model.

A single review may justify investigation, but does not by itself justify adding a persistent field.

### Capability/design candidate

Use when the improvement belongs to HAIL tooling or workflow rather than the user's interaction profile.

For example, difficulty selecting historical conversations is a review-source UX issue, not a new semantic preference.

## User feedback as calibration

After a review, the user's response to the findings is useful evidence.

Examples:

```text
"yes, exactly"
→ strengthens confidence in the interpretation

"no, I wanted that here, just not normally"
→ reclassify toward situational override

"that's not what bothered me"
→ the review missed the relevant signal; do not defend the original finding
```

This feedback MAY improve the current review or produce a proposed follow-up action.

It MUST NOT silently rewrite the persistent profile.

## Current-conversation review

When reviewing the active conversation, the reviewing assistant may also be one of the participants whose behavior is being evaluated.

HAIL should account for that limitation.

The review MUST privilege transcript evidence—especially user corrections and explicit feedback—over the assistant's retrospective explanation of its own intent.

The assistant should not excuse a visible interaction failure because it can construct a plausible reason for why it behaved that way.

Current-conversation review should remain safe to invoke without ending, rewriting, or corrupting the active session.

## Historical-conversation review

Reviewing another conversation should be read-only by default.

The capability MUST NOT:

- resume the target session merely to analyze it;
- inject review messages into the historical conversation;
- edit the stored transcript;
- delete turns;
- rewrite what either participant said; or
- depend on mutating harness session files.

Conversation history is evidence, not HAIL-owned state.

## Privacy and retention

Review may expose highly personal or project-specific conversation content.

Therefore:

- process only conversations the user has explicitly invoked, selected, supplied, or made available through the active harness workflow;
- do not copy the full transcript into persistent HAIL configuration;
- do not persist raw conversation content merely because review occurred;
- if saving an observation or eval is later supported, persist only the minimum evidence required and make that action explicit to the user.

Harness-specific access permissions remain the responsibility of the integration layer.

## Relationship to `compare`

Conversation review analyzes one interaction.

Conversation comparison should build on compatible review concepts but remains a separate capability because its question is comparative:

> What differed between these interactions, and which difference most plausibly explains the different collaboration outcome?

A future compare implementation MAY reuse normalized events/findings from review, but should not be implemented by hiding two independent review dumps next to each other.

## Relationship to review guidance

[`review-guidance.md`](review-guidance.md) addresses how HAIL can support a human who is reviewing AI-generated work with appropriate skepticism, confidence, and verification scaffolding.

This specification addresses something different:

> HAIL reviewing the **human–AI interaction itself** after or during a conversation.

The capabilities may share principles, but they should not be conflated.

## Validation scenarios

Initial manual evaluation should include cases where the expected classification differs even when surface behavior looks similar.

### Scenario A — profile mismatch

Profile:

```yaml
decision_mode: options
```

Conversation:

- assistant repeatedly offers neutral alternatives;
- user repeatedly asks for a recommendation.

Expected review:

- detect recurring decision-ownership friction;
- classify persistent profile mismatch as plausible;
- propose `recommend_first` as a candidate;
- do not persist automatically.

### Scenario B — projection failure

Profile:

```yaml
step_pacing: continuous
```

Conversation:

- user delegates execution;
- assistant repeatedly stops for approval;
- user tells it to continue without asking.

Expected review:

- detect unnecessary confirmation;
- recognize that the existing profile already requests continuous pacing;
- recommend projection/enforcement investigation rather than changing the semantic value.

### Scenario C — situational override

Profile:

```yaml
verbosity: detailed
```

Conversation:

- user says "I only have a minute—give me the short version";
- assistant gives a compact answer;
- no repeated evidence suggests a durable preference change.

Expected review:

- treat compactness as context-specific;
- recommend no persistent change.

### Scenario D — unmodeled signal

Conversation:

- user repeatedly corrects a collaboration behavior not expressible through current HAIL fields;
- the behavior causes meaningful repeated friction.

Expected review:

- describe the pattern in plain language;
- mark it as unmodeled rather than forcing it into the closest field;
- optionally identify it as a semantic/taxonomy research candidate;
- do not invent a new profile field.

### Scenario E — successful alignment

Conversation:

- assistant initially misreads desired pacing;
- user corrects it once;
- assistant adapts and maintains the correction;
- interaction then proceeds smoothly.

Expected review:

- acknowledge the initial friction;
- recognize successful correction uptake;
- avoid exaggerating one repaired event into a persistent failure.

### Scenario F — correctness vs interaction

Conversation:

- assistant gives one technically incorrect answer;
- user corrects it;
- assistant accepts the correction and proceeds appropriately.

Expected review:

- distinguish the technical error from interaction behavior;
- note that correction handling worked;
- do not misclassify the factual error as a HAIL profile problem.

## Implementation sequence

The smallest useful experiment is:

1. review the current Claude conversation from transcript context available to the skill;
2. identify explicit corrections and repeated overrides;
3. cluster those events into a small number of findings;
4. compare findings against the active HAIL profile when available;
5. classify findings as profile mismatch, projection failure, situational, unmodeled, task/domain, or uncertain;
6. produce evidence-backed recommendations without mutation;
7. manually test the validation scenarios above;
8. only then add historical-session source adapters.

This sequence tests whether the review reasoning is useful before investing in broad conversation-history plumbing.

## Non-goals

This capability does not require:

- automatic profile mutation;
- automatic semantic-field creation;
- automatic eval persistence;
- a shared runtime or MCP server;
- cloud conversation synchronization;
- universal access to all harness history;
- sentiment scoring;
- diagnosis or neurotype inference;
- editing historical session files;
- judging every technical claim in the conversation.

## Success criteria

Conversation review is useful when:

- a user can point HAIL at an interaction and receive an understandable explanation of what collaboration patterns helped or hurt;
- findings are grounded in visible conversational evidence;
- repeated friction is distinguished from isolated events;
- existing-profile mismatch is distinguished from weak harness enforcement;
- situational behavior is not promoted into durable preference by default;
- meaningful unmodeled signals can be surfaced without expanding the schema prematurely;
- review produces actionable next steps while leaving persistence under explicit user control; and
- real conversation failures can become better HAIL evidence without requiring users to understand the schema first.
