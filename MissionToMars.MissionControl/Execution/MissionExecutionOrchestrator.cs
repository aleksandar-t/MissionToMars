using MissionToMars.Domain.Constants;
using MissionToMars.Domain.Enums;
using MissionToMars.Domain.Models;
using MissionToMars.Domain.Results;
using MissionToMars.MissionControl.Abstractions;

namespace MissionToMars.MissionControl.Execution;

public sealed class MissionExecutionOrchestrator : IMissionExecutionOrchestrator
{
    private readonly ICommandOutbox _outbox;
    private readonly IRoverCommandDispatcher _dispatcher;
    private readonly IRoverAcknowledgementReceiver _acknowledgementReceiver;
    private readonly IRoverStateStore _stateStore;

    public MissionExecutionOrchestrator(
        ICommandOutbox outbox,
        IRoverCommandDispatcher dispatcher,
        IRoverAcknowledgementReceiver acknowledgementReceiver,
        IRoverStateStore stateStore)
    {
        _outbox = outbox;
        _dispatcher = dispatcher;
        _acknowledgementReceiver = acknowledgementReceiver;
        _stateStore = stateStore;
    }

    public MissionExecutionResult Execute(PlannedMission mission)
    {
        var finalStates = new List<RoverState>();

        foreach (var step in mission.Steps)
        {
            var command = new RoverCommand(
                step.MissionId,
                step.CommandId,
                step.SequenceNumber,
                step.RoverId,
                step.ExpectedStartState,
                step.Instructions,
                step.ExpectedFinalState);

            _outbox.Save(command);

            var dispatchResult = _dispatcher.Dispatch(command);
            if (!dispatchResult.IsAccepted)
            {
                var reason = dispatchResult.FailureReason ?? "The command dispatcher rejected the command.";
                _outbox.MarkFailed(command.CommandId, reason);

                return MissionExecutionResult.Failure(
                    MissionSignals.CommandDispatchFailed,
                    $"Rover {step.SequenceNumber} command was not accepted for delivery. {reason}");
            }

            _outbox.MarkDispatched(command.CommandId);

            var acknowledgement = _acknowledgementReceiver.GetAcknowledgement(command);
            if (acknowledgement.Status != AcknowledgementStatus.Completed || acknowledgement.ActualFinalState is null)
            {
                var reason = acknowledgement.FailureReason ?? "Rover acknowledgement was missing or failed.";
                _outbox.MarkFailed(command.CommandId, reason);

                return MissionExecutionResult.Failure(
                    MissionSignals.RoverAcknowledgementFailed,
                    $"Rover {step.SequenceNumber} did not complete successfully. {reason}");
            }

            if (!SameState(acknowledgement.ActualFinalState, step.ExpectedFinalState))
            {
                var reason = $"Expected {step.ExpectedFinalState}, actual {acknowledgement.ActualFinalState}.";
                _outbox.MarkFailed(command.CommandId, reason);

                return MissionExecutionResult.Failure(
                    MissionSignals.RoverFinalStateMismatch,
                    $"Rover {step.SequenceNumber} acknowledgement does not match the planned final state. {reason} Next rover commands were not dispatched.");
            }

            _outbox.MarkAcknowledged(command.CommandId);
            _stateStore.UpdateState(step.RoverId, acknowledgement.ActualFinalState);
            finalStates.Add(acknowledgement.ActualFinalState);
        }

        return MissionExecutionResult.Success(finalStates);
    }

    private static bool SameState(RoverState actual, RoverState expected) =>
        actual.Position.Equals(expected.Position) &&
        actual.Direction == expected.Direction;
}