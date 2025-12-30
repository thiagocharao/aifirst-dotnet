# Copilot Instructions

## CI-First Workflow (MANDATORY)

Keep the pipeline green. Do not suggest or generate changes that would fail these steps.

Always run build and tests before pushing (matches `.github/workflows/build.yml`):
```bash
dotnet restore
dotnet build --configuration Release --no-restore
dotnet test --configuration Release --no-build --verbosity normal
```

CI also explicitly builds each library per target framework. When touching `src/**`, run:
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

- Preserve layering: `AIFirst.Core` has no dependencies on MCP, Roslyn, CLI, or orchestrators.
- `AIFirst.Mcp` and `AIFirst.Roslyn` depend on `AIFirst.Core` only.
- `AIFirst.Cli` depends on `AIFirst.Mcp`.
- `AIFirst.DotNet` is a meta package (no runtime code).
- Policy pipeline order is fixed: `OnBeforeToolCall` -> MCP invocation -> `OnAfterToolCall`.
- Avoid reflection and dynamic dispatch in hot paths; favor generated code.
- Source generators/analyzers must be deterministic, incremental, and avoid I/O or network access.

## Target Frameworks & Project Rules

- Libraries (`AIFirst.Core`, `AIFirst.Mcp`, `AIFirst.Roslyn`, `AIFirst.DotNet`):
  `netstandard2.0;net6.0;net8.0`
- Tests and samples: `net8.0`, `<IsPackable>false</IsPackable>`, no XML docs.
- CLI tool: `net8.0` and packable (`PackAsTool=true`).
- Do not change target frameworks or `global.json` without updating CI and docs.
- Ensure new APIs compile cleanly under `netstandard2.0` (use `#if` when needed).

## Code Style & API Design

- Follow `.editorconfig` rules (file-scoped namespaces, `var`, Async suffix, `_` private fields).
- Nullable is enabled: avoid null suppression and fix warnings instead.
- Prefer records and immutable models (`init`, `with`).
- Avoid `async void` (except event handlers) and sync-over-async.
- Add XML docs to all public APIs in packable projects.
- Keep builds warning-free.

## Testing Requirements

- Use xUnit; add tests for new behavior and regressions.
- Schema parsing and generator output must be covered with tests.
- Tests must be deterministic and avoid external network or file system dependencies.
- Update docs (`docs/design.md`, `docs/threat-model.md`) when behavior or security posture changes.
- If you change `src/**`, run the full test suite (`dotnet test ...`) before pushing.
- For faster iteration, run the component tests you touched:
  - `src/AIFirst.Core/**` -> `dotnet test tests/AIFirst.Core.Tests/AIFirst.Core.Tests.csproj`
  - `src/AIFirst.Mcp/**` or `src/AIFirst.Mcp/Transport/**` -> `dotnet test tests/AIFirst.Mcp.Tests/AIFirst.Mcp.Tests.csproj`
  - `src/AIFirst.Roslyn/**` -> `dotnet test tests/AIFirst.Roslyn.Tests/AIFirst.Roslyn.Tests.csproj`
  - `src/AIFirst.Cli/**` -> `dotnet test tests/AIFirst.Cli.Tests/AIFirst.Cli.Tests.csproj`
  - Cross-cutting changes in `AIFirst.Core` or shared contracts -> run all tests.

## Security & Observability

- Treat tool outputs as untrusted; apply safety checks in `OnAfterToolCall`.
- Redact secrets (`password`, `token`, `secret`, etc.) before logging.
- Tracing/logging must be opt-in; default to no logging.
- Use allowlists for tool names and network endpoints in production.

## Packaging & Dependencies

- Package versions live in `Directory.Packages.props` only (no versions in `.csproj`).
- New dependencies must be justified and netstandard2.0-compatible for libraries.
- Analyzer/test packages should use `PrivateAssets=all` when appropriate.
- Keep packaging assets valid (`README.md`, `CHANGELOG.md`, `assets/aifirst-icon.png`).
- Update `CHANGELOG.md` for user-facing changes.

## Scope Discipline

- Keep PRs small (≤400 LOC) and focused.
- Prefer minimal, composable changes over large rewrites.
