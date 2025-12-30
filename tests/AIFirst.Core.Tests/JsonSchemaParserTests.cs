using AIFirst.Core.Schema;
using Xunit;

namespace AIFirst.Core.Tests;

public class JsonSchemaParserTests
{
    [Fact]
    public void Parse_EmptyJson_ReturnsDefaultSchema()
    {
        var schema = JsonSchemaParser.Parse("{}");
        
        Assert.Equal("object", schema.Type);
        Assert.Empty(schema.Properties);
        Assert.Empty(schema.Required);
    }

    [Fact]
    public void Parse_SimpleObject_ParsesCorrectly()
    {
        var json = @"{
            ""type"": ""object"",
            ""title"": ""Person"",
            ""properties"": {
                ""name"": { ""type"": ""string"" },
                ""age"": { ""type"": ""integer"" }
            },
            ""required"": [""name""]
        }";

        var schema = JsonSchemaParser.Parse(json);

        Assert.Equal("object", schema.Type);
        Assert.Equal("Person", schema.Title);
        Assert.Equal(2, schema.Properties.Count);
        Assert.Contains("name", schema.Properties.Keys);
        Assert.Contains("age", schema.Properties.Keys);
        Assert.Equal("string", schema.Properties["name"].Type);
        Assert.Equal("integer", schema.Properties["age"].Type);
        Assert.Single(schema.Required);
        Assert.Contains("name", schema.Required);
    }

    [Fact]
    public void Parse_ArrayType_ParsesItems()
    {
        var json = @"{
            ""type"": ""array"",
            ""items"": { ""type"": ""string"" }
        }";

        var schema = JsonSchemaParser.Parse(json);

        Assert.Equal("array", schema.Type);
        Assert.NotNull(schema.Items);
        Assert.Equal("string", schema.Items.Type);
    }

    [Fact]
    public void Parse_NestedObject_ParsesRecursively()
    {
        var json = @"{
            ""type"": ""object"",
            ""properties"": {
                ""address"": {
                    ""type"": ""object"",
                    ""properties"": {
                        ""street"": { ""type"": ""string"" },
                        ""city"": { ""type"": ""string"" }
                    }
                }
            }
        }";

        var schema = JsonSchemaParser.Parse(json);

        Assert.Equal("object", schema.Type);
        var address = schema.Properties["address"];
        Assert.Equal("object", address.Type);
        Assert.Equal(2, address.Properties.Count);
        Assert.Equal("string", address.Properties["street"].Type);
    }

    [Fact]
    public void Parse_WithFormat_ParsesFormat()
    {
        var json = @"{
            ""type"": ""string"",
            ""format"": ""date-time""
        }";

        var schema = JsonSchemaParser.Parse(json);

        Assert.Equal("string", schema.Type);
        Assert.Equal("date-time", schema.Format);
    }

    [Fact]
    public void Parse_WithNullable_ParsesNullable()
    {
        var json = @"{
            ""type"": ""integer"",
            ""nullable"": true
        }";

        var schema = JsonSchemaParser.Parse(json);

        Assert.Equal("integer", schema.Type);
        Assert.True(schema.Nullable);
    }

    [Fact]
    public void Parse_WithEnum_ParsesEnumValues()
    {
        var json = @"{
            ""type"": ""string"",
            ""enum"": [""red"", ""green"", ""blue""]
        }";

        var schema = JsonSchemaParser.Parse(json);

        Assert.NotNull(schema.Enum);
        Assert.Equal(3, schema.Enum.Count);
        Assert.Contains("red", schema.Enum);
        Assert.Contains("green", schema.Enum);
        Assert.Contains("blue", schema.Enum);
    }
}
