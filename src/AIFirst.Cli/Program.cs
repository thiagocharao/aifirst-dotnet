using System.Text.Json;
using AIFirst.Core;
using AIFirst.Core.CodeGen;
using AIFirst.Core.Schema;
using AIFirst.Mcp;
using AIFirst.Mcp.Transport;

namespace AIFirst.Cli;

/// <summary>
/// AIFirst CLI entry point.
/// </summary>
public static class Program
{
    /// <summary>
    /// CLI entry point.
    /// </summary>
    public static async Task<int> Main(string[] args)
    {
        if (args.Length == 0)
        {
            PrintHelp();
            return 0;
        }

        var command = args[0].ToLowerInvariant();
        var commandArgs = args.Skip(1).ToArray();

        return command switch
        {
            "pull-tools" => await PullToolsAsync(commandArgs),
            "gen" => GenerateCode(commandArgs),
            "help" or "--help" or "-h" => PrintHelp(),
            _ => UnknownCommand(command)
        };
    }

    private static int PrintHelp()
    {
        Console.WriteLine("AIFirst CLI - Tool for working with MCP tools");
        Console.WriteLine();
        Console.WriteLine("Commands:");
        Console.WriteLine("  pull-tools <server> [args...]  Connect to MCP server and save tool manifest");
        Console.WriteLine("  gen <manifest> [options]       Generate C# DTOs from tool manifest");
        Console.WriteLine("  help                           Show this help message");
        Console.WriteLine();
        Console.WriteLine("Examples:");
        Console.WriteLine("  aifirst pull-tools npx @modelcontextprotocol/server-filesystem /tmp");
        Console.WriteLine("  aifirst gen aifirst.tools.json --namespace MyApp.Tools --output Tools.cs");
        return 0;
    }

    private static int UnknownCommand(string command)
    {
        Console.Error.WriteLine($"Unknown command: {command}");
        Console.Error.WriteLine("Run 'aifirst help' for usage information.");
        return 1;
    }

    private static async Task<int> PullToolsAsync(string[] args)
    {
        if (args.Length < 1)
        {
            Console.Error.WriteLine("Usage: aifirst pull-tools <serverCommand> [serverArgs...]");
            return 1;
        }

        var serverCommand = args[0];
        var serverArgs = args.Skip(1).ToArray();
        var outputPath = "aifirst.tools.json";

        Console.WriteLine($"Connecting to MCP server: {serverCommand} {string.Join(" ", serverArgs)}");

        try
        {
            await using var transport = new StdioMcpTransport(serverCommand, serverArgs);
            await using var client = new McpClient(transport);

            Console.WriteLine("Discovering tools...");
            var tools = await client.ListToolsAsync();

            Console.WriteLine($"Found {tools.Count} tool(s)");

            var manifest = new
            {
                version = "1.0",
                generatedAt = DateTime.UtcNow.ToString("o"),
                server = serverCommand,
                tools = tools.Select(t => new
                {
                    name = t.Name,
                    description = t.Description,
                    inputSchema = t.ParametersSchemaJson,
                    metadata = t.Metadata
                })
            };

            var json = JsonSerializer.Serialize(manifest, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(outputPath, json);

            Console.WriteLine($"Manifest saved to: {outputPath}");
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return 1;
        }
    }

    private static int GenerateCode(string[] args)
    {
        var manifestPath = "aifirst.tools.json";
        var outputPath = "Tools.g.cs";
        var namespaceName = "Generated";

        for (var i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--namespace" or "-n" when i + 1 < args.Length:
                    namespaceName = args[++i];
                    break;
                case "--output" or "-o" when i + 1 < args.Length:
                    outputPath = args[++i];
                    break;
                default:
                    if (!args[i].StartsWith("-") && File.Exists(args[i]))
                        manifestPath = args[i];
                    break;
            }
        }

        if (!File.Exists(manifestPath))
        {
            Console.Error.WriteLine($"Manifest file not found: {manifestPath}");
            Console.Error.WriteLine("Usage: aifirst gen [manifest] [--namespace <name>] [--output <path>]");
            return 1;
        }

        try
        {
            Console.WriteLine($"Reading manifest: {manifestPath}");
            var json = File.ReadAllText(manifestPath);
            var doc = JsonDocument.Parse(json);

            var tools = new List<ToolContract>();
            if (doc.RootElement.TryGetProperty("tools", out var toolsElement))
            {
                foreach (var toolElement in toolsElement.EnumerateArray())
                {
                    var name = toolElement.GetProperty("name").GetString() ?? "";
                    var description = toolElement.TryGetProperty("description", out var descElem) 
                        ? descElem.GetString() ?? "" 
                        : "";
                    var inputSchema = toolElement.TryGetProperty("inputSchema", out var schemaElem)
                        ? schemaElem.GetRawText()
                        : "{}";

                    tools.Add(new ToolContract(name, description, inputSchema, "{}", 
                        new Dictionary<string, string>()));
                }
            }

            Console.WriteLine($"Generating DTOs for {tools.Count} tool(s)...");

            var code = DtoGenerator.GenerateFromTools(tools, namespaceName);
            File.WriteAllText(outputPath, code);

            Console.WriteLine($"Generated code saved to: {outputPath}");
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return 1;
        }
    }
}
