using AIFirst.Core;

namespace AIFirst.Mcp;

/// <summary>
/// Minimal MCP client interface for tool discovery and invocation.
/// </summary>
public interface IMcpClient
{
    /// <summary>
    /// Lists all available tools from the MCP server.
    /// </summary>
    Task<IReadOnlyList<ToolContract>> ListToolsAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Calls a tool via the MCP server.
    /// </summary>
    Task<ToolResult> CallToolAsync(ToolCall call, CancellationToken cancellationToken = default);
}

/// <summary>
/// Placeholder MCP client adapter. Replace with a real transport implementation.
/// </summary>
public sealed class McpClientAdapter : IMcpClient
{
    /// <inheritdoc />
    public Task<IReadOnlyList<ToolContract>> ListToolsAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<ToolContract> tools = Array.Empty<ToolContract>();
        return Task.FromResult(tools);
    }

    /// <inheritdoc />
    public Task<ToolResult> CallToolAsync(ToolCall call, CancellationToken cancellationToken = default)
    {
        var result = new ToolResult(call.Name, "{}");
        return Task.FromResult(result);
    }
}
