using MissionToMars.Domain.Constants;
using MissionToMars.Domain.Enums;
using MissionToMars.Domain.Models;
using MissionToMars.MissionControl.Planning;
using Xunit;

namespace MissionToMars.Tests.Planning;

public sealed class MissionPlannerTests
{
    private readonly MissionPlanner _planner = new();

    [Fact]
    public void ValidateAndPlan_ClassicSample_ReturnsExpectedFinalStates()
    {
        var plan = new MissionPlan(
            new Plateau(5, 5),
            new[]
            {
                new RoverMissionInput("Rover-1", new RoverState(new Position(1, 2), Direction.North), "LMLMLMLMM"),
                new RoverMissionInput("Rover-2", new RoverState(new Position(3, 3), Direction.East), "MMRMMRMRRM")
            });

        var result = _planner.ValidateAndPlan(plan);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.PlannedMission);
        Assert.Equal("1 3 N", result.PlannedMission!.Steps[0].ExpectedFinalState.ToString());
        Assert.Equal("5 1 E", result.PlannedMission.Steps[1].ExpectedFinalState.ToString());
    }

    [Fact]
    public void ValidateAndPlan_AllowsSecondRoverToStartWhereFirstRoverStarted_WhenFirstRoverMovedAway()
    {
        var plan = new MissionPlan(
            new Plateau(5, 5),
            new[]
            {
                new RoverMissionInput("Rover-1", new RoverState(new Position(1, 1), Direction.North), "M"),
                new RoverMissionInput("Rover-2", new RoverState(new Position(1, 1), Direction.East), "M")
            });

        var result = _planner.ValidateAndPlan(plan);

        Assert.True(result.IsSuccess);
        Assert.Equal("1 2 N", result.PlannedMission!.Steps[0].ExpectedFinalState.ToString());
        Assert.Equal("2 1 E", result.PlannedMission.Steps[1].ExpectedFinalState.ToString());
    }

    [Fact]
    public void ValidateAndPlan_ReturnsFailure_WhenRoverLeavesPlateau()
    {
        var plan = new MissionPlan(
            new Plateau(5, 5),
            new[]
            {
                new RoverMissionInput("Rover-1", new RoverState(new Position(5, 5), Direction.North), "M")
            });

        var result = _planner.ValidateAndPlan(plan);

        Assert.False(result.IsSuccess);
        Assert.Equal(MissionSignals.RoverLeftPlateau, result.Error!.Code);
    }

    [Fact]
    public void ValidateAndPlan_ReturnsFailure_WhenRoverStartsOutsidePlateau()
    {
        var plan = new MissionPlan(
            new Plateau(5, 5),
            new[]
            {
                new RoverMissionInput("Rover-1", new RoverState(new Position(6, 1), Direction.North), "M")
            });

        var result = _planner.ValidateAndPlan(plan);

        Assert.False(result.IsSuccess);
        Assert.Equal(MissionSignals.RoverStartOutsidePlateau, result.Error!.Code);
    }

    [Fact]
    public void ValidateAndPlan_ReturnsFailure_WhenInstructionIsInvalid()
    {
        var plan = new MissionPlan(
            new Plateau(5, 5),
            new[]
            {
                new RoverMissionInput("Rover-1", new RoverState(new Position(1, 1), Direction.North), "LMA")
            });

        var result = _planner.ValidateAndPlan(plan);

        Assert.False(result.IsSuccess);
        Assert.Equal(MissionSignals.InvalidInstruction, result.Error!.Code);
    }

    [Fact]
    public void ValidateAndPlan_ReturnsFailure_WhenRoverMovesIntoPreviousFinalPosition()
    {
        var plan = new MissionPlan(
            new Plateau(5, 5),
            new[]
            {
                new RoverMissionInput("Rover-1", new RoverState(new Position(1, 2), Direction.North), "M"),
                new RoverMissionInput("Rover-2", new RoverState(new Position(1, 2), Direction.North), "M")
            });

        var result = _planner.ValidateAndPlan(plan);

        Assert.False(result.IsSuccess);
        Assert.Equal(MissionSignals.RoverCollision, result.Error!.Code);
    }

    [Fact]
    public void ValidateAndPlan_ReturnsFailure_WhenRoverStartsOnPreviousFinalPosition()
    {
        var plan = new MissionPlan(
            new Plateau(5, 5),
            new[]
            {
                new RoverMissionInput("Rover-1", new RoverState(new Position(1, 2), Direction.North), "M"),
                new RoverMissionInput("Rover-2", new RoverState(new Position(1, 3), Direction.East), "M")
            });

        var result = _planner.ValidateAndPlan(plan);

        Assert.False(result.IsSuccess);
        Assert.Equal(MissionSignals.RoverStartCollision, result.Error!.Code);
    }
}