# HAIL Project Instructions

@AGENTS.md

## Claude Code notes

You are working on HAIL itself, not consuming a user's HAIL profile.

Follow `AGENTS.md` for project purpose, scope discipline, design rules, validation, and working style.

When modifying Claude Code support, keep Claude-specific behavior inside the Claude adapter or installer. Do not encode Claude concepts into the vendor-neutral profile model.

Do not confuse this repository-level `CLAUDE.md` with the user-level `~/.claude/CLAUDE.md` that HAIL's installer updates. The repository file guides development of HAIL; the user-level file imports generated HAIL interaction instructions for normal Claude Code usage.
