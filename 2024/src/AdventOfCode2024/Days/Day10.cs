using Spectre.Console;
using System.Reflection;

namespace AdventOfCode2024.Days;

public class Day10 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var map = ParseInputToDisk(Input.Value);

        var results = new List<List<(int y, int x)>>();
        for (var y = 0; y < map.Count; y++)
        {
            for(var x = 0; x < map[y].Count; x++)
            {
                if (map[y][x] == 0)
                {
                    MoveGrid(map, new List<(int y, int x)>{(y, x)}, 0, results);
                }
            }
        }

        var total = results
           .Select(list => (First: list.First(), Last: list.Last()))
           .Distinct()
           .Count();
        return new ValueTask<string>(total.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var map = ParseInputToDisk(Input.Value);

        var results = new List<List<(int y, int x)>>();
        for (var y = 0; y < map.Count; y++)
        {
            for (var x = 0; x < map[y].Count; x++)
            {
                if (map[y][x] == 0)
                {
                    MoveGrid(map, new List<(int y, int x)> { (y, x) }, 0, results);
                }
            }
        }

        var total = results
           .Distinct()
           .Count();
        return new ValueTask<string>(total.ToString());
    }

    private static void MoveGrid(List<List<int>> map, List<(int y, int x)> path, int current, List<List<(int y, int x)>> results)
    {
        for(int i = 0; i < 4; i++)
        {
            var direction = i switch
            {
                0 => Direction.Up,
                1 => Direction.Down,
                2 => Direction.Left,
                _ => Direction.Right
            };

            var (yChange, xChange) = GetDirectionChange(direction);
            var newY = path.Last().y + yChange;
            var newX = path.Last().x + xChange;
            var newLastValue = current + 1;
            var newPath = new List<(int y, int x)>(path);
            newPath.Add((newY, newX));

            if(newLastValue == 9 && IsWithinBounds(newY, newX, map) && map[newY][newX] == 9)
            {
                results.Add(newPath);
                continue;
            }

            if (IsWithinBounds(newY, newX, map) && map[newY][newX] == newLastValue)
            {
                MoveGrid(map, newPath, newLastValue, results);
            }
        }
    }

    private static bool IsWithinBounds(int y, int x, List<List<int>> grid)
    {
        return y >= 0 && y < grid.Count && x >= 0 && x < grid[0].Count;
    }

    private static (int yChange, int xChange) GetDirectionChange(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return (-1, 0);
            case Direction.Down:
                return (1, 0);
            case Direction.Left:
                return (0, -1);
            case Direction.Right:
                return (0, 1);
            default:
                throw new Exception("");
        }
    }

    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    private static List<List<int>> ParseInputToDisk(string input)
    {
        return input.SplitByLine().Select(x => x.ToCharArray().Select(e => int.Parse($"{e}")).ToList()).ToList();
    }
}
