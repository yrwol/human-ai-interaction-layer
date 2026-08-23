# Manual Claude eval workflow

HAIL includes a manually dispatched GitHub Actions workflow for running Claude Code against a selected HAIL profile.

## Safety model

- The workflow is triggered only with `workflow_dispatch`.
- The Claude execution job references the `claude-evals` GitHub Environment.
- Configure that environment with required reviewers so the job cannot run or access its environment secrets until approved.
- No OAuth token or other Claude credential is committed to the repository.
- `CLAUDE_CODE_OAUTH_TOKEN` is referenced only as an environment secret placeholder.
- The workflow fails closed when that secret has not been configured.
- Repository permissions are read-only.
- Results are uploaded as workflow artifacts and are not committed automatically.

## GitHub setup

1. Open the repository on GitHub and go to **Settings → Environments**.
2. Create an environment named exactly `claude-evals`.
3. Enable **Required reviewers** and choose the account(s) allowed to approve a run.
4. Recommended: enable **Prevent self-review** if someone other than the dispatcher should approve runs.
5. Recommended: disable administrator bypass if you want the approval rule to be mandatory.
6. Under **Environment secrets**, add a secret named `CLAUDE_CODE_OAUTH_TOKEN` after generating the token locally.

GitHub does not expose environment secrets to a job until the environment's protection rules have passed.

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
3. Supply:
   - `prompt`: the eval prompt to send to Claude.
   - `model`: a Claude model alias or model ID; defaults to `sonnet`.
   - `hail_profile`: repository-relative HAIL profile path; defaults to `profiles/example.yaml`.
4. Dispatch the workflow.
5. The preflight job validates the request.
6. The `Run Claude eval` job waits for approval on the `claude-evals` environment.
7. After approval, Claude Code is installed, HAIL is installed into the runner's temporary home, and the prompt is run.
8. Download the `claude-eval-<run id>` artifact to inspect the response.

## Current scope

This is intentionally a small harness. It runs one prompt against one model/profile combination per dispatch. It does not automatically grade results, commit results, fan out matrices, or run on pushes or pull requests. Those can be added later once the basic authenticated/manual path has been validated.
