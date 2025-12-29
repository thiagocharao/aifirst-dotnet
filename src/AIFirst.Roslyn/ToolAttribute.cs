namespace AIFirst.Roslyn;

/// <summary>
/// Marks a partial method as an MCP tool call.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class ToolAttribute : Attribute
{
    public ToolAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}
