using Nixill.Utils;

namespace Nixill.AdventOfCode;

public class Day4 : AdventDay
{
  public override void Run()
  {
    IEnumerable<char[]> gridBase = InputStream.GetAllLines()
      .Select(s => s.Prepend('.').Append('.').ToArray());
    
    char[][] grid = gridBase
      .Prepend(Enumerable.Repeat('.', gridBase.First().Length).ToArray())
      .Append(Enumerable.Repeat('.', gridBase.First().Length).ToArray())
      .ToArray();

    int answer1 = 0;
    int answer2 = 0;

    for (int r = 0; r < grid.Length; r++)
    {
      for (int c = 0; c < grid[r].Length; c++)
      {
        if (grid[r][c] == 'X')
        {
          answer1 += Find(grid, r, c, "XMAS");
        }

        if (grid[r][c] == 'A')
        {
          answer2 += Xmas(grid, r, c);
        }
      }
    }

    Part1Answer = answer1.ToString();
    Part2Answer = answer2.ToString();
  }

  static int Find(char[][] grid, int r, int c, string word)
  {
    int found = 0;

    foreach (int dr in Enumerable.Range(-1, 3))
    {
      foreach (int dc in Enumerable.Range(-1, 3))
      {
        if (dr != 0 || dc != 0)
        {
          if (IsWord(grid, r, c, word, dr, dc)) found += 1;
        }
      }
    }

    return found;
  }

  static bool IsWord(char[][] grid, int r, int c, string word, int dr, int dc)
  {
    foreach (char chr in word)
    {
      if (grid[r][c] != chr) return false;
      r += dr;
      c += dc;
    }
    return true;
  }

  static int Xmas(char[][] grid, int r, int c)
  {
    
  }
}