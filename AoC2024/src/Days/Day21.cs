
using Nixill.Collections;
using Nixill.Utils.Extensions;

namespace Nixill.AdventOfCode;

public class Day21 : AdventDay
{
  static readonly Dictionary<char, IntVector2> Positions;

  static readonly Grid<char> NumericKeypad;
  static readonly Grid<char> ArrowKeypad;

  static readonly Dictionary<(int Level, char From, char To), List<List<char>>> MoveCache;

  static Day21()
  {
    NumericKeypad = new(["789", "456", "123", " 0A"]);
    ArrowKeypad = new([" ↑A", "←↓→"]);
    Positions = [];

    IntVector2 NumericA = NumericKeypad.IndexOf('A')!.Value;
    Positions.AddMissing(NumericKeypad.Flatten().Select(pair => (pair.Item, pair.Reference - NumericA)));

    IntVector2 ArrowA = ArrowKeypad.IndexOf('A')!.Value;
    Positions.AddMissing(ArrowKeypad.Flatten().Select(pair => (pair.Item, pair.Reference - ArrowA)));

    MoveCache = [];
  }

  public override void Run(StreamReader input)
  {
    foreach (string code in input.GetLines())
    {
      int number = int.Parse(code[..^1]);
      Part1Number += number * GetMoves([code], 2).Count();
    }
  }

  static public IEnumerable<IEnumerable<char>> GetMoves(IEnumerable<IEnumerable<char>> sequence, int level)
    => sequence.SelectMany(s => s.Prepend('A').Pairs().SelectMany(p => GetMoves(level, p.Item1, p.Item2))).MinManyBy(s => s.Count());

  static public IEnumerable<IEnumerable<char>> GetMoves(int level, char from, char to)
    => MoveCache.GetOrSet((level, from, to), () => GetMovesUncached(level, from, to).Select(i => i.ToList()).ToList());

  static IEnumerable<IEnumerable<char>> GetMovesUncached(int level, char from, char to)
    => (level == 0) ? GetLevel0Moves(from, to) : GetNotLevel0Moves(level, from, to);

  static IEnumerable<IEnumerable<char>> GetLevel0Moves(char from, char to)
  {
    IntVector2 start = Positions[from];
    IntVector2 end = Positions[to];
    return GetLevel0Moves(start, end);
  }

  static IEnumerable<IEnumerable<char>> GetLevel0Moves(IntVector2 start, IntVector2 end)
  {
    if (start == end)
    {
      yield return ['A'];
      yield break;
    }

    if (start.X > end.X && start + IntVector2.Left != (-2, 0))
    {
      foreach (var list in GetLevel0Moves(start + IntVector2.Left, end)) yield return list.Prepend('←');
    }

    if (start.Y > end.Y)
    {
      foreach (var list in GetLevel0Moves(start + IntVector2.Up, end)) yield return list.Prepend('↑');
    }

    if (start.Y < end.Y && start + IntVector2.Down != (-2, 0))
    {
      foreach (var list in GetLevel0Moves(start + IntVector2.Down, end)) yield return list.Prepend('↓');
    }

    if (start.X < end.X)
    {
      foreach (var list in GetLevel0Moves(start + IntVector2.Right, end)) yield return list.Prepend('→');
    }
  }

  static IEnumerable<IEnumerable<char>> GetNotLevel0Moves(int level, char from, char to)
    => GetMoves(GetLevel0Moves(from, to), level - 1);
}