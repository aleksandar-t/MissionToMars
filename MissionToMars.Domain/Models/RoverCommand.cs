namespace MissionToMars.Domain.Models;

public sealed class RoverCommand
{
    public RoverCommand(
        Guid missionId,
        Guid commandId,
        int sequenceNumber,
        string roverId,
        RoverState expectedStartState,
        string instructions,
        RoverState expectedFinalState)
    {
        MissionId = missionId;
        CommandId = commandId;
        SequenceNumber = sequenceNumber;
        RoverId = roverId;
        ExpectedStartState = expectedStartState;
        Instructions = instructions;
        ExpectedFinalState = expectedFinalState;
    }

    public Guid MissionId { get; }

    public Guid CommandId { get; }

    public int SequenceNumber { get; }

    public string RoverId { get; }

    public RoverState ExpectedStartState { get; }

    public string Instructions { get; }

    public RoverState ExpectedFinalState { get; }
}