using MissionToMars.MissionControl.Results;

namespace MissionToMars.MissionControl.Abstractions;

public interface IMissionInputParser
{
    ParseMissionResult Parse(string input);
}