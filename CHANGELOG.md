# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

#### Milestone 2: Typed Tools
- JSON Schema parser (`JsonSchemaParser`) for parsing MCP tool schemas
- C# type mapper (`CSharpTypeMapper`) for JSON → C# type conversion
- DTO code generator (`DtoGenerator`) for generating C# records from schemas
- CLI `pull-tools` command for MCP tool discovery
- CLI `gen` command for generating DTOs from tool manifests
- Support for nested objects, arrays, format hints (date-time, uri, uuid)
- netstandard2.0 compatibility with appropriate fallbacks

#### Milestone 1: Hello MCP
- MCP client (`McpClient`) with JSON-RPC 2.0 protocol
- Stdio transport (`StdioMcpTransport`) for process-based communication
- Tool discovery via `tools/list` RPC
- Tool invocation via `tools/call` RPC
- Async request/response correlation with thread-safe queueing

#### Milestone 0: Build Foundation
- Initial project structure
- Core abstractions: `ToolContract`, `ToolCall`, `ToolResult`
- Policy pipeline: `IPolicy`, `PolicyPipeline`
- Tracing primitives: `TraceEvent`, `ITraceSink`
- `[Tool]` attribute for marking tool methods
- Test projects for all libraries
- GitHub Actions CI/CD workflows
- .editorconfig for code style consistency
- NuGet package metadata for all projects

### Infrastructure
- Multi-target builds: netstandard2.0, net6.0, net8.0
- CodeQL security scanning
- All tests passing (34 tests)
- CI runs on every push/PR

## [0.1.0-alpha] - TBD

First alpha release targeting Milestone 3 completion.

[Unreleased]: https://github.com/thiagocharao/aifirst-dotnet/compare/main...HEAD
