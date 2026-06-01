namespace MissionToMars.Domain.Models;

public sealed class PlannedMission
{
    public PlannedMission(Guid missionId, Plateau plateau, IReadOnlyList<PlannedRoverStep> steps)
    {
        MissionId = missionId;
        Plateau = plateau;
        Steps = steps;
    }

    public Guid MissionId { get; }

    public Plateau Plateau { get; }

    public IReadOnlyList<PlannedRoverStep> Steps { get; }
} 