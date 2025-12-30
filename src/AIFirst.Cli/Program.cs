using AIFirst.Mcp;
using AIFirst.Mcp.Transport;

namespace AIFirst.Cli;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        Console.WriteLine("AIFirst CLI (skeleton)");
        Console.WriteLine("Commands: pull-tools, gen, replay");

        if (args.Length == 0)
        {
            return 0;
        }

        if (args[0].Equals("list-tools", StringComparison.OrdinalIgnoreCase))
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: list-tools <serverCommand> [serverArgs...]");
                return 1;
            }

            var serverCommand = args[1];
            var serverArgs = args.Skip(2).ToArray();
            await using var client = new McpClient(new StdioMcpTransport(serverCommand, serverArgs));
            var tools = await client.ListToolsAsync();
            Console.WriteLine($"Tools discovered: {tools.Count}");
            return 0;
        }

        Console.WriteLine($"Unknown command: {args[0]}");
        return 1;
    }
}
