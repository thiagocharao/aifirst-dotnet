# AIFirst.DotNet

[![Build and Test](https://github.com/thiagocharao/aifirst-dotnet/actions/workflows/build.yml/badge.svg)](https://github.com/thiagocharao/aifirst-dotnet/actions/workflows/build.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/)

AIFirst.DotNet is an AI-first .NET SDK that makes MCP tool-calling feel like native C# with compile-time safety, policy enforcement, and observability. The MVP uses an attribute-based DSL so tool contracts become strongly-typed methods with analyzer support.

## Why AIFirst.DotNet?

If you're building AI-powered applications in .NET, you've likely hit these problems:

- 🔴 **Runtime tool failures** — typos in tool names, schema mismatches
- 🔴 **Security concerns** — PII leakage, prompt injection via tool outputs
- 🔴 **Debugging nightmares** — "Why did the agent do that?"
- 🔴 **Governance gaps** — no allowlists, no audit trails

AIFirst.DotNet solves these with:

✅ **Compile-time safety** — tool calls are strongly-typed, validated at build time  
✅ **Policy enforcement** — allowlists, redaction, and output checks built-in  
✅ **Observability** — trace every prompt and tool call for debugging and compliance  
✅ **MCP-native** — leverage the emerging standard for tool interoperability

## Features

### MCP Client
Connect to any MCP-compliant server and discover tools:

```csharp
await using var transport = new StdioMcpTransport("npx", "@modelcontextprotocol/server-filesystem", "/tmp");
await using var client = new McpClient(transport);

var tools = await client.ListToolsAsync();
var result = await client.CallToolAsync("read_file", new { path = "/tmp/test.txt" });
```

### JSON Schema Parser & Code Generation
Parse tool schemas and generate strongly-typed DTOs:

```csharp
// Parse JSON Schema
var schema = JsonSchemaParser.Parse(toolSchema);

// Generate C# records
var code = DtoGenerator.GenerateRecord(schema, "WeatherRequest", "MyApp.Tools");
```

### CLI Tool
```bash
# Discover tools from MCP server and save manifest
aifirst pull-tools npx @modelcontextprotocol/server-filesystem /tmp

# Generate C# DTOs from manifest
aifirst gen aifirst.tools.json --namespace MyApp.Tools --output Tools.cs
```

### Attribute DSL (Coming Soon)
```csharp
[Tool("weather.getForecast")]
public static partial Task<Forecast> GetForecastAsync(ForecastRequest request);
```

The source generator will emit MCP calls for these tool methods.

## Repo Layout

```
/src
  /AIFirst.Core          # Core abstractions, schema parser, code generator
  /AIFirst.Mcp           # MCP client and transports
  /AIFirst.Roslyn        # Source generator + analyzer
  /AIFirst.Cli           # CLI tool (aifirst)
  /AIFirst.DotNet        # Meta package (Core + Mcp + Roslyn)
/samples
  /HelloMcp              # Basic MCP connectivity
  /TypedToolsDemo        # Type-safe tool calls and code generation
  /PolicyAndTracingDemo  # Governance and observability
/tests
  /AIFirst.Core.Tests
  /AIFirst.Mcp.Tests
  /AIFirst.Roslyn.Tests
  /AIFirst.Cli.Tests
/docs
  design.md              # Architecture
  threat-model.md        # Security considerations
```

## Build

**Requirements:**
- .NET 8 SDK (CI environment requirement)
- .NET 6 SDK minimum for local development (with rollForward)

```bash
dotnet restore
dotnet build
dotnet test
```

## Packages

| Package | Description |
|---------|-------------|
| `AIFirst.DotNet` | Meta package (includes all below) |
| `AIFirst.Core` | Core abstractions, schema parser, code generator |
| `AIFirst.Mcp` | MCP client and transports |
| `AIFirst.Roslyn` | Source generator and analyzers |
| `AIFirst.Cli` | CLI tool (`aifirst`) |

## Roadmap

Track development progress and upcoming features on the [Issues page](https://github.com/thiagocharao/aifirst-dotnet/issues).

**Completed:**
- ✅ M0: Build foundation
- ✅ M1: MCP client with stdio transport
- ✅ M2: JSON Schema parser and DTO code generator

**In Progress:**
- 🚧 M3: Source generator and analyzers

**Planned:**
- M4: Policy pipeline and tracing
- M5: Documentation and release

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

## License

MIT License - see [LICENSE](LICENSE) for details.
