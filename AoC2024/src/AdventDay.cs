public abstract class AdventDay
{
  public required StreamReader InputStream { protected get; set; }

  protected string? _Part1Answer;
  protected string? _Part2Answer;

  public bool Part1Complete { get; private set; } = false;
  public bool Part2Complete { get; private set; } = false;

  public abstract void Run();

  public string Part1Answer
  {
    get => Part1Complete ? _Part1Answer! : throw new InvalidOperationException("Part 1 is not yet complete!");
    protected set
    {
      _Part1Answer = value;
      Part1Complete = true;
    }
  }

  long P1Num = 0;
  protected long Part1Number
  {
    set
    {
      P1Num = value;
      Part1Answer = value.ToString();
    }
    get => P1Num;
  }

  public string Part2Answer
  {
    get => Part2Complete ? _Part2Answer! : throw new InvalidOperationException("Part 2 is not yet complete!");
    protected set
    {
      _Part2Answer = value;
      Part2Complete = true;
    }
  }

  long P2Num = 0;
  protected long Part2Number
  {
    set
    {
      P2Num = value;
      Part2Answer = value.ToString();
    }
    get => P2Num;
  }
}