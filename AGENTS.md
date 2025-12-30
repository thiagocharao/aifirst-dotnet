# AI Agent Instructions

These instructions apply to all coding agents working in this repo.

## CI-First (Mandatory)

Always run build and tests before pushing:
```bash
dotnet restore
dotnet build --configuration Release --no-restore
dotnet test --configuration Release --no-build --verbosity normal
```

If you changed anything in `src/**`, also build each library per target framework:
```bash
for project in src/AIFirst.Core/AIFirst.Core.csproj \
               src/AIFirst.Mcp/AIFirst.Mcp.csproj \
               src/AIFirst.Roslyn/AIFirst.Roslyn.csproj \
               src/AIFirst.DotNet/AIFirst.DotNet.csproj; do
  for framework in netstandard2.0 net6.0 net8.0; do
    dotnet build "$project" --configuration Release --no-restore -f "$framework"
  done
done
```

If you touched any packable project (`src/**`), `Directory.Build.props`, `Directory.Packages.props`,
`CHANGELOG.md`, or `README.md`, also run:
```bash
dotnet pack --configuration Release --no-build --output ./artifacts
```

## Architecture & Boundaries

- `AIFirst.Core` must not depend on MCP, Roslyn, CLI, or any orchestrator.
- `AIFirst.Mcp` and `AIFirst.Roslyn` depend on `AIFirst.Core` only.
- `AIFirst.Cli` depends on `AIFirst.Mcp`.
- `AIFirst.DotNet` is a meta package (no runtime code).
- Policy pipeline order is fixed: `OnBeforeToolCall` -> MCP invocation -> `OnAfterToolCall`.
- Avoid reflection/dynamic dispatch in hot paths; prefer generated code.
- Source generators/analyzers must be deterministic, incremental, and avoid I/O or network access.

## Target Frameworks & Compatibility

- Libraries target `netstandard2.0;net6.0;net8.0`.
- Tests and samples target `net8.0` only and are not packable.
- CLI targets `net8.0` and is packable as a tool.
- Ensure new APIs compile under `netstandard2.0` (use `#if` or polyfills when needed).
- Do not change target frameworks or `global.json` without updating CI and docs.

## Code Style & Quality

- Follow `.editorconfig` and keep builds warning-free.
- Nullable is enabled: fix warnings, avoid null suppression.
- Prefer records/immutable models (`init`, `with`).
- Add XML docs to public APIs in packable projects.

## Testing Guidance

- Add or update xUnit tests for behavior changes and regressions.
- Generator output and schema parsing must be covered by tests.
- Tests must be deterministic and avoid external network or filesystem dependencies.
- For quick iteration, run component tests for the area touched, then full tests before push.

## Security & Observability

- Treat tool outputs as untrusted; apply safety checks in `OnAfterToolCall`.
- Redact secrets before logging; tracing/logging must be opt-in by default.
- Use allowlists for tool names and network endpoints in production.

## Packaging & Dependencies

- Package versions are centralized in `Directory.Packages.props`.
- New dependencies must be justified and netstandard2.0-compatible for libraries.
- Keep packaging assets valid (`README.md`, `CHANGELOG.md`, `assets/aifirst-icon.png`).
- Update `CHANGELOG.md` for user-facing changes.
