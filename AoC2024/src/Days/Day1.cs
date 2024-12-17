using System.Text.RegularExpressions;
using Nixill.Collections;

namespace Nixill.AdventOfCode;

public class Day1 : AdventDay
{
  DictionaryGenerator<int, int> numberCountsLeft = new AVLTreeDictionary<int, int>().WithGenerator(new DefaultGenerator<int, int>());
  DictionaryGenerator<int, int> numberCountsRight = new AVLTreeDictionary<int, int>().WithGenerator(new DefaultGenerator<int, int>());

  static readonly Regex numberSplitter = new(@"(\d+)\D+(\d+)");

  static (int, int) Split(string line)
    => (int.Parse(numberSplitter.Match(line).Groups.AssignTo(out var grps)[1].Value), int.Parse(grps[2].Value));

  static IEnumerable<int> NumbersOrdered(DictionaryGenerator<int, int> input)
  {
    if (input.Dictionary is AVLTreeDictionary<int, int> avlDict)
    {
      foreach (var kvp in avlDict)
      {
        foreach (int i in Enumerable.Repeat(kvp.Key, kvp.Value)) yield return i;
      }
    }
  }

  int SortedDiffs = 0;

  // I just have a little feeling (okay I was wrong but I'm still curious)...
  int UnsortedDiffs = 0;

  int Similarity = 0;

  public override void Run(StreamReader input)
  {
    foreach (string line in input.GetLines())
    {
      (int left, int right) = Split(line);
      UnsortedDiffs += Math.Abs(left - right);

      numberCountsLeft[left] += 1;
      numberCountsRight[right] += 1;
    }

    foreach ((int left, int right) in NumbersOrdered(numberCountsLeft).Zip(NumbersOrdered(numberCountsRight)))
    {
      SortedDiffs += Math.Abs(left - right);
    }

    numberCountsRight.StoreGeneratedValues = false;

    foreach (var kvp in numberCountsLeft)
    {
      Similarity += kvp.Key * kvp.Value * numberCountsRight[kvp.Key];
    }

    Part1Number = SortedDiffs;
    Part2Number = Similarity;
  }
}