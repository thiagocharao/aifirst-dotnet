# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Initial project structure
- Core abstractions: `ToolContract`, `ToolCall`, `ToolResult`
- Policy pipeline: `IPolicy`, `PolicyPipeline`
- Tracing primitives: `TraceEvent`, `ITraceSink`
- MCP client interfaces: `IMcpClient`, `McpClientAdapter` (placeholder)
- Roslyn source generator skeleton: `ToolGenerator`
- `[Tool]` attribute for marking tool methods
- CLI skeleton with placeholder commands
- Test projects for all libraries
- GitHub Actions CI/CD workflows
- .editorconfig for code style consistency
- NuGet package metadata for all projects

### Infrastructure
- Build succeeds on .NET 6.0
- All tests passing
- CI runs on every push/PR
- Release workflow ready for NuGet publishing

## [0.1.0-alpha] - TBD

First alpha release - coming soon!

[Unreleased]: https://github.com/thiagocharao/ai-first/compare/main...HEAD
