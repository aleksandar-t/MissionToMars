using MissionToMars.MissionControl;
using MissionToMars.MissionControl.Abstractions;
using MissionToMars.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MissionToMars.Tests.Composition;

public sealed class ServiceRegistrationTests
{
    [Fact]
    public void ServiceCollection_ResolvesMissionRunnerThroughAbstractions()
    {
        var services = new ServiceCollection()
            .AddMissionControl()
            .AddMissionInfrastructure();

        using var provider = services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateOnBuild = true,
            ValidateScopes = true
        });
        using var scope = provider.CreateScope();

        var runner = scope.ServiceProvider.GetRequiredService<IMissionRunner>();

        Assert.NotNull(runner);
    }
}