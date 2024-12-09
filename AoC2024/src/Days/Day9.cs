using Nixill.Utils;
using Nixill.Utils.Extensions;

namespace Nixill.AdventOfCode;

public class Day9 : AdventDay
{
  public override void Run()
  {
    List<int> blocks = InputStream
      .GetEverything()
      .Select(c => (int)(c - '0'))
      .Chunk(2)
      .WithIndex()
      .SelectMany(t => Enumerable.Repeat(t.Index, t.Item[0])
        .Concat(Enumerable.Repeat(-1, t.Item.ElementAtOrDefault(1))))
      .ToList();

    int i = 0;
    int l = blocks.Count - 1;

    while (i < l)
    {
      if (blocks[i] == -1)
      {
        while (blocks[l] == -1)
        {
          blocks.RemoveAt(l--);
        }

        blocks[i] = blocks[l];
        blocks.RemoveAt(l--);
      }

      Part1Number += blocks[i] * i;
      i++;
    }
  }
}