using MissionToMars.MissionControl.Abstractions;
using MissionToMars.Domain.Models;
using MissionToMars.Domain.Results;

namespace MissionToMars.Infrastructure.Dispatching;

public sealed class MockRoverCommandDispatcher : IRoverCommandDispatcher
{
    private readonly HashSet<Guid> _acceptedCommandIds = new();
    private readonly HashSet<Guid> _commandsToReject;

    public MockRoverCommandDispatcher(IEnumerable<Guid>? commandsToReject = null)
    {
        _commandsToReject = commandsToReject?.ToHashSet() ?? new HashSet<Guid>();
    }

    public int DispatchCount { get; private set; }
    public IReadOnlyCollection<Guid> AcceptedCommandIds => _acceptedCommandIds;

    public DispatchResult Dispatch(RoverCommand command)
    {
        DispatchCount++;

        if (_commandsToReject.Contains(command.CommandId))
        {
            return DispatchResult.Rejected($"Mock dispatcher rejected command {command.CommandId}.");
        }

        _acceptedCommandIds.Add(command.CommandId);
        return DispatchResult.Accepted();
    }
}