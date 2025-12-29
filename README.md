# AIFirst.CSharp

AIFirst.CSharp is an AI-first C# SDK that makes MCP tool-calling feel like native C# with compile-time safety, policy enforcement, and observability. The MVP uses an attribute-based DSL (Option A) so tool contracts become strongly-typed methods with analyzer support.

## Goals

- **Typed tools** generated from MCP schemas
- **Compile-time validation** for tool names and argument shapes
- **Policy pipeline** for allowlists, redaction, and safety checks
- **Tracing + replay** for observability and debugging

## Repo layout

```
/src
  /AIFirst.Core
  /AIFirst.Mcp
  /AIFirst.Roslyn
  /AIFirst.Cli
/samples
  /HelloMcp
  /TypedToolsDemo
  /PolicyAndTracingDemo
/docs
  design.md
  roadmap.md
  threat-model.md
```

## Attribute DSL (Option A)

```csharp
[Tool("weather.getForecast")]
public static Task<Forecast> GetForecastAsync(ForecastRequest request);
```

The source generator will emit MCP calls for these tool methods.

## Build

This repository is currently a skeleton. Once .NET is available:

```
 dotnet build AIFirst.CSharp.sln
```

## Next steps

See `docs/roadmap.md` for the staged plan.
