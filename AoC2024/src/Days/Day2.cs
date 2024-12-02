using System.Text.RegularExpressions;
using Nixill.Utils.Extensions;

namespace Nixill.AdventOfCode;

public class Day2 : AdventDay
{
  int SafeLines = 0;

  public override void Run()
  {
    foreach (string line in InputStream.GetLines())
    {
      bool? up = null;
      foreach (var mtcs in Regexes.Number.Matches(line).Pairs())
      {
        int left = int.Parse(mtcs.Item1.Value);
        int right = int.Parse(mtcs.Item2.Value);

        if (left > right)
        {
          if (up == true) goto nextLine;
          if (left - right > 3) goto nextLine;
          up = false;
        }
        else if (left < right)
        {
          if (up == false) goto nextLine;
          if (right - left > 3) goto nextLine;
          up = true;
        }
      }

      SafeLines += 1;
    nextLine:;
    }

    Part1Answer = SafeLines.ToString();
  }
}