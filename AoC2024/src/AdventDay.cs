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

  public string Part2Answer
  {
    get => Part2Complete ? _Part2Answer! : throw new InvalidOperationException("Part 2 is not yet complete!");
    protected set
    {
      _Part2Answer = value;
      Part2Complete = true;
    }
  }
}