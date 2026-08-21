# Prompt-Hardening Experiment — `<preference>`

## Semantic intent

Describe the harness-neutral human intent. Do not write Claude- or Codex-specific wording here.

```text
<semantic intent>
```

## Current semantic value under test

```yaml
<preference>: <value>
```

## Baseline projection wording

### Claude

```text
<paste current Claude projection wording>
```

### Codex

```text
<paste current Codex projection wording>
```

## Observed failure mode

### Claude

- Expected:
- Actual:
- Failure pattern:
- Strength: strong / moderate / weak / unsupported / unclear

### Codex

- Expected:
- Actual:
- Failure pattern:
- Strength: strong / moderate / weak / unsupported / unclear

## Test scenarios

Use the same user prompts for all wording candidates unless the scenario itself is being revised.

### Scenario 1

Prompt:

```text
<prompt>
```

Expected observable behavior:

- 
- 

### Scenario 2

Prompt:

```text
<prompt>
```

Expected observable behavior:

- 
- 

### Scenario 3 — boundary / counterexample

Prompt:

```text
<prompt designed to ensure the preference does not over-apply>
```

Expected observable behavior:

- 
- 

## Candidate wording A

### Rationale

What specific failure is this wording trying to correct?

### Claude candidate

```text
<candidate wording>
```

### Codex candidate

```text
<candidate wording>
```

## Candidate A results

### Claude raw result

```text
<paste raw result>
```

Assessment:

- Scenario compliance:
- New unwanted behavior:
- Semantic drift observed: yes / no
- Strength: strong / moderate / weak / unsupported

### Codex raw result

```text
<paste raw result>
```

Assessment:

- Scenario compliance:
- New unwanted behavior:
- Semantic drift observed: yes / no
- Strength: strong / moderate / weak / unsupported

## Candidate wording B

Repeat only if candidate A leaves a concrete failure to address.

### Rationale

### Claude candidate

```text
<candidate wording>
```

### Codex candidate

```text
<candidate wording>
```

## Composition check

Only after the preference works reasonably alone, test it with one interacting semantic preference.

Interacting preference:

```yaml
<other_preference>: <value>
```

Expected composition:

- 
- 

Observed Claude behavior:

```text
<result>
```

Observed Codex behavior:

```text
<result>
```

## Decision

Chosen wording:

### Claude

```text
<chosen wording or no change>
```

### Codex

```text
<chosen wording or no change>
```

Why this wording was selected:

- 

Rejected alternatives and why:

- 

## Outcome

- Claude: strong / moderate / weak / unsupported
- Codex: strong / moderate / weak / unsupported
- Cross-harness semantic intent preserved: yes / no
- Ready to port into integration: yes / no

## Notes

A valid outcome can be **no change** if stronger wording creates over-application or semantic drift.
