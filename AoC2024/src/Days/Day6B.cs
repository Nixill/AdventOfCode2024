using Nixill.Utils.Extensions;

public class Day6 : AdventDay
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
      D6PosDir next = Guard.Forward();

      while (Obstacles.Contains(next.Position))
      {
        Guard = Guard.TurnRight();
        next = Guard.Forward();
      }

      if (next.X == Width || next.X == -1 || next.Y == Height || next.Y == -1) break;

      Guard = next;
      VisitedPosDirs.Add(Guard);
      VisitedPositions.Add(Guard.Position);

      if (VisitedPosDirs.Contains(Guard.TurnRight()) && !VisitedPositions.Contains(Guard.Forward().Position))
        answer2 += 1;
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

  public D6PosDir Forward()
    => new(Position + Direction, Direction);

  public D6PosDir TurnRight()
    => new(Position, Direction.RotateRight());
}