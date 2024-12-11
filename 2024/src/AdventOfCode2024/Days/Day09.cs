namespace AdventOfCode2024.Days;

public class Day09 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var disk = ParseInputToDisk(Input.Value);

        while(true)
        {
            var dotIndex = disk.IndexOf(".");
            var numberIndex = GetLastIndexOfNot(".", disk);

            if(numberIndex < dotIndex)
            {
                // sorted
                break;
            }

            Swap(disk ,dotIndex, numberIndex);
        }

        var total = Checksum(disk);
        return new ValueTask<string>(total.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var disk = ParseInputToDisk(Input.Value);

        var index = disk.Count - 1;
        while (true)
        {
            if(index < 0)
            {
                break;
            }

            var dotIndex = 0;
            while (true)
            {
                var (dotStartIndex, dotEndIndex) = GetIndexOfPartAfterIndex(".", disk, dotIndex);
                var (numberStartIndex, numberEndIndex) = GetLastIndexOfNotPartBeforeIndex(".", disk, index);

                if(index < dotIndex ||
                   numberStartIndex < dotStartIndex)
                {
                    // no match found skipping
                    index = numberStartIndex - 1;
                    break;
                }

                var numberCount = numberEndIndex - numberStartIndex + 1;
                var dotCount = dotEndIndex - dotStartIndex + 1;
                if (numberCount <= dotCount)
                {
                    SwapSegments(disk, numberStartIndex, dotStartIndex, numberCount);
                    index = numberStartIndex - 1;
                    break;
                }

                dotIndex = dotEndIndex + 1;
            }
        }

        var total = Checksum(disk);
        return new ValueTask<string>(total.ToString());
    }

    static long Checksum(List<string> disk)
    {
        long total = 0;
        for (int i = 0; i < disk.Count; i++)
        {
            if (disk[i] != ".")
            {
                total += long.Parse(disk[i]) * i;
            }
        }

        return total;
    }

    static (int startIndex, int endIndex) GetIndexOfPartAfterIndex(string value, List<string> disk, int afterIndex)
    {
        var startIndex = -1;
        var endIndex = -1;

        for (int i = afterIndex; i < disk.Count; i++)
        {
            if (disk[i] == value && startIndex == -1)
            {
                startIndex = i;
            }
            if (disk[i] != value && startIndex != -1)
            {
                endIndex = i - 1;
                break;
            }
        }

        return (startIndex, endIndex);
    }

    static (int startIndex, int endIndex) GetLastIndexOfNotPartBeforeIndex(string value, List<string> disk, int beforeIndex)
    {
        var startIndex = -1;
        var endIndex = -1;

        string unknown = "";
        for (int i = beforeIndex; i >= 0; i--)
        {
            if (disk[i] != value && unknown == "")
            {
                unknown = disk[i];
                endIndex = i;
            }
            if (disk[i] != unknown && unknown != "")
            {
                startIndex = i + 1;
                break;
            }
        }

        return (startIndex, endIndex);
    }

    static int GetLastIndexOfNot(string value, List<string> disk)
    {
        for (int i = disk.Count - 1; i >= 0; i--)
        {
            if (disk[i] != value)
            {
                return i;
            }
        }
        return -1;
    }

    private static void SwapSegments(List<string> list, int numberStartIndex, int dotStartIndex, int length)
    {
        for(int i = 0; i < length; i++)
        {
            var test = list[numberStartIndex + i];
            var test2 = list[dotStartIndex + i];
            Swap(list, numberStartIndex + i, dotStartIndex + i);
        }
    }

    private static void Swap(List<string> list, int index1, int index2)
    {
        string temp = list[index1];
        list[index1] = list[index2];
        list[index2] = temp;
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
