namespace AdventOfCode2024.Days;

using Spectre.Console;
using System.Text.RegularExpressions;

public class Day04 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var grid = Input.Value.SplitByLine().Select(e => e.ToCharArray().ToList()).ToList();

        var searchString = "XMAS";

        var total = 0;
        for(int y = 0; y < grid.Count; y++)
        {
            for (int x = 0; x < grid[0].Count; x++)
            {
                if (grid[y][x] == searchString[0])
                {
                    total += GetXmas(y, x, grid, searchString);
                }
            }
        }

        return new ValueTask<string>(total.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var grid = Input.Value.SplitByLine().Select(e => e.ToCharArray().ToList()).ToList();

        var total = 0;
        for (int y = 0; y < grid.Count; y++)
        {
            for (int x = 0; x < grid[0].Count; x++)
            {
                if (grid[y][x] == 'A')
                {
                    total += GetMas(y, x, grid);
                }
            }
        }

        return new ValueTask<string>(total.ToString());
    }

    private static int GetXmas(int y, int x, List<List<char>> grid, string searchString)
    {
        var directions = new (int changeY, int changeX)[]
        {
                (0, 1),   // Forward
                (0, -1),  // Backward
                (1, 0),   // Down
                (-1, 0),  // Up
                (1, -1),  // Diagonal Down Left
                (1, 1),   // Diagonal Down Right
                (-1, -1), // Diagonal Up Left
                (-1, 1)   // Diagonal Up Right
        };

        var occurences = 0;
        foreach (var (changeY, changeX) in directions)
        {
            occurences += CheckXmas(y, x, changeY, changeX, grid, searchString);
        }

        return occurences;
    }

    private static int CheckXmas(int y, int x, int changeY, int changeX, List<List<char>> grid, string searchString)
    {
        for (int z = 1; z < searchString.Length; z++)
        {
            y += changeY;
            x += changeX;

            if (y < 0 || y >= grid.Count ||
                x < 0 || x >= grid[0].Count ||
                grid[y][x] != searchString[z])
            {
                return 0;
            }
        }

        return 1;
    }

    private static int GetMas(int y, int x, List<List<char>> grid)
    {
        if (0 <= y - 1 && y + 1 < grid.Count &&
            0 <= x - 1 && x + 1 < grid[0].Count &&
            (grid[y - 1][x - 1] == 'M' && grid[y + 1][x + 1] == 'S' ||
            (grid[y - 1][x - 1] == 'S' && grid[y + 1][x + 1] == 'M')) &&
            (grid[y - 1][x + 1] == 'M' && grid[y + 1][x - 1] == 'S' ||
            (grid[y - 1][x + 1] == 'S' && grid[y + 1][x - 1] == 'M')))
        {
            return 1;
        }

        return 0;
    }
}
