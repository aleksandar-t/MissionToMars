namespace MissionToMars.Domain.Models;

public sealed class RoverMissionInput
{
    public RoverMissionInput(string roverId, RoverState startState, string instructions)
    {
        RoverId = roverId;
        StartState = startState;
        Instructions = instructions;
    }

    public string RoverId { get; }

    public RoverState StartState { get; }

    public string Instructions { get; }
}