namespace AIFirst.Roslyn;

/// <summary>
/// Marks a partial method as an MCP tool call.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class ToolAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ToolAttribute"/> class.
    /// </summary>
    /// <param name="name">The name of the MCP tool to invoke.</param>
    public ToolAttribute(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Gets the name of the MCP tool.
    /// </summary>
    public string Name { get; }
}
