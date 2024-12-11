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

  static int GetMagnitude(ulong input) => Magnitudes.FloorEntry(input).Value;
  static ulong GetNumberWithMagnitude(int input) => ReverseMagnitudes[input];

  public override void Run()
  {
    IEnumerable<ulong> stones = InputStream.GetEverything().Split(' ').Select(ulong.Parse);

    foreach (int i in Enumerable.Range(1, 25))
    {
      stones = stones.SelectMany<ulong, ulong>(n =>
        n == 0 ? [1] :
        GetMagnitude(n).AssignTo(out int mag) % 2 == 0 ? [Math.DivRem(n, GetNumberWithMagnitude(mag / 2)).AssignTo(out (ulong Quotient, ulong Remainder) divRem).Quotient, divRem.Remainder] :
        [n * 2048]
      );
    }

    Part1Number = stones.Count();
  }
}