using MissionToMars.Domain.Enums;

namespace MissionToMars.Domain.Models;

public sealed class RoverAcknowledgement
{
    public RoverAcknowledgement(
        Guid missionId,
        Guid commandId,
        string roverId,
        AcknowledgementStatus status,
        RoverState? actualFinalState,
        string? failureReason = null)
    {
        MissionId = missionId;
        CommandId = commandId;
        RoverId = roverId;
        Status = status;
        ActualFinalState = actualFinalState;
        FailureReason = failureReason;
    }

    public Guid MissionId { get; }

    public Guid CommandId { get; }

    public string RoverId { get; }

    public AcknowledgementStatus Status { get; }

    public RoverState? ActualFinalState { get; }

    public string? FailureReason { get; }
}