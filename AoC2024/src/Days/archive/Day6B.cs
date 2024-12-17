using System.Runtime.InteropServices.Marshalling;
using Nixill.Utils.Extensions;

namespace Nixill.AdventOfCode;

public class Day6B : AdventDay
{
  HashSet<IntVector2> Obstacles = [];
  HashSet<IntVector2> VisitedPositions = [];
  HashSet<D6PosDir> VisitedPosDirs = [];
  D6PosDir Guard;
  int Width = 0;
  int Height = 0;

  public override void Run(StreamReader input)
  {
    foreach ((string line, int Y) in input.GetAllLines().WithIndex())
    {
      foreach ((char c, int X) in line.WithIndex())
      {
        if (c == '#') Obstacles.Add((X, Y));
        else if (c == '^') Guard = new((X, Y));
      }

      Width = line.Length;
      Height = Y + 1;
    }

    VisitedPosDirs.Add(Guard);
    VisitedPositions.Add(Guard.Position);

    int answer2 = 0;

    while (true)
    {
      D6PosDir next = Guard.Forward;

      if (Obstacles.Contains(next.Position))
      {
        Guard = Guard.TurnRight;
      }
      else
      {
        if (next.X == Width || next.X == -1 || next.Y == Height || next.Y == -1) break;

        Guard = next;
        VisitedPositions.Add(Guard.Position);
      }

      VisitedPosDirs.Add(Guard);
      if (VisitedPosDirs.Contains(Guard.TurnRight) && !VisitedPositions.Contains(Guard.Forward.Position))
        answer2 += 1;

      if (Obstacles.Contains(Guard.PosToLeft))
      {
        D6PosDir virtualGuard = Guard.TurnLeft;
        while (!Obstacles.Contains(virtualGuard.Position) &&
          !(virtualGuard.X == Width || virtualGuard.X == -1 || virtualGuard.Y == Height || virtualGuard.Y == -1))
        {
          VisitedPosDirs.Add(virtualGuard);
          virtualGuard = virtualGuard.Backward;
        }
      }
    }

    Part1Number = VisitedPositions.Count();
    Part2Number = answer2;
  }
}

// D6PosDir is defined in the file Day6.cs outside the archive.