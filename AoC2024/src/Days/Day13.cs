using System.Text.RegularExpressions;

namespace Nixill.AdventOfCode;

public class Day13 : AdventDay
{
  Regex PushButtonText = new(@"^Button [AB]: X\+(\d+), Y\+(\d+)$");
  Regex PrizeLocationText = new(@"^Prize: X=(\d+), Y=(\d+)");

  public override void Run()
  {
    foreach (string[] chunk in InputStream.GetChunksByLine().Select(c => c.ToArray()))
    {
      Match pressARegex = PushButtonText.Match(chunk[0]);
      IntVector2 buttonAMove = (int.Parse(pressARegex.Groups[1].Value), int.Parse(pressARegex.Groups[2].Value));

      Match pressBRegex = PushButtonText.Match(chunk[1]);
      IntVector2 buttonBMove = (int.Parse(pressBRegex.Groups[1].Value), int.Parse(pressBRegex.Groups[2].Value));

      Match prizeLocRegex = PrizeLocationText.Match(chunk[2]);
      IntVector2 prizeLocation = (int.Parse(prizeLocRegex.Groups[1].Value), int.Parse(prizeLocRegex.Groups[2].Value));

      IEnumerable<(int A, int B)> ways = ListWays(buttonAMove, buttonBMove, prizeLocation).ToArray();

      Part1Number += ways.Select(t => t.A * 3 + t.B).Order().FirstOrDefault(0);
      Part2Number += ways.Count(); // officially guessing what part 2 is
    }
  }

  IEnumerable<(int A, int B)> ListWays(IntVector2 moveA, IntVector2 moveB, IntVector2 target)
  {
    for (int a = 0; a <= 100; a++)
    {
      IntVector2 crane = moveA * a;

      if (crane == target)
      {
        yield return (a, 0);
        yield break;
      }

      if (crane.X > target.X || crane.Y > target.Y)
      {
        yield break;
      }

      foreach (int b in Enumerable.Range(1, 100))
      {
        crane += moveB;

        if (crane == target)
        {
          yield return (a, b);
          break;
        }

        if (crane.X > target.X || crane.Y > target.Y)
        {
          break;
        }
      }
    }
  }
}