using MissionToMars.Domain.Enums;

namespace MissionToMars.Domain.Models;

public sealed class RoverState
{
    public RoverState(Position position, Direction direction)
    {
        Position = position;
        Direction = direction;
    }

    public Position Position { get; }

    public Direction Direction { get; }

    public override string ToString() => $"{Position.X} {Position.Y} {Direction.ToOutputChar()}";
}