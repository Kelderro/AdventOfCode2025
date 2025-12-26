namespace Aoc.Solutions.Y2025.D01;

[PuzzleInfo("Secret Entrance")]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();

        var sampleInput = """
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
                          """.Split(Environment.NewLine);

        input = sampleInput;
        
        LogsEnabled = false;
        
        return part switch
        {
            1 => Part01(input), // 1076
            2 => Part02(input), // 6379
            _ => PuzzleNotSolvedString
        };
    }

    private int Part01(IEnumerable<string> input)
    {
        var (dialEndsAtZeroCount, _) = Solve(input);
        
        return dialEndsAtZeroCount;
    }

    private int Part02(IEnumerable<string> input)
    {
        var (_, dialPassingByZeroCount) = Solve(input);
        
        return dialPassingByZeroCount;
    }

    private (int dialEndAtZeroCount, int dialPassingByZeroCount) Solve(IEnumerable<string> input)
    {
        var dialPosition = 50;
        var dialEndsAtZeroCount = 0;
        var dialPassingByZeroCount = 0;
        
        foreach (var inputLine in input)
        {
            var direction = inputLine[0];
            var amount = int.Parse(inputLine[1..], CultureInfo.InvariantCulture);
            
            for(var i = 1; i <= amount; i++)
            {
                if (direction == 'L')
                {
                    dialPosition--;

                    if (dialPosition < 0) dialPosition = 99;
                }
                else
                {
                    dialPosition++;
                    
                    if (dialPosition > 99) dialPosition = 0;
                }
                
                if (dialPosition == 0)
                {
                    dialPassingByZeroCount++;
                }
            }
            
            if (dialPosition == 0)
            {
                dialEndsAtZeroCount++;
            }
            
            Log($"The dial is rotated '{inputLine}' to point at '{dialPosition}'. Dial ends at zero count: {dialEndsAtZeroCount}. Dial passing by zero count: {dialPassingByZeroCount}");
        }
        
        return (dialEndsAtZeroCount, dialPassingByZeroCount);
    }
}