using Nixill.Collections;
using Nixill.Utils;

namespace Nixill.AdventOfCode;

public class Day16 : AdventDay
{
  public override void Run(StreamReader input)
  {
    Grid<char> maze = input.CharacterGrid();

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

    int score = 0;
    D6PosDir posDir = (startPos, IntVector2.Right);

    while (!moveQueue.IsEmpty())
    {
      (score, posDir) = moveQueue.LowestValue();
      moveQueue.DeleteMin();

      if (scores.ContainsKey(posDir) || scores.ContainsKey(posDir.Forward.TurnAround)) continue;

      if (posDir.Position == endPos)
      {
        Part1Number = score;
        moveQueue.Clear();
        break;
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

    HashSet<IntVector2> tiles = [];

    moveQueue.Add((score, posDir));

    while (!moveQueue.IsEmpty())
    {
      (score, posDir) = moveQueue.HighestValue();
      moveQueue.DeleteMax();

      tiles.Add(posDir.Position);

      if (scores.TryGetValue(posDir.Backward, out int backScore) && backScore == score - 1)
      {
        moveQueue.Add((score - 1, posDir.Backward));
      }

      if (scores.TryGetValue(posDir.TurnRight, out int rightScore) && rightScore == score - 1000)
      {
        moveQueue.Add((score - 1000, posDir.TurnRight));
      }

      if (scores.TryGetValue(posDir.TurnLeft, out int leftScore) && leftScore == score - 1000)
      {
        moveQueue.Add((score - 1000, posDir.TurnLeft));
      }
    }

    tiles.Add(startPos); tiles.Add(endPos); // just in case
    Part2Number = tiles.Count;
  }
}

// D6PosDir is defined in Day6.cs, as its name implies. The type is again
// being used unchanged.