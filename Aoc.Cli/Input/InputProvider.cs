using System.Globalization;
using System.Text;
using Aoc.Cli.Client;
using Microsoft.Extensions.Configuration;

namespace Aoc.Cli.Input;

/// <summary>
///     A utility which manages downloading, caching, and providing puzzle input files
/// </summary>
internal sealed class InputProvider(IConfiguration configuration)
{
    private const string InputCachePathKey = "InputCachePath";
    private const string InputRequestRouteFormat = "{0}/day/{1}/input";
    private const string DefaultInputDirectoryName = "Inputs";
    private const string DefaultInputPathFormat = "Y{0}/D{1:D2}/input.txt";

    public bool CheckCacheForInput(int year, int day)
    {
        return File.Exists(FormCachedInputFilePath(year, day));
    }

    public async Task<bool> TryDownloadInputToCache(int year, int day, string userSession)
    {
        var dirPath = FormInputPath(year, day);
        var filePath = FormCachedInputFilePath(year, day);
        var requestRoute = FormDomainRelativeInputRequest(year, day);

        try
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
                Log($"Creating cache directory [{dirPath}]", ConsoleColor.Gray);
            }

            Log($"Requesting input [GET {AocHttpClient.Domain}/{requestRoute}]", ConsoleColor.Gray);
            var responseMessage = await AocHttpClient.SendRequest(requestRoute, userSession).ConfigureAwait(false);
            var responseContent = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (responseMessage.IsSuccessStatusCode)
            {
                Log($"Response received [{responseMessage.StatusCode}]", ConsoleColor.Gray);
                await File.WriteAllTextAsync(filePath, responseContent).ConfigureAwait(false);
                Log($"Input written to file [{filePath}]", ConsoleColor.Gray);
                return true;
            }

            Log($"Request error [{responseMessage.StatusCode}]", ConsoleColor.Red);
            return false;
        }
        catch (Exception e)
        {
            Log($"Error downloading input [GET {AocHttpClient.Domain}/{requestRoute}]:\n{e}", ConsoleColor.Red);
            return false;
        }
    }

    private string FormInputPath(int year, int day)
    {
        var fullInputPath = FormCachedInputFilePath(year, day);
        return Path.GetDirectoryName(fullInputPath) ?? throw new InvalidOperationException();
    }
    
    public string FormCachedInputFilePath(int year, int day)
    {
        var solutionDirectoryPath = GetCachePath();
        
        var defaultInputPathFormat = CompositeFormat.Parse(DefaultInputPathFormat);
        var relativeDirectoryPath = string.Format(CultureInfo.InvariantCulture, defaultInputPathFormat, year, day);

        //  Inputs are stored under the cache directory based on year: <cache directory>/<year>/<day>.txt
        return Path.Combine(solutionDirectoryPath, relativeDirectoryPath);
    }

    private string GetCachePath()
    {
        var fullPath = configuration[InputCachePathKey];
        return string.IsNullOrWhiteSpace(fullPath)
            ? GetDefaultCachePath()
            : fullPath;
    }

    private static string GetDefaultCachePath()
    {
        return Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            DefaultInputDirectoryName);
    }

    private static string FormDomainRelativeInputRequest(int year, int day)
    {
        var inputRequestRouteFormat = CompositeFormat.Parse(InputRequestRouteFormat);
        return string.Format(CultureInfo.InvariantCulture, inputRequestRouteFormat, year, day);
    }

    private static void Log(string log, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(log);
        Console.ResetColor();
    }
}