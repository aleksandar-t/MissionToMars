namespace MissionToMars.Domain.Results;

public sealed class ValidationError
{
    public ValidationError(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Code { get; }

    public string Message { get; }
}