using Nixill.Collections;
using Nixill.Utils.Extensions;

namespace Nixill.AdventOfCode;

public class Day6A : AdventDay
{
  HashSet<(int X, int Y)> Obstacles = [];
  HashSet<(int X, int Y)> VisitedTiles = [];
  HashSet<((int X, int Y), (int dX, int dY))> Bumps = [];
  (int X, int Y) GuardPosition;
  (int dX, int dY) GuardDirection = (0, -1);
  int Width = 0;
  int Height = 0;

  public override void Run()
  {
    foreach ((string line, int Y) in InputStream.GetAllLines().WithIndex())
    {
      foreach ((char c, int X) in line.WithIndex())
      {
        if (c == '#') Obstacles.Add((X, Y));
        else if (c == '^') GuardPosition = (X, Y);
      }

      Width = line.Length;
      Height = Y + 1;
    }

    VisitedTiles.Add(GuardPosition);

    while (true)
    {
      (int X, int Y) nextTile = (GuardPosition.X + GuardDirection.dX, GuardPosition.Y + GuardDirection.dY);
      while (Obstacles.Contains(nextTile))
      {
        ((int X, int Y), (int dX, int dY)) bump = (nextTile, GuardDirection);
        if (Bumps.Contains(bump)) break;
        Bumps.Add(bump);

        GuardDirection = RotateRight(GuardDirection);
        nextTile = (GuardPosition.X + GuardDirection.dX, GuardPosition.Y + GuardDirection.dY);
      }
      GuardPosition = nextTile;

      if (nextTile.X == Width || nextTile.X == -1 || nextTile.Y == Height || nextTile.Y == -1) break;

      VisitedTiles.Add(nextTile);
    }

    Part1Answer = VisitedTiles.Count.ToString();
  }

  (int dX, int dY) RotateRight((int dX, int dY) from)
  {
    return (-from.dY, from.dX);
  }

  (int dX, int dY) RotateLeft((int dX, int dY) from)
    => (from.dY, -from.dX);
}
