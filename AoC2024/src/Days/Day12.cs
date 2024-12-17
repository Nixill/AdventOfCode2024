using Nixill.Collections;
using Nixill.Utils.Extensions;

namespace Nixill.AdventOfCode;

public class Day12 : AdventDay
{
  public override void Run(StreamReader input)
  {
    Grid<char> plots = input.CharacterGrid();
    Grid<int> regions = new(plots.Width, plots.Height, 0);

    int nextRegionNumber = 1;

    Dictionary<int, int> areas = [];
    Dictionary<int, int> perimeters = [];
    Dictionary<int, int> sides = [];

    // Construct region map and get areas
    foreach ((char chr, IntVector2 rfc) in plots.Flatten())
    {
      if (regions[rfc] == 0)
      {
        int regionNumber = nextRegionNumber++;
        areas[regionNumber] = 0;
        perimeters[regionNumber] = 0;
        sides[regionNumber] = 0;

        foreach ((char chr2, IntVector2 rfc2) in plots.OrthogonalFloodSelect(rfc, (c, r) => c == chr))
        {
          regions[rfc2] = regionNumber;
          areas[regionNumber] += 1;
        }
      }
    }

    // Now get perimeters and sides
    GetEdges(regions, perimeters, sides);
    GetEdges(regions.GetTransposedGrid(), perimeters, sides);

    // and get answer
    Part1Number = areas.Keys.Select(k => (long)perimeters[k] * areas[k]).Sum();
    Part2Number = areas.Keys.Select(k => (long)sides[k] * areas[k]).Sum();
  }

  private static void GetEdges(IGrid<int> regions, Dictionary<int, int> perimeters, Dictionary<int, int> sides)
  {
    foreach ((IEnumerable<int> r, int y) in regions.Rows.SkipLast(1).WithIndex())
    {
      int lastTop = -1;
      int lastBot = -1;

      foreach ((int c, int x) in r.WithIndex())
      {
        int top = c;
        int bot = regions[GridRef.XY(x, y + 1)];

        if (top != bot)
        {
          perimeters[top] += 1;
          perimeters[bot] += 1;
          if (top != lastTop) { sides[top] += 1; lastTop = top; }
          if (bot != lastBot) { sides[bot] += 1; lastBot = bot; }
        }
        else
        {
          lastTop = -1;
          lastBot = -1;
        }
      }
    }

    int lastReg = -1;
    foreach (int region in regions.GetRow(0))
    {
      perimeters[region] += 1;
      if (region != lastReg) { sides[region] += 1; lastReg = region; }
    }

    lastReg = -1;
    foreach (int region in regions.GetRow(regions.Height - 1))
    {
      perimeters[region] += 1;
      if (region != lastReg) { sides[region] += 1; lastReg = region; }
    }
  }
}