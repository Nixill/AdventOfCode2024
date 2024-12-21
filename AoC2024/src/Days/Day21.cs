
using Nixill.Collections;
using Nixill.Utils.Extensions;

namespace Nixill.AdventOfCode;

public class Day21 : AdventDay
{
  static readonly Dictionary<char, IntVector2> Positions;

  static readonly Grid<char> NumericKeypad;
  static readonly Grid<char> ArrowKeypad;

  static readonly Dictionary<(int Level, char From, char To), List<char>> MoveCache;

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
      Part1Number += number * GetMoves(code, 2).Count();
    }
  }

  static public IEnumerable<char> GetMoves(IEnumerable<char> sequence, int level)
    => sequence.Prepend('A').Pairs().SelectMany(p => GetMove(level, p.Item1, p.Item2));

  static public IEnumerable<char> GetMove(int level, char from, char to)
    => MoveCache.GetOrSet((level, from, to), () => GetMoveUncached(level, from, to).ToList());

  static IEnumerable<char> GetMoveUncached(int level, char from, char to)
    => (level == 0) ? GetLevel0Move(from, to) : GetNotLevel0Move(level, from, to);

  static IEnumerable<char> GetLevel0Move(char from, char to)
  {
    IntVector2 start = Positions[from];
    IntVector2 end = Positions[to];

    if (end.X > start.X) foreach (int x in Enumerable.Range(1, end.X - start.X)) yield return '→';
    if (end.X < start.X && (end.X != -2 || start.Y != 0)) foreach (int _ in Enumerable.Range(1, start.X - end.X)) yield return '←';
    if (end.Y > start.Y) foreach (int y in Enumerable.Range(1, end.Y - start.Y)) yield return '↓';
    if (end.Y < start.Y) foreach (int y in Enumerable.Range(1, start.Y - end.Y)) yield return '↑';
    if (end.X < start.X && (end.X == -2 && start.Y == 0)) foreach (int _ in Enumerable.Range(1, start.X - end.X)) yield return '←';

    yield return 'A';
  }

  static IEnumerable<char> GetNotLevel0Move(int level, char from, char to)
    => GetMoves(GetLevel0Move(from, to), level - 1);
}