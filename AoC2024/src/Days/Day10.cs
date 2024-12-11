using Nixill.Collections;
using Nixill.Utils.Extensions;

namespace Nixill.AdventOfCode;

public class Day10 : AdventDay
{
  Grid<int> Topology = [];
  Grid<HashSet<GridReference>> ReachablePeaks = [];
  Grid<int> Trails = [];

  public override void Run()
  {
    Topology = InputStream.Grid(c => (int)(c - '0'));
    Trails = new Grid<int>(Topology.Width, Topology.Height);
    ReachablePeaks = new Grid<HashSet<GridReference>>(Topology.Width, Topology.Height);

    var referencesByHeight = Topology.Flatten().GroupBy(t => t.Item, t => t.Reference).ToDictionary();

    foreach (GridReference rfc in referencesByHeight[9])
    {
      Trails[rfc] = 1;
      ReachablePeaks[rfc] = [rfc];
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
          foreach (GridReference rfc3 in ReachablePeaks[rfc2] ?? [])
          {
            (ReachablePeaks[rfc] ??= new()).Add(rfc3);
          }
        }
        Trails[rfc] = count;
        if (level == 0)
        {
          Part1Number += (ReachablePeaks[rfc] ?? []).Count;
          Part2Number += count;
        }
      }
    }
  }

  // debug functions!
  public void GetTopology() => PrettyFormatGrid(Topology);
  public void GetTrails() => PrettyFormatGrid(Trails);

  public void PrettyFormatGrid(Grid<int> ints)
  {
    int largest = ints.Flatten().Max(t => t.Item);
    int largestMagnitude = 1;
    for (; largest > 0; largest /= 10)
    {
      largestMagnitude += 1;
    }

    if (largestMagnitude < 2) largestMagnitude = 2;

    Console.WriteLine(string.Join('\n', ints.Select(r => string.Join(' ', r.Select(i => string.Format($"{{0,{largestMagnitude}}}", i))))));
  }
}