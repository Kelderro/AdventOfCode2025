using AdventOfCode2024.Day07;

namespace AdventOfCode2024.Tests;

public class Day07Tests
{
    [Fact]
    public void Part01_Sample_Test()
    {
        // Arrange
        const string input = """
                             190: 10 19
                             3267: 81 40 27
                             83: 17 5
                             156: 15 6
                             7290: 6 8 6 15
                             161011: 16 10 13
                             192: 17 8 14
                             21037: 9 7 18 13
                             292: 11 6 16 20
                             """;

        // Act
        var result = Puzzle.Part01(input.Split(Environment.NewLine));
        
        // Assert
        result.Should().Be(3749);
    }
    
    [Fact]
    public void Part02_Sample_Test()
    {
        const string input = """
                             156: 15 6
                             """;
        
        var result = Puzzle.Part02(input.Split(Environment.NewLine));
    
        result.Should().Be(11387);
    }
}