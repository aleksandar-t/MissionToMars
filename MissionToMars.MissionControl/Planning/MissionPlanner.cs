using MissionToMars.Domain.Constants;
using MissionToMars.Domain.Models;
using MissionToMars.Domain.Results;
using MissionToMars.MissionControl.Abstractions;

namespace MissionToMars.MissionControl.Planning;

public sealed class MissionPlanner : IMissionPlanner
{
    public MissionPlanningResult ValidateAndPlan(MissionPlan missionPlan)
    {
        if (missionPlan.Plateau.MaxX < 0 || missionPlan.Plateau.MaxY < 0)
        {
            return MissionPlanningResult.Failure(
                MissionSignals.InvalidPlateau,
                "Plateau upper-right coordinates must be greater than or equal to 0,0.");
        }

        var missionId = Guid.NewGuid();
        var reservedFinalPositions = new Dictionary<Position, string>();
        var plannedSteps = new List<PlannedRoverStep>();

        for (var roverIndex = 0; roverIndex < missionPlan.Rovers.Count; roverIndex++)
        {
            var rover = missionPlan.Rovers[roverIndex];
            var roverNumber = roverIndex + 1;

            if (!missionPlan.Plateau.Contains(rover.StartState.Position))
            {
                return MissionPlanningResult.Failure(
                    MissionSignals.RoverStartOutsidePlateau,
                    $"Rover {roverNumber} starts outside the plateau at {rover.StartState}. Plateau bounds are x=0..{missionPlan.Plateau.MaxX}, y=0..{missionPlan.Plateau.MaxY}.");
            }

            if (reservedFinalPositions.TryGetValue(rover.StartState.Position, out var occupyingRoverId))
            {
                return MissionPlanningResult.Failure(
                    MissionSignals.RoverStartCollision,
                    $"Rover {roverNumber} starts at {rover.StartState.Position}, which is already reserved by {occupyingRoverId}'s planned final position.");
            }

            var currentState = rover.StartState;

            for (var commandIndex = 0; commandIndex < rover.Instructions.Length; commandIndex++)
            {
                var instruction = rover.Instructions[commandIndex];
                var commandNumber = commandIndex + 1;

                if (instruction is 'L' or 'R')
                {
                    currentState = RoverNavigator.ApplyTurn(currentState, instruction);
                    continue;
                }

                if (instruction != 'M')
                {
                    return MissionPlanningResult.Failure(
                        MissionSignals.InvalidInstruction,
                        $"Rover {roverNumber}, command {commandNumber} contains invalid instruction '{instruction}'. Allowed instructions are L, R and M.");
                }

                var candidateState = RoverNavigator.MoveForward(currentState);

                if (!missionPlan.Plateau.Contains(candidateState.Position))
                {
                    return MissionPlanningResult.Failure(
                        MissionSignals.RoverLeftPlateau,
                        $"Rover {roverNumber}, command {commandNumber} would move from {currentState} to {candidateState}, outside plateau bounds x=0..{missionPlan.Plateau.MaxX}, y=0..{missionPlan.Plateau.MaxY}.");
                }

                if (reservedFinalPositions.TryGetValue(candidateState.Position, out occupyingRoverId))
                {
                    return MissionPlanningResult.Failure(
                        MissionSignals.RoverCollision,
                        $"Rover {roverNumber}, command {commandNumber} would move into {candidateState.Position}, already reserved by {occupyingRoverId}'s planned final position.");
                }

                currentState = candidateState;
            }

            if (reservedFinalPositions.TryGetValue(currentState.Position, out occupyingRoverId))
            {
                return MissionPlanningResult.Failure(
                    MissionSignals.RoverFinalCollision,
                    $"Rover {roverNumber} would finish at {currentState.Position}, already reserved by {occupyingRoverId}'s planned final position.");
            }

            reservedFinalPositions[currentState.Position] = rover.RoverId;
            plannedSteps.Add(new PlannedRoverStep(
                missionId,
                Guid.NewGuid(),
                roverNumber,
                rover.RoverId,
                rover.StartState,
                rover.Instructions,
                currentState));
        }

        return MissionPlanningResult.Success(new PlannedMission(missionId, missionPlan.Plateau, plannedSteps));
    }
}