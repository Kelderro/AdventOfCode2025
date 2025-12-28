using System.Diagnostics;
using Aoc.Utilities;
using JetBrains.Annotations;

namespace Aoc.Solutions.Common;

[UsedImplicitly]
// Idea of the class copied from https://github.com/tmbarker/advent-of-code/tree/main
public abstract class SolutionBase
{
    public const string DayStringFormat = "{0:D2}";
    public const string PuzzleNotSolvedString = "Puzzle not solved!";

    public static int Parts => 2;

    public bool LogsEnabled { get; set; }
    public string InputPath { get; set; } = string.Empty;

    /// <summary>
    ///     Run the specified Solution <paramref name="part" />
    /// </summary>
    /// <param name="part">The one-based solution part</param>
    /// <returns>The solution part result</returns>
    public abstract object Run(int part);

    protected void Log(string log)
    {
        if (LogsEnabled)
            Console.WriteLine(log);
    }

    protected void Log(IEnumerable<string> log)
    {
        if (LogsEnabled)
            log.ToList().ForEach(Console.WriteLine);
    }
    
    protected void LogGrid(char[,] array)
    {
        if (LogsEnabled)
        {
            for (var r = 0; r < array.GetLength(0); r++)
            {
                for (var c = 0; c < array.GetLength(1); c++)
                {
                    Console.Write(array[r, c]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }

    protected string[] GetInputLines()
    {
        AssertInputExists();
        return File.ReadAllLines(InputPath);
    }

    protected IList<string[]> ChunkInputByNonEmpty()
    {
        return GetInputLines().ChunkByNonEmpty();
    }

    protected string GetInputText()
    {
        AssertInputExists();
        return File.ReadAllText(InputPath).TrimEnd();
    }

    protected T[] ParseInputLines<T>(Func<string, T> parseFunc)
    {
        return GetInputLines().Select(parseFunc).ToArray();
    }

    private void AssertInputExists()
    {
        Debug.Assert(InputFileExists(), $"Input file does not exist [{InputPath}]");
    }

    private bool InputFileExists()
    {
        return File.Exists(InputPath);
    }
}