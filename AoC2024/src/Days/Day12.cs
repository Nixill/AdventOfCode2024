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
    Dictionary<int, int> sides = [];

    // Construct region map and get areas
    foreach ((char chr, GridReference rfc) in plots.Flatten())
    {
      if (regions[rfc] == 0)
      {
        int regionNumber = nextRegionNumber++;
        areas[regionNumber] = 0;
        perimeters[regionNumber] = 0;
        sides[regionNumber] = 0;

        foreach ((char chr2, GridReference rfc2) in plots.OrthogonalFloodSelect(rfc, (c, r) => c == chr))
        {
          regions[rfc2] = regionNumber;
          areas[regionNumber] += 1;
        }
      }
    }

    // Now get perimeters and sides
    GetInternalEdges(regions, perimeters, sides, false);

    int lastReg1 = -1, lastReg2 = -1;

    int maxX = regions.Width - 1;
    int maxY = regions.Height - 1;

    for (int x = 0; x <= maxX; x++)
    {
      int reg1 = regions[GridReference.XY(x, 0)];
      perimeters[reg1] += 1;
      if (reg1 != lastReg1)
      {
        sides[reg1] += 1;
        lastReg1 = reg1;
      }

      int reg2 = regions[GridReference.XY(x, maxY)];
      perimeters[reg2] += 1;
      if (reg2 != lastReg2)
      {
        sides[reg2] += 1;
        lastReg2 = reg2;
      }
    }

    lastReg1 = -1; lastReg2 = -1;

    for (int y = 0; y <= maxY; y++)
    {
      int reg1 = regions[GridReference.XY(0, y)];
      perimeters[reg1] += 1;
      if (reg1 != lastReg1)
      {
        sides[reg1] += 1;
        lastReg1 = reg1;
      }

      int reg2 = regions[GridReference.XY(maxX, y)];
      perimeters[reg2] += 1;
      if (reg2 != lastReg2)
      {
        sides[reg2] += 1;
        lastReg2 = reg2;
      }
    }

    // and get answer
    Part1Number = areas.Keys.Select(k => (long)perimeters[k] * areas[k]).Sum();
    Part2Number = areas.Keys.Select(k => (long)sides[k] * areas[k]).Sum();
  }

  private static void GetInternalEdges(Grid<int> regions, Dictionary<int, int> perimeters, Dictionary<int, int> sides, bool transposed)
  {
    (int lastReg1, int lastReg2) = (-1, -1);
    int lastCoord = -1;

    foreach ((int reg1, GridReference rfc1) in (transposed ? regions.FlattenTransposed() : regions.Flatten()))
    {
      if (lastCoord != (transposed ? rfc1.Column : rfc1.Row))
      {
        lastReg1 = -1;
        lastReg2 = -1;
        lastCoord = (transposed ? rfc1.Column : rfc1.Row);
      }

      foreach ((int reg2, GridReference rfc2) in regions.NearbyCells(rfc1, [(transposed ? (1, 0) : (0, 1))]))
      {
        perimeters[reg1] += 1;
        perimeters[reg2] += 1;

        if (lastReg1 != reg1)
        {
          sides[reg1] += 1;
          lastReg1 = reg1;
        }

        if (lastReg2 != reg2)
        {
          sides[reg2] += 1;
          lastReg2 = reg2;
        }
      }
    }
  }
}