using System.Text;

namespace Aoc.Solutions.Y2025.D07;

[PuzzleInfo("Laboratories")]
internal sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();

        var sampleInput = """
                          .......S.......
                          ...............
                          .......^.......
                          ...............
                          ......^.^......
                          ...............
                          .....^.^.^.....
                          ...............
                          ....^.^...^....
                          ...............
                          ...^.^...^.^...
                          ...............
                          ..^...^.....^..
                          ...............
                          .^.^.^.^.^...^.
                          ............... 
                          """.Split(Environment.NewLine);

        // input = sampleInput;
        
        LogsEnabled = true;
        
        return part switch
        {
            1 => Part01(input), // 1646
            // 2 => Part02(input),
            _ => PuzzleNotSolvedString
        };
    }

    internal int Part01(IList<string> input)
    {
        var beamSplitCount = 0;
        for (var r = 1; r < input.Count; r++)
        {
            // Look at the previous row
            var previousRow = input[r - 1];
            var currentRow = new StringBuilder(input[r]);
            
            for (var c = 0; c < currentRow.Length; c++)
            {
                if (currentRow[c] == '^' && previousRow[c] == '|')
                {
                    currentRow[c - 1] = '|';
                    currentRow[c + 1] = '|';
                    beamSplitCount++;
                }
                
                if (currentRow[c] != '.')
                    continue;
                
                if (previousRow[c] == 'S')
                {
                    currentRow[c] = '|';
                }
                
                if (previousRow[c] == '|')
                {
                    currentRow[c] = '|';
                }
            }
            
            input[r] = currentRow.ToString();
        }

        Log(input);
        
        return beamSplitCount;
    }
}