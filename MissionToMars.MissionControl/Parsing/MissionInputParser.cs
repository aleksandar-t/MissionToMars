using MissionToMars.Domain.Constants;
using MissionToMars.Domain.Enums;
using MissionToMars.Domain.Models;
using MissionToMars.MissionControl.Abstractions;
using MissionToMars.MissionControl.Results;

namespace MissionToMars.MissionControl.Parsing;

public sealed class MissionInputParser : IMissionInputParser
{
    public ParseMissionResult Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return ParseMissionResult.Failure(MissionSignals.EmptyInput, "Mission input cannot be empty.");
        }

        var lines = input
            .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToArray();

        if (lines.Length < 3 || lines.Length % 2 == 0)
        {
            return ParseMissionResult.Failure(
                MissionSignals.InvalidInputShape,
                "Input must contain plateau coordinates followed by pairs of rover position and instruction lines.");
        }

        var plateauParts = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (plateauParts.Length != 2 || !int.TryParse(plateauParts[0], out var maxX) || !int.TryParse(plateauParts[1], out var maxY))
        {
            return ParseMissionResult.Failure(MissionSignals.InvalidPlateau, "First line must contain two integer plateau coordinates.");
        }

        var rovers = new List<RoverMissionInput>();
        var roverNumber = 1;

        for (var index = 1; index < lines.Length; index += 2)
        {
            var positionParts = lines[index].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (positionParts.Length != 3 ||
                !int.TryParse(positionParts[0], out var x) ||
                !int.TryParse(positionParts[1], out var y) ||
                positionParts[2].Length != 1 ||
                !DirectionExtensions.TryParse(positionParts[2][0], out var direction))
            {
                return ParseMissionResult.Failure(
                    MissionSignals.InvalidRoverPosition,
                    $"Rover {roverNumber} position line must contain X, Y and one of N, E, S, W.");
            }

            var instructions = lines[index + 1].Trim().ToUpperInvariant();
            rovers.Add(new RoverMissionInput(
                $"Rover-{roverNumber}",
                new RoverState(new Position(x, y), direction),
                instructions));

            roverNumber++;
        }

        return ParseMissionResult.Success(new MissionPlan(new Plateau(maxX, maxY), rovers));
    }
}