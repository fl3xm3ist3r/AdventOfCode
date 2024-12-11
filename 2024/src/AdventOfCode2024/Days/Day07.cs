using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Days;

public class Day07 : DayBase
{
    public override ValueTask<string> Solve_1()
    {

        var x = 32;
        string comb = Convert.ToString(x, 2).PadLeft(5, '0');

        var total = 0;
        return new ValueTask<string>(total.ToString());
    }

   

    //private static (int yOffset, int xOffset) GetDirectionOffset(Direction direction)
    //{

    //}
}
