using System.Collections.Generic;
using AIFirst.Core.Schema;
using Xunit;

namespace AIFirst.Core.Tests;

public class CSharpTypeMapperTests
{
    [Theory]
    [InlineData("string", "string")]
    [InlineData("integer", "int")]
    [InlineData("number", "double")]
    [InlineData("boolean", "bool")]
    public void MapToCSharpType_PrimitiveTypes_MapsCorrectly(string jsonType, string expectedCSharp)
    {
        var schema = new JsonSchema { Type = jsonType };
        
        var result = CSharpTypeMapper.MapToCSharpType(schema);
        
        Assert.Equal(expectedCSharp, result);
    }

    [Fact]
    public void MapToCSharpType_NullableInteger_AddsQuestionMark()
    {
        var schema = new JsonSchema { Type = "integer", Nullable = true };
        
        var result = CSharpTypeMapper.MapToCSharpType(schema);
        
        Assert.Equal("int?", result);
    }

    [Fact]
    public void MapToCSharpType_Array_MapsToIReadOnlyList()
    {
        var schema = new JsonSchema 
        { 
            Type = "array",
            Items = new JsonSchema { Type = "string" }
        };
        
        var result = CSharpTypeMapper.MapToCSharpType(schema);
        
        Assert.Equal("IReadOnlyList<string>", result);
    }

    [Fact]
    public void MapToCSharpType_ObjectWithTitle_UsesTitle()
    {
        var schema = new JsonSchema 
        { 
            Type = "object",
            Title = "MyCustomType"
        };
        
        var result = CSharpTypeMapper.MapToCSharpType(schema);
        
        Assert.Equal("MyCustomType", result);
    }

    [Fact]
    public void MapToCSharpType_ObjectWithoutTitle_UsesPropertyName()
    {
        var schema = new JsonSchema 
        { 
            Type = "object",
            Properties = new Dictionary<string, JsonSchema>
            {
                ["name"] = new JsonSchema { Type = "string" }
            }
        };
        
        var result = CSharpTypeMapper.MapToCSharpType(schema, "Address");
        
        Assert.Equal("Address", result);
    }

    [Theory]
    [InlineData("date-time", "DateTimeOffset")]
    [InlineData("uri", "Uri")]
    [InlineData("uuid", "Guid")]
    public void MapToCSharpType_StringWithFormat_MapsToSpecialType(string format, string expectedType)
    {
        var schema = new JsonSchema { Type = "string", Format = format };
        
        var result = CSharpTypeMapper.MapToCSharpType(schema);
        
        Assert.Equal(expectedType, result);
    }

    [Fact]
    public void MapToCSharpType_RefType_ExtractsTypeName()
    {
        var schema = new JsonSchema { Ref = "#/definitions/Address" };
        
        var result = CSharpTypeMapper.MapToCSharpType(schema);
        
        Assert.Equal("Address", result);
    }

    [Theory]
    [InlineData("my_type", "MyType")]
    [InlineData("my-type", "MyType")]
    [InlineData("my type", "MyType")]
    [InlineData("myType", "MyType")]
    public void ToPascalCase_ConvertsCorrectly(string input, string expected)
    {
        var result = CSharpTypeMapper.ToPascalCase(input);
        
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("MyType", "myType")]
    [InlineData("my_type", "myType")]
    public void ToCamelCase_ConvertsCorrectly(string input, string expected)
    {
        var result = CSharpTypeMapper.ToCamelCase(input);
        
        Assert.Equal(expected, result);
    }
}
