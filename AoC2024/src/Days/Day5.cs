using Nixill.Utils.Extensions;

namespace Nixill.AdventOfCode;

public class Day5 : AdventDay
{
  Dictionary<(int, int), int> SortingRules = [];

  void AddSortingRule(int first, int second)
  {
    if (first < second) SortingRules[(first, second)] = -1;
    else if (second < first) SortingRules[(second, first)] = 1;
    else throw new ArgumentOutOfRangeException("The two arguments cannot be equal.");
  }

  int GetSortingRule(int first, int second)
  {
    if (first < second) return SortingRules.GetValueOrDefault((first, second), 0);
    else if (second < first) return -SortingRules.GetValueOrDefault((second, first), 0);
    else return 0;
  }

  bool IsCorrectlySorted(int first, int second)
  {
    return GetSortingRule(first, second) <= 0;
  }

  public override void Run(StreamReader input)
  {
    foreach ((int first, int second) in input
      .GetLinesOfChunk()
      .Select(s => s.Split("|").Select(int.Parse))
      .Select(e => (e.First(), e.Last())))
    {
      AddSortingRule(first, second);
    }

    int answer1 = 0;
    int answer2 = 0;

    foreach (IEnumerable<int> ints in input.GetLinesOfChunk()
      .Select(s => s.Split(",").Select(int.Parse)))
    {
      if (ints.Pairs().All(p => IsCorrectlySorted(p.Item1, p.Item2)))
        answer1 += ints.Middle(true);
      else
        answer2 += ints.Order(Comparer<int>.Create(GetSortingRule)).Middle(true);
    }

    Part1Number = answer1;
    Part2Number = answer2;
  }
}