using System.Numerics;

namespace Aoc.Solutions.Y2025.D02;

[PuzzleInfo("Gift Shop")]
internal sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();

        var sampleInput = """
                          11-22,95-115,998-1012,1188511880-1188511890,222220-222224,
                          1698522-1698528,446443-446449,38593856-38593862,565653-565659,
                          824824821-824824827,2121212118-2121212124
                          """.Split(",");

        input = sampleInput;
        
        LogsEnabled = true;
        
        return part switch
        {
            1 => Part01(input),
            //2 => Part02(input),
            _ => PuzzleNotSolvedString
        };
    }

    public int Part01(IEnumerable<string> input)
    {
        var splitInput = input.Select(x => x.Split('-'));

        foreach (var split in splitInput)
        {
            var startingNumber = BigInteger.Parse(split[0]);
            var endingNumber = BigInteger.Parse(split[1]);
            var invalidNumbers = new List<string>();
            
            for (var current = startingNumber; current <= endingNumber; current++)
            {
            }    
        }
        
        return 0;
    }

    public int Part02(IEnumerable<string> input)
    {
        return 0;
    }
}