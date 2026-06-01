using MissionToMars.Domain.Constants;
using MissionToMars.Domain.Results;
using MissionToMars.MissionControl.Abstractions;

namespace MissionToMars.MissionControl.Running;

public sealed class MissionRunner : IMissionRunner
{
    private readonly IMissionInputParser _parser;
    private readonly IMissionPlanner _planner;
    private readonly IMissionExecutionOrchestrator _orchestrator;

    public MissionRunner(
        IMissionInputParser parser,
        IMissionPlanner planner,
        IMissionExecutionOrchestrator orchestrator)
    {
        _parser = parser;
        _planner = planner;
        _orchestrator = orchestrator;
    }

    public MissionRunResult Run(string input)
    {
        var parseResult = _parser.Parse(input);
        if (!parseResult.IsSuccess || parseResult.MissionPlan is null)
        {
            return MissionRunResult.Failure(
                parseResult.Error ?? new ValidationError(MissionSignals.InvalidInput, "Mission input is invalid."));
        }

        var planningResult = _planner.ValidateAndPlan(parseResult.MissionPlan);
        if (!planningResult.IsSuccess || planningResult.PlannedMission is null)
        {
            return MissionRunResult.Failure(
                planningResult.Error ?? new ValidationError(MissionSignals.InvalidMissionPlan, "Mission plan is invalid."));
        }

        var executionResult = _orchestrator.Execute(planningResult.PlannedMission);
        if (!executionResult.IsSuccess)
        {
            return MissionRunResult.Failure(
                executionResult.Error ?? new ValidationError(MissionSignals.MissionExecutionFailed, "Mission execution failed."));
        }

        return MissionRunResult.Success(executionResult.FinalStates);
    }
}