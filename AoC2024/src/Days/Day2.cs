using System.Text.RegularExpressions;
using Nixill.Utils.Extensions;

namespace Nixill.AdventOfCode;

public class Day2 : AdventDay
{
  int SafeLines = 0;
  int SafeLinesWithDampener = 0;

  public override void Run()
  {
    foreach (string line in InputStream.GetLines())
    {
      int[] numbers = Regexes.Number.Matches(line).Select(m => int.Parse(m.Value)).ToArray();

      (bool isSafe, int index) = IsSafeLine(numbers);

      if (isSafe)
      {
        SafeLines += 1;
        SafeLinesWithDampener += 1;

        continue;
      }

      if (IsSafeLine(numbers[..index].Concat(numbers[(index + 1)..])).Safe
        || IsSafeLine(numbers[..(index - 1)].Concat(numbers[index..])).Safe)
        SafeLinesWithDampener += 1;
    }

    Part1Answer = SafeLines.ToString();
    Part2Answer = SafeLinesWithDampener.ToString();
  }

  private static (bool Safe, int Where) IsSafeLine(IEnumerable<int> numbers)
  {
    int? last = null;
    bool? up = null;

    foreach ((int now, int index) in numbers.WithIndex())
    {
      // First: Go through line without dampener.
      if (!last.HasValue)
      {
        last = now;
        continue;
      }

      if (last > now)
      {
        if (up == true) return (false, index);
        if (last - now > 3) return (false, index);
        up = false;
      }
      else if (last < now)
      {
        if (up == false) return (false, index);
        if (now - last > 3) return (false, index);
        up = true;
      }
      else if (last == now)
      {
        return (false, index);
      }

      last = now;
    }

    return (true, -1);
  }
}