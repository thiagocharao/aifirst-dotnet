using AIFirst.Mcp;
using AIFirst.Mcp.Transport;

Console.WriteLine("AIFirst.DotNet - Hello MCP Sample");
Console.WriteLine("==================================\n");

// Example: Connect to an MCP server via stdio
// Replace with actual MCP server command (e.g., "npx", "@modelcontextprotocol/server-filesystem")
var serverCommand = "echo"; // Placeholder - replace with real MCP server
var serverArgs = new[] { "{\"jsonrpc\":\"2.0\",\"result\":{\"tools\":[]}}" };

try
{
    await using var transport = new StdioMcpTransport(serverCommand, serverArgs);
    await using var client = new McpClient(transport);

    Console.WriteLine("Listing tools from MCP server...");
    var tools = await client.ListToolsAsync();

    if (tools.Count == 0)
    {
        Console.WriteLine("No tools available.");
        Console.WriteLine("\nNote: This is a demo. Configure a real MCP server in the code.");
    }
    else
    {
        Console.WriteLine($"Found {tools.Count} tool(s):\n");
        foreach (var tool in tools)
        {
            Console.WriteLine($"  - {tool.Name}: {tool.Description}");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    Console.WriteLine("\nNote: This sample requires a running MCP server.");
}
