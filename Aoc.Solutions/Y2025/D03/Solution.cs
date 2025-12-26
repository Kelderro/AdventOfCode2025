namespace Aoc.Solutions.Y2025.D03;

[PuzzleInfo("Lobby")]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();

        var sampleInput = """
                          987654321111111
                          811111111111119
                          234234234234278
                          818181911112111
                          """.Split(Environment.NewLine);

        input = sampleInput;
        
        LogsEnabled = false;
        
        return part switch
        {
            1 => Part01(input), // 17493
            2 => Part02(input),
            _ => PuzzleNotSolvedString
        };
    }

    private int Part01(IList<string> input)
    {
        var numbers = new List<int>();
        foreach (var line in input)
        {
            var currentHighest = 0;
            for (var i = 0; i < line.Length - 1; i++)
            {
                for (var j = i + 1; j < line.Length; j++)
                {
                    var number = int.Parse(string.Concat(line[i], line[j]), CultureInfo.InvariantCulture);
                    if (number > currentHighest)
                    {
                        currentHighest = number;
                    }
                }
            }
            
            Log($"In {line}, you can make the largest joltage possible by turning on the batteries labeled {0} and {0}, producing {currentHighest} jolts");
            numbers.Add(currentHighest);
        }
        
        return numbers.Sum();
    }
    
    private int Part02(IList<string> input)
    {
        var numbers = new List<int>();
        
        // Find the highest number in the input
        foreach (var line in input)
        {
            var currentHighest = 0;
            var maxLength = 12;

            for (var left = 0; left < line.Length; left++)
            {
                var right = maxLength - left;
                var range = line.Substring(left, line.Length - right);
                
            }
            for (var i = 12; i > 0; i++)
            {
                var range = line[..i];
                
            }
            var maxVoltage = line[^11..];
            
            // Find the highest number in the line except the last 11 characters
            var remainingLine = line[..^11];
            var highestNumber = remainingLine.Max();
            
            // Make chunks
            
            
            
            for (var i = 0; i < line.Length - 1; i++)
            {
                for (var j = i + 1; j < line.Length; j++)
                {
                    var number = int.Parse(string.Concat(line[i], line[j]), CultureInfo.InvariantCulture);
                    if (number > currentHighest)
                    {
                        currentHighest = number;
                    }
                }
            }
            
            Log($"In {line}, you can make the largest joltage possible by turning on the batteries labeled {0} and {0}, producing {currentHighest} jolts");
            numbers.Add(currentHighest);
        }
        
        return numbers.Sum();
    }
}