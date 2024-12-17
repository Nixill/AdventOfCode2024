using System.Text.RegularExpressions;

namespace Nixill.AdventOfCode;

public class Day17 : AdventDay
{
  static Regex RegisterValue = new Regex(@"Register (.+): (\d+)");
  static Regex ProgramValue = new Regex(@"Program: ((?:\d+,)+\d+)");

  public long A { get; private set; } = 0;
  public long B { get; private set; } = 0;
  public long C { get; private set; } = 0;

  public int[] Program { get; private set; } = [];

  public override void Run(StreamReader input)
  {
    string[] registers = input.GetLinesOfChunk().ToArray();

    A = GetRegisterValue(registers[0]);
    B = GetRegisterValue(registers[1]);
    C = GetRegisterValue(registers[2]);

    string programLine = input.GetEverything();
    Program = ProgramValue.Match(programLine).Groups[1].Value.Split(",").Select(int.Parse).ToArray();

    Part1String = string.Join(',', RunProgram());
  }

  static long GetRegisterValue(string line)
    => long.Parse(RegisterValue.Match(line).Groups[2].Value);

  public IEnumerable<int> RunProgram(long a, long b, long c)
  {
    (A, B, C) = (a, b, c);
    return RunProgram();
  }

  public IEnumerable<int> RunProgram()
  {
    int pointer = 0;

    int opcode = 0, parameter = 0;

    while (pointer < Program.Length)
    {
      opcode = Program[pointer++];
      parameter = Program[pointer++];

      switch (opcode)
      {
        case 0: // ADV
          A = A >> (int)GetComboParam(parameter, A, B, C);
          break;
        case 1: // BXL
          B = B ^ parameter;
          break;
        case 2: // BST
          B = GetComboParam(parameter, A, B, C) & 7;
          break;
        case 3: // JNZ
          if (A != 0) pointer = parameter;
          break;
        case 4: // BXC
          B = B ^ C;
          break;
        case 5: // OUT
          yield return (int)GetComboParam(parameter, A, B, C) & 7;
          break;
        case 6: // BDV
          B = A >> (int)GetComboParam(parameter, A, B, C);
          break;
        case 7: // CDV
          C = A >> (int)GetComboParam(parameter, A, B, C);
          break;
      }
    }
  }

  static long GetComboParam(int value, long a, long b, long c)
    => value switch
    {
      >= 0 and <= 3 => value,
      4 => a,
      5 => b,
      6 => c,
      _ => throw new InvalidDataException($"Combo param of {value} detected")
    };
}