namespace Nixill.AdventOfCode;

public class Day1
{
  public Day1(string fname, StreamReader input)
  {
    foreach (string line in input.GetLines())
    {

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
    return "Hi!";
  }
}
