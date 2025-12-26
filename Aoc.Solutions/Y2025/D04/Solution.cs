using System.Drawing;

namespace Aoc.Solutions.Y2025.D04;

[PuzzleInfo("Printing Department")]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();

        var sampleInput = """
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
                          """.Split(Environment.NewLine);

        // input = sampleInput;
        
        LogsEnabled = true;
        
        return part switch
        {
            1 => Part01(input), // 1587
            2 => Part02(input), // 8946
            _ => PuzzleNotSolvedString
        };
    }

    private int Part01(IList<string> input)
    {
        // Create grid
        var grid = new char[input.Count, input[0].Length];

        for (var row = 0; row < input.Count; row++)
        {
            for (var col = 0; col < input[row].Length; col++)
            {
                grid[row, col] = input[row][col];
            }
        }

        LogGrid(grid);
        
        Solve(grid);
        
        LogGrid(grid);
        
        return CountMarkers(grid);
    }
    
    private int Part02(IList<string> input)
    {
        // Create grid
        var grid = new char[input.Count, input[0].Length];

        for (var row = 0; row < input.Count; row++)
        {
            for (var col = 0; col < input[row].Length; col++)
            {
                grid[row, col] = input[row][col];
            }
        }

        Log("Initial state:");
        LogGrid(grid);

        var totalRolls = 0;

        while (true)
        {
            Solve(grid);
            var attemptCount = CountMarkers(grid);

            totalRolls += attemptCount;
            
            if (attemptCount == 0)
                return totalRolls;

            Log($"Remove {attemptCount} roll of paper");
            LogGrid(grid);
            
            RemoveMarkers(grid);
            
            Log($"Removed {attemptCount} roll of paper");
            LogGrid(grid);
        }
    }

    private void Solve(char[,] grid)
    {
        for (var row = 0; row < grid.GetLength(0); row++)
        {
            for (var col = 0; col < grid.GetLength(1); col++)
            {
                var roleCount = 0;
                if (grid[row, col] == '.') continue;
                
                roleCount += HasRole(grid, new Point(col - 1, row - 1)) ? 1 : 0;
                roleCount += HasRole(grid, new Point(col - 1, row)) ? 1 : 0;
                roleCount += HasRole(grid, new Point(col - 1, row + 1)) ? 1 : 0;
                
                roleCount += HasRole(grid, new Point(col, row - 1)) ? 1 : 0;
                roleCount += HasRole(grid, new Point(col, row + 1)) ? 1 : 0;
                
                roleCount += HasRole(grid, new Point(col + 1, row - 1)) ? 1 : 0;
                roleCount += HasRole(grid, new Point(col + 1, row)) ? 1 : 0;
                roleCount += HasRole(grid, new Point(col + 1, row + 1)) ? 1 : 0;

                if (roleCount < 4)
                {
                    grid[row, col] = 'x';
                }
            }
        }
    }

    private bool HasRole(char[,] grid, Point pos)
    {
        if (pos.Y < 0) return false;
        if (pos.X < 0) return false;
        if (pos.Y >= grid.GetLength(0)) return false;
        if (pos.X >= grid.GetLength(1)) return false;
        
        return grid[pos.Y, pos.X] != '.';
    }

    private int CountMarkers(char[,] grid)
    {
        var counter = 0;
        
        for (var row = 0; row < grid.GetLength(0); row++)
        {
            for (var col = 0; col < grid.GetLength(1); col++)
            {
                if (grid[row, col] == 'x') counter++;
            }
        }
        
        return counter;
    }
    
    private void RemoveMarkers(char[,] grid)
    {
        for (var row = 0; row < grid.GetLength(0); row++)
        {
            for (var col = 0; col < grid.GetLength(1); col++)
            {
                if (grid[row, col] == 'x')
                {
                    grid[row, col] = '.';
                }
            }
        }
    }
}