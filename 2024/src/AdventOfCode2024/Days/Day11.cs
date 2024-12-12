using System.Text;

namespace AdventOfCode2024.Days;

public class Day11 : DayBase
{
    public static Dictionary<string, long> cache = new Dictionary<string, long>();

    public override ValueTask<string> Solve_1()
    {
        long stoneCount = GetNumberCountAfterIterations(Input.Value.SplitByLine()[0], 25);
        long stoneCount2 = Input.Value.SplitByLine()[0].SplitByWhitespace().ToList().Sum(e => Recursion(long.Parse(e), 25));
        return new ValueTask<string>(stoneCount.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        long stoneCount = Input.Value.SplitByLine()[0].SplitByWhitespace().ToList().Sum(e => Recursion(long.Parse(e), 75));
        return new ValueTask<string>(stoneCount.ToString());
    }

    private static long Recursion(long input, int iterations)
    {
        var inputString = input.ToString();
        var key = input + "_" + iterations.ToString();
        if (cache.TryGetValue(key, out long value)) {
            return value;
        }

        long stoneCount = 0;

        if(iterations == 0)
        {
            return 1;
        }
        if (input == 0)
        {
            stoneCount = Recursion(1, iterations - 1);
        }
        else
        {
            int length = inputString.Length;
            if (length % 2 == 0)
            {
                int mid = length / 2;
                var leftPart = long.Parse(inputString[..mid]);
                var rightPart = long.Parse(inputString[mid..]);

                stoneCount = Recursion(leftPart, iterations - 1) + Recursion(rightPart, iterations - 1);

            }
            else
            {
                stoneCount = Recursion(input * 2024, iterations - 1);
            }
        }

        cache.Add(key, stoneCount);

        return stoneCount;
    }

    private static long GetNumberCountAfterIterations(string input, int iterations)
    {
        for (var x = 0; x < iterations; x++)
        {
            var newInput = new StringBuilder();
            foreach (var entry in input.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                if (entry == "0")
                {
                    newInput.Append("1 ");
                }
                else
                {
                    int length = entry.Length;
                    if (length % 2 == 0)
                    {
                        int mid = length / 2;
                        var leftPart = entry[..mid].TrimStart('0');
                        var rightPart = entry[mid..].TrimStart('0');

                        if (string.IsNullOrEmpty(leftPart)) leftPart = "0";
                        if (string.IsNullOrEmpty(rightPart)) rightPart = "0";

                        newInput.Append(leftPart).Append(' ').Append(rightPart).Append(' ');
                    }
                    else
                    {
                        newInput.Append(long.Parse(entry) * 2024).Append(' ');
                    }
                }
            }
            input = newInput.ToString().Trim();
        }

        return input.Count(c => c == ' ') + 1;
    }
}
