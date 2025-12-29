namespace AIFirst.Core;

/// <summary>
/// Provides policy hooks for tool execution.
/// </summary>
public interface IPolicy
{
    /// <summary>
    /// Called before a tool is invoked. Use for validation, allowlist checks, or argument redaction.
    /// </summary>
    ValueTask OnBeforeToolCallAsync(ToolCall call, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Called after a tool completes. Use for output validation, audit logging, or result transformation.
    /// </summary>
    ValueTask OnAfterToolCallAsync(ToolCall call, ToolResult result, CancellationToken cancellationToken = default);
}

/// <summary>
/// Executes policies in order for tool calls.
/// </summary>
public sealed class PolicyPipeline
{
    private readonly IReadOnlyList<IPolicy> _policies;

    /// <summary>
    /// Creates a new policy pipeline with the specified policies.
    /// </summary>
    public PolicyPipeline(IEnumerable<IPolicy> policies)
    {
        _policies = policies.ToList();
    }

    /// <summary>
    /// Executes all policies' OnBeforeToolCallAsync hooks in order.
    /// </summary>
    public async ValueTask ExecuteBeforeAsync(ToolCall call, CancellationToken cancellationToken = default)
    {
        foreach (var policy in _policies)
        {
            await policy.OnBeforeToolCallAsync(call, cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Executes all policies' OnAfterToolCallAsync hooks in order.
    /// </summary>
    public async ValueTask ExecuteAfterAsync(ToolCall call, ToolResult result, CancellationToken cancellationToken = default)
    {
        foreach (var policy in _policies)
        {
            await policy.OnAfterToolCallAsync(call, result, cancellationToken).ConfigureAwait(false);
        }
    }
}
