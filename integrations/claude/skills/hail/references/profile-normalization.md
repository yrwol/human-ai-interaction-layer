# HAIL profile normalization contract

This bundled reference is the runtime normalization contract for the Claude HAIL plugin. Repository-level semantic authority remains `spec/semantics.md`; this file exists so installed skills do not depend on repository-only documentation.

## Order of operations

For any stored profile:

1. read the stored profile without mutating it;
2. normalize supported legacy shapes into the current semantic model;
3. apply explicit current HAIL configuration intent after normalization;
4. validate the resulting current-schema profile;
5. persist only when the invoked action is allowed to mutate persistent configuration;
6. generate the Claude projection only from the validated normalized/current profile.

Precedence is:

```text
explicit current HAIL intent
→ stored current-schema value
→ compatibility-derived value
```

## Current compatibility rules

### Legacy profile without `step_pacing`

A valid stored profile that predates `step_pacing` is interpreted as:

```yaml
step_pacing: continuous
```

This preserves the effective behavior that existed before pacing became configurable.

If the current explicit HAIL request specifies a different pacing value, that requested value wins.

## Mutation boundary

Normalization is interpretation, not authorization to persist.

- Read-only actions such as `show` MUST NOT rewrite the stored profile merely to materialize a compatibility-derived value.
- Explicit mutating actions such as `setup`, `change`, or `reset` may persist a validated current-schema profile as part of the requested configuration change.
- Ordinary conversation MUST NOT persist profile changes.

## Maintenance rule

Do not add migration or compatibility behavior to individual action skills. Add or change compatibility semantics in the repository semantic contract first, then update this bundled runtime reference to match before release.
