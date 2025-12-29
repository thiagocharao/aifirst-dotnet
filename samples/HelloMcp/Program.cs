using AIFirst.Mcp;

var client = new McpClientAdapter();
var tools = await client.ListToolsAsync();
Console.WriteLine($"Hello MCP. Tools discovered: {tools.Count}");
