using MissionToMars.Domain.Models;

namespace MissionToMars.Domain.Results;

public sealed class MissionPlanningResult
{
    private MissionPlanningResult(bool isSuccess, PlannedMission? plannedMission, ValidationError? error)
    {
        IsSuccess = isSuccess;
        PlannedMission = plannedMission;
        Error = error;
    }

    public bool IsSuccess { get; }
    public PlannedMission? PlannedMission { get; }
    public ValidationError? Error { get; }

    public static MissionPlanningResult Success(PlannedMission mission) => new(true, mission, null);

    public static MissionPlanningResult Failure(string code, string message) =>
        new(false, null, new ValidationError(code, message));
}