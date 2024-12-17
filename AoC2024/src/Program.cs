using System.Diagnostics;
using System.Reflection;

namespace Nixill.AdventOfCode;

public static class Program
{
  const string DataFolder = "AoC2024Data";

  static void Main(string[] args)
  {
    string which = "";

    if (args.Any()) which = args.First();
    else
    {
      Console.Write("Which day's program should be run? (Enter a number): ");
      which = Console.ReadLine()!;
    }

    Type? dayType = null;
    string ns = typeof(Program).Namespace.AssignTo(out var nsQuestion) == null ? "" : $"{nsQuestion}.";

    if (which == "latest")
    {
      foreach (int i in Enumerable.Range(0, 26).Reverse())
      {
        which = i.ToString();
        dayType = Type.GetType($"{ns}Day{i}");
        if (dayType != null) break;
      }
      Console.WriteLine($"Latest requested - Day {which} selected.");
    }
    else
    {
      dayType = Type.GetType($"{ns}Day{which}");
    }

    // A few checks to run first...
    if (dayType == null)
    {
      Console.WriteLine("This day hasn't been written yet!");
      return;
    }

    if (!dayType.IsAssignableTo(typeof(AdventDay)))
    {
      Console.WriteLine("This day's code isn't an AdventDay!");
      return;
    }

    if (!Directory.Exists($"{DataFolder}/day{which}"))
    {
      Console.WriteLine("This day's data directory doesn't exist!");
      return;
    }

    ConstructorInfo? con = dayType.GetConstructor(Type.EmptyTypes);
    if (con == null)
    {
      Console.WriteLine("This day's code doesn't have an empty constructor!");
      return;
    }

    // Now we'll start running it for each day.
    Func<AdventDay> newAdventDay = () => (AdventDay)con.Invoke([]);

    // Run all tests first:
    string[] examples = Directory.GetFiles($"{DataFolder}/day{which}", "example*.txt");
    int p1Pass = 0;
    int p2Pass = 0;
    int p1Fail = 0;
    int p2Fail = 0;

    Console.WriteLine();
    Console.WriteLine("Running tests against examples...");

    foreach (string fpath in examples)
    {
      Console.WriteLine();
      using StreamReader input = new(File.OpenRead(fpath));

      // The first two lines of an example are the expected answers.
      string? p1Answer = input.ReadLine();
      string? p2Answer = input.ReadLine();

      bool? pass = null;

      string fname = new FileInfo(fpath).Name;

      Stopwatch watch = new();
      AdventDay day = newAdventDay();
      day.InputStream = input;
      day.InputFilename = fname;

      Console.Write($"Test file: {fname} / ");

      // set up skips
      if (p1Answer == @"\SKIP") day.SkipPart1 = true;
      else if (p1Answer!.StartsWith(@"\\")) p1Answer = p1Answer[1..];
      else if (p1Answer!.StartsWith(@"\")) throw new InvalidDataException("Unrecognized magic sequence in part 1 answer");

      if (p2Answer == @"\SKIP") day.SkipPart2 = true;
      else if (p2Answer!.StartsWith(@"\\")) p2Answer = p2Answer[1..];
      else if (p2Answer!.StartsWith(@"\")) throw new InvalidDataException("Unrecognized magic sequence in part 2 answer");

      watch.Start();
      day.Run();
      watch.Stop();
      day.InputStream = null!;
      input.Dispose();

      // Output the results
      Console.WriteLine($"Elapsed time: {watch.ElapsedMilliseconds} ms");

      if (day.Part1Complete)
        PrintTestResult(day.Part1Answer, p1Answer, 1, ref p1Pass, ref p1Fail, ref pass);
      if (day.Part2Complete)
        PrintTestResult(day.Part2Answer, p2Answer, 2, ref p2Pass, ref p2Fail, ref pass);

      Console.WriteLine($"File result: {(pass == null ? "No tests" : pass.Value ? "Pass" : "FAIL")}");
    }

    Console.WriteLine();
    Console.WriteLine($"All files:\nPart 1: {p1Pass} passed, {p1Fail} failed\nPart 2: {p2Pass} passed, {p2Fail} failed");
    Console.WriteLine();

    if (p1Fail != 0 || p2Fail != 0)
    {
      Console.WriteLine("Please fix failed tests and try again.");
      return;
    }

    // Run the real thing
    {
      Stopwatch watch = new();
      using StreamReader input = new(File.OpenRead($"{DataFolder}/day{which}/input.txt"));

      Console.Write("Puzzle input data / ");

      AdventDay day = newAdventDay();
      day.InputStream = input;
      day.InputFilename = "input.txt";
      watch.Start();
      day.Run();
      watch.Stop();
      day.InputStream = null!;
      input.Dispose();

      Console.WriteLine($"Elapsed time: {watch.ElapsedMilliseconds} ms");
      if (day.Part1Complete)
        Console.WriteLine($"Part 1 answer: {day.Part1Answer}");
      if (day.Part2Complete)
        Console.WriteLine($"Part 2 answer: {day.Part2Answer}");
    }
  }

  private static void PrintTestResult(string givenAnswer, string? expectedAnswer, int whichPart, ref int passCounter,
    ref int failCounter, ref bool? passStatus)
  {
    if (expectedAnswer != "" && expectedAnswer != "?")
    {
      Console.Write($"Part {whichPart} / Expected answer: {expectedAnswer} / Actual answer: {givenAnswer} / Result: ");
      if (expectedAnswer == givenAnswer)
      {
        passStatus = (passStatus != false);
        passCounter += 1;
        Console.WriteLine("Pass");
      }
      else
      {
        passStatus = false;
        failCounter += 1;
        Console.WriteLine("FAIL");
      }
    }
    else
    {
      Console.WriteLine($"Part {whichPart} / Given answer: {givenAnswer}");
    }
  }
}