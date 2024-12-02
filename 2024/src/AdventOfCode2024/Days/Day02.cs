namespace AdventOfCode2024.Days;

public class Day02 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var lines = Input.Value.SplitByLine().Select(e => e.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(e => int.Parse(e)).ToList()).ToList();

        int total = lines.Sum(IsSafe);

        return new ValueTask<string>(total.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var lines = Input.Value.SplitByLine().Select(e => e.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(e => int.Parse(e)).ToList()).ToList();

        int total = lines.Sum(IsSafeWithTolerance);

        return new ValueTask<string>(total.ToString());
    }

    private static int IsSafe(List<int> list)
    {
        var isAscending = list[0] < list[1];

        for(var i = 0; i < list.Count - 1; i++)
        {
            var distance = isAscending ? list[i + 1] - list[i] : list[i] - list[i + 1];

            if(distance < 1 || distance > 3)
            {
                return 0;
            }
        }

        return 1;
    }

    private static int IsSafeWithTolerance(List<int> list)
    {
        if (IsSafe(list) == 1)
        {
            return 1;
        }

        for(int i = 0; i < list.Count; i++)
        {
            var coppy = new List<int>(list);
            coppy.RemoveAt(i);
            if (IsSafe(coppy) == 1)
            {
                return 1;
            }
        }

        return 0;
    }
}
