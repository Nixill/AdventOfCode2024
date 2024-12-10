using Nixill.Collections;
using Nixill.Utils.Extensions;

namespace Nixill.AdventOfCode;

public class Day10 : AdventDay
{
  Grid<int> Topology = [];
  Grid<int> Trails = [];

  public override void Run()
  {
    Topology = InputStream.Grid(c => (int)(c - '0'));
    Trails = new Grid<int>(Topology.Width, Topology.Height);

    var referencesByHeight = Topology.Flatten().GroupBy(t => t.Item, t => t.Reference).ToDictionary();

    foreach (GridReference rfc in referencesByHeight[9])
    {
      Trails[rfc] = 1;
    }

    for (int level = 8; level >= 0; level--)
    {
      foreach (GridReference rfc in referencesByHeight[level])
      {
        int count = 0;
        foreach (GridReference rfc2 in Topology
          .OrthogonallyAdjacentCells(rfc)
          .Where(t => t.Item == level + 1)
          .Select(t => t.Reference))
        {
          count += Trails[rfc2];
        }
        Trails[rfc] = count;
        if (level == 0) Part1Answer += count;
      }
    }
  }
}