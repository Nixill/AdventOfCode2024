using System.Text.RegularExpressions;
using Nixill.Collections;

namespace Nixill.AdventOfCode;

public class Day3 : AdventDay
{
  static Regex FunctionParser = new(@"([a-z_]+)\((\d+(?:,\d+)*)\)", RegexOptions.IgnoreCase);

  IDictionary<string, List<List<int>>> Functions = new EmptyConstructorGenerator<string, List<List<int>>>().Wrap();

  public override void Run()
  {
    string input = InputStream.GetEverything();

    foreach (Match mtc in FunctionParser.Matches(input))
    {
      if ((mtc.Groups[2].Value ?? "") == "")
      {
        Functions[mtc.Groups[1].Value].Add([]);
      }
      else
      {
        Functions[mtc.Groups[1].Value].Add(mtc.Groups[2].Value.Split(",").Select(int.Parse).ToList());
      }
    }

    int answer = 0;

    // Part 1 wants all the muls with two parameters.
    foreach (List<int> paramList in Functions.Where(kvp => kvp.Key.EndsWith("mul")).SelectMany(kvp => kvp.Value))
    {
      if (paramList.Count == 2)
      {
        answer += paramList[0] * paramList[1];
      }
    }

    Part1Answer = answer.ToString();
  }
}