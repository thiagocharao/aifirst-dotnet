using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AIFirst.Core;
using AIFirst.Mcp;
using AIFirst.Mcp.Transport;
using Xunit;

namespace AIFirst.Mcp.Tests;

/// <summary>
/// Tests for McpClient.
/// </summary>
public sealed class McpClientTests
{
    [Fact]
    public async Task ListToolsAsync_ParsesToolsCorrectly()
    {
        // Arrange
        var mockTransport = new MockMcpTransport();
        mockTransport.SetNextResponse(@"{
            ""jsonrpc"": ""2.0"",
            ""id"": ""1"",
            ""result"": {
                ""tools"": [
                    {
                        ""name"": ""test_tool"",
                        ""description"": ""A test tool"",
                        ""inputSchema"": {
                            ""type"": ""object"",
                            ""properties"": {
                                ""param1"": { ""type"": ""string"" }
                            }
                        }
                    }
                ]
            }
        }");

        var client = new McpClient(mockTransport);

        // Act
        var tools = await client.ListToolsAsync();

        // Assert
        Assert.Single(tools);
        Assert.Equal("test_tool", tools[0].Name);
        Assert.Equal("A test tool", tools[0].Description);
        Assert.Contains("param1", tools[0].ParametersSchemaJson);
    }

    [Fact]
    public async Task CallToolAsync_SendsCorrectRequest()
    {
        // Arrange
        var mockTransport = new MockMcpTransport();
        mockTransport.SetNextResponse(@"{
            ""jsonrpc"": ""2.0"",
            ""id"": ""1"",
            ""result"": {
                ""content"": ""success""
            }
        }");

        var client = new McpClient(mockTransport);
        var call = new ToolCall("test_tool", @"{""param1"": ""value1""}");

        // Act
        var result = await client.CallToolAsync(call);

        // Assert
        Assert.Equal("test_tool", result.Name);
        Assert.Contains("success", result.ResultJson);
        
        // Verify request was sent correctly
        var sentRequest = mockTransport.LastSentRequest;
        Assert.NotNull(sentRequest);
        Assert.Contains("tools/call", sentRequest);
        Assert.Contains("test_tool", sentRequest);
    }

    [Fact]
    public async Task CallToolAsync_ThrowsOnError()
    {
        // Arrange
        var mockTransport = new MockMcpTransport();
        mockTransport.SetNextResponse(@"{
            ""jsonrpc"": ""2.0"",
            ""id"": ""1"",
            ""error"": {
                ""code"": -32601,
                ""message"": ""Tool not found""
            }
        }");

        var client = new McpClient(mockTransport);
        var call = new ToolCall("nonexistent_tool", "{}");

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => client.CallToolAsync(call));
        Assert.Contains("Tool not found", ex.Message);
    }
}

/// <summary>
/// Mock transport for testing.
/// </summary>
internal sealed class MockMcpTransport : IMcpTransport
{
    private string? _nextResponse;
    public string? LastSentRequest { get; private set; }

    public bool IsConnected => true;

    public void SetNextResponse(string response)
    {
        _nextResponse = response;
    }

    public Task<string> SendRequestAsync(string jsonRpcRequest, CancellationToken cancellationToken = default)
    {
        LastSentRequest = jsonRpcRequest;
        return Task.FromResult(_nextResponse ?? "{}");
    }

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
