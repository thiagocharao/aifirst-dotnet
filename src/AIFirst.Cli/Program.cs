using AIFirst.Mcp;

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
            var client = new McpClientAdapter();
            var tools = await client.ListToolsAsync();
            Console.WriteLine($"Tools discovered: {tools.Count}");
            return 0;
        }

        Console.WriteLine($"Unknown command: {args[0]}");
        return 1;
    }
}
