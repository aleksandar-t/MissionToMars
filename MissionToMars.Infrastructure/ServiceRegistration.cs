using MissionToMars.MissionControl.Abstractions;
using MissionToMars.Infrastructure.Acknowledgements;
using MissionToMars.Infrastructure.Dispatching;
using MissionToMars.Infrastructure.Outbox;
using MissionToMars.Infrastructure.State;
using Microsoft.Extensions.DependencyInjection;

namespace MissionToMars.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddMissionInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ICommandOutbox, InMemoryCommandOutbox>();
        services.AddScoped<IRoverCommandDispatcher, MockRoverCommandDispatcher>();
        services.AddScoped<IRoverAcknowledgementReceiver, MockRoverAcknowledgementReceiver>();
        services.AddScoped<IRoverStateStore, InMemoryRoverStateStore>();

        return services;
    }
}