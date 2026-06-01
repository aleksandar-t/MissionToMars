using MissionToMars.MissionControl.Abstractions;
using MissionToMars.Domain.Enums;
using MissionToMars.Domain.Models;

namespace MissionToMars.Infrastructure.Acknowledgements;

public sealed class MockRoverAcknowledgementReceiver : IRoverAcknowledgementReceiver
{
    private readonly Dictionary<Guid, RoverAcknowledgement> _processedAcknowledgements = new();
    private readonly Dictionary<Guid, RoverAcknowledgement> _overrides = new();

    public void OverrideAcknowledgement(Guid commandId, RoverAcknowledgement acknowledgement) =>
        _overrides[commandId] = acknowledgement;

    public RoverAcknowledgement GetAcknowledgement(RoverCommand command)
    {
        // Idempotency simulation: retrying the same command returns the same acknowledgement
        // instead of executing the movement twice.
        if (_processedAcknowledgements.TryGetValue(command.CommandId, out var previousAcknowledgement))
        {
            return previousAcknowledgement;
        }

        var acknowledgement = _overrides.TryGetValue(command.CommandId, out var overridden)
            ? overridden
            : new RoverAcknowledgement(
                command.MissionId,
                command.CommandId,
                command.RoverId,
                AcknowledgementStatus.Completed,
                command.ExpectedFinalState);

        _processedAcknowledgements[command.CommandId] = acknowledgement;
        return acknowledgement;
    }
}