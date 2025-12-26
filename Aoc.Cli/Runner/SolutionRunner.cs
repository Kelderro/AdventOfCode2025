using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using Aoc.Cli.Input;
using Aoc.Solutions.Attributes;
using Aoc.Solutions.Common;
using Microsoft.Extensions.Configuration;

namespace Aoc.Cli.Runner;

internal sealed class SolutionRunner(IConfiguration configuration, InputProvider inputProvider)
{
    private const string QualifiedSolutionTypeNameFormat = "{0}.Y{1}.D{2}.{3}";
    private const string SolutionTypeName = "Solution";
    private const string UserSessionKey = "UserSession";

    /// <summary>
    ///     Instantiate the puzzle solution associated with the specified <paramref name="year" /> and
    ///     <paramref name="day" />. If the input file path is not provided, try to get it from the cache, downloading
    ///     first if necessary. Next, run and log the puzzle solution.
    /// </summary>
    /// <param name="year">The year associated with the puzzle</param>
    /// <param name="day">The day associated with the puzzle</param>
    /// <param name="inputPath">
    ///     Used to manually specify the input file path, if unset the <see cref="SolutionRunner" />
    ///     will attempt to get the input from the cache
    /// </param>
    /// <param name="showLogs">
    ///     Some solutions emit logs as they run, when set they will be printed to the console
    /// </param>
    public async Task Run(int year, int day, string inputPath = "", bool showLogs = false)
    {
        if (!TryCreateSolutionInstance(year, day, out var solution))
        {
            Log(SolutionBase.PuzzleNotSolvedString, ConsoleColor.Red);
            return;
        }

        if (!string.IsNullOrWhiteSpace(inputPath))
        {
            if (!File.Exists(inputPath))
            {
                Log($"No input file exists at the specified path [{inputPath}]", ConsoleColor.Red);
                return;
            }

            RunInternal(solution, year, day, inputPath, showLogs);
            return;
        }

        inputPath = inputProvider.FormCachedInputFilePath(year, day);
        if (inputProvider.CheckCacheForInput(year, day))
        {
            Log($"Input found in cache [{inputPath}]", ConsoleColor.Gray);
            RunInternal(solution, year, day, inputPath, showLogs);
            return;
        }

        if (!TryGetUserSession(out var userSession))
        {
            Log("Cannot download input file, user session not set", ConsoleColor.Red);
            return;
        }

        var downloadSuccess = await inputProvider.TryDownloadInputToCache(year, day, userSession).ConfigureAwait(false);
        if (downloadSuccess) RunInternal(solution, year, day, inputPath, showLogs);
    }

    private static void RunInternal(SolutionBase solution, int year, int day, string inputPath, bool showLogs)
    {
        solution.InputPath = inputPath;
        solution.LogsEnabled = showLogs;

        if (CheckSolutionInputSpecific(solution, out var message)) Log(year, day, message, ConsoleColor.DarkYellow);

        for (var i = 0; i < solution.Parts; i++) RunPartInternal(solution, year, day, i + 1);
    }

    private static void RunPartInternal(SolutionBase solutionInstance, int year, int day, int part)
    {
        var stopwatch = new Stopwatch();
        try
        {
            stopwatch.Start();
            var result = solutionInstance.Run(part);
            var elapsed = FormElapsedString(stopwatch.Elapsed);
            Log(year, day, $"[Elapsed: {elapsed}] Part {part} solution => {result}", ConsoleColor.Green);
        }
        catch (Exception e)
        {
            Log(year, day, $"Error running solution:\n{e}", ConsoleColor.Red);
        }
        finally
        {
            stopwatch.Stop();
        }
    }

    private static bool CheckSolutionInputSpecific(SolutionBase instance, out string message)
    {
        message = string.Empty;
        var attr = Attribute.GetCustomAttribute(
            instance.GetType(),
            typeof(InputSpecificSolutionAttribute));

        if (attr != null) message = $"[Warning] {((InputSpecificSolutionAttribute)attr).Message}";

        return attr != null;
    }

    private static bool TryCreateSolutionInstance(int year, int day, [NotNullWhen(true)] out SolutionBase? instance)
    {
        try
        {
            var assembly = typeof(SolutionBase).Assembly;

            var type = assembly.GetType(GetQualifiedSolutionTypeName(year, day));

            if (type == null)
            {
                Log(year, day, $"Failed to {SolutionTypeName} type", ConsoleColor.Red);
                instance = null;
                return false;
            }

            instance = (SolutionBase)Activator.CreateInstance(type)!;
            return true;
        }
        catch (Exception e)
        {
            Log(year, day, $"Failed to create {SolutionTypeName} instance:\n{e}", ConsoleColor.Red);
        }

        instance = null;
        return false;
    }

    private static string GetQualifiedSolutionTypeName(int year, int day)
    {
        var owningAssemblyName = typeof(SolutionBase).Assembly.GetName().Name;
        var dayStringFormat = CompositeFormat.Parse(SolutionBase.DayStringFormat);
        var formattedDayString = string.Format(CultureInfo.InvariantCulture, dayStringFormat, day);

        var qualifiedSolutionTypeNameFormat = CompositeFormat.Parse(QualifiedSolutionTypeNameFormat);

        return string.Format(CultureInfo.InvariantCulture,
            qualifiedSolutionTypeNameFormat,
            owningAssemblyName,
            year,
            formattedDayString,
            SolutionTypeName);
    }

    private bool TryGetUserSession(out string userSession)
    {
        userSession = configuration[UserSessionKey] ?? string.Empty;
        return !string.IsNullOrWhiteSpace(userSession);
    }

    private static string FormElapsedString(TimeSpan elapsed)
    {
        var sb = new StringBuilder();
        var overASecond = false;

        if (elapsed.TotalSeconds >= 1f)
        {
            sb.Append(CultureInfo.InvariantCulture, $"{(int)elapsed.TotalSeconds}.");
            overASecond = true;
        }

        sb.Append(overASecond ? $"{elapsed.Milliseconds:D3}" : $"{elapsed.Milliseconds}");
        sb.Append(overASecond ? "s" : "ms");

        return sb.ToString();
    }

    private static void Log(int year, int day, string log, ConsoleColor color = default)
    {
        Log($"[Year: {year}, Day: {day}] {log}", color);
    }

    private static void Log(string log, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(log);
        Console.ResetColor();
    }
}