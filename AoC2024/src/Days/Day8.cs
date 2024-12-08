using Nixill.Collections;
using Nixill.Utils.Extensions;

namespace Nixill.AdventOfCode;

public class Day8 : AdventDay
{
  DictionaryGenerator<char, List<IntVector2>> Nodes = new(new EmptyConstructorGenerator<char, List<IntVector2>>());
  HashSet<IntVector2> Antinodes = [];
  HashSet<IntVector2> ResonantAntinodes = [];
  int Height;
  int Width;

  public override void Run()
  {
    Grid<char> grid = InputStream.CharacterGrid();

    (Height, Width) = (grid.Height, grid.Width);

    foreach (var tile in grid.Flatten().Where(t => t.Item != '.' && t.Item != '#'))
    {
      Nodes[tile.Item].Add(tile.Reference);
    }

    foreach (List<IntVector2> list in Nodes.Values.Where(l => l.Count >= 2))
    {
      foreach (var pair in list.Combinations(2).Select(e => e.Double()))
      {
        IntVector2 offset = pair.Second - pair.First;
        Antinodes.Add(pair.First - offset);
        Antinodes.Add(pair.Second + offset);

        for (IntVector2 current = pair.First; grid.IsWithinGrid(current); current -= offset)
          ResonantAntinodes.Add(current);
        for (IntVector2 current = pair.Second; grid.IsWithinGrid(current); current += offset)
          ResonantAntinodes.Add(current);
      }
    }

    Part1Number = Antinodes.Where(p => grid.IsWithinGrid(p)).Count();
    Part2Number = ResonantAntinodes.Count();
  }
}