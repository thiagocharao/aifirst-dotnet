using System.Diagnostics;
using System.Text.Json;

namespace AIFirst.Mcp.Transport;

/// <summary>
/// MCP transport over stdio (standard input/output).
/// Communicates with MCP servers via process stdin/stdout using JSON-RPC 2.0.
/// </summary>
public sealed class StdioMcpTransport : IMcpTransport
{
    private readonly Process _process;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly Dictionary<string, TaskCompletionSource<string>> _pendingRequests = new();
    private readonly CancellationTokenSource _disposalCts = new();
    private int _nextRequestId;
    private Task? _readerTask;

    /// <summary>
    /// Creates a new stdio MCP transport.
    /// </summary>
    /// <param name="serverCommand">Command to start the MCP server (e.g., "npx", "python").</param>
    /// <param name="serverArgs">Arguments for the server command.</param>
    public StdioMcpTransport(string serverCommand, string[] serverArgs)
    {
        _process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = serverCommand,
                Arguments = string.Join(" ", serverArgs),
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            }
        };

        _process.Start();
        _readerTask = Task.Run(ReadResponsesAsync);
    }

    /// <inheritdoc />
    public bool IsConnected => _process is { HasExited: false };

    /// <inheritdoc />
    public async Task<string> SendRequestAsync(string jsonRpcRequest, CancellationToken cancellationToken = default)
    {
        if (!IsConnected)
            throw new InvalidOperationException("Transport is not connected");

        var requestId = Interlocked.Increment(ref _nextRequestId).ToString();
        var tcs = new TaskCompletionSource<string>();

        await _lock.WaitAsync(cancellationToken);
        try
        {
            _pendingRequests[requestId] = tcs;

            var requestDoc = JsonDocument.Parse(jsonRpcRequest);
            var modifiedRequest = new
            {
                jsonrpc = "2.0",
                id = requestId,
                method = requestDoc.RootElement.GetProperty("method").GetString(),
                @params = requestDoc.RootElement.TryGetProperty("params", out var paramsElem)
                    ? JsonSerializer.Deserialize<object>(paramsElem.GetRawText())
                    : null
            };

            var requestJson = JsonSerializer.Serialize(modifiedRequest);
            await _process.StandardInput.WriteLineAsync(requestJson);
            await _process.StandardInput.FlushAsync();
        }
        finally
        {
            _lock.Release();
        }

        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _disposalCts.Token);
        linkedCts.Token.Register(() => tcs.TrySetCanceled());

        return await tcs.Task;
    }

    private async Task ReadResponsesAsync()
    {
        try
        {
            while (!_disposalCts.Token.IsCancellationRequested && IsConnected)
            {
                var line = await _process.StandardOutput.ReadLineAsync();
                if (line == null) break;
                if (string.IsNullOrWhiteSpace(line)) continue;

                try
                {
                    var responseDoc = JsonDocument.Parse(line);
                    if (responseDoc.RootElement.TryGetProperty("id", out var idElem))
                    {
                        var id = idElem.GetString();
                        if (id != null && _pendingRequests.TryGetValue(id, out var tcs))
                        {
                            _pendingRequests.Remove(id);
                            tcs.TrySetResult(line);
                        }
                    }
                }
                catch
                {
                    // Ignore malformed responses
                }
            }
        }
        catch (ObjectDisposedException)
        {
            // Expected during disposal
        }
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        _disposalCts.Cancel();

        if (_readerTask != null)
        {
            try { await _readerTask; }
            catch { /* Ignore */ }
        }

        foreach (var tcs in _pendingRequests.Values)
        {
            tcs.TrySetCanceled();
        }
        _pendingRequests.Clear();

        try
        {
            if (!_process.HasExited)
            {
                _process.Kill();
                _process.WaitForExit(1000);
            }
        }
        catch { /* Process already exited */ }

        _process.Dispose();
        _lock.Dispose();
        _disposalCts.Dispose();
    }
}
