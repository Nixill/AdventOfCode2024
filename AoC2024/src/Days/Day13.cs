using System.Text.RegularExpressions;

namespace Nixill.AdventOfCode;

public class Day13 : AdventDay
{
  Regex PushButtonText = new(@"^Button [AB]: X\+(\d+), Y\+(\d+)$");
  Regex PrizeLocationText = new(@"^Prize: X=(\d+), Y=(\d+)");

  static LongVector2 Part2Modifier = new LongVector2(10000000000000, 10000000000000);

  public override void Run()
  {
    foreach (string[] chunk in InputStream.GetChunksByLine().Select(c => c.ToArray()))
    {
      Match pressARegex = PushButtonText.Match(chunk[0]);
      LongVector2 buttonAMove = (int.Parse(pressARegex.Groups[1].Value), int.Parse(pressARegex.Groups[2].Value));

      Match pressBRegex = PushButtonText.Match(chunk[1]);
      LongVector2 buttonBMove = (int.Parse(pressBRegex.Groups[1].Value), int.Parse(pressBRegex.Groups[2].Value));

      Match prizeLocRegex = PrizeLocationText.Match(chunk[2]);
      LongVector2 prizeLocation = (int.Parse(prizeLocRegex.Groups[1].Value), int.Parse(prizeLocRegex.Groups[2].Value));

      Part1Number += Solve(buttonAMove, buttonBMove, prizeLocation);
      Part2Number += Solve(buttonAMove, buttonBMove, prizeLocation + Part2Modifier);
    }
  }

  long Solve(LongVector2 moveA, LongVector2 moveB, LongVector2 target)
  {
    decimal Ax = moveA.X, Ay = moveA.Y, Bx = moveB.X, By = moveB.Y, Tx = target.X, Ty = target.Y;

    decimal intX = (Ty - (Ay / Ax) * Tx) / (By / Bx - Ay / Ax);
    decimal intY = (By / Bx) * intX;

    long pressesB = (long)Math.Round(intX / Bx);
    long pressesA = (long)Math.Round(Tx / (Ax - intX));

    // verify by actually doing it
    if (moveB * pressesB + moveA * pressesA == target) return pressesB + 3 * pressesA;
    return 0; // unsolvable ones contribute nothing to the total
  }
}