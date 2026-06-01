namespace MissionToMars.Domain.Constants;

public static class MissionSignals
{
    public const string EmptyInput = "EMPTY_INPUT";
    public const string InvalidInput = "INVALID_INPUT";
    public const string InvalidInputShape = "INVALID_INPUT_SHAPE";

    public const string InvalidPlateau = "INVALID_PLATEAU";
    public const string InvalidRoverPosition = "INVALID_ROVER_POSITION";
    public const string InvalidInstruction = "INVALID_INSTRUCTION";

    public const string InvalidMissionPlan = "INVALID_MISSION_PLAN";
    public const string MissionExecutionFailed = "MISSION_EXECUTION_FAILED";

    public const string RoverStartOutsidePlateau = "ROVER_START_OUTSIDE_PLATEAU";
    public const string RoverStartCollision = "ROVER_START_COLLISION";
    public const string RoverLeftPlateau = "ROVER_LEFT_PLATEAU";
    public const string RoverCollision = "ROVER_COLLISION";
    public const string RoverFinalCollision = "ROVER_FINAL_COLLISION";

    public const string CommandDispatchFailed = "COMMAND_DISPATCH_FAILED";
    public const string RoverAcknowledgementFailed = "ROVER_ACKNOWLEDGEMENT_FAILED";
    public const string RoverFinalStateMismatch = "ROVER_FINAL_STATE_MISMATCH";
}