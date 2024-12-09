namespace Nixill.AdventOfCode;

public class Day7 : AdventDay
{
  public override void Run()
  {
    Part1Number = 0;
    Part2Number = 0;

    foreach (string line in InputStream.GetLines())
    {
      long answer = long.Parse(line[..line.IndexOf(':')]);
      long[] numbers = line.Split(' ').Skip(1).Select(long.Parse).ToArray();

      (bool p1, bool p2) = IsEquationSolvable(numbers, answer);

      if (p1) Part1Number += answer;
      if (p2) Part2Number += answer;
    }
  }

  public static (bool, bool) IsEquationSolvable(long[] longs, long target)
  {
    if (longs.Length == 0) return (false, false);
    if (longs.Length == 1) return (longs[0] == target, longs[0] == target);

    long lastLong = longs[^1];

    bool p1 = false;
    bool p2 = false;

    (long div, long rem) = Math.DivRem(target, lastLong);
    if (rem == 0)
    {
      (bool thisP1, bool thisP2) = IsEquationSolvable(longs[..^1], div);
      p1 |= thisP1;
      p2 |= thisP2;
    }

    if (target >= lastLong)
    {
      (bool thisP1, bool thisP2) = IsEquationSolvable(longs[..^1], target - lastLong);
      p1 |= thisP1;
      p2 |= thisP2;
    }

    (div, rem) = Math.DivRem(target, Next10(lastLong));
    if (rem == lastLong)
    {
      (bool thisP1, bool thisP2) = IsEquationSolvable(longs[..^1], div);
      // not solvable by p1 rules
      p2 |= thisP2;
    }

    return (p1, p2);
  }

  public static long Next10(long input)
  {
    long test = 1;
    while (test <= input) test *= 10;
    return test;
  }
}