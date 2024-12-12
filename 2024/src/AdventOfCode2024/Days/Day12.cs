using Spectre.Console;
using System.Net;
using System.Text;

namespace AdventOfCode2024.Days;

public class Day12 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var grid = ParseInputToGrid(Input.Value);

        var regions = GetRegions(grid);

        var total = 0;
        foreach (var region in regions)
        {
            var regionPerimeterCount = 0;
            foreach (var coordinate in region.coordinates)
            {
                regionPerimeterCount += GetPerimeter(grid, coordinate.y, coordinate.x, region.current);
            }

            total += region.coordinates.Count * regionPerimeterCount;
        }

        return new ValueTask<string>(total.ToString());
    }


    private static List<(char current, List<(int y, int x)> coordinates)> GetRegions(List<List<char>> grid)
    {
        var regions = new List<(char c, List<(int y, int x)> coordinates)>();

        for (var y = 0; y < grid.Count; y++)
        {
            for (var x = 0; x < grid[y].Count; x++)
            {
                var current = grid[y][x];
                if(!regions.Where(region => region.c == current).Any(region => region.coordinates.Contains((y, x))))
                {
                    var regionCoordinates = new List<(int y, int x)>{(y, x)};
                    GetRegion(grid, y, x, current, regionCoordinates);
                    regions.Add((current, regionCoordinates));
                }
            }
        }

        return regions;
    }

    private static void GetRegion(List<List<char>> grid, int y, int x, char current, List<(int y, int x)> coordinates)
    {
        for (int i = 0; i < 4; i++)
        {
            var direction = i switch
            {
                0 => Direction.Up,
                1 => Direction.Down,
                2 => Direction.Left,
                _ => Direction.Right
            };

            var (yChange, xChange) = GetDirectionChange(direction);
            var newY = y + yChange;
            var newX = x + xChange;

            if (!coordinates.Any(e => e.y == newY && e.x == newX) && IsWithinBounds(newY, newX, grid) && grid[newY][newX] == current)
            {
                coordinates.Add((newY, newX));
                GetRegion(grid, newY, newX, current, coordinates);
            }
        }
    }

    private static int GetPerimeter(List<List<char>> grid, int y, int x, char current)
    {
        var perimeterCount = 0;
        for (int i = 0; i < 4; i++)
        {
            var direction = i switch
            {
                0 => Direction.Up,
                1 => Direction.Down,
                2 => Direction.Left,
                _ => Direction.Right
            };

            var (yChange, xChange) = GetDirectionChange(direction);
            var newY = y + yChange;
            var newX = x + xChange;

            if (!IsWithinBounds(newY, newX, grid) ||  grid[newY][newX] != current)
            {
                perimeterCount++;
            }
        }

        return perimeterCount;
    }

    private static bool IsWithinBounds(int y, int x, List<List<char>> grid)
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

    private static List<List<char>> ParseInputToGrid(string input)
    {
        return input.SplitByLine().Select(x => x.ToCharArray().ToList()).ToList();
    }
}
