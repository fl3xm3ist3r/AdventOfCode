namespace AdventOfCode2024.Days;

public class Day01 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var (first, second) = SplitPairs(Input.Value.SplitByLine());

        first.Sort();
        second.Sort();

        int total = 0;
        for (int i = 0; i < first.Count; i++)
        {
            total += Math.Abs(first[i] - second[i]);
        }

        return new ValueTask<string>(total.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var (first, second) = SplitPairs(Input.Value.SplitByLine());

        var grouped = second.GroupBy(x => x).Select(g => new { Number = g.Key, Count = g.Count() });

        var total = first.Sum(f => f * (grouped.FirstOrDefault(e => e.Number == f)?.Count ?? 0));

        return new ValueTask<string>(total.ToString());
    }

    private static (List<int> First, List<int> Second) SplitPairs(IEnumerable<string> lines)
    {
        var first = new List<int>();
        var second = new List<int>();
        foreach (var line in lines)
        {
            var numbers = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            first.Add(int.Parse(numbers[0]));
            second.Add(int.Parse(numbers[1]));
        }
        return (first, second);
    }
}
