using MissionToMars.MissionControl.Abstractions;
using MissionToMars.MissionControl.Execution;
using MissionToMars.MissionControl.Parsing;
using MissionToMars.MissionControl.Planning;
using MissionToMars.MissionControl.Running;
using Microsoft.Extensions.DependencyInjection;

namespace MissionToMars.MissionControl;

public static class ServiceRegistration
{
    public static IServiceCollection AddMissionControl(this IServiceCollection services)
    {
        services.AddSingleton<IMissionInputParser, MissionInputParser>();
        services.AddSingleton<IMissionPlanner, MissionPlanner>();
        services.AddScoped<IMissionExecutionOrchestrator, MissionExecutionOrchestrator>();
        services.AddScoped<IMissionRunner, MissionRunner>();

        return services;
    }
}