using System.Text.RegularExpressions;
using Nixill.Utils;
using Nixill.Utils.Extensions;

namespace Nixill.AdventOfCode;

public class Day14 : AdventDay
{
  public override void Run()
  {
    string roomSizeLine = InputStream.ReadLine()!;
    int xPos = roomSizeLine.IndexOf('x');
    IntVector2 roomSize = (int.Parse(roomSizeLine[0..xPos]), int.Parse(roomSizeLine[(xPos + 1)..]));

    D14Robot[] robots = InputStream.GetAllLines().Select(l => new D14Robot(l)).ToArray();

    IntVector2 roomCenter = (roomSize.X / 2, roomSize.Y / 2);

    var robotsByQuadrant = robots
      .Select(r => r.PositionAfter(100, roomSize))
      .Where(p => p.X != roomCenter.X && p.Y != roomCenter.Y)
      .GroupBy(p => (p.X > roomCenter.X, p.Y > roomCenter.Y));

    Part1Number = robotsByQuadrant
      .Aggregate((long)1, (p, c) => p * c.Count());

    int maxSteps = (int)NumberUtils.LCM(roomSize.X, roomSize.Y);

    using StreamWriter output = new(File.OpenWrite($"AoC2024Data/day14/outputs/{InputFilename}"));

    foreach (int i in Enumerable.Range(0, maxSteps))
    {
      var positionSet = robots.Select(r => r.PositionAfter(i, roomSize)).ToHashSet();
      output.WriteLine(i);

      foreach (int y in Enumerable.Range(0, roomSize.Y))
      {
        output.WriteLine(Enumerable
          .Range(0, roomSize.X)
          .Select(x => positionSet.Contains((x, y)) ? '█' : ' ')
          .FormString());
      }

      output.WriteLine(new string('-', roomSize.X));
    }

    output.Flush();
    output.Close();
  }
}

public readonly struct D14Robot
{
  static readonly Regex RobotParser = new(@"^p=(\d+),(\d+) v=(-?\d+),(-?\d+)$");

  IntVector2 Position { get; init; }
  IntVector2 Velocity { get; init; }

  public D14Robot(string line)
  {
    Match match = RobotParser.Match(line);
    Position = (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
    Velocity = (int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));
  }

  public IntVector2 RawPositionAfter(int steps)
    => Position + Velocity * steps;

  public IntVector2 PositionAfter(int steps, IntVector2 roomSize)
    => D14Math.NNMod(RawPositionAfter(steps), roomSize);

  public long StepsToLoop(IntVector2 roomSize)
    => NumberUtils.LCM(NumberUtils.LCM(Velocity.X, roomSize.X), NumberUtils.LCM(Velocity.Y, roomSize.Y));
}

internal static class D14Math
{
  public static IntVector2 NNMod(IntVector2 num, IntVector2 den)
    => (NumberUtils.NNMod(num.X, den.X), NumberUtils.NNMod(num.Y, den.Y));
}