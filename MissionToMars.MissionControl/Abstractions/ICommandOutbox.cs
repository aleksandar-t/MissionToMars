using MissionToMars.Domain.Models;

namespace MissionToMars.MissionControl.Abstractions;

public interface ICommandOutbox
{
    void Save(RoverCommand command);
    void MarkDispatched(Guid commandId);
    void MarkAcknowledged(Guid commandId);
    void MarkFailed(Guid commandId, string reason);
    OutboxCommandRecord? Get(Guid commandId);
}