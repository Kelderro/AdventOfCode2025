namespace Aoc.Solutions.UnitTests.Y2025.D04;

public class Tests
{
    private readonly Solutions.Y2025.D04.Solution _solution = new();

    public static TheoryData<string[]> SharedTestData =>
    [
        """
            ..@@.@@@@.
            @@@.@.@.@@
            @@@@@.@.@@
            @.@@@@..@.
            @@.@@@@.@@
            .@@@@@@@.@
            .@.@.@.@@@
            @.@@@.@@@@
            .@@@@@@@@.
            @.@.@@@.@.
            """.Split(Environment.NewLine)
    ];
    
    [Theory]
    [MemberData(nameof(SharedTestData))]
    public void Part01_Sample_Test(string[] input)
    {
        // Act
        var result = _solution.Part01(input);

        // Assert
        result.Should().Be(13);
    }
    
    [Theory]
    [MemberData(nameof(SharedTestData))]
    public void Part02_Sample_Test(string[] input)
    {
        // Act
        var result = _solution.Part02(input);

        // Assert
        result.Should().Be(43);
    }
}