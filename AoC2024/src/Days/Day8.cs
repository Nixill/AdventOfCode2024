using Nixill.Collections;
using Nixill.Utils.Extensions;

namespace Nixill.AdventOfCode;

public class Day8 : AdventDay
{
  DictionaryGenerator<char, List<IntVector2>> Nodes = new(new EmptyConstructorGenerator<char, List<IntVector2>>());
  HashSet<IntVector2> Antinodes = [];
  int Height;
  int Width;

  public override void Run()
  {
    Grid<char> grid = InputStream.CharacterGrid();

    (Height, Width) = (grid.Height, grid.Width);

    foreach (var tile in grid.Flatten().Where(t => t.Item != '.'))
    {
      Nodes[tile.Item].Add(tile.Reference);
    }

    foreach (List<IntVector2> list in Nodes.Values.Where(l => l.Count >= 2))
    {
      foreach (var pair in list.Combinations(2).Select(e => e.Double()))
      {
        Antinodes.Add(2 * pair.First - pair.Second);
        Antinodes.Add(2 * pair.Second - pair.First);
      }
    }

    Part1Number = Antinodes.Where(p => p.X < Width && p.X >= 0 && p.Y < Height && p.Y >= 0).Count();
  }
}