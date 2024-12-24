using System.Text.RegularExpressions;
using Nixill.Utils.Extensions;

namespace Nixill.AdventOfCode;

public class Day24 : AdventDay
{
  Dictionary<string, bool> Bytes = [];
  Dictionary<string, (string, D24Op, string)> Operations = [];

  static readonly Regex ByteMatcher = new(@"(\w+): ([01])");
  static readonly Regex OpMatcher = new(@"(\w+) (AND|X?OR) (\w+) -> (\w+)");
  static readonly Regex IsZBit = new(@"z\d\d");

  public override void Run(StreamReader input)
  {
    // Input processing
    Bytes = input
      .GetLinesOfChunk()
      .Select(l => ByteMatcher.Match(l))
      .Select(m => (m.Groups[1].Value, m.Groups[2].Value == "1"))
      .ToDictionary();

    Operations = input
      .GetLinesOfChunk()
      .Select(l => OpMatcher.Match(l))
      .Select(m => (m.Groups[4].Value, (m.Groups[1].Value, Enum.Parse<D24Op>(m.Groups[2].Value), m.Groups[3].Value)))
      .ToDictionary();

    // Part 1 code
    string[] zBits = Operations
      .Where(kvp => IsZBit.IsMatch(kvp.Key))
      .Select(kvp => kvp.Key)
      .OrderDescending()
      .ToArray();

    Part1Number = zBits.Select(GetValue)
      .Select(b => b ? 1 : 0)
      .Aggregate((long)0, (a, n) => a * 2 + n);
  }

  public bool GetValue(string of)
    => Bytes.GetOrSet(of, () => GetOperation(of));

  public bool GetOperation(string of)
  {
    (string leftByte, D24Op op, string rightByte) = Operations[of];
    bool left = GetValue(leftByte);
    bool right = GetValue(rightByte);

    return op switch
    {
      D24Op.OR => left || right,
      D24Op.AND => left && right,
      D24Op.XOR => left != right,
      _ => throw new Exception("Invalid enum data")
    };
  }
}

public enum D24Op
{
  AND = 0,
  OR = 1,
  XOR = 2
}