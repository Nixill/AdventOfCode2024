using System.Text.RegularExpressions;
using Nixill.Collections;

namespace Nixill.AdventOfCode;

public class Day3 : AdventDay
{
  static Regex FunctionParser = new(@"([a-z_\']+)\((\d+(?:,\d+)*)\)", RegexOptions.IgnoreCase);

  List<D3Function> Functions = [];

  public override void Run()
  {
    string input = InputStream.GetEverything();

    foreach (Match mtc in FunctionParser.Matches(input))
    {
      if ((mtc.Groups[2].Value ?? "") == "")
      {
        Functions.Add(new D3Function
        {
          Name = mtc.Groups[1].Value,
          Index = mtc.Index + mtc.Groups[1].Length,
          Params = []
        });
      }
      else
      {
        Functions.Add(new D3Function
        {
          Name = mtc.Groups[1].Value,
          Index = mtc.Index + mtc.Groups[1].Length,
          Params = mtc.Groups[2].Value.Split(",").Select(int.Parse).ToArray()
        });
      }
    }

    // Part 1 wants all the muls with two parameters.
    Part1Answer = Functions
      .Where(f => f.Name.EndsWith("mul") && f.Params.Length == 2)
      .Select(f => f.Params[0] * f.Params[1])
      .Sum()
      .ToString();

    // Part 2 wants something more involved
    int answer = 0;
    bool on = true;
    foreach (var function in Functions)
    {
      if (function.Name.EndsWith("do")) on = true;
      else if (function.Name.EndsWith("don't")) on = false;
      else if (function.Name.EndsWith("mul") && on)
      {
        if (function.Params.Length == 2) answer += function.Params[0] * function.Params[1];
      }
    }

    Part2Answer = answer.ToString();
  }
}

public readonly struct D3Function
{
  public required string Name { get; init; }
  public required int Index { get; init; }
  public required int[] Params { get; init; }
}