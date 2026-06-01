using MissionToMars.Domain.Models;

namespace MissionToMars.MissionControl.Abstractions;

public interface IRoverAcknowledgementReceiver
{
    RoverAcknowledgement GetAcknowledgement(RoverCommand command);
}