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

  internal static T AssignTo<T>(this T input, out T variable)
  {
    variable = input;
    return input;
  }
}
