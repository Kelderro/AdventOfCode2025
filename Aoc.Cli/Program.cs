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
var yearArg = new Argument<int>(
    "year",
    "The year the puzzle belongs to");
var dayArg = new Argument<int>(
    "day",
    "The puzzle day");
var inputPathOption = new Option<string>(
    ["--input", "--path"],
    description: "Manually specify the path to the input file",
    getDefaultValue: () => string.Empty);
var logsOption = new Option<bool>(
    ["--logs"],
    description: "Some solutions emit logs as they run, print any such logs to the console",
    getDefaultValue: () => false);

solveCommand.AddArgument(yearArg);
solveCommand.AddArgument(dayArg);
solveCommand.AddOption(logsOption);
solveCommand.AddOption(inputPathOption);
solveCommand.SetHandler(
    async (year, day, inputPath, showLogs) =>
        await solutionRunner.Run(year, day, inputPath, showLogs).ConfigureAwait(false),
    yearArg,
    dayArg,
    inputPathOption,
    logsOption);

// var updateReadmeCommand = new Command(
//     name: "update-readme",
//     description: "Update the generated README.md content");
//
// updateReadmeCommand.SetHandler(ReadmeUtils.UpdateReadme);
//
// var scratchCommand = new Command(
//     name: "scratch",
//     description: $"Execute the {nameof(ScratchPad)}");
//
// scratchCommand.SetHandler(handle: ScratchPad.Execute);

var rootCommand = new RootCommand("CLI entry point for running AoC puzzle solutions");
rootCommand.AddCommand(solveCommand);
// rootCommand.AddCommand(updateReadmeCommand);
// rootCommand.AddCommand(scratchCommand);

return await rootCommand.InvokeAsync(args);