using MissionToMars.Domain.Results;

namespace MissionToMars.MissionControl.Abstractions;

public interface IMissionRunner
{
    MissionRunResult Run(string input);
}