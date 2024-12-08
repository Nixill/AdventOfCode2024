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
    foreach ((char c, GridReference p) in Grid.Flatten())
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

// "D6PosDir" is defined under Day6B. The type was reused unchanged.