namespace AdventOfCode2024.Days;

public class Day09 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var disk = ParseInputToDisk(Input.Value);

        while (true)
        {
            var dotIndex = disk.IndexOf(".");
            var numberIndex = GetLastIndexOfNot(".", disk);

            if (numberIndex < dotIndex)
            {
                break; // Sorted
            }

            Swap(disk, dotIndex, numberIndex);
        }

        var total = Checksum(disk);
        return new ValueTask<string>(total.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var disk = ParseInputToDisk(Input.Value);
        int index = disk.Count - 1;

        while (index >= 0)
        {
            for (int dotIndex = 0; dotIndex <= index;)
            {
                var (dotStart, dotEnd) = GetIndexOfPartAfterIndex(".", disk, dotIndex);
                var (numStart, numEnd) = GetLastIndexOfNotPartBeforeIndex(".", disk, index);

                if (numStart < 0 || dotStart < 0 || numStart < dotStart)
                {
                    index = numStart - 1;
                    break;
                }

                int numberLength = numEnd - numStart + 1;
                int dotLength = dotEnd - dotStart + 1;

                if (numberLength <= dotLength)
                {
                    SwapSegments(disk, numStart, dotStart, numberLength);
                    index = numStart - 1;
                    break;
                }

                dotIndex = dotEnd + 1;
            }
        }

        var total = Checksum(disk);
        return new ValueTask<string>(total.ToString());
    }

    private static long Checksum(List<string> disk)
    {
        return disk
            .Select((value, index) => value == "." ? 0 : long.Parse(value) * index)
            .Sum();
    }

    private static (int startIndex, int endIndex) GetIndexOfPartAfterIndex(string value, List<string> disk, int afterIndex)
    {
        int startIndex = disk.FindIndex(afterIndex, v => v == value);

        if (startIndex == -1)
        {
            return (-1, -1);
        }

        int endIndex = disk.FindIndex(startIndex, v => v != value);
        return endIndex == -1 ? (startIndex, disk.Count - 1) : (startIndex, endIndex - 1);
    }

    private static (int startIndex, int endIndex) GetLastIndexOfNotPartBeforeIndex(string value, List<string> disk, int beforeIndex)
    {
        int endIndex = disk.FindLastIndex(beforeIndex, v => v != value);

        if (endIndex == -1)
        {
            return (-1, -1);
        }

        string matchValue = disk[endIndex];
        int startIndex = disk.FindLastIndex(endIndex, v => v != matchValue) + 1;
        return (startIndex, endIndex);
    }

    private static int GetLastIndexOfNot(string value, List<string> disk)
    {
        return disk.FindLastIndex(v => v != value);
    }

    private static void SwapSegments(List<string> list, int start1, int start2, int length)
    {
        for (int i = 0; i < length; i++)
        {
            Swap(list, start1 + i, start2 + i);
        }
    }

    private static void Swap(List<string> list, int index1, int index2)
    {
        (list[index1], list[index2]) = (list[index2], list[index1]);
    }

    private static List<string> ParseInputToDisk(string Input)
    {
        var disk = new List<string>();
        var isNumber = true;
        var count = 0;

        foreach (char value in Input)
        {
            for (int x = 0; x < value - '0'; x++)
            {
                disk.Add(isNumber ? count.ToString() : ".");
            }
            if (isNumber)
            {
                count++;
            }
            isNumber = !isNumber;
        }

        return disk;
    }
}
