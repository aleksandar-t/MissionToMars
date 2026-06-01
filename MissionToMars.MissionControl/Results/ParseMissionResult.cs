using MissionToMars.Domain.Models;
using MissionToMars.Domain.Results;

namespace MissionToMars.MissionControl.Results;

public sealed class ParseMissionResult
{
    private ParseMissionResult(bool isSuccess, MissionPlan? missionPlan, ValidationError? error)
    {
        IsSuccess = isSuccess;
        MissionPlan = missionPlan;
        Error = error;
    }

    public bool IsSuccess { get; }
    public MissionPlan? MissionPlan { get; }
    public ValidationError? Error { get; }

    public static ParseMissionResult Success(MissionPlan missionPlan) =>
        new(true, missionPlan, null);

    public static ParseMissionResult Failure(string code, string message) =>
        new(false, null, new ValidationError(code, message));
}