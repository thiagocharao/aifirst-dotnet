using System.Text.Json;
using AIFirst.Core;
using AIFirst.Mcp.Transport;

namespace AIFirst.Mcp;

/// <summary>
/// MCP client interface for tool discovery and invocation.
/// </summary>
public interface IMcpClient : IAsyncDisposable
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
/// MCP client implementation using JSON-RPC 2.0 over a transport.
/// </summary>
public sealed class McpClient : IMcpClient
{
    private readonly IMcpTransport _transport;

    /// <summary>
    /// Creates a new MCP client with the specified transport.
    /// </summary>
    public McpClient(IMcpTransport transport)
    {
        _transport = transport ?? throw new ArgumentNullException(nameof(transport));
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<ToolContract>> ListToolsAsync(CancellationToken cancellationToken = default)
    {
        var request = JsonSerializer.Serialize(new
        {
            jsonrpc = "2.0",
            method = "tools/list",
            @params = new { }
        });

        var responseJson = await _transport.SendRequestAsync(request, cancellationToken);
        var responseDoc = JsonDocument.Parse(responseJson);

        if (responseDoc.RootElement.TryGetProperty("error", out var errorElem))
        {
            var errorMessage = errorElem.TryGetProperty("message", out var msgElem)
                ? msgElem.GetString()
                : "Unknown error";
            throw new InvalidOperationException($"MCP error: {errorMessage}");
        }

        var tools = new List<ToolContract>();
        if (responseDoc.RootElement.TryGetProperty("result", out var resultElem) &&
            resultElem.TryGetProperty("tools", out var toolsElem))
        {
            foreach (var toolElem in toolsElem.EnumerateArray())
            {
                var name = toolElem.GetProperty("name").GetString() ?? string.Empty;
                var description = toolElem.TryGetProperty("description", out var descElem)
                    ? descElem.GetString() ?? string.Empty
                    : string.Empty;

                var inputSchema = toolElem.TryGetProperty("inputSchema", out var inputElem)
                    ? inputElem.GetRawText()
                    : "{}";

                // MCP doesn't always provide return schemas, default to empty
                var returnSchema = "{}";

                var metadata = new Dictionary<string, string>();

                tools.Add(new ToolContract(name, description, inputSchema, returnSchema, metadata));
            }
        }

        return tools;
    }

    /// <inheritdoc />
    public async Task<ToolResult> CallToolAsync(ToolCall call, CancellationToken cancellationToken = default)
    {
        var argumentsObj = JsonSerializer.Deserialize<object>(call.ArgumentsJson);
        
        var request = JsonSerializer.Serialize(new
        {
            jsonrpc = "2.0",
            method = "tools/call",
            @params = new
            {
                name = call.Name,
                arguments = argumentsObj
            }
        });

        var responseJson = await _transport.SendRequestAsync(request, cancellationToken);
        var responseDoc = JsonDocument.Parse(responseJson);

        if (responseDoc.RootElement.TryGetProperty("error", out var errorElem))
        {
            var errorMessage = errorElem.TryGetProperty("message", out var msgElem)
                ? msgElem.GetString()
                : "Unknown error";
            throw new InvalidOperationException($"MCP error calling tool '{call.Name}': {errorMessage}");
        }

        var resultJson = responseDoc.RootElement.TryGetProperty("result", out var resultElem)
            ? resultElem.GetRawText()
            : "{}";

        return new ToolResult(call.Name, resultJson);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        await _transport.DisposeAsync();
    }
}
