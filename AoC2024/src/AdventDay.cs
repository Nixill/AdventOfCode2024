public abstract class AdventDay
{
  public required StreamReader InputStream { protected get; set; }

  public bool Part1Complete { get; private set; } = false;
  public bool Part2Complete { get; private set; } = false;

  public abstract void Run();

  string? StrIfNotEmpty(string input) => (input == "") ? null : input;

  public string Part1Answer
    => Part1Complete ? (StrIfNotEmpty(_Part1String) ?? _Part1Number.ToString())
      : throw new InvalidOperationException("Part 1 is not yet complete!");

  public string Part2Answer
    => Part2Complete ? (StrIfNotEmpty(_Part2String) ?? _Part2Number.ToString())
      : throw new InvalidOperationException("Part 2 is not yet complete!");

  string _Part1String = "";
  string _Part2String = "";

  protected string Part1String
  {
    get => _Part1String;
    set
    {
      _Part1String = value;
      Part1Complete = true;
    }
  }

  protected string Part2String
  {
    get => _Part2String;
    set
    {
      _Part2String = value;
      Part2Complete = true;
    }
  }

  long _Part1Number;
  long _Part2Number;

  protected long Part1Number
  {
    get => _Part1Number;
    set
    {
      _Part1Number = value;
      Part1Complete = true;
    }
  }

  protected long Part2Number
  {
    get => _Part2Number;
    set
    {
      _Part2Number = value;
      Part2Complete = true;
    }
  }
}