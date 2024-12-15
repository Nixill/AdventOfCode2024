using Nixill.Collections;
using Nixill.Utils.Extensions;

namespace Nixill.AdventOfCode;

public class Day15 : AdventDay
{
  public override void Run()
  {
    Grid<char> layout = InputStream.CharacterGridChunk();
    IEnumerable<IntVector2> moves = InputStream.GetEverything().SelectUnerrored(c => (c switch
    {
      '<' => IntVector2.Left,
      '>' => IntVector2.Right,
      '^' => IntVector2.Up,
      'v' or 'V' => IntVector2.Down,
      _ => throw new KeyNotFoundException()
    })).ToArray();

    Grid<char> sim1 = Simulate(layout, moves, 5000);
    Part1Number = Calculate(sim1);
  }

  static Grid<char> Simulate(Grid<char> original, IEnumerable<IntVector2> moveSequence, int pushLimit, char cWall = '#',
    char cRobot = '@', char cFloor = '.', char cBox = 'O')
  {
    Grid<char> layout = new(original);
    IntVector2 robot = layout.IndexOf(cRobot)!.Value;

    foreach (IntVector2 move in moveSequence)
    {
      IntVector2 target = robot + move;

      if (!layout.IsWithinGrid(target))
      {
        return layout;
      }

      char targetChar = layout[target];

      if (targetChar == cFloor)
      {
        layout[robot] = cFloor;
        layout[target] = cRobot;
        robot = target;
      }
      else if (targetChar == cBox)
      {
        (IntVector2 boxTarget, char boxTargetChar) = NextNonBox(layout, target, move, cBox);

        if ((boxTarget - target).Magnitude() < pushLimit)
        {
          if (boxTargetChar == cFloor)
          {
            layout[boxTarget] = cBox;
            layout[target] = cRobot;
            layout[robot] = cFloor;
            robot = target;
          }
          else if (boxTargetChar == '\0')
          {
            layout[target] = cRobot;
            layout[robot] = cFloor;
            robot = target;
          }
        }
      }
    }

    return layout;
  }

  static (IntVector2 position, char item) NextNonBox(Grid<char> grid, IntVector2 from, IntVector2 move, char cBox)
  {
    IntVector2 target = from;
    while (grid.IsWithinGrid(target) && grid[target] == cBox)
    {
      target += move;
    }

    if (grid.IsWithinGrid(target))
    {
      return (target, grid[target]);
    }

    return (target, '\0');
  }

  static long Calculate(Grid<char> grid, char cBox = 'O')
  {
    long answer = 0;
    foreach ((char chr, IntVector2 vec) in grid.Flatten())
    {
      if (chr == cBox)
      {
        answer += 100 * vec.Y + vec.X;
      }
    }
    return answer;
  }
}

file static class D15Extensions
{
  public static int Magnitude(this IntVector2 vector)
    => int.Abs(vector.X) + int.Abs(vector.Y);
}