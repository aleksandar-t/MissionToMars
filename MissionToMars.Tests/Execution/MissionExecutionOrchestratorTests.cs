using MissionToMars.Domain.Constants;
using MissionToMars.Domain.Enums;
using MissionToMars.Domain.Models;
using MissionToMars.Infrastructure.Acknowledgements;
using MissionToMars.Infrastructure.Dispatching;
using MissionToMars.Infrastructure.Outbox;
using MissionToMars.Infrastructure.State;
using MissionToMars.MissionControl.Execution;
using Xunit;

namespace MissionToMars.Tests.Execution;

public sealed class MissionExecutionOrchestratorTests
{
    [Fact]
    public void Execute_DispatchesSecondRoverOnlyAfterFirstRoverIsAcknowledgedAndVerified()
    {
        var mission = CreateMission();
        var dispatcher = new MockRoverCommandDispatcher();
        var acknowledgementReceiver = new MockRoverAcknowledgementReceiver();
        var stateStore = new InMemoryRoverStateStore();
        var orchestrator = new MissionExecutionOrchestrator(
            new InMemoryCommandOutbox(),
            dispatcher,
            acknowledgementReceiver,
            stateStore);

        var result = orchestrator.Execute(mission);

        Assert.True(result.IsSuccess);
        Assert.Equal(2, dispatcher.DispatchCount);
        Assert.Equal("1 3 N", stateStore.GetState("Rover-1")!.ToString());
        Assert.Equal("5 1 E", stateStore.GetState("Rover-2")!.ToString());
    }

    [Fact]
    public void Execute_StopsMission_WhenAcknowledgementDoesNotMatchExpectedFinalState()
    {
        var mission = CreateMission();
        var firstStep = mission.Steps[0];
        var dispatcher = new MockRoverCommandDispatcher();
        var acknowledgementReceiver = new MockRoverAcknowledgementReceiver();
        acknowledgementReceiver.OverrideAcknowledgement(
            firstStep.CommandId,
            new RoverAcknowledgement(
                firstStep.MissionId,
                firstStep.CommandId,
                firstStep.RoverId,
                AcknowledgementStatus.Completed,
                firstStep.ExpectedStartState));

        var orchestrator = new MissionExecutionOrchestrator(
            new InMemoryCommandOutbox(),
            dispatcher,
            acknowledgementReceiver,
            new InMemoryRoverStateStore());

        var result = orchestrator.Execute(mission);

        Assert.False(result.IsSuccess);
        Assert.Equal(MissionSignals.RoverFinalStateMismatch, result.Error!.Code);
        Assert.Equal(1, dispatcher.DispatchCount);
    }

    [Fact]
    public void Execute_StopsMission_WhenDispatcherRejectsCommand()
    {
        var mission = CreateMission();
        var firstStep = mission.Steps[0];
        var dispatcher = new MockRoverCommandDispatcher(new[] { firstStep.CommandId });
        var orchestrator = new MissionExecutionOrchestrator(
            new InMemoryCommandOutbox(),
            dispatcher,
            new MockRoverAcknowledgementReceiver(),
            new InMemoryRoverStateStore());

        var result = orchestrator.Execute(mission);

        Assert.False(result.IsSuccess);
        Assert.Equal(MissionSignals.CommandDispatchFailed, result.Error!.Code);
        Assert.Equal(1, dispatcher.DispatchCount);
    }

    private static PlannedMission CreateMission()
    {
        var missionId = Guid.NewGuid();
        return new PlannedMission(
            missionId,
            new Plateau(5, 5),
            new[]
            {
                new PlannedRoverStep(
                    missionId,
                    Guid.NewGuid(),
                    1,
                    "Rover-1",
                    new RoverState(new Position(1, 2), Direction.North),
                    "LMLMLMLMM",
                    new RoverState(new Position(1, 3), Direction.North)),
                new PlannedRoverStep(
                    missionId,
                    Guid.NewGuid(),
                    2,
                    "Rover-2",
                    new RoverState(new Position(3, 3), Direction.East),
                    "MMRMMRMRRM",
                    new RoverState(new Position(5, 1), Direction.East))
            });
    }
}