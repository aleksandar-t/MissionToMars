using MissionToMars.Domain.Constants;
using MissionToMars.MissionControl.Parsing;
using Xunit;

namespace MissionToMars.Tests.Parsing;

public sealed class MissionInputParserTests
{
    private readonly MissionInputParser _parser = new();

    [Fact]
    public void Parse_ValidInput_ReturnsMissionPlan()
    {
        const string input = """
            5 5
            1 2 N
            LMLMLMLMM
            3 3 E
            MMRMMRMRRM
            """;

        var result = _parser.Parse(input);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.MissionPlan);
        Assert.Equal(2, result.MissionPlan!.Rovers.Count);
    }

    [Fact]
    public void Parse_InvalidDirection_ReturnsFailure()
    {
        const string input = """
            5 5
            1 2 X
            M
            """;

        var result = _parser.Parse(input);

        Assert.False(result.IsSuccess);
        Assert.Equal(MissionSignals.InvalidRoverPosition, result.Error!.Code);
    }

    [Fact]
    public void Parse_InvalidInputShape_ReturnsFailure()
    {
        const string input = """
            5 5
            1 2 N
            """;

        var result = _parser.Parse(input);

        Assert.False(result.IsSuccess);
        Assert.Equal(MissionSignals.InvalidInputShape, result.Error!.Code);
    }
}