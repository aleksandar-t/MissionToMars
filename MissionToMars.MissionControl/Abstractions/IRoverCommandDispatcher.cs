using MissionToMars.Domain.Models;
using MissionToMars.Domain.Results;

namespace MissionToMars.MissionControl.Abstractions;

public interface IRoverCommandDispatcher
{
    DispatchResult Dispatch(RoverCommand command);
}