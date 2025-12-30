using System.Collections.Generic;
using AIFirst.Core;
using AIFirst.Core.CodeGen;
using AIFirst.Core.Schema;
using Xunit;

namespace AIFirst.Core.Tests;

public class DtoGeneratorTests
{
    [Fact]
    public void GenerateRecord_SimpleSchema_GeneratesCorrectCode()
    {
        var schema = new JsonSchema
        {
            Type = "object",
            Properties = new Dictionary<string, JsonSchema>
            {
                ["name"] = new JsonSchema { Type = "string" },
                ["age"] = new JsonSchema { Type = "integer" }
            },
            Required = new[] { "name" }
        };

        var code = DtoGenerator.GenerateRecord(schema, "Person", "MyApp");

        Assert.Contains("namespace MyApp;", code);
        Assert.Contains("public sealed record Person(", code);
        Assert.Contains("string Name", code);
        Assert.Contains("int? Age", code);
    }

    [Fact]
    public void GenerateRecord_WithDescription_AddsXmlDoc()
    {
        var schema = new JsonSchema
        {
            Type = "object",
            Description = "Represents a person",
            Properties = new Dictionary<string, JsonSchema>
            {
                ["name"] = new JsonSchema { Type = "string", Description = "The person's name" }
            },
            Required = new[] { "name" }
        };

        var code = DtoGenerator.GenerateRecord(schema, "Person", "MyApp");

        Assert.Contains("/// <summary>", code);
        Assert.Contains("Represents a person", code);
    }

    [Fact]
    public void GenerateRecord_NestedObject_GeneratesNestedType()
    {
        var schema = new JsonSchema
        {
            Type = "object",
            Properties = new Dictionary<string, JsonSchema>
            {
                ["address"] = new JsonSchema
                {
                    Type = "object",
                    Properties = new Dictionary<string, JsonSchema>
                    {
                        ["street"] = new JsonSchema { Type = "string" },
                        ["city"] = new JsonSchema { Type = "string" }
                    }
                }
            }
        };

        var code = DtoGenerator.GenerateRecord(schema, "Person", "MyApp");

        Assert.Contains("PersonAddress", code);
        Assert.Contains("public sealed record PersonAddress(", code);
    }

    [Fact]
    public void GenerateFromTools_GeneratesRequestTypes()
    {
        var tools = new List<ToolContract>
        {
            new ToolContract(
                "get_weather",
                "Gets weather for a location",
                @"{""type"":""object"",""properties"":{""city"":{""type"":""string""}},""required"":[""city""]}",
                "{}",
                new Dictionary<string, string>())
        };

        var code = DtoGenerator.GenerateFromTools(tools, "MyApp.Tools");

        Assert.Contains("namespace MyApp.Tools;", code);
        Assert.Contains("public sealed record GetWeatherRequest(", code);
        Assert.Contains("string City", code);
    }
}
