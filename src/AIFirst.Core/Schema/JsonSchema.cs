namespace AIFirst.Core.Schema;

/// <summary>
/// Represents a parsed JSON Schema.
/// </summary>
public sealed record JsonSchema
{
    /// <summary>
    /// The JSON Schema type (object, string, number, integer, boolean, array, null).
    /// </summary>
    public string Type { get; init; } = "object";

    /// <summary>
    /// Schema title, used for generating class names.
    /// </summary>
    public string? Title { get; init; }

    /// <summary>
    /// Schema description.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Properties for object types.
    /// </summary>
    public IReadOnlyDictionary<string, JsonSchema> Properties { get; init; } = new Dictionary<string, JsonSchema>();

    /// <summary>
    /// Required property names for object types.
    /// </summary>
    public IReadOnlyList<string> Required { get; init; } = Array.Empty<string>();

    /// <summary>
    /// Schema for array items.
    /// </summary>
    public JsonSchema? Items { get; init; }

    /// <summary>
    /// Enum values for string types.
    /// </summary>
    public IReadOnlyList<string>? Enum { get; init; }

    /// <summary>
    /// Default value.
    /// </summary>
    public object? Default { get; init; }

    /// <summary>
    /// Whether this property is nullable.
    /// </summary>
    public bool Nullable { get; init; }

    /// <summary>
    /// Reference to another schema ($ref).
    /// </summary>
    public string? Ref { get; init; }

    /// <summary>
    /// Format hint (e.g., date-time, uri, email).
    /// </summary>
    public string? Format { get; init; }
}
