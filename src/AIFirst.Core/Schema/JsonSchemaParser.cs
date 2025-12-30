using System.Text.Json;

namespace AIFirst.Core.Schema;

/// <summary>
/// Parses JSON Schema documents into <see cref="JsonSchema"/> objects.
/// </summary>
public static class JsonSchemaParser
{
    /// <summary>
    /// Parses a JSON Schema from a JSON string.
    /// </summary>
    /// <param name="json">The JSON Schema as a string.</param>
    /// <returns>A parsed <see cref="JsonSchema"/> object.</returns>
    public static JsonSchema Parse(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return new JsonSchema();

        using var doc = JsonDocument.Parse(json);
        return ParseElement(doc.RootElement);
    }

    /// <summary>
    /// Parses a JSON Schema from a <see cref="JsonElement"/>.
    /// </summary>
    public static JsonSchema ParseElement(JsonElement element)
    {
        if (element.ValueKind != JsonValueKind.Object)
            return new JsonSchema();

        var type = GetStringProperty(element, "type") ?? "object";
        var title = GetStringProperty(element, "title");
        var description = GetStringProperty(element, "description");
        var format = GetStringProperty(element, "format");
        var refValue = GetStringProperty(element, "$ref");
        var nullable = GetBoolProperty(element, "nullable");

        var properties = new Dictionary<string, JsonSchema>();
        if (element.TryGetProperty("properties", out var propsElement) && 
            propsElement.ValueKind == JsonValueKind.Object)
        {
            foreach (var prop in propsElement.EnumerateObject())
            {
                properties[prop.Name] = ParseElement(prop.Value);
            }
        }

        var required = new List<string>();
        if (element.TryGetProperty("required", out var reqElement) && 
            reqElement.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in reqElement.EnumerateArray())
            {
                if (item.ValueKind == JsonValueKind.String)
                    required.Add(item.GetString()!);
            }
        }

        JsonSchema? items = null;
        if (element.TryGetProperty("items", out var itemsElement))
        {
            items = ParseElement(itemsElement);
        }

        List<string>? enumValues = null;
        if (element.TryGetProperty("enum", out var enumElement) && 
            enumElement.ValueKind == JsonValueKind.Array)
        {
            enumValues = new List<string>();
            foreach (var item in enumElement.EnumerateArray())
            {
                if (item.ValueKind == JsonValueKind.String)
                    enumValues.Add(item.GetString()!);
            }
        }

        object? defaultValue = null;
        if (element.TryGetProperty("default", out var defaultElement))
        {
            defaultValue = GetDefaultValue(defaultElement);
        }

        return new JsonSchema
        {
            Type = type,
            Title = title,
            Description = description,
            Properties = properties,
            Required = required,
            Items = items,
            Enum = enumValues,
            Default = defaultValue,
            Nullable = nullable,
            Ref = refValue,
            Format = format
        };
    }

    private static string? GetStringProperty(JsonElement element, string name)
    {
        if (element.TryGetProperty(name, out var prop) && prop.ValueKind == JsonValueKind.String)
            return prop.GetString();
        return null;
    }

    private static bool GetBoolProperty(JsonElement element, string name)
    {
        if (element.TryGetProperty(name, out var prop))
        {
            if (prop.ValueKind == JsonValueKind.True) return true;
            if (prop.ValueKind == JsonValueKind.False) return false;
        }
        return false;
    }

    private static object? GetDefaultValue(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number => element.TryGetInt64(out var l) ? l : element.GetDouble(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => null,
            _ => element.GetRawText()
        };
    }
}
