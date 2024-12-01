﻿using System.Diagnostics;
using System.Reflection;

namespace Nixill.AdventOfCode;

public static class Program
{
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
    string ns = Assign(typeof(Program).Namespace, out string? nsQuestion) == null ? "" : nsQuestion! + ".";

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

    if (dayType == null)
    {
      Console.WriteLine("This day hasn't been written yet!");
      return;
    }

    Console.WriteLine(Directory.GetCurrentDirectory());

    if (!Directory.Exists($"AoC2024Data/day{which}"))
    {
      Console.WriteLine("This day's data directory doesn't exist!");
      return;
    }

    MethodInfo? part1Method = dayType.GetMethod("Part1", new Type[] { typeof(string), typeof(StreamReader) });
    if (part1Method != null)
    {
      var del1 = part1Method.CreateDelegate<Func<string, StreamReader, string>>();
      Console.WriteLine("Running part 1...");
      if (RunTests(which, 1, del1))
      {
        RunCode(which, 1, del1);
      }
      Console.WriteLine();
    }

    MethodInfo? part2Method = dayType.GetMethod("Part2", new Type[] { typeof(string), typeof(StreamReader) });
    if (part2Method != null)
    {
      var del2 = part2Method.CreateDelegate<Func<string, StreamReader, string>>();
      Console.WriteLine("Running part 2...");
      if (RunTests(which, 2, del2))
      {
        RunCode(which, 2, del2);
      }
      Console.WriteLine();
    }
  }

  static bool RunTests(string day, int part, Func<string, StreamReader, string> method)
  {
    string[] examples = Directory.GetFiles($"AoC2024Data/day{day}", "example*.txt");
    bool tested = false;

    foreach (string fpath in examples)
    {
      using StreamReader input = new(File.OpenRead(fpath));

      // The first two lines are the expected answers
      string? p1Answer = input.ReadLine();
      string? p2Answer = input.ReadLine();
      string? answer = (part == 1) ? p1Answer : p2Answer;

      // A blank line means that this input file doesn't apply to this part
      if (answer == "") continue;

      // This is for timing stuff.
      Stopwatch watch = new();

      // Now perform the test!
      tested = true;
      string fname = (new FileInfo(fpath)).Name;
      watch.Start();
      string given = method(fname, input);
      watch.Stop();

      // Output the results!
      Console.Write($"Test file: {fname} / ");
      if (answer != "?") Console.Write($"Expected output: {answer} / Actual output: ");
      else Console.Write($"Output: ");
      Console.WriteLine($"{given} / Time: {watch.ElapsedMilliseconds} ms");

      if (given != answer && answer != "?") return false;
    }

    if (!tested) Console.WriteLine("No test cases were provided (if this is part 2, did you remember to add answers?)! Continuing anyway...");
    else Console.WriteLine("All tests passed!");

    return true;
  }

  static void RunCode(string day, int part, Func<string, StreamReader, string> method)
  {
    using StreamReader input = new(File.OpenRead($"AoC2024Data/day{day}/input.txt"));

    Stopwatch watch = new();
    watch.Start();
    string answer = method("input.txt", input);
    watch.Stop();

    Console.WriteLine($"Result on puzzle input: {answer} / Time: {watch.ElapsedMilliseconds} ms");
  }

  // For convenience:
  public static IEnumerable<string> GetLines(this StreamReader input)
  {
    bool blankLine = false;
    for (string? line = input.ReadLine(); line != null; line = input.ReadLine())
    {
      if (blankLine)
      {
        yield return "";
        blankLine = false;
      }

      if (line == "")
      {
        blankLine = true;
      }
      else
      {
        yield return line;
      }
    }
  }

  public static string[] GetAllLines(this StreamReader input) => input.GetLines().ToArray();

  public static string GetEverything(this StreamReader input)
  {
    string inp = input.ReadToEnd();
    if (inp.EndsWith("\r\n")) return inp[..^2];
    else if (inp.EndsWith("\n")) return inp[..^1];
    else return inp;
  }

  public static IEnumerable<string> GetLinesOfChunk(this StreamReader input)
  {
    for (string? line = input.ReadLine(); line != null; line = input.ReadLine())
    {
      if (line == "")
      {
        yield break;
      }
      else
      {
        yield return line;
      }
    }
  }

  private static T Assign<T>(T input, out T variable)
  {
    variable = input;
    return input;
  }

  internal static T AssignTo<T>(this T input, out T variable)
  {
    variable = input;
    return input;
  }
}