namespace AIFirst.Core;

/// <summary>
/// Provides policy hooks for tool execution.
/// </summary>
public interface IPolicy
{
    ValueTask OnBeforeToolCallAsync(ToolCall call, CancellationToken cancellationToken = default);
    ValueTask OnAfterToolCallAsync(ToolCall call, ToolResult result, CancellationToken cancellationToken = default);
}

/// <summary>
/// Executes policies in order for tool calls.
/// </summary>
public sealed class PolicyPipeline
{
    private readonly IReadOnlyList<IPolicy> _policies;

    public PolicyPipeline(IEnumerable<IPolicy> policies)
    {
        _policies = policies.ToList();
    }

    public async ValueTask ExecuteBeforeAsync(ToolCall call, CancellationToken cancellationToken = default)
    {
        foreach (var policy in _policies)
        {
            await policy.OnBeforeToolCallAsync(call, cancellationToken).ConfigureAwait(false);
        }
    }

    public async ValueTask ExecuteAfterAsync(ToolCall call, ToolResult result, CancellationToken cancellationToken = default)
    {
        foreach (var policy in _policies)
        {
            await policy.OnAfterToolCallAsync(call, result, cancellationToken).ConfigureAwait(false);
        }
    }
}
