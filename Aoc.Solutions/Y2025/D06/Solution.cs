using System.Numerics;
using System.Text;

namespace Aoc.Solutions.Y2025.D06;

[PuzzleInfo("Trash Compactor")]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();

        var sampleInput = """
                          123 328  51 64 
                           45 64  387 23 
                            6 98  215 314
                          *   +   *   +  
                          """.Split(Environment.NewLine);

        // input = sampleInput;
        
        LogsEnabled = true;
        
        return part switch
        {
            1 => Part01(input), // 4719804927602
            2 => Part02(input), // 9608327000261
            _ => PuzzleNotSolvedString
        };
    }

    private static BigInteger Part01(IList<string> input)
    {
        var rows = input.Select(i => i.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .ToList();
        
        var columns = Transpose(rows);
        var grandTotal = BigInteger.Zero;

        foreach (var column in columns)
        {
            var values = column.Take(column.Count - 1)
                .Select(BigInteger.Parse)
                .ToList();
            var symbol = column[^1];

            Func<BigInteger, BigInteger, BigInteger> operation = symbol.Equals("+", StringComparison.OrdinalIgnoreCase) 
                ? (a, b) => a + b
                : (a, b) => a * b;
            
            var total = values.Aggregate(operation);
            grandTotal += total;
        }
        
        return grandTotal;
    }
    
    private BigInteger Part02(IList<string> input)
    {
        var columns = GetColumns(input);
        var grandTotal = BigInteger.Zero;

        foreach (var column in columns)
        {
            var newList = new List<BigInteger>();
            
            for (var i = column.Values[0].Length - 1; i >= 0; i--)
            {
                var numberBuilder = new StringBuilder();
                foreach (var value in column.Values)
                {
                    if (value[i] != ' ')
                        numberBuilder.Append(value[i]);
                }

                newList.Add(int.Parse(numberBuilder.ToString(), CultureInfo.InvariantCulture));
            }
            
            Func<BigInteger, BigInteger, BigInteger> operation = column.Symbol.Equals('+') 
                ? (a, b) => a + b
                : (a, b) => a * b;

            var total = newList.Aggregate(operation);
            grandTotal += total;
        }
        
        return grandTotal;
    }

    private List<(char Symbol, List<string> Values)> GetColumns(IList<string> input)
    {
        var symbolRow = input[^1];
        
        var symbolIndices = new List<(char Symbol, int Index)>();

        for (var i = 0; i < symbolRow.Length; i++)
        {
            var symbol = symbolRow[i];
            
            if (symbol != '*' && symbol != '+')
                continue;

            (char Symbol, int Index) symbolIndex = (symbol, i);
            symbolIndices.Add(symbolIndex);
            
            Log($"Found '{symbolIndex.Symbol}' at index: {symbolIndex.Index}");
        }

        var columns = new List<(char Symbol, List<string> Values)>();

        for (var i = 0; i < symbolIndices.Count; i++)
        {
            var (symbol, startAtIndex) = symbolIndices[i];
            
            var endAtIndex = i == symbolIndices.Count - 1
                ? symbolRow.Length
                : symbolIndices[i + 1].Index - 1;
            
            var column = input
                .Select(lineInput => lineInput[startAtIndex..endAtIndex])
                .Take(input.Count - 1)
                .ToList();

            columns.Add((symbol, column));
        }
        return columns;
    }

    private static List<List<string>> Transpose(List<string[]> rows)
    {
        var columns = new List<List<string>>();

        for (var r = 0; r < rows.Count; r++)
        {
            for (var c = 0; c < rows[r].Length; c++)
            {
                if (r == 0)
                    columns.Add([rows[r][c]]);                    
                else
                    columns[c].Add(rows[r][c]);
            }
        }
        return columns;
    }
}