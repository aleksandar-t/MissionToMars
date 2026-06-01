using MissionToMars.Domain.Enums;
using MissionToMars.Domain.Models;

namespace MissionToMars.MissionControl.Planning;

public static class RoverNavigator
{
    public static RoverState ApplyTurn(RoverState state, char instruction)
    {
        return instruction switch
        {
            'L' => new RoverState(state.Position, state.Direction.TurnLeft()),
            'R' => new RoverState(state.Position, state.Direction.TurnRight()),
            _ => throw new ArgumentException($"Unsupported turn instruction '{instruction}'.", nameof(instruction))
        };
    }

    public static RoverState MoveForward(RoverState state)
    {
        var currentPosition = state.Position;

        var nextPosition = state.Direction switch
        {
            Direction.North => new Position(currentPosition.X, currentPosition.Y + 1),
            Direction.East => new Position(currentPosition.X + 1, currentPosition.Y),
            Direction.South => new Position(currentPosition.X, currentPosition.Y - 1),
            Direction.West => new Position(currentPosition.X - 1, currentPosition.Y),
            _ => throw new ArgumentOutOfRangeException(nameof(state.Direction), state.Direction, "Unknown direction.")
        };

        return new RoverState(nextPosition, state.Direction);
    }
}