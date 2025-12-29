# Copilot Instructions

- Keep PRs small (≤400 LOC).
- Prefer records + immutable models.
- No reflection in hot paths; favor generated code.
- Add tests for schema parsing and generator output.
- Keep `AIFirst.Core` independent of any orchestrator.
- Tracing/logging must be opt-in and redact secrets by default.
