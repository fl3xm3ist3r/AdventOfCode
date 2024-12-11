using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Days;

public class Day07 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var calculations = ParseInput(Input.Value);

        long result = 0;

        foreach (var calculation in calculations)
        {
            var combinations = GetCombinations(calculation.numbers.Count, 2);

            foreach (var combination in combinations)
            {
                if(IsCorrectCalculation(calculation.result, calculation.numbers, combination))
                {
                    result += calculation.result;
                    break;
                }
            }
        }

        return new ValueTask<string>(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var calculations = ParseInput(Input.Value);

        long result = 0;

        foreach (var calculation in calculations)
        {
            var combinations = GetCombinations(calculation.numbers.Count, 3);

            foreach (var combination in combinations)
            {
                if (IsCorrectCalculation(calculation.result, calculation.numbers, combination))
                {
                    result += calculation.result;
                    break;
                }
            }
        }

        return new ValueTask<string>(result.ToString());
    }

    private static bool IsCorrectCalculation(long result, List<long> numbers, string combination)
    {
        long calculatedResult = 0;

        for (var i = 0; i < numbers.Count; i++)
        {
            switch (combination[i] - '0')
            {
                case 0:
                    calculatedResult += numbers[i];
                    break;
                case 1:
                    calculatedResult *= numbers[i];
                    break;
                case 2:
                    calculatedResult = long.Parse($"{calculatedResult}{numbers[i]}");
                    break;
                default:
                    throw new InvalidOperationException("Invalid operation");
            }
        }

        return result == calculatedResult;
    }

    private static List<string> GetCombinations(int listCount, int insertCharCount)
    {
        var possibilities = Math.Pow(insertCharCount, listCount);
        var combinations = new List<string>();
        for(long i = 0; i < possibilities; i++)
        {
            combinations.Add(ConvertToBase(i, listCount, insertCharCount));
        }

        return combinations;
    }

    private static string ConvertToBase(long value, int length, int baseNumber)
    {
        var result = new StringBuilder();

        while (value > 0)
        {
            result.Insert(0, value % baseNumber);
            value /= baseNumber;
        }

        while (result.Length < length)
        {
            result.Insert(0, '0');
        }

        return result.ToString();
    }

    private static List<(long result, List<long> numbers)> ParseInput(string input)
    {
        return input.SplitByLine().Select(e =>
        {
            var result = long.Parse(e.Split(':')[0]);
            var numbers = e.Split(":")[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(e => long.Parse(e)).ToList();

            return (result, numbers);
        }).ToList();
    }
}
