# AIFirst.CSharp

[![Build and Test](https://github.com/thiagocharao/ai-first/actions/workflows/build.yml/badge.svg)](https://github.com/thiagocharao/ai-first/actions/workflows/build.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-6.0-purple.svg)](https://dotnet.microsoft.com/)

AIFirst.CSharp is an AI-first C# SDK that makes MCP tool-calling feel like native C# with compile-time safety, policy enforcement, and observability. The MVP uses an attribute-based DSL (Option A) so tool contracts become strongly-typed methods with analyzer support.

## Why AIFirst.CSharp?

If you're building AI-powered applications in .NET, you've likely hit these problems:

- 🔴 **Runtime tool failures** — typos in tool names, schema mismatches
- 🔴 **Security concerns** — PII leakage, prompt injection via tool outputs
- 🔴 **Debugging nightmares** — "Why did the agent do that?"
- 🔴 **Governance gaps** — no allowlists, no audit trails

AIFirst.CSharp solves these with:

✅ **Compile-time safety** — tool calls are strongly-typed, validated at build time  
✅ **Policy enforcement** — allowlists, redaction, and output checks built-in  
✅ **Observability** — trace every prompt and tool call for debugging and compliance  
✅ **MCP-native** — leverage the emerging standard for tool interoperability

## Goals

- **Typed tools** generated from MCP schemas
- **Compile-time validation** for tool names and argument shapes
- **Policy pipeline** for allowlists, redaction, and safety checks
- **Tracing + replay** for observability and debugging

## Repo layout

```
/src
  /AIFirst.Core          # Core abstractions
  /AIFirst.Mcp           # MCP client
  /AIFirst.Roslyn        # Source generator + analyzer
  /AIFirst.Cli           # CLI tool (aifirst)
/samples
  /HelloMcp              # Basic MCP connectivity
  /TypedToolsDemo        # Type-safe tool calls
  /PolicyAndTracingDemo  # Governance and observability
/tests
  /AIFirst.Core.Tests
  /AIFirst.Mcp.Tests
  /AIFirst.Roslyn.Tests
  /AIFirst.Cli.Tests
/docs
  design.md              # Architecture
  roadmap.md             # Development plan
  threat-model.md        # Security considerations
```

## Attribute DSL (Option A)

```csharp
[Tool("weather.getForecast")]
public static partial Task<Forecast> GetForecastAsync(ForecastRequest request);
```

The source generator will emit MCP calls for these tool methods.

## Build

```bash
dotnet restore
dotnet build
dotnet test
```

## Current Status

🚧 **MVP in Progress** — Milestone 0 (Build Foundation) complete

See [CHANGELOG.md](CHANGELOG.md) for details and [docs/roadmap.md](docs/roadmap.md) for the 6-week plan.

## Next steps

See `docs/roadmap.md` for the staged plan.

## Contributing

This project is in early development. Contributions welcome once Milestone 3 is complete.

## License

MIT License - see [LICENSE](LICENSE) for details.
