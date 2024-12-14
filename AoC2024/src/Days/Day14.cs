using System.Text.RegularExpressions;
using Nixill.Utils;

namespace Nixill.AdventOfCode;

public class Day14 : AdventDay
{
  public override void Run()
  {
    string roomSizeLine = InputStream.ReadLine()!;
    int x = roomSizeLine.IndexOf('x');
    LongVector2 roomSize = (int.Parse(roomSizeLine[0..x]), int.Parse(roomSizeLine[(x + 1)..]));

    D14Robot[] robots = InputStream.GetAllLines().Select(l => new D14Robot(l)).ToArray();

    var divRemX = Math.DivRem(roomSize.X, 2);
    var divRemY = Math.DivRem(roomSize.Y, 2);
    LongVector2 roomCenter = (divRemX.Quotient + divRemX.Remainder, divRemY.Quotient + divRemY.Remainder);

    var robotsByQuadrant = robots
      .Select(r => r.PositionAfter(100, roomSize))
      .Where(r => r.X != roomCenter.X && r.Y != roomCenter.Y)
      .GroupBy(r => (r.X > roomCenter.X, r.Y > roomCenter.Y));

    Part1Number = robotsByQuadrant
      .Aggregate((long)1, (p, c) => p * c.Count());
  }
}

public readonly struct D14Robot
{
  static readonly Regex RobotParser = new(@"^p=(\d+),(\d+) v=(-?\d+),(-?\d+)$");

  LongVector2 Position { get; init; }
  LongVector2 Velocity { get; init; }

  public D14Robot(string line)
  {
    Match match = RobotParser.Match(line);
    Position = (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
    Velocity = (int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));
  }

  public LongVector2 RawPositionAfter(int steps)
    => Position + Velocity * steps;

  public LongVector2 PositionAfter(int steps, LongVector2 roomSize)
    => D14Math.NNMod(RawPositionAfter(steps), roomSize);

  public long StepsToLoop(LongVector2 roomSize)
    => NumberUtils.LCM(NumberUtils.LCM(Velocity.X, roomSize.X), NumberUtils.LCM(Velocity.Y, roomSize.Y));
}

internal static class D14Math
{
  public static LongVector2 NNMod(LongVector2 num, LongVector2 den)
    => (NumberUtils.NNMod(num.X, den.X), NumberUtils.NNMod(num.Y, den.Y));
}