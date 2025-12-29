using AIFirst.Core;

var call = new ToolCall("demo.echo", "{\"message\":\"hello\"}");
var result = new ToolResult(call.Name, "{\"message\":\"hello\"}");

var pipeline = new PolicyPipeline(new IPolicy[] { new ConsolePolicy() });
await pipeline.ExecuteBeforeAsync(call);
await pipeline.ExecuteAfterAsync(call, result);

Console.WriteLine("Policy pipeline executed.");

internal sealed class ConsolePolicy : IPolicy
{
    public ValueTask OnBeforeToolCallAsync(ToolCall call, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Before tool call: {call.Name}");
        return ValueTask.CompletedTask;
    }

    public ValueTask OnAfterToolCallAsync(ToolCall call, ToolResult result, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"After tool call: {call.Name}");
        return ValueTask.CompletedTask;
    }
}
