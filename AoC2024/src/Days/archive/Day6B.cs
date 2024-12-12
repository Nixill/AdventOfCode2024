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

  public override void Run()
  {
    foreach ((string line, int Y) in InputStream.GetAllLines().WithIndex())
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

    Part1Answer = VisitedPositions.Count().ToString();
    Part2Answer = answer2.ToString();
  }
}

public readonly struct D6PosDir
{
  public IntVector2 Position { get; init; }
  public IntVector2 Direction { get; init; }

  public int X => Position.X;
  public int Y => Position.Y;
  public int dX => Direction.X;
  public int dY => Direction.Y;

  public D6PosDir(IntVector2 position) => (Position, Direction) = (position, IntVector2.Up);

  public D6PosDir(IntVector2 position, IntVector2 direction) => (Position, Direction) = (position, direction);

  public static implicit operator (IntVector2 Position, IntVector2 Direction)(D6PosDir input)
    => (input.Position, input.Direction);
  public static implicit operator D6PosDir((IntVector2 Position, IntVector2 Direction) input)
    => new(input.Position, input.Direction);

  public D6PosDir Forward => new(Position + Direction, Direction);
  public D6PosDir TurnRight => new(Position, Direction.RotateRight());
  public D6PosDir Backward => new(Position - Direction, Direction);
  public D6PosDir TurnLeft => new(Position, Direction.RotateLeft());
  public IntVector2 PosToLeft => Position + Direction.RotateLeft();
}