using MissionToMars.Domain.Models;

namespace MissionToMars.MissionControl.Abstractions;

public interface IRoverStateStore
{
    RoverState? GetState(string roverId);
    void UpdateState(string roverId, RoverState state);
}