using MissionToMars.MissionControl;
using MissionToMars.MissionControl.Abstractions;
using MissionToMars.Domain.Results;
using MissionToMars.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection()
    .AddMissionControl()
    .AddMissionInfrastructure();

using var serviceProvider = services.BuildServiceProvider(new ServiceProviderOptions
{
    ValidateOnBuild = true,
    ValidateScopes = true
});

while (true)
{
    Console.WriteLine("Enter mission input. Press Enter on an empty line to run, 'exit' to close.");

    var input = ReadMissionInput();
    if (input is null)
    {
        break;
    }

    if (string.IsNullOrWhiteSpace(input))
    {
        Console.WriteLine();
        continue;
    }

    using var scope = serviceProvider.CreateScope();
    var runner = scope.ServiceProvider.GetRequiredService<IMissionRunner>();
    var result = runner.Run(input);

    PrintResult(result);
    Console.WriteLine(); 
}

static string? ReadMissionInput()
{
    var lines = new List<string>();

    while (true)
    {
        var line = Console.ReadLine();

        if (line is null)
        {
            return null;
        }

        if (lines.Count == 0 && line.Equals("exit", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(line))
        {
            return lines.Count == 0
                ? string.Empty
                : string.Join(Environment.NewLine, lines);
        }

        lines.Add(line);
    }
}

static void PrintResult(MissionRunResult result)
{
    if (!result.IsSuccess)
    {
        Console.WriteLine($"Mission failed. {result.Error?.Message}");
        return;
    }

    foreach (var finalState in result.FinalStates)
    {
        Console.WriteLine(finalState);
    }
}
