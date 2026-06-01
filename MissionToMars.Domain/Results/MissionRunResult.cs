using MissionToMars.Domain.Models;

namespace MissionToMars.Domain.Results;

public sealed class MissionRunResult
{
    private MissionRunResult(bool isSuccess, IReadOnlyList<RoverState> finalStates, ValidationError? error)
    {
        IsSuccess = isSuccess;
        FinalStates = finalStates;
        Error = error;
    }

    public bool IsSuccess { get; }
    public IReadOnlyList<RoverState> FinalStates { get; }
    public ValidationError? Error { get; }

    public static MissionRunResult Success(IReadOnlyList<RoverState> finalStates) =>
        new(true, finalStates, null);

    public static MissionRunResult Failure(string code, string message) =>
        new(false, Array.Empty<RoverState>(), new ValidationError(code, message));

    public static MissionRunResult Failure(ValidationError error) =>
        new(false, Array.Empty<RoverState>(), error);
}