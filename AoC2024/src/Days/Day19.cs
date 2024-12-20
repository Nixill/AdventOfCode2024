
using Nixill.Collections;
using Nixill.Utils.Extensions;

namespace Nixill.AdventOfCode;

public class Day19 : AdventDay
{
  RecursiveSet<char> TowelPieces = [];
  Dictionary<string, long> UsablePatternCache = [];

  public override void Run(StreamReader input)
  {
    TowelPieces = [.. input.ReadLine()!.Split(", ")];
    input.ReadLine(); // get past the blank

    foreach (string request in input.GetLines())
    {
      long results = UsablePatterns(request);
      if (results != 0)
      {
        Part1Number += 1;
        Part2Number += results; // making an official guess again :D
      }
    }
  }

  public long UsablePatterns(string request)
    => UsablePatternCache.GetOrSet(request, () => CountUsablePatterns(request));

  public long CountUsablePatterns(string request)
  {
    IRecursiveSet<char> usableStarterPieces = TowelPieces;
    long answer = 0;

    while (true)
    {
      if (!usableStarterPieces.TryGetPrefix(request[..1], out usableStarterPieces!)) return answer;
      request = request[1..];

      if (usableStarterPieces.HasEmptyValue) answer += UsablePatterns(request);
    }
  }
}