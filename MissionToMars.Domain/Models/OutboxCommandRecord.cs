namespace MissionToMars.Domain.Models;

public sealed class OutboxCommandRecord
{
    public OutboxCommandRecord(
        RoverCommand command,
        bool isDispatched,
        bool isAcknowledged,
        bool isFailed,
        string? failureReason)
    {
        Command = command;
        IsDispatched = isDispatched;
        IsAcknowledged = isAcknowledged;
        IsFailed = isFailed;
        FailureReason = failureReason;
    }

    public RoverCommand Command { get; }

    public bool IsDispatched { get; }

    public bool IsAcknowledged { get; }

    public bool IsFailed { get; }

    public string? FailureReason { get; }

    public OutboxCommandRecord MarkDispatched() =>
        new(Command, true, IsAcknowledged, IsFailed, FailureReason);

    public OutboxCommandRecord MarkAcknowledged() =>
        new(Command, IsDispatched, true, IsFailed, FailureReason);

    public OutboxCommandRecord MarkFailed(string reason) =>
        new(Command, IsDispatched, IsAcknowledged, true, reason);
}