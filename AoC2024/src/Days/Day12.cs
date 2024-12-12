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

    Console.WriteLine($"There are {nextRegionNumber - 1} regions.");
  }
}