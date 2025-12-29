namespace AIFirst.Core;

/// <summary>
/// Describes a tool contract discovered from an MCP server.
/// </summary>
public sealed record ToolContract(
    string Name,
    string Description,
    string ParametersSchemaJson,
    string ReturnsSchemaJson,
    IReadOnlyDictionary<string, string> Metadata);

/// <summary>
/// Represents a tool call request.
/// </summary>
public sealed record ToolCall(string Name, string ArgumentsJson);

/// <summary>
/// Represents the result of a tool call.
/// </summary>
public sealed record ToolResult(string Name, string ResultJson);
