namespace MissionToMars.Domain.Enums;

public static class DirectionExtensions
{
    private static readonly Direction[] Clockwise =
    {
        Direction.North,
        Direction.East,
        Direction.South,
        Direction.West
    };

    public static Direction TurnLeft(this Direction direction)
    {
        var index = GetValidDirectionIndex(direction);
        return Clockwise[(index + Clockwise.Length - 1) % Clockwise.Length];
    }

    public static Direction TurnRight(this Direction direction)
    {
        var index = GetValidDirectionIndex(direction);
        return Clockwise[(index + 1) % Clockwise.Length];
    }

    public static char ToOutputChar(this Direction direction) => direction switch
    {
        Direction.North => 'N',
        Direction.East => 'E',
        Direction.South => 'S',
        Direction.West => 'W',
        _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "Unknown direction.")
    };

    public static bool TryParse(char value, out Direction direction)
    {
        direction = char.ToUpperInvariant(value) switch
        {
            'N' => Direction.North,
            'E' => Direction.East,
            'S' => Direction.South,
            'W' => Direction.West,
            _ => default
        };

        return char.ToUpperInvariant(value) is 'N' or 'E' or 'S' or 'W';
    }

    private static int GetValidDirectionIndex(Direction direction)
    {
        var index = Array.IndexOf(Clockwise, direction);
        if (index < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(direction), direction, "Unknown direction.");
        }

        return index;
    }
}