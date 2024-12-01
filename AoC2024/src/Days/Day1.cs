using System.Text.RegularExpressions;
using Nixill.Collections;

namespace Nixill.AdventOfCode;

public class Day1
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

  // I just have a little feeling (and honestly I'm curious even if it's wrong)...
  int UnsortedDiffs = 0;

  public Day1(string fname, StreamReader input)
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
  }

  static Dictionary<string, Day1> results = new();

  static Day1 Get(string fname, StreamReader input)
  {
    if (!results.ContainsKey(fname))
      results[fname] = new Day1(fname, input);
    return results[fname];
  }

  public static string Part1(string fname, StreamReader input)
  {
    Day1 result = Get(fname, input);
    return result.SortedDiffs.ToString();
  }
}
