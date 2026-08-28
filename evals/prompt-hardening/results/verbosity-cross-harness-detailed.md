# Verbosity detailed projection — cross-harness hardening

## Status

Pass. The strengthened `detailed` wording improves Codex differentiation while preserving Claude behavior and proportionality on simple factual questions.

## Candidate wording

`Provide detailed responses when the task benefits from depth. Add useful explanatory layers such as reasoning, interactions, examples, tradeoffs, edge cases, or implementation implications rather than merely expanding wording. Remain proportionate to simple requests.`

This wording was applied identically to the Claude and Codex projections for the experiment.

## Methodology

Single-turn only. Each run used a fresh harness session with the same HAIL semantic profile (`verbosity: detailed`) and the same prompt per harness. No multi-turn behavior is claimed here.

Harnesses:

- Codex: `gpt-5.5`, high reasoning effort
- Claude: Sonnet

HAIL ref: `eval/cross-harness-verbosity-detailed`

## Test 1 — differentiation prompt

Prompt:

`Explain how a horse temperament system could affect gameplay in a cozy horse game.`

### Codex

Run: `33130573422` (#60)

Observed response: ~602 words. The response added multiple distinct explanatory layers: care routines, training style, exploration, bonding, story/identity, economy/choices, and design guardrails. The additional depth was substantive rather than repetitive.

Compared with the previous Codex `detailed` result (~302 words), the candidate produced a clear increase in information depth and better separation from the prior balanced result (~291 words).

Verdict: pass.

### Claude

Run: `33130583992` (#62)

Observed response: ~615 words. The response included trait axes, concrete examples, downstream system interactions, trust progression, training minigames, riding feel, role suitability, breeding implications, cozy-specific constraints, and implementation-depth options.

The strengthened wording preserved Claude's useful detailed behavior and remained task-relevant.

Verdict: pass.

## Test 2 — simple factual proportionality

Prompt:

`What is a palomino?`

### Codex

Run: `33130578620` (#61)

Observed response: ~50 words. It gave the essential definition, clarified that palomino is a color rather than a breed, and briefly explained the chestnut + cream-dilution genetics.

The stronger `detailed` wording did not cause unnecessary expansion on a simple question.

Verdict: pass.

### Claude

Run: `33130589567` (#63)

Observed response: ~150 words. It added useful genetics, appearance range, registry context, and breed examples while staying proportionate to the simple factual request.

The response was more detailed than Codex but did not become indiscriminate or essay-like.

Verdict: pass.

## Conclusion

The candidate wording is a better cross-harness semantic projection for `verbosity: detailed` than the previous wording.

It fixes the observed Codex failure mode where `detailed` behaved too similarly to `balanced`, while preserving Claude's existing ability to add meaningful depth. Both harnesses also remained proportionate on the simple factual boundary.

Recommended action: promote the strengthened `detailed` projection in both Claude and Codex integrations.
