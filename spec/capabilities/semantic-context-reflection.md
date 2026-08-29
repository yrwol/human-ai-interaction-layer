# Semantic Context Reflection Capability

## Status

- **Capability type:** semantic conversation interpretation / context proposal generation
- **Status:** proposed; implementation and evaluation pending
- **Profile field:** none
- **Initial provider target:** Ollama through a provider abstraction
- **Persistence behavior:** non-mutating; reflection may propose context but MUST NOT directly alter canonical HAIL profile state

This specification defines how HAIL may examine conversational evidence and identify **candidate human-context signals** without relying on fixed keyword or phrase matching.

The capability exists to answer:

> Given what the user actually communicated in context, what—if anything—did this interaction teach us that could improve future human–AI collaboration?

It does not define automatic preference learning. It defines a governed semantic interpretation step that produces inspectable proposals for later policy, review, or explicit user action.

## Why this capability exists

Human-context signals are often expressed indirectly.

Useful evidence may look like:

- "I prefer short answers."
- "I like how you worded that."
- "That explanation made way more sense."
- "The first version was better."
- "I lose track when you give me all the steps at once."
- "Do it like this from now on."

A phrase-driven extractor can identify explicit constructions such as `I prefer`, but it will miss semantically similar feedback and over-apply when those same words appear in task-local statements such as:

- "I prefer option B for this implementation."
- "I like PostgreSQL."
- "I want to use Redis here."

The desired behavior is therefore semantic rather than lexical:

> HAIL should evaluate meaning, not vocabulary. Lexical patterns may be evidence, but they must not define context eligibility.

This capability introduces a semantic reflection layer that can interpret a bounded conversation window, identify context-relevant evidence, and return structured candidate proposals while keeping persistence and governance outside the model.

## Relationship to conversation review

Semantic context reflection and [conversation review](conversation-review.md) are related but distinct.

**Conversation review** is a retrospective capability that explains collaboration friction, success, and possible improvements to a user.

**Semantic context reflection** is an interpretation mechanism that converts conversational evidence into structured candidate context.

Conceptually:

```text
normalized conversation / evidence
        |
        +--> conversation review
        |       -> retrospective findings and recommendations
        |
        +--> semantic context reflection
                -> structured ContextProposal candidates
```

Both capabilities share evidence-first principles, but reflection is not itself a user-facing review report.

## Design principles

### Meaning over keywords

The reflector MUST interpret statements in conversational context rather than using specific words as the definition of eligibility.

Equivalent meanings may be expressed with different wording, and identical wording may have different scope depending on the surrounding conversation.

### Evidence before inference

The reflector should preserve the distinction between:

1. what was directly observed;
2. what may reasonably be inferred; and
3. what is not yet supported strongly enough to become durable context.

For example:

```text
User: "I like how you worded that."

Observation:
The user positively evaluated the wording of the previous assistant response.

Possible inference:
The user may prefer some characteristics of that response style.

Not yet justified:
A specific durable preference such as "prefers concise playful wording"
unless the evidence actually supports those characteristics.
```

### Models propose; HAIL governs

A reflection provider may interpret meaning and propose candidate context.

It MUST NOT be the authority that decides whether candidate context becomes canonical or persistent.

```text
LLM / reflector = understand meaning
HAIL policy      = decide what may happen with that meaning
```

### No silent persistence

Ordinary conversation MUST NOT silently rewrite persistent HAIL preferences.

Reflection may produce proposals from ordinary conversation, but persistent mutation remains subject to explicit HAIL governance and user-control rules.

### Context is scoped

A useful statement may be:

- globally relevant;
- interaction-specific;
- project-specific;
- task-specific;
- session-specific;
- temporary;
- or too ambiguous to scope confidently.

The reflector may suggest scope. HAIL owns scope validation and any promotion decision.

### No-result is valid

A successful reflection may produce zero proposals.

Ordinary acknowledgements, social filler, task-only details, or insufficiently informative messages should not be forced into human-context objects merely because reflection ran.

## Capability boundary

Semantic context reflection is responsible for identifying candidate human-context evidence and expressing it in a structured form.

It is not:

- a keyword preference extractor;
- an automatic profile updater;
- a personality profiler;
- a diagnostic system;
- a sentiment classifier;
- a general conversation summarizer;
- a replacement for HAIL persistence policy;
- a mechanism for widening user-data scope;
- or a requirement that every conversation produce context.

The capability MAY identify interaction feedback, contextual constraints, or working-pattern evidence even when that evidence does not map to a current persistent HAIL field.

An unsupported concept may remain a proposal or unmodeled signal without forcing schema expansion.

## Conceptual architecture

```text
Conversation / Evidence
        |
        v
normalized bounded evidence window
        |
        v
Semantic Context Reflector
        |
        v
ContextProposal[]
        |
        v
HAIL validation / scope / policy
        |
        +--> reject
        +--> retain as evidence
        +--> request review
        +--> expose as candidate
        +--> explicit promotion path
```

The reflector is intentionally separated from canonical profile state.

## Inputs

### Bounded conversation evidence

The reflector SHOULD receive enough surrounding context to resolve references such as:

- "that";
- "this way";
- "the first one";
- "exactly";
- "much better";
- "too much";
- "do that again."

A single isolated user message is often insufficient.

At minimum, normalized turns should preserve:

```text
role
content
order
```

When available, reflection input MAY also include:

```text
timestamp
message/session identifier
consumer/harness
active HAIL context
explicit feedback metadata
evidence identifiers
```

The original conversational wording should be preserved. Input normalization must not paraphrase away the evidence before semantic interpretation occurs.

### Example conceptual input

```yaml
reflection_input:
  evidence:
    - role: assistant
      content: "Here is the explanation..."
    - role: user
      content: "I like how you worded that"

  metadata:
    session_id: example-session
    consumer: claude-code
```

The final transport schema is not frozen by this document.

## Output model

Reflection returns zero or more structured candidate proposals.

The existing HAIL proposal/evidence model should be reused where practical rather than creating an independent persistence system.

A proposal should preserve both the observation and any interpretation.

Conceptually:

```yaml
proposal:
  kind: interaction_feedback

  observation:
    text: "User positively responded to the wording of the prior response."

  candidate_context:
    text: "User may prefer characteristics of this response style."

  evidence:
    - evidence_event_id: example-evidence-id

  suggested_scope: interaction
  confidence: 0.58

  inference:
    level: weak
    basis: explicit_positive_feedback

  durability:
    suggested: provisional
```

The exact schema may evolve, but implementations SHOULD preserve these distinctions:

- source evidence;
- observed signal;
- candidate abstraction;
- suggested scope;
- uncertainty/confidence;
- inference strength;
- provenance.

A valid result may be:

```yaml
proposals: []
```

## Signal families

Signal families help reflection and evaluation organize meaning. They MUST NOT become phrase dictionaries.

Initial families may include:

- explicit preference;
- implicit preference or feedback;
- positive interaction feedback;
- negative interaction feedback;
- correction;
- constraint;
- working pattern;
- communication need;
- stable contextual fact;
- project or task context;
- comparative preference;
- insufficient or non-durable signal.

These categories are interpretive labels, not persistence guarantees.

For example, a statement may clearly contain a preference while still being task-local and inappropriate for persistent HAIL profile state.

## Scope interpretation

Reflection should distinguish semantic usefulness from persistence scope.

Examples:

| Evidence | Likely semantic signal | Persistence implication |
| --- | --- | --- |
| "I prefer short answers." | explicit interaction preference | durable candidate may be justified |
| "I like how you worded that." | positive interaction feedback | retain evidence; specific durable preference may not yet be justified |
| "The first version was better." | comparative feedback | useful evidence; scope depends on what differed |
| "I prefer option B for this implementation." | task-local decision preference | should not become a global interaction preference |
| "I lose track when you give me all the steps at once." | interaction constraint | strong candidate for interaction-level context |
| "Cool." | ambiguous acknowledgement | usually no proposal |
| "Thanks!" | conversational acknowledgement | usually no proposal |

The important separation is:

```text
"contains useful context signal"
!=
"belongs in persistent global profile"
```

## Provider abstraction

Ollama is the first intended provider because local semantic interpretation can support privacy-sensitive, user-controlled reflection without requiring a hosted model dependency.

Ollama MUST NOT become a hard architectural dependency of HAIL Core.

Conceptually:

```csharp
public interface IContextReflector
{
    Task<ReflectionResult> ReflectAsync(
        ReflectionInput input,
        CancellationToken cancellationToken);
}
```

Potential providers:

```text
IContextReflector
├── OllamaContextReflector
└── future providers
    ├── OpenAI-compatible provider
    ├── Anthropic provider
    └── harness-native provider
```

All providers should emit the same provider-neutral reflection result shape.

Provider-specific prompt wording, model selection, and transport belong in adapters rather than in the semantic capability contract.

## Governance boundary

### Reflector responsibilities

The reflector may:

- interpret references using surrounding conversation;
- identify context-relevant evidence;
- classify signal families;
- synthesize related evidence within the bounded window;
- suggest an abstraction;
- suggest scope;
- estimate confidence or inference strength;
- explain which evidence supports the proposal.

### HAIL responsibilities

HAIL policy and surrounding capability logic own:

- allowed scope;
- permission boundaries;
- validation;
- deduplication;
- conflicts;
- provenance;
- lifecycle and expiry;
- promotion rules;
- review requirements;
- canonical context mutation;
- persistence.

### Explicit prohibitions

A reflector MUST NOT:

- directly modify canonical context;
- modify persistent HAIL profile state;
- approve its own proposal;
- alter permissions;
- silently widen scope;
- delete existing context;
- infer diagnoses or protected/sensitive identity claims as interaction preferences;
- treat confidence alone as authorization to persist.

## Reflection lifecycle

The initial conceptual lifecycle is:

```text
1. interaction occurs
2. evidence is available for reflection
3. a bounded evidence window is assembled
4. IContextReflector is invoked
5. structured proposals are returned
6. HAIL validates proposal shape and policy
7. accepted proposals are retained as candidates/evidence
8. any persistent change follows a separate governed path
```

For the initial implementation, reflection SHOULD NOT be required synchronously on every message.

Batching, session-boundary reflection, explicit user-triggered reflection, or other trigger strategies may be evaluated separately.

Trigger policy is intentionally not frozen in this specification.

## Evaluation model

Evaluation should test semantic interpretation, not keyword recognition.

The central evaluation question is:

> Given this conversation window, what human-context evidence should the reflector surface, and what should HAIL subsequently accept, reject, or keep provisional?

### Evaluation layers

Reflection and policy should be tested separately where possible.

```text
Interpreter evaluation
"Did the model understand the evidence appropriately?"

Policy evaluation
"Did HAIL make the correct governance decision about that interpretation?"
```

This separation makes failures attributable.

### Semantic fixture families

Fixtures SHOULD include lexical diversity and semantic lookalikes.

#### Positive feedback

Examples:

```text
"I prefer answers like that."
"I like how you worded that."
"YES exactly."
"That explanation finally clicked."
"Do it like this from now on."
```

These may carry related semantic signals despite very different wording.

#### Non-contextual or differently scoped lookalikes

Examples:

```text
"I like PostgreSQL."
"I enjoyed the movie."
"I want to use Redis for this."
"I prefer option B for this implementation."
```

These may contain meaningful task or personal content without justifying a global interaction preference.

#### Context-dependent ambiguity

Examples:

```text
"Perfect."
"That."
"Much better."
"lol exactly"
```

These require surrounding turns and may still produce no durable proposal.

### Evaluation dimensions

Initial evaluation SHOULD cover:

- **semantic recall:** useful indirect signals are recognized;
- **semantic precision:** ordinary or differently scoped statements do not become inappropriate HAIL preferences;
- **context resolution:** references such as "that" are interpreted using the bounded conversation window;
- **inference restraint:** proposals do not invent unsupported reasons;
- **scope restraint:** task-local evidence is not silently generalized;
- **no-result behavior:** empty proposal sets are produced when appropriate;
- **provider consistency:** different providers can be compared against the same fixtures without changing capability semantics;
- **malformed-output handling:** provider output cannot bypass HAIL validation.

Hardening should focus on meaning and boundary behavior rather than adding phrase-specific rules each time a lexical variant fails.

## Initial implementation slice

The first implementation milestone should remain deliberately small.

It is complete when HAIL can:

1. assemble a bounded normalized conversation/evidence window;
2. invoke an Ollama implementation through `IContextReflector`;
3. receive schema-valid zero-or-more context proposals;
4. preserve observation, inference, evidence, confidence, and suggested scope distinctions;
5. reject malformed or policy-invalid proposals without trusting the model;
6. demonstrate semantic fixtures where indirect evidence is recognized;
7. demonstrate counterexamples where similar words must not create the same persistent-context implication.

The initial slice does **not** require:

- automatic persistent profile mutation;
- embeddings or vector retrieval;
- long-term cross-session evidence aggregation;
- automatic confidence-based promotion;
- cloud sync;
- hosted model support;
- every-message synchronous reflection;
- a polished user interface;
- a frozen universal proposal schema.

## Privacy and data handling

Reflection operates on conversational evidence and may therefore receive sensitive user content.

Provider adapters SHOULD minimize the evidence window to what is reasonably required for interpretation.

Local providers such as Ollama are useful because they can keep reflection on the user's machine, but locality does not remove the need for explicit data boundaries.

HAIL MUST continue to avoid indiscriminately forwarding interaction/profile data to downstream tools.

Future remote-provider support must make data transmission explicit and consistent with HAIL's privacy principles.

## Open questions

The following are intentionally left for implementation/evaluation evidence:

- What reflection trigger strategy provides the best usefulness/cost tradeoff?
- How large should the default evidence window be?
- Should multiple related weak observations be aggregated before becoming a stronger proposal?
- Which confidence representation is useful enough to standardize?
- Which proposal types should be eligible for automatic retention as evidence without user review?
- How should contradictory evidence be represented?
- How should reflection interact with temporary/session-specific state?
- When should a reflection finding feed conversation review versus remain internal proposal evidence?
- How should provider/model/version metadata be recorded for reproducibility?

These should be resolved through focused experiments rather than prematurely hardened into the semantic contract.

## Documentation impact

This capability should be referenced from:

- `spec/README.md` as a focused capability specification;
- `spec/product.md` as a proposed capability that extends the current architecture without changing the persistent-profile source-of-truth rule;
- `spec/roadmap.md` when implementation/evaluation work is scheduled.

No change to `spec/semantics.md` is required merely because this capability is specified. Reflection may discover candidate signals broader than the currently supported persistent semantic fields.

## Core acceptance principle

The capability should preserve this boundary:

> HAIL may use an LLM to understand what a user communicated, but the LLM does not get to decide what becomes durable user context.

And the hardening principle is:

> Evaluate what the user meant in context; do not teach the system an ever-growing dictionary of preference phrases.
