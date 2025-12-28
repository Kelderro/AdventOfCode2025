using System.CommandLine;
using Aoc.Cli.Input;
using Aoc.Cli.Runner;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationManager()
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile("appsettings.Development.json", true, true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables()
    .Build();

var inputProvider = new InputProvider(configuration);
var solutionRunner = new SolutionRunner(configuration, inputProvider);

var solveCommand = new Command(
    "solve",
    "Run the specified puzzle solution, if it exists");

var yearArg = new Argument<int>("Year")
{
    Description = @"The year the puzzle belongs to"
};

var dayArg = new Argument<int>("day")
{
    Description = "The puzzle day"
};

var inputPathOption = new Option<string>(
    "--input",
    "--path")
{
    Description = "Manually specify the path to the input file",
    DefaultValueFactory = _ => string.Empty
};

var logsOption = new Option<bool>(
    "--logs")
{
    Description = "Some solutions emit logs as they run, print any such logs to the console",
    DefaultValueFactory = _ => false
};

solveCommand.Arguments.Add(yearArg);
solveCommand.Arguments.Add(dayArg);
solveCommand.Options.Add(inputPathOption);
solveCommand.Options.Add(logsOption);

var rootCommand = new RootCommand("CLI entry point for running AoC puzzle solutions");
rootCommand.Subcommands.Add(solveCommand);

var parseResult = rootCommand.Parse(args);

if (parseResult.Errors.Count == 0
    && parseResult.GetValue(yearArg) is var year
    && parseResult.GetValue(dayArg) is var day
    && parseResult.GetValue(logsOption) is var showLogs
    && parseResult.GetValue(inputPathOption) is var inputPath)
{
    await solutionRunner.Run(year, day, inputPath!, showLogs).ConfigureAwait(false);
    return 0;
}

foreach (var parseError in parseResult.Errors)
{
    await Console.Error.WriteLineAsync(parseError.Message);
}

return 1;