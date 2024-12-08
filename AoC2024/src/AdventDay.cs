public abstract class AdventDay
{
  public required StreamReader InputStream { protected get; set; }

  protected string? _Part1Answer = null;
  protected string? _Part2Answer = null;

  public bool Part1Complete { get; private set; } = false;
  public bool Part2Complete { get; private set; } = false;

  public abstract void Run();

  public string Part1Answer
  {
    get => Part1Complete ? (_Part1Answer ?? _Part1Number.ToString()) : throw new InvalidOperationException("Part 1 is not yet complete!");
    protected set
    {
      _Part1Answer = value;
      Part1Complete = true;
    }
  }

  public string Part2Answer
  {
    get => Part2Complete ? (_Part2Answer ?? _Part2Number.ToString()) : throw new InvalidOperationException("Part 2 is not yet complete!");
    protected set
    {
      _Part2Answer = value;
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