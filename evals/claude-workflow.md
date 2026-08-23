# Manual Claude eval workflow

HAIL includes a manually dispatched GitHub Actions workflow for running Claude Code against a selected HAIL profile.

## Safety model

- The workflow is triggered only with `workflow_dispatch`.
- Both workflow jobs require `github.ref == 'refs/heads/main'`.
- The Claude execution job references the `claude-evals` GitHub Environment.
- The `claude-evals` environment MUST restrict deployment branches/tags to `main` only. This is the primary control preventing a modified workflow on another branch from receiving the credential.
- Configure the environment with required reviewers so the job cannot run or access its environment secrets until approved.
- Enable **Prevent self-review** when a separate approver is available.
- Disable administrator bypass if you want environment protections to be mandatory during normal operation.
- Store `CLAUDE_CODE_OAUTH_TOKEN` ONLY as an environment secret in `claude-evals`. Do not duplicate it as a repository or organization secret.
- No OAuth token or other Claude credential is committed to the repository.
- The workflow fails closed when the environment secret has not been configured.
- Repository permissions are read-only.
- Results are uploaded as workflow artifacts and are not committed automatically.
- `.github/CODEOWNERS` covers the credential-consuming workflow; branch protection/rulesets must require Code Owner review for that to become an enforced merge requirement.

The workflow-level `main` checks are defense in depth. GitHub Environment deployment restrictions are the security boundary for secret release.

## Threat model

Anyone able to modify workflow YAML can write code that attempts to read or exfiltrate any secret exposed to that job. GitHub secret masking is not a security boundary.

For that reason, approving the `claude-evals` environment means trusting the exact credential-consuming workflow code already merged on `main`. Do not approve a run after workflow changes unless the merged version has been reviewed.

The environment's branch restriction ensures a feature branch cannot obtain the `claude-evals` secret merely by dispatching its own workflow revision. Workflow YAML cannot override the environment's configured branch restriction.

Repository administrators can still change environment or branch-protection configuration. This workflow does not attempt to protect credentials from a repository administrator who deliberately removes those controls.

## Required GitHub setup

1. Open the repository on GitHub and go to **Settings → Environments**.
2. Create an environment named exactly `claude-evals`.
3. Under **Deployment branches and tags**, select **Selected branches and tags** and allow only `main`.
4. Enable **Required reviewers** and choose the account(s) allowed to approve a run.
5. Enable **Prevent self-review** when someone other than the dispatcher can approve runs.
6. Disable administrator bypass if you want the configured protection rules to be mandatory during normal operation.
7. Under **Environment secrets**, add a secret named `CLAUDE_CODE_OAUTH_TOKEN` after generating the token locally.
8. Confirm that `CLAUDE_CODE_OAUTH_TOKEN` does NOT also exist as a repository-level or organization-level secret available to this repository.
9. Protect `main` and require pull-request review before merge.
10. Enable **Require review from Code Owners** so changes to `.github/workflows/claude-eval.yml` require explicit review from the owner listed in `.github/CODEOWNERS`.

GitHub does not expose environment secrets to a job until the environment's protection rules have passed.

## Claude credential setup

Generate the credential locally, outside GitHub Actions:

```bash
claude setup-token
```

Copy the resulting token directly into the `CLAUDE_CODE_OAUTH_TOKEN` environment secret in GitHub. Do not add the token to this repository, a workflow file, an issue, a PR, an eval result, or a repository/organization secret.

The workflow itself never generates, copies, or stores credentials in repository content.

## Running an eval

1. Open **Actions → Claude eval (manual)**.
2. Choose **Run workflow**.
3. Select the `main` branch. Runs from any other branch are skipped by the workflow and must also be blocked by the `claude-evals` environment configuration.
4. Supply:
   - `prompt`: the eval prompt to send to Claude.
   - `model`: a Claude model alias or model ID; defaults to `sonnet`.
   - `hail_profile`: repository-relative HAIL profile path; defaults to `profiles/example.yaml`.
5. Dispatch the workflow.
6. The preflight job validates the request.
7. The `Run Claude eval` job waits for approval on the `claude-evals` environment.
8. Before approving, confirm the run is from `main` and review any recent changes to `.github/workflows/claude-eval.yml`.
9. After approval, Claude Code is installed, HAIL is installed into the runner's temporary home, and the prompt is run.
10. Download the `claude-eval-<run id>` artifact to inspect the response.

## Codex parity

The same architecture can be used later for Codex with a separate protected environment such as `codex-evals` and a separate credential. Keep provider credentials isolated and apply the same manual-only, `main`-only, environment-only-secret, approval, and workflow-review protections.

Codex credential wiring is intentionally not part of this change.

## Current scope

This is intentionally a small harness. It runs one prompt against one model/profile combination per dispatch. It does not automatically grade results, commit results, fan out matrices, or run on pushes or pull requests. Those can be added later once the basic authenticated/manual path has been validated.
