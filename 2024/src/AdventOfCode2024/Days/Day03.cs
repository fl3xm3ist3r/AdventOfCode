namespace AdventOfCode2024.Days;
using System.Text.RegularExpressions;

public class Day03 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        string pattern = @"mul\((\d{1,3}),(\d{1,3})\)";

        MatchCollection matches = Regex.Matches(Input.Value, pattern);

        var result = matches.Sum(e => int.Parse(e.Groups[1].Value) * int.Parse(e.Groups[2].Value));


        return new ValueTask<string>(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        string pattern = @"(mul\((\d{1,3}),(\d{1,3})\)|do\(\)|don't\(\))";

        MatchCollection matches = Regex.Matches(Input.Value, pattern);

        var result = 0;
        var enabled = true;

        foreach (Match match in matches)
        {
            switch (match.Value)
            {
                case "do()":
                    enabled = true; break;
                case "don't()":
                    enabled = false; break;
                default:
                    if(enabled)
                    {
                        result += int.Parse(match.Groups[2].Value) * int.Parse(match.Groups[3].Value);
                    }
                    break;
            }
        }

        return new ValueTask<string>(result.ToString());
    }
}
