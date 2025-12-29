using AIFirst.Roslyn;

namespace TypedToolsDemo;

public static class Program
{
    public static async Task Main()
    {
        var forecast = await ToolSamples.GetForecastAsync(new ForecastRequest("Lisbon"));
        Console.WriteLine($"Forecast summary: {forecast.Summary}");
    }
}

public static class ToolSamples
{
    [Tool("weather.getForecast")]
    public static Task<Forecast> GetForecastAsync(ForecastRequest request)
    {
        // Placeholder implementation until the source generator emits MCP calls.
        return Task.FromResult(new Forecast("Unavailable"));
    }
}

public sealed record ForecastRequest(string City);
public sealed record Forecast(string Summary);
