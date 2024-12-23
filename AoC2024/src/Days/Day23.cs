using Nixill.Collections;
using Nixill.Utils.Extensions;

namespace Nixill.AdventOfCode;

public class Day23 : AdventDay
{
  AVLTreeDictionary<string, AVLTreeSet<string>> Connections = [];

  public override void Run(StreamReader input)
  {
    // Input processing
    var pairs = input
      .GetLines()
      .Select(l => l.Split('-').Order().Double())
      .ToArray();
    Connections = new(pairs
      .GroupBy(p => p.Item1)
      .Select(g => new KeyValuePair<string, AVLTreeSet<string>>(
        g.Key,
        new AVLTreeSet<string>(g.Select(p => p.Item2))
      )));

    List<(string, string, string)> triplets = [];

    // Code
    foreach ((string pc1, AVLTreeSet<string> conns) in Connections)
    {
      if (conns.Count < 2) continue;

      foreach ((string pc2, string pc3) in conns.Combinations(2).Select(c => c.Double()))
      {
        if (Connections.TryGetValue(pc2, out var conns2) && conns2.Contains(pc3))
        {
          triplets.Add((pc1, pc2, pc3));
          if (pc1.StartsWith('t') || pc2.StartsWith('t') || pc3.StartsWith('t')) Part1Number += 1;
        }
      }
    }
  }
}