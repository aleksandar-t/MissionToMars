namespace MissionToMars.Domain.Models;

public sealed class Plateau
{
    public Plateau(int maxX, int maxY)
    {
        MaxX = maxX;
        MaxY = maxY;
    }

    public int MaxX { get; }

    public int MaxY { get; }

    public bool Contains(Position position) =>
        position.X >= 0 && position.X <= MaxX &&
        position.Y >= 0 && position.Y <= MaxY;
}