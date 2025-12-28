namespace Aoc.Solutions.UnitTests.Y2025.D01;

public class Tests
{
    private readonly Solutions.Y2025.D01.Solution _solution = new();

    public static TheoryData<string[]> SharedTestData =>
    [
        """
        L68
        L30
        R48
        L5
        R60
        L55
        L1
        L99
        R14
        L82
        """.Split(Environment.NewLine)
    ];
    
    [Theory]
    [MemberData(nameof(SharedTestData))]
    public void Part01_Sample_Test(string[] input)
    {
        // Act
        var result = _solution.Part01(input);

        // Assert
        result.Should().Be(3);
    }
    
    [Theory]
    [MemberData(nameof(SharedTestData))]
    public void Part02_Sample_Test(string[] input)
    {
        // Act
        var result = _solution.Part02(input);
    
        // Assert
        result.Should().Be(6);
    }
}