using System.Security.Cryptography.X509Certificates;
using System.Transactions;

namespace AdventOfCode2024.Days;

using Spectre.Console;
using System.Text.RegularExpressions;

public class Day06 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var grid = Input.Value.SplitByLine().Select(e => e.ToCharArray().ToList()).ToList();

        var startX = 0;
        var startY = 0;
        List<List<bool>> walkHistory = new List<List<bool>>();
        for (int y = 0; y < grid.Count; y++)
        {
            List<bool> newLine = new List<bool>();
            for (int x = 0; x < grid[0].Count; x++)
            {
                var currentChar = grid[y][x];

                if (currentChar == '^')
                {
                    startX = x;
                    startY = y;

                    newLine.Add(true);
                    continue;
                }

                newLine.Add(false);
            }
            walkHistory.Add(newLine);
        }

        WalkTroughGrid(grid, walkHistory, startY, startX);

        var total = walkHistory.Sum(e => e.Count(g => g));
        return new ValueTask<string>(total.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var grid = Input.Value.SplitByLine().Select(e => e.ToCharArray().ToList()).ToList();

        var startX = 0;
        var startY = 0;
        List<List<bool>> walkHistory = new List<List<bool>>();
        for (int y = 0; y < grid.Count; y++)
        {
            List<bool> newLine = new List<bool>();
            for (int x = 0; x < grid[0].Count; x++)
            {
                var currentChar = grid[y][x];

                if (currentChar == '^')
                {
                    startX = x;
                    startY = y;

                    newLine.Add(true);
                    continue;
                }

                newLine.Add(false);
            }
            walkHistory.Add(newLine);
        }

        var total = 0;
        for (int y = 0; y < grid.Count; y++)
        {
            for (int x = 0; x < grid[0].Count; x++)
            {
                if (x != startX || y != startY)
                {
                    List<List<char>> coppyGrid = grid.Select(row => new List<char>(row)).ToList();
                    coppyGrid[y][x] = '#';
                    List<List<bool>> walkHistoryCoppy = walkHistory.Select(row => new List<bool>(row)).ToList();

                    if (WalkTroughGrid(coppyGrid, walkHistoryCoppy, startY, startX, true))
                    {
                        total++;
                    }
                }
            }
        }

        return new ValueTask<string>(total.ToString());
    }

    private static bool WalkTroughGrid(List<List<char>> grid, List<List<bool>> walkHistory, int startY, int startX, bool part2 = false)
    {
        var direction = Direction.Up;
        var y = startY;
        var x = startX;

        var path = new HashSet<string>();

        while (true)
        {
            var check = CheckNextStep(grid, ref y,ref x, direction);
            if (check == State.OutOfBound)
            {
                break;
            }

            if (check == State.Free)
            {
                if (!part2)
                {
                    walkHistory[y][x] = true;
                }
                else
                {
                    var current = $"{y}, {x}, {direction}";
                    if (path.Contains(current))
                    {
                        return true;
                    }

                    path.Add(current);
                }
            }
            else
            {
                direction = TurnRight(direction);
            }
        }

        return false;
    }

    private static Direction TurnRight(Direction oldDirection)
    {
        switch (oldDirection)
        {
            case Direction.Up:
                return Direction.Right;
            case Direction.Down:
                return Direction.Left;
            case Direction.Left:
                return Direction.Up;
            case Direction.Right:
                return Direction.Down;
        }

        throw new Exception();
    }

    private static State CheckNextStep(List<List<char>> grid, ref int y, ref int x, Direction direction)
    {
        var (newY, newX) = GetDirectionChange(direction);
        newY += y;
        newX += x;

        if (grid.Count <= newY || newY < 0 ||
            grid[0].Count <= newX || newX < 0)
        {
            return State.OutOfBound;
        }
        if (grid[newY][newX] == '#')
        {
            return State.Wall;
        }

        y = newY;
        x = newX;
        return State.Free;
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
        }

        throw new Exception();
    }

    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    private enum State
    {
        OutOfBound,
        Free,
        Wall,
        Loop
    }
}
