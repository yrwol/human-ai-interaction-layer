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

The workflow-level `main` checks are defense in depth. GitHub Environment deployment restrictions are the security boundary for secret release.

## Threat model

Anyone able to modify workflow YAML can write code that attempts to read or exfiltrate any secret exposed to that job. GitHub secret masking is not a security boundary.

For that reason, do not approve a run after changing the secret-consuming workflow unless you have reviewed the version on `main` that will execute. The environment's branch restriction ensures a feature branch cannot obtain the `claude-evals` secret merely by dispatching its own workflow revision.

Repository administrators can still change environment configuration. This workflow does not attempt to protect credentials from a repository administrator who deliberately removes those protections.

## Required GitHub setup

1. Open the repository on GitHub and go to **Settings → Environments**.
2. Create an environment named exactly `claude-evals`.
3. Under **Deployment branches and tags**, select **Selected branches and tags** and allow only `main`.
4. Enable **Required reviewers** and choose the account(s) allowed to approve a run.
5. Enable **Prevent self-review** when someone other than the dispatcher can approve runs.
6. Disable administrator bypass if you want the configured protection rules to be mandatory during normal operation.
7. Under **Environment secrets**, add a secret named `CLAUDE_CODE_OAUTH_TOKEN` after generating the token locally.
8. Confirm that `CLAUDE_CODE_OAUTH_TOKEN` does NOT also exist as a repository-level or organization-level secret available to this repository.

GitHub does not expose environment secrets to a job until the environment's protection rules have passed.

## Recommended repository protection

If branch protection is enabled for `main`, require review before merge and consider CODEOWNERS coverage for `.github/workflows/claude-eval.yml`. This adds a review checkpoint before any secret-consuming workflow change can reach `main`.

These repository protections complement the environment gate; they do not replace the `main`-only environment deployment restriction.

## Claude credential setup

Generate the credential locally, outside GitHub Actions:

```bash
claude setup-token
```

Copy the resulting token directly into the `CLAUDE_CODE_OAUTH_TOKEN` environment secret in GitHub. Do not add the token to this repository, a workflow file, an issue, a PR, or an eval result.

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

## Current scope

This is intentionally a small harness. It runs one prompt against one model/profile combination per dispatch. It does not automatically grade results, commit results, fan out matrices, or run on pushes or pull requests. Those can be added later once the basic authenticated/manual path has been validated.
