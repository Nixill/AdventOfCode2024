using Nixill.Collections;
using Nixill.Utils;

namespace Nixill.AdventOfCode;

public class Day16 : AdventDay
{
  public override void Run()
  {
    Grid<char> maze = InputStream.CharacterGrid();

    IntVector2 startPos = maze.IndexOf('S')!.Value;
    IntVector2 endPos = maze.IndexOf('E')!.Value;

    AVLTreeSet<(int Score, D6PosDir Head)> moveQueue = new(
      [(0, (startPos, IntVector2.Right))],
      (l, r) => Sequence.FirstNonZero(
        l.Score - r.Score,
        GridRef.Compare(l.Head.Position, r.Head.Position),
        GridRef.Compare(l.Head.Direction, r.Head.Direction)
      )
    );

    Dictionary<D6PosDir, int> scores = new();

    while (!moveQueue.IsEmpty())
    {
      (int score, D6PosDir posDir) = moveQueue.LowestValue();
      moveQueue.DeleteMin();

      if (scores.ContainsKey(posDir) || scores.ContainsKey(posDir.Forward.TurnAround)) continue;

      if (posDir.Position == endPos)
      {
        Part1Number = score;
        return;
      }

      scores[posDir] = score;

      if (maze[posDir.Forward.Position] != '#')
      {
        moveQueue.Add((score + 1, posDir.Forward));
      }

      if (maze[posDir.PosToLeft] != '#')
      {
        moveQueue.Add((score + 1000, posDir.TurnLeft));
      }

      if (maze[posDir.PosToRight] != '#')
      {
        moveQueue.Add((score + 1000, posDir.TurnRight));
      }
    }
  }
}

// D6PosDir is defined in Day6.cs, as its name implies. The type is again
// being used unchanged.