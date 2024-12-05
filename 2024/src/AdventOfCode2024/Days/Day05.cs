using System.Transactions;

namespace AdventOfCode2024.Days;

using Spectre.Console;
using System.Text.RegularExpressions;

public class Day05 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var (configurations, pages) = ParseInput(Input.Value);

        int total = pages
            .Where(page => CheckConfigurationViolation(page, configurations) == null)
            .Sum(page => page[GetMiddle(page.Count)]);

        return new ValueTask<string>(total.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var (configurations, pages) = ParseInput(Input.Value);

        int total = pages
            .Where(page => CheckConfigurationViolation(page, configurations) != null)
            .Sum(page =>
            {
                var correctedPage = CorrectPage(page, configurations);
                return correctedPage[GetMiddle(correctedPage.Count)];
            });

        return new ValueTask<string>(total.ToString());
    }

    private static (List<KeyValuePair<int, int>> Configurations, List<List<int>> Pages) ParseInput(string input)
    {
        var parts = input.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);

        var configurations = parts[0]
            .SplitByLine()
            .Select(line => line.Split("|").Select(int.Parse).ToArray())
            .Select(arr => new KeyValuePair<int, int>(arr[0], arr[1]))
            .ToList();

        var pages = parts[1]
            .SplitByLine()
            .Select(line => line.Split(",").Select(int.Parse).ToList())
            .ToList();

        return (configurations, pages);
    }

    private static int GetMiddle(int count) => (count - 1) / 2;

    private static (int , KeyValuePair<int, int>)? CheckConfigurationViolation(List<int> page, List<KeyValuePair<int, int>> configurations)
    {
        for (int i = 0; i < page.Count; i++)
        {
            var current = page[i];
            var currentConfigs = configurations.Where(e => e.Key == current || e.Value == current);

            foreach (var config in currentConfigs)
            {
                var index = i;
                int configIndex = current == config.Key ? page.FindIndex(e => e == config.Value) : page.FindIndex(e => e == config.Key);

                if (configIndex != -1 &&
                    ((current == config.Key && index > configIndex) ||
                     (current == config.Value && configIndex > index)))
                {
                    return (current ,config);
                }
            }
        }

        return null;
    }

    private static List<int> CorrectPage(List<int> page, List<KeyValuePair<int, int>> configurations)
    {
        var correctedPage = new List<int>(page);

        while (true)
        {
            var violation = CheckConfigurationViolation(correctedPage, configurations);
            if (violation == null)
                break;

            int current = violation.Value.Item1;
            correctedPage.Remove(current);

            var rule = violation.Value.Item2;
            int newIndex = rule.Key == current
                ? correctedPage.IndexOf(rule.Value)
                : correctedPage.IndexOf(rule.Key) + 1;

            correctedPage.Insert(newIndex, current);
        }

        return correctedPage;
    }
}
