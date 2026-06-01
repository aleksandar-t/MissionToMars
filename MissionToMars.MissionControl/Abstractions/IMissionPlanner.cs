using MissionToMars.Domain.Models;
using MissionToMars.Domain.Results;

namespace MissionToMars.MissionControl.Abstractions;

public interface IMissionPlanner
{
    MissionPlanningResult ValidateAndPlan(MissionPlan missionPlan);
}