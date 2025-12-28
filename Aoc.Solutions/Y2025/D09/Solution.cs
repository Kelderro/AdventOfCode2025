using System.Numerics;

namespace Aoc.Solutions.Y2025.D09;

[PuzzleInfo("Movie Theater")]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();

        var sampleInput = """
                          7,1
                          11,1
                          11,7
                          9,7
                          9,5
                          2,5
                          2,3
                          7,3
                          """.Split(Environment.NewLine);

        // input = sampleInput;
        
        LogsEnabled = true;
        
        return part switch
        {
            1 => Part01(input), // 4749838800
            // 2 => Part02(input),
            _ => PuzzleNotSolvedString
        };
    }

    private BigInteger Part01(IList<string> input)
    {
        var rows = input.Select(x => x.Split(','))
            .Select(x => (BigInteger.Parse(x[0], CultureInfo.InvariantCulture), BigInteger.Parse(x[1], CultureInfo.InvariantCulture)))
            .ToList<(BigInteger ColumnIndex, BigInteger RowIndex)>();
        
        // Grid grid
        var largestArea = BigInteger.Zero;
        
        for (var r = 0; r < rows.Count; r++)
        {
            var currentRow = rows[r];

            for (var other = r + 1; other < input.Count; other++)
            {
                var otherRow = rows[other];
                var width = BigInteger.Abs(currentRow.ColumnIndex - otherRow.ColumnIndex) + 1;
                var height = BigInteger.Abs(currentRow.RowIndex - otherRow.RowIndex) + 1;
                var size = width * height;
                Log($"{currentRow.ColumnIndex},{currentRow.RowIndex} and {otherRow.ColumnIndex},{otherRow.RowIndex} has a total size of {size}");

                largestArea = BigInteger.Max(largestArea, size);
            }
        }
        return largestArea;
    }
}