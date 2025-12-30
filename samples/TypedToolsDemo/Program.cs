using AIFirst.Core.CodeGen;
using AIFirst.Core.Schema;
using AIFirst.Roslyn;

namespace TypedToolsDemo;

/// <summary>
/// Demonstrates typed tool invocation with generated DTOs.
/// </summary>
public static class Program
{
    public static async Task Main()
    {
        Console.WriteLine("AIFirst.DotNet - Typed Tools Demo");
        Console.WriteLine("==================================\n");

        // Demo 1: Use existing typed tool
        Console.WriteLine("1. Using typed tool with [Tool] attribute:");
        var forecast = await ToolSamples.GetForecastAsync(new ForecastRequest("Lisbon"));
        Console.WriteLine($"   Forecast: {forecast.Summary}\n");

        // Demo 2: Show JSON Schema parsing
        Console.WriteLine("2. JSON Schema parsing demo:");
        var schemaJson = @"{
            ""type"": ""object"",
            ""title"": ""WeatherRequest"",
            ""properties"": {
                ""city"": { ""type"": ""string"", ""description"": ""City name"" },
                ""units"": { ""type"": ""string"", ""enum"": [""celsius"", ""fahrenheit""] }
            },
            ""required"": [""city""]
        }";
        
        var schema = JsonSchemaParser.Parse(schemaJson);
        Console.WriteLine($"   Parsed schema: {schema.Title}");
        Console.WriteLine($"   Properties: {string.Join(", ", schema.Properties.Keys)}");
        Console.WriteLine($"   Required: {string.Join(", ", schema.Required)}\n");

        // Demo 3: Show C# type mapping
        Console.WriteLine("3. C# type mapping:");
        foreach (var (name, propSchema) in schema.Properties)
        {
            var csharpType = CSharpTypeMapper.MapToCSharpType(propSchema, name);
            var isRequired = schema.Required.Contains(name);
            Console.WriteLine($"   {name}: {propSchema.Type} -> {csharpType} (required: {isRequired})");
        }
        Console.WriteLine();

        // Demo 4: Show DTO code generation
        Console.WriteLine("4. Generated C# DTO code:");
        var generatedCode = DtoGenerator.GenerateRecord(schema, "WeatherRequest", "TypedToolsDemo.Generated");
        Console.WriteLine(generatedCode);
    }
}

/// <summary>
/// Sample tool implementations using the [Tool] attribute.
/// </summary>
public static class ToolSamples
{
    /// <summary>
    /// Gets weather forecast for a city.
    /// </summary>
    [Tool("weather.getForecast")]
    public static Task<Forecast> GetForecastAsync(ForecastRequest request)
    {
        return Task.FromResult(new Forecast($"Sunny in {request.City}"));
    }
}

/// <summary>
/// Request DTO for weather forecast.
/// </summary>
public sealed record ForecastRequest(string City);

/// <summary>
/// Response DTO for weather forecast.
/// </summary>
public sealed record Forecast(string Summary);
