namespace AIFirst.Mcp.Transport;

/// <summary>
/// Transport abstraction for MCP communication.
/// </summary>
public interface IMcpTransport : IAsyncDisposable
{
    /// <summary>
    /// Sends a JSON-RPC request and waits for the response.
    /// </summary>
    Task<string> SendRequestAsync(string jsonRpcRequest, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Checks if the transport is connected.
    /// </summary>
    bool IsConnected { get; }
}
