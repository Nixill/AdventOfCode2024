using Nixill.Collections;

namespace Nixill.AdventOfCode;

public class Day12 : AdventDay
{
  public override void Run()
  {
    Grid<char> plots = InputStream.CharacterGrid();
    Grid<int> regions = new(plots.Width, plots.Height);

    int nextRegionNumber = 1;

    Dictionary<int, int> areas = [];
    Dictionary<int, int> perimeters = [];

    // Construct region map and get areas
    foreach ((char chr, GridReference rfc) in plots.Flatten())
    {
      if (regions[rfc] == 0)
      {
        int regionNumber = nextRegionNumber++;
        areas[regionNumber] = 0;
        perimeters[regionNumber] = 0;

        foreach ((char chr2, GridReference rfc2) in plots.OrthogonalFloodSelect(rfc, (c, r) => c == chr))
        {
          regions[rfc2] = regionNumber;
          areas[regionNumber] += 1;
        }
      }
    }

    // Now get perimeters
    foreach ((int reg1, GridReference rfc1) in regions.Flatten())
    {
      foreach ((int reg2, GridReference rfc2) in regions.NearbyCells(rfc1, [(1, 0), (0, 1)]))
      {
        if (reg1 != reg2)
        {
          perimeters[reg1] += 1;
          perimeters[reg2] += 1;
        }
      }
    }

    int maxX = regions.Width - 1;
    int maxY = regions.Height - 1;

    for (int x = 0; x <= maxX; x++)
    {
      perimeters[regions[GridReference.XY(x, 0)]] += 1;
      perimeters[regions[GridReference.XY(x, maxY)]] += 1;
    }

    for (int y = 0; y <= maxY; y++)
    {
      perimeters[regions[GridReference.XY(0, y)]] += 1;
      perimeters[regions[GridReference.XY(maxX, y)]] += 1;
    }

    // and get answer
    Part1Number = areas.Keys.Select(k => (long)perimeters[k] * areas[k]).Sum();
  }
}