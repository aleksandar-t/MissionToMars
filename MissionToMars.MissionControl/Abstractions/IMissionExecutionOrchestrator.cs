using MissionToMars.Domain.Models;
using MissionToMars.Domain.Results;

namespace MissionToMars.MissionControl.Abstractions;

public interface IMissionExecutionOrchestrator
{
    MissionExecutionResult Execute(PlannedMission mission);
}