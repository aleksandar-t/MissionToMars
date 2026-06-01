using MissionToMars.MissionControl.Abstractions;
using MissionToMars.Domain.Models;

namespace MissionToMars.Infrastructure.Outbox;

public sealed class InMemoryCommandOutbox : ICommandOutbox
{
    private readonly Dictionary<Guid, OutboxCommandRecord> _records = new();

    public void Save(RoverCommand command)
    {
        if (_records.ContainsKey(command.CommandId))
        {
            return;
        }

        _records[command.CommandId] = new OutboxCommandRecord(command, false, false, false, null);
    }

    public void MarkDispatched(Guid commandId)
    {
        var record = Require(commandId);
        _records[commandId] = record.MarkDispatched();
    }

    public void MarkAcknowledged(Guid commandId)
    {
        var record = Require(commandId);
        _records[commandId] = record.MarkAcknowledged();
    }

    public void MarkFailed(Guid commandId, string reason)
    {
        var record = Require(commandId);
        _records[commandId] = record.MarkFailed(reason);
    }

    public OutboxCommandRecord? Get(Guid commandId) =>
        _records.TryGetValue(commandId, out var record) ? record : null;

    private OutboxCommandRecord Require(Guid commandId) =>
        _records.TryGetValue(commandId, out var record)
            ? record
            : throw new InvalidOperationException($"Command '{commandId}' was not saved in the outbox.");
}