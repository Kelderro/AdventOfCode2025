namespace Aoc.Solutions.Y2025.D05;

[PuzzleInfo("Cafeteria ")]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();

        var sampleInput = """
                          3-5
                          10-14
                          16-20
                          12-18
                          
                          1
                          5
                          8
                          11
                          17
                          32
                          """.Split(Environment.NewLine);

        input = sampleInput;
        
        LogsEnabled = true;
        
        return part switch
        {
            1 => Part01(input),
            //2 => Part02(input),
            _ => PuzzleNotSolvedString
        };
    }

    private int Part01(IList<string> input)
    {
        // Split based on the empty row
        var emptyRowNumber = input.IndexOf(string.Empty);
        var ranges = input.Take(emptyRowNumber);
        var ingredientIds = input.Skip(emptyRowNumber + 1);
        
        // Except 3
        return 0;
    }
}