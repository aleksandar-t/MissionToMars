namespace MissionToMars.Domain.Models;

public sealed class MissionPlan
{
    public MissionPlan(Plateau plateau, IReadOnlyList<RoverMissionInput> rovers)
    {
        Plateau = plateau;
        Rovers = rovers;
    }

    public Plateau Plateau { get; }

    public IReadOnlyList<RoverMissionInput> Rovers { get; }
}