using Nixill.Collections;

namespace Nixill.AdventOfCode;

public class Day6 : AdventDay
{
  HashSet<IntVector2> Obstacles = [];
  IntVector2 GuardStart;
  Grid<char> Grid = [];
  HashSet<IntVector2> VisitedTiles = [];

  public override void Run()
  {
    Grid = InputStream.CharacterGrid();
    foreach ((char c, IntVector2 p) in Grid.Flatten())
    {
      if (c == '#') Obstacles.Add(p);
      else if (c == '^') GuardStart = p;
    }

    var path = GetPath(new(GuardStart)).ToArray();

    foreach (D6PosDir start in path)
    {
      VisitedTiles.Add(start.Position);
      if (VisitedTiles.Contains(start.Forward.Position)) continue;
      var subpath = GetPath(start, start.Forward.Position);
      if (subpath.SkipLast(1).Contains(subpath.Last())) Part2Number += 1;
    }

    Part1Number = VisitedTiles.Count;
  }

  public IEnumerable<D6PosDir> GetPath(D6PosDir start) => GetPath(start, (-1, -1));

  public IEnumerable<D6PosDir> GetPath(D6PosDir start, IntVector2 extraObstacle)
  {
    D6PosDir guard = start;
    HashSet<D6PosDir> VisitedPosDirs = [guard];

    yield return start;

    while (true)
    {
      D6PosDir next = guard.Forward;

      if (Obstacles.Contains(next.Position) || extraObstacle == next.Position) guard = guard.TurnRight;
      else if (!Grid.IsWithinGrid(next.Position)) yield break;
      else guard = next;

      yield return guard;

      if (VisitedPosDirs.Contains(guard)) yield break;
      VisitedPosDirs.Add(guard);
    }
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