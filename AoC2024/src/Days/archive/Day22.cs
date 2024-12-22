namespace Nixill.AdventOfCode;

public class Day22 : AdventDay
{
  public override void Run(StreamReader input)
  {
    foreach (int number in input.GetLines().Select(int.Parse))
    {
      Part1Number += SecretNumber(number).ElementAt(2000);
    }
  }

  public static IEnumerable<int> SecretNumber(int input)
  {
    yield return input; // zero-index so that ElementAt(0) is 0 times
    while (true)
    {
      input = (input ^ (input * 64)) % 16777216;
      input = (input ^ (input / 32)) % 16777216;
      input = (input ^ (input * 2048)) % 16777216;
      yield return input;
    }
  }
}