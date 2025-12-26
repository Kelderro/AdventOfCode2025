using AdventOfCode2024.Day06;
using Xunit.Abstractions;

namespace AdventOfCode2024.Tests;

public class Day06Tests(ITestOutputHelper output)
{
    [Fact]
    public void Part01_Sample_Test()
    {
        const string input = """
                             ....#.....
                             .........#
                             ..........
                             ..#.......
                             .......#..
                             ..........
                             .#..^.....
                             ........#.
                             #.........
                             ......#...
                             """;

        var result = 0;
        
        var consoleOutput = ConsoleOutputHelper.CaptureOutput(() =>
        {
            result = Puzzle.Part01(input.Split(Environment.NewLine));
        });
        
        output.WriteLine(consoleOutput);
        
        result.Should().Be(41);
    }
    
    [Fact(Skip = "WIP")]
    public void Part02_Sample_Test()
    {
        const string input = """
                             .
                             """;
        
        var result = Puzzle.Part02(input.Split(Environment.NewLine));
    
        result.Should().Be(0);
    }
}

internal static class ConsoleOutputHelper
{
    public static string CaptureOutput(Action actionThatWritesToConsole)
    {
        using var stringWriter = new StringWriter();
        var originalOutput = Console.Out;
        Console.SetOut(stringWriter);

        try
        {
            actionThatWritesToConsole();
            return stringWriter.ToString().Trim();
        }
        finally 
        {
            Console.SetOut(originalOutput);
        }
    }
}