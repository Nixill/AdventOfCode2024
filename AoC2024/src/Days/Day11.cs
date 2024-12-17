using Nixill.Collections;
using Nixill.Utils;
using Nixill.Utils.Extensions;

namespace Nixill.AdventOfCode;

public class Day11 : AdventDay
{
  static AVLTreeDictionary<ulong, int> Magnitudes =
    [.. Sequence.For((ulong)10, i => i % 10 == 0, i => i * 10)
      .WithIndex()
      .Prepend((Item: (ulong)0, Index: -1))
      .Select(t => new KeyValuePair<ulong,int>(t.Item, t.Index + 2))];
  static AVLTreeDictionary<int, ulong> ReverseMagnitudes =
    [.. Magnitudes
      .Select(kvp => new KeyValuePair<int, ulong>(kvp.Value, ulong.Max(kvp.Key, 1)))];
  static Dictionary<(ulong, int), ulong> StoneCache = [];

  static int GetMagnitude(ulong input) => Magnitudes.FloorEntry(input).Value;
  static ulong GetNumberWithMagnitude(int input) => ReverseMagnitudes[input];

  public override void Run(StreamReader input)
  {
    ulong[] stones = input.GetEverything().Split(' ').Select(ulong.Parse).ToArray();

    Part1Number = (long)stones.Select(s => RecursiveCountStones(s, 25)).Sum();
    Part2Number = (long)stones.Select(s => RecursiveCountStones(s, 75)).Sum();
  }

  static ulong RecursiveCountStones(ulong start, int iterations)
    => StoneCache.GetOrSet((start, iterations), () =>
    {
      if (iterations == 0) return 1;
      if (start == 0) return RecursiveCountStones(1, iterations - 1);

      int mag = GetMagnitude(start);

      if (mag % 2 == 0)
      {
        var divRem = Math.DivRem(start, GetNumberWithMagnitude((mag / 2) + 1));

        return RecursiveCountStones(divRem.Quotient, iterations - 1)
          + RecursiveCountStones(divRem.Remainder, iterations - 1);
      }

      return RecursiveCountStones(start * 2024, iterations - 1);
    });
}