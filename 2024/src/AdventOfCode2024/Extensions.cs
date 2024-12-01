namespace AdventOfCode2024;

public static class Extensions
{
    public static int Multiply(this IEnumerable<int> items) => items.Aggregate(1, (a, b) => a * b);

    public static string[] SplitByLine(this string input) => input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

    public static string[] SplitByWhitespace(this string input) => input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    public static string[] SplitByGroup(this string input) => input.Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries);

    public static char[][] ToGrid(this string input) => input.SplitByLine().Select(s => s.ToCharArray()).ToArray();

    public static int[][] ToIntGrid(this string input) => input.SplitByLine().Select(s => s.ToCharArray().Where(char.IsDigit).Select(c => c - '0').ToArray()).ToArray();
}
