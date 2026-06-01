using MissionToMars.MissionControl.Abstractions;
using MissionToMars.Domain.Models;

namespace MissionToMars.Infrastructure.State;

public sealed class InMemoryRoverStateStore : IRoverStateStore
{
    private readonly Dictionary<string, RoverState> _states = new(StringComparer.OrdinalIgnoreCase);

    public RoverState? GetState(string roverId) =>
        _states.TryGetValue(roverId, out var state) ? state : null;

    public void UpdateState(string roverId, RoverState state) =>
        _states[roverId] = state;
}