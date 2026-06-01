using MissionToMars.Domain.Constants;
using MissionToMars.Infrastructure.Acknowledgements;
using MissionToMars.Infrastructure.Dispatching;
using MissionToMars.Infrastructure.Outbox;
using MissionToMars.Infrastructure.State;
using MissionToMars.MissionControl.Execution;
using MissionToMars.MissionControl.Parsing;
using MissionToMars.MissionControl.Planning;
using MissionToMars.MissionControl.Running;
using Xunit;

namespace MissionToMars.Tests.Running;

public sealed class MissionRunnerTests
{
    [Fact]
    public void Run_ClassicSample_ReturnsExpectedFinalStates()
    {
        var runner = CreateRunner();
        const string input = """
            5 5
            1 2 N
            LMLMLMLMM
            3 3 E
            MMRMMRMRRM
            """;

        var result = runner.Run(input);

        Assert.True(result.IsSuccess);
        Assert.Collection(
            result.FinalStates,
            first => Assert.Equal("1 3 N", first.ToString()),
            second => Assert.Equal("5 1 E", second.ToString()));
    }

    [Fact]
    public void Run_InvalidPlan_ReturnsFailureWithoutExecutionOutput()
    {
        var runner = CreateRunner();
        const string input = """
            5 5
            5 5 N
            M
            """;

        var result = runner.Run(input);

        Assert.False(result.IsSuccess);
        Assert.Equal(MissionSignals.RoverLeftPlateau, result.Error!.Code);
        Assert.Empty(result.FinalStates);
    }

    private static MissionRunner CreateRunner() =>
        new(
            new MissionInputParser(),
            new MissionPlanner(),
            new MissionExecutionOrchestrator(
                new InMemoryCommandOutbox(),
                new MockRoverCommandDispatcher(),
                new MockRoverAcknowledgementReceiver(),
                new InMemoryRoverStateStore()));
}