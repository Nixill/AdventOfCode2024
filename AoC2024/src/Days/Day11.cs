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
    int runCount = int.Parse(InputStream.ReadLine()!);

    ulong[] stones = InputStream.GetEverything().Split(' ').Select(ulong.Parse).ToArray();

    foreach (int i in Enumerable.Range(1, runCount))
    {
      stones = stones.SelectMany<ulong, ulong>(ProcessStone).ToArray();
      Console.WriteLine($"Ran {i} times, have {stones.Length} stones...");
      if (i == 25) Part1Number = stones.Length;
      if (i == 75) Part2Number = stones.Length;
    }
  }

  static IEnumerable<ulong> ProcessStone(ulong input)
  {
    if (input == 0)
    {
      yield return 1;
      yield break;
    }
    else
    {
      int magnitude = GetMagnitude(input);
      if (magnitude % 2 == 0)
      {
        var divRem = Math.DivRem(input, GetNumberWithMagnitude(magnitude / 2 + 1));
        yield return divRem.Quotient;
        yield return divRem.Remainder;
        yield break;
      }
      else
      {
        yield return input * 2024;
        yield break;
      }
    }
  }
}