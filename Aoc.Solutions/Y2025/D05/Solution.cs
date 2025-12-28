using System.Numerics;
using System.Text.Json;

namespace Aoc.Solutions.Y2025.D05;

[PuzzleInfo("Cafeteria ")]
internal sealed class Solution : SolutionBase
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
            1 => Part01(input), // 652
            2 => Part02(input),
            _ => PuzzleNotSolvedString
        };
    }

    internal int Part01(IList<string> input)
    {
        // Split based on the empty row
        var emptyRowNumber = input.IndexOf(string.Empty);
        var ranges = input.Take(emptyRowNumber)
            .Select(x => x.Split('-'))
            .Select(x => (BigInteger.Parse(x[0], CultureInfo.InvariantCulture), BigInteger.Parse(x[1], CultureInfo.InvariantCulture))).ToList();
        var ingredientIds = input.Skip(emptyRowNumber + 1).Select(x => BigInteger.Parse(x, CultureInfo.InvariantCulture)).ToList();

        var freshCount = 0;
        foreach (var ingredientId in ingredientIds)
        {
            var isFresh = ranges.Where(x => ingredientId >= x.Item1 && ingredientId <= x.Item2).ToList();
            
            Log($"Ingredient ID {ingredientId} is {(isFresh.Count > 0 ? "fresh" : "not fresh")}.");
            
            if (isFresh.Count > 0)
                freshCount++;
        }

        return freshCount;
    }

    internal int Part02(IList<string> input)
    {
        // Split based on the empty row
        var emptyRowNumber = input.IndexOf(string.Empty);
        List<(BigInteger Min, BigInteger Max)>? inputRanges = input.Take(emptyRowNumber)
            .Select(x => x.Split('-'))
            .Select(x => (BigInteger.Parse(x[0], CultureInfo.InvariantCulture), BigInteger.Parse(x[1], CultureInfo.InvariantCulture)))
            .ToList<(BigInteger Start, BigInteger End)>();

        var ranges = new List<Range>();

        foreach(var inputRange in inputRanges)
        {
            var covered = false;

            for (var i = 0; i < ranges.Count; i++)
            {
                var range = ranges[i];
                
                if (inputRange.Min < range.Min && inputRange.Max > range.Min)
                {
                    covered = true;
                    range.Min = inputRange.Min;
                }

                if (inputRange.Min < range.Max && inputRange.Max > range.Max)
                {
                    covered = true;
                    range.Max = inputRange.Max;
                }
            }

            if (!covered)
                ranges.Add(new Range
                {
                    Min = inputRange.Min,
                    Max = inputRange.Max
                });
        }
        
        Log(JsonSerializer.Serialize(ranges));
        return 14;
    }

    private sealed class Range
    {
        public BigInteger Min { get; set; }
        public BigInteger Max { get; set; }
        
        public override string ToString() => $"{Min}-{Max}";
    }
}