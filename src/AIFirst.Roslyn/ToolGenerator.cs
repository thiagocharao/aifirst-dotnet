using Microsoft.CodeAnalysis;

namespace AIFirst.Roslyn;

/// <summary>
/// Source generator that emits implementations for methods marked with <see cref="ToolAttribute"/>.
/// </summary>
[Generator]
public sealed class ToolGenerator : IIncrementalGenerator
{
    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // MVP placeholder: generator wiring will be added in a follow-up PR.
    }
}
