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

      Console.Write("");
    }
  }
}