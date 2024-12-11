namespace AdventOfCode2024.Days;

public class Day06 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var (grid, startX, startY, walkHistory) = InitializeGridAndHistory(Input.Value);

        TraverseGrid(grid, walkHistory, startY, startX);

        var total = walkHistory.Sum(row => row.Count(cell => cell));
        return new ValueTask<string>(total.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var (grid, startX, startY, walkHistory) = InitializeGridAndHistory(Input.Value);
        var total = 0;

        for (int y = 0; y < grid.Count; y++)
        {
            for (int x = 0; x < grid[0].Count; x++)
            {
                if (x == startX && y == startY) continue;

                var modifiedGrid = CopyGrid(grid);
                modifiedGrid[y][x] = '#';

                var walkHistoryCopy = CopyWalkHistory(walkHistory);

                if (TraverseGrid(modifiedGrid, walkHistoryCopy, startY, startX, true))
                {
                    total++;
                }
            }
        }

        return new ValueTask<string>(total.ToString());
    }

    private static (List<List<char>> grid, int startX, int startY, List<List<bool>> walkHistory) InitializeGridAndHistory(string input)
    {
        var grid = input.SplitByLine()
            .Select(line => line.ToCharArray().ToList())
            .ToList();

        var startX = 0;
        var startY = 0;
        var walkHistory = new List<List<bool>>();

        for (int y = 0; y < grid.Count; y++)
        {
            var newLine = new List<bool>();
            for (int x = 0; x < grid[0].Count; x++)
            {
                if (grid[y][x] == '^')
                {
                    startX = x;
                    startY = y;
                    newLine.Add(true);
                }
                else
                {
                    newLine.Add(false);
                }
            }
            walkHistory.Add(newLine);
        }

        return (grid, startX, startY, walkHistory);
    }

    private static List<List<char>> CopyGrid(List<List<char>> grid) =>
        grid.Select(row => new List<char>(row)).ToList();

    private static List<List<bool>> CopyWalkHistory(List<List<bool>> walkHistory) =>
        walkHistory.Select(row => new List<bool>(row)).ToList();

    private static bool TraverseGrid(List<List<char>> grid, List<List<bool>> walkHistory, int startY, int startX, bool part2 = false)
    {
        var direction = Direction.Up;
        var y = startY;
        var x = startX;
        var visitedPaths = new HashSet<string>();

        while (true)
        {
            var state = GetNextState(grid, ref y, ref x, direction);

            if (state == State.OutOfBound) break;

            if (state == State.Free)
            {
                if (!part2)
                {
                    walkHistory[y][x] = true;
                }
                else
                {
                    var currentPath = $"{y},{x},{direction}";
                    if (visitedPaths.Contains(currentPath)) return true;

                    visitedPaths.Add(currentPath);
                }
            }
            else
            {
                direction = TurnRight(direction);
            }
        }

        return false;
    }

    private static Direction TurnRight(Direction direction) =>
        direction switch
        {
            Direction.Up => Direction.Right,
            Direction.Right => Direction.Down,
            Direction.Down => Direction.Left,
            Direction.Left => Direction.Up,
            _ => throw new InvalidOperationException("Invalid direction")
        };

    private static State GetNextState(List<List<char>> grid, ref int y, ref int x, Direction direction)
    {
        var (dy, dx) = GetDirectionOffset(direction);
        var newY = y + dy;
        var newX = x + dx;

        if (newY < 0 || newY >= grid.Count || newX < 0 || newX >= grid[0].Count)
        {
            return State.OutOfBound;
        }

        if (grid[newY][newX] == '#') return State.Wall;

        y = newY;
        x = newX;
        return State.Free;
    }

    private static (int yOffset, int xOffset) GetDirectionOffset(Direction direction) =>
        direction switch
        {
            Direction.Up => (-1, 0),
            Direction.Right => (0, 1),
            Direction.Down => (1, 0),
            Direction.Left => (0, -1),
            _ => throw new InvalidOperationException("Invalid direction")
        };

    private enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    private enum State
    {
        OutOfBound,
        Free,
        Wall
    }
}
