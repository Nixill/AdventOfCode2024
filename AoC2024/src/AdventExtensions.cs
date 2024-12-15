using Nixill.Collections;
using Nixill.Utils.Extensions;

public static class AdventExtensions
{
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

  public static IEnumerable<IEnumerable<string>> GetChunksByLine(this StreamReader input)
  {
    while (!input.EndOfStream)
    {
      yield return input.GetLinesOfChunk();
    }
  }

  public static IEnumerable<string> GetWholeChunks(this StreamReader input)
  {
    while (!input.EndOfStream)
    {
      yield return string.Join('\n', input.GetLinesOfChunk());
    }
  }

  internal static T AssignTo<T>(this T input, out T variable)
  {
    variable = input;
    return input;
  }

  public static Grid<char> CharacterGrid(this StreamReader input)
    => new Grid<char>(input.GetAllLines());

  public static Grid<char> CharacterGridChunk(this StreamReader input)
    => new Grid<char>(input.GetLinesOfChunk());

  public static Grid<T> Grid<T>(this StreamReader input, Func<char, T> mutator)
    => new Grid<T>(input.GetAllLines().Select(s => s.Select(mutator)));

  public static Grid<T> GridChunk<T>(this StreamReader input, Func<char, T> mutator)
    => new Grid<T>(input.GetLinesOfChunk().Select(s => s.Select(mutator)));

  public static void PrintGrid(this IGrid<char> chars)
    => chars.Do(l => Console.WriteLine(l.FormString()));
}
