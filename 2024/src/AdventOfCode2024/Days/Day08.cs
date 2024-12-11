using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Days;

public class Day08 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var grid = Input.Value.SplitByLine().Select(e => e.ToCharArray().ToList()).ToList();
        var values = GetValues(grid);

        var nodes = new List<(int y, int x)>();
        foreach (var value in values)
        {
            var sameValues = values.Where(e => e.c == value.c && !e.Equals(value)).ToList();

            foreach (var sameValue in sameValues)
            {
                (int y, int x) = GetStreckungAfterFirstPointByMultiplier(value.y, value.x, sameValue.y, sameValue.x);
                if (IsWithinBounds(y, x, grid))
                {
                    nodes.Add((y, x));
                }
            }
        }

        int total = nodes.Distinct().Count();
        return new ValueTask<string>(total.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var grid = Input.Value.SplitByLine().Select(e => e.ToCharArray().ToList()).ToList();
        var values = GetValues(grid);

        var nodes = new List<(int y, int x)>();
        foreach (var value in values)
        {
            var sameValues = values.Where(e => e.c == value.c && !e.Equals(value)).ToList();

            foreach (var sameValue in sameValues)
            {
                for(int multiplier = 1; multiplier > 0; multiplier++)
                {
                    (int y, int x) = GetStreckungAfterFirstPointByMultiplier(value.y, value.x, sameValue.y, sameValue.x, multiplier);
                    if (IsWithinBounds(y, x, grid))
                    {
                        nodes.Add((y, x));
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        int total = nodes.Concat(values.Select(e => (e.y, e.x)).ToList()).Distinct().Count();
        return new ValueTask<string>(total.ToString());
    }

    private static (int y3, int x3) GetStreckungAfterFirstPointByMultiplier(int y1, int x1, int y2, int x2, int multiplier = 1)
    {
        int dx = x1 - x2;
        int dy = y1 - y2;
        int x3 = x1 + multiplier * dx;
        int y3 = y1 + multiplier * dy;
        return (y3, x3);
    }

    private static bool IsWithinBounds(int y, int x, List<List<char>> grid)
    {
        return y >= 0 && y < grid.Count && x >= 0 && x < grid[0].Count;
    }

    private static List<(char c, int y, int x)> GetValues(List<List<char>> grid)
    {
        var values = new List<(char, int, int)>();
        for (var y = 0; y < grid.Count; y++)
        {
            for (var x = 0; x < grid[y].Count; x++)
            {
                if (grid[y][x] != '.')
                {
                    values.Add((grid[y][x], y, x));
                }
            }
        }

        return values;
    }
}
