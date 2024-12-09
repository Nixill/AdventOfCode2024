namespace Nixill.AdventOfCode;

public class Day7 : AdventDay
{
  public override void Run()
  {
    Part1Number = 0;

    foreach (string line in InputStream.GetLines())
    {
      long answer = long.Parse(line[..line.IndexOf(':')]);
      long[] numbers = line.Split(' ').Skip(1).Select(long.Parse).ToArray();

      if (IsEquationSolvable(numbers, answer)) Part1Number += answer;
    }
  }

  public static bool IsEquationSolvable(long[] longs, long target)
  {
    if (longs.Length == 0) return false;
    if (longs.Length == 1) return longs[0] == target;

    long lastLong = longs[^1];

    bool solvable = false;
    (long div, long rem) = Math.DivRem(target, lastLong);
    if (rem == 0) solvable |= IsEquationSolvable(longs[..^1], div);
    if (target >= lastLong) solvable |= IsEquationSolvable(longs[..^1], target - lastLong);

    return solvable;
  }
}