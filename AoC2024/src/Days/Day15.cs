using Nixill.Collections;
using Nixill.Utils.Extensions;

namespace Nixill.AdventOfCode;

public class Day15 : AdventDay
{
  public override void Run(StreamReader input)
  {
    Grid<char> layout = new(input.CharacterGridChunk().Select(
      r => r.SelectMany(c => c switch
      {
        '#' => "##",
        '@' => "@.",
        'O' => "[]",
        '.' => "..",
        _ => ""
      })
    ));

    IEnumerable<IntVector2> moves = input.GetEverything().SelectUnerrored(c => (c switch
    {
      '<' => IntVector2.Left,
      '>' => IntVector2.Right,
      '^' => IntVector2.Up,
      'v' or 'V' => IntVector2.Down,
      _ => throw new KeyNotFoundException()
    })).ToArray();

    Grid<char> layout1 = Simulate(layout, DuplicateMoves(moves));
    Part1Number = Calculate(layout1, true);

    Grid<char> layout2 = Simulate(layout, moves);
    Part2Number = Calculate(layout2, false);
  }

  static IEnumerable<IntVector2> DuplicateMoves(IEnumerable<IntVector2> original)
  {
    bool isOnRight = false;
    foreach (IntVector2 move in original)
    {
      if (move == IntVector2.Left)
      {
        if (isOnRight) yield return move;
        yield return move;
        yield return move;
        isOnRight = false;
      }
      else if (move == IntVector2.Right)
      {
        if (!isOnRight) yield return move;
        yield return move;
        yield return move;
        isOnRight = true;
      }
      else yield return move;
    }
  }

  static Grid<char> Simulate(Grid<char> original, IEnumerable<IntVector2> moves)
  {
    Grid<char> grid = new(original);
    IntVector2 robot = grid.IndexOf('@')!.Value;

    foreach (IntVector2 move in moves)
    {
      IntVector2 target = robot + move;
      char targetChar = grid[target];

      if (targetChar == '.')
      {
        grid[target] = '@';
        grid[robot] = '.';
        robot = target;
      }
      else if (targetChar == '[' || targetChar == ']')
      {
        IntVector2 firstBox = target - ((targetChar - '[') / 2, 0);
        IEnumerable<IntVector2> boxesToMove = RecursiveGetBoxes(grid, firstBox, move);

        if (boxesToMove.Any(t => grid[t] == '#'))
        {
          continue;
        }

        foreach (IntVector2 box in boxesToMove)
        {
          MoveBox(grid, box, move);
        }

        grid[target] = '@';
        grid[robot] = '.';
        robot = target;
      }
    }

    return grid;
  }

  static IEnumerable<IntVector2> RecursiveGetBoxes(Grid<char> grid, IntVector2 firstBox, IntVector2 move)
  {
    // sanity check
    if (grid[firstBox] != '[' || grid[firstBox + IntVector2.Right] != ']') return [];
    IEnumerable<IntVector2> boxes = [];

    // tile near left side of box
    if (move != IntVector2.Right)
    {
      IntVector2 lTarget = firstBox + move;
      char lTargetChar = grid[lTarget];

      if (lTargetChar == '[') boxes = boxes.Concat(RecursiveGetBoxes(grid, lTarget, move));
      else if (lTargetChar == ']') boxes = boxes.Concat(RecursiveGetBoxes(grid, lTarget + IntVector2.Left, move));
      else if (lTargetChar == '#') boxes = boxes.Append(lTarget);
    }

    // tile near right side of box
    if (move != IntVector2.Left)
    {
      IntVector2 rTarget = firstBox + move + IntVector2.Right;
      char rTargetChar = grid[rTarget];

      if (rTargetChar == '[') boxes = boxes.Concat(RecursiveGetBoxes(grid, rTarget, move));
      else if (rTargetChar == '#') boxes = boxes.Append(rTarget);
      // not checking ']' because it'll be the same box as the left found
    }

    return boxes.Append(firstBox).Distinct();
  }

  static void MoveBox(Grid<char> grid, IntVector2 box, IntVector2 by)
  {
    grid[box] = '.';
    grid[box + IntVector2.Right] = '.';

    // this should never trigger but you know how programmers are with "should"s.
    if (grid[box + by] != '.' || grid[box + by + IntVector2.Right] != '.') throw new Exception("Box collision detected!");

    grid[box + by] = '[';
    grid[box + by + IntVector2.Right] = ']';
  }

  static long Calculate(Grid<char> grid, bool isDoubleScaled)
  {
    long answer = 0;
    foreach ((char chr, IntVector2 pos) in grid.Flatten())
    {
      if (chr == '[') answer += 100 * pos.Y + (pos.X / (isDoubleScaled ? 2 : 1));
    }
    return answer;
  }
}