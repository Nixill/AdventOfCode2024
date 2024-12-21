
using Nixill.Collections;
using Nixill.Utils;

namespace Nixill.AdventOfCode;

public class Day20 : AdventDay
{
  Grid<char> Maze = [];
  Grid<int> Distances = [];
  IntVector2 Start = (0, 0);
  List<(IntVector2 FirstTile, IntVector2 SecondTile, int Savings)> Cheats = [];

  public override void Run(StreamReader input)
  {
    int minimum = InputFilename switch
    {
      "input.txt" => 100,
      "example1.txt" => 20,
      _ => 0
    };

    Maze = input.CharacterGrid();
    Distances = new Grid<int>(Maze.SizeVector, int.MaxValue);
    Start = Maze.IndexOf('S')!.Value;

    int distance = 1;

    foreach ((char chr, IntVector2 iv2) in Maze.FloodSelect(Start, (c, iv2b) => c == '.', [(0, -1)], GridTransforms.Rotate90))
    {
      Distances[iv2] = distance;

      foreach (IntVector2 iv2o in Sequence.Of((0, -2), (2, 0), (0, 2), (-2, 0)))
      {
        if (Distances.IsWithinGrid(iv2 + iv2o) && Distances[iv2 + iv2o] < distance - 2 && Maze[iv2 + (iv2o / 2)] == '#')
        {
          Cheats.Add((iv2 + iv2o, iv2 + (iv2o / 2), distance - Distances[iv2 + iv2o] - 2));
        }
      }
    }

    Part1Number = Cheats.Where(c => c.Savings >= 100).Count();
  }
}