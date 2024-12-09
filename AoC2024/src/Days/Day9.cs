using Nixill.Collections;
using Nixill.Utils;
using Nixill.Utils.Extensions;

namespace Nixill.AdventOfCode;

public class Day9 : AdventDay
{
  public override void Run()
  {
    string input = InputStream.GetEverything();

    #region Part 1
    List<int> blocksP1 = input
      .Select(c => (int)(c - '0'))
      .Chunk(2)
      .WithIndex()
      .SelectMany(t => Enumerable.Repeat(t.Index, t.Item[0])
        .Concat(Enumerable.Repeat(-1, t.Item.ElementAtOrDefault(1))))
      .ToList();

    int i = 0;
    int l = blocksP1.Count - 1;

    while (i <= l)
    {
      if (blocksP1[i] == -1)
      {
        while (blocksP1[l] == -1)
        {
          blocksP1.RemoveAt(l--);
          if (l < i) goto endPart1;
        }

        blocksP1[i] = blocksP1[l];
        blocksP1.RemoveAt(l--);
      }

      Part1Number += blocksP1[i] * i;
      i++;
    }
  #endregion

  endPart1:;

    #region Part 2
    AVLTreeDictionary<int, int> blocksP2 = [];
    int cursor = 0;

    foreach (var pair in input
      .Select(c => (int)(c - '0'))
      .Chunk(2)
      .WithIndex())
    {
      blocksP2[cursor] = pair.Index;
      cursor += pair.Item[0];
      blocksP2[cursor] = -1;
      cursor += pair.Item.ElementAtOrDefault(1);
    }

    while (cursor > 0)
    {
      (cursor, int fileSize, int fileID) = LowerFile(blocksP2, cursor);
      int freeSpace = LowestFreeSpace(blocksP2, fileSize, cursor);

      if (freeSpace < cursor)
      {
        blocksP2.Remove(cursor);
        if (blocksP2.LowerEntry(cursor).Value != -1) blocksP2[cursor] = -1;
        if (blocksP2.HigherEntry(cursor).AssignTo(out var higherEntry).Value == -1) blocksP2.Remove(higherEntry.Key);

        blocksP2[freeSpace] = fileID;
        if (!blocksP2.ContainsKey(freeSpace + fileSize)) blocksP2[freeSpace + fileSize] = -1;
      }
    }

    foreach (var pair in blocksP2.Pairs())
    {
      if (pair.Item1.Value != -1)
        Part2Number += pair.Item1.Value * Enumerable.Range(pair.Item1.Key, pair.Item2.Key - pair.Item1.Key).Sum();
    }
    #endregion
  }

  (int Start, int Size, int ID) LowerFile(AVLTreeDictionary<int, int> blocks, int from)
  {
    var lowerEntry = blocks.LowerEntry(from);
    while (lowerEntry.Value == -1)
    {
      from = lowerEntry.Key;
      lowerEntry = blocks.LowerEntry(from);
    }

    return (lowerEntry.Key, from - lowerEntry.Key, lowerEntry.Value);
  }

  int LowestFreeSpace(AVLTreeDictionary<int, int> blocks, int size, int maxIndex)
  {
    int start = 0;

    while (start < maxIndex)
    {
      int higherKey = blocks.HigherKey(start);
      if (blocks[start] == -1 && higherKey - start >= size)
        return start;
      start = higherKey;
    }

    return maxIndex;
  }
}