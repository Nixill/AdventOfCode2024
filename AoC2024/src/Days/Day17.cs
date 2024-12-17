using System.Text.RegularExpressions;

namespace Nixill.AdventOfCode;

public class Day17 : AdventDay
{
  static Regex RegisterValue = new Regex(@"Register (.+): (\d+)");
  static Regex ProgramValue = new Regex(@"Program: ((?:\d+,)+\d+)");

  public int[] Program { get; private set; } = [];

  public override void Run(StreamReader input)
  {
    string[] registers = input.GetLinesOfChunk().ToArray();

    long a = GetRegisterValue(registers[0]);
    long b = GetRegisterValue(registers[1]);
    long c = GetRegisterValue(registers[2]);

    string programLine = input.GetEverything();
    Program = ProgramValue.Match(programLine).Groups[1].Value.Split(",").Select(int.Parse).ToArray();

    Part1String = string.Join(',', RunProgram(a, b, c));
  }

  static long GetRegisterValue(string line)
    => long.Parse(RegisterValue.Match(line).Groups[2].Value);

  public IEnumerable<int> RunProgram(long a, long b, long c)
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
          a = a >> (int)GetComboParam(parameter, a, b, c);
          break;
        case 1: // BXL
          b = b ^ parameter;
          break;
        case 2: // BST
          b = GetComboParam(parameter, a, b, c) & 7;
          break;
        case 3: // JNZ
          if (a != 0) pointer = parameter;
          break;
        case 4: // BXC
          b = b ^ c;
          break;
        case 5: // OUT
          yield return (int)GetComboParam(parameter, a, b, c) & 7;
          break;
        case 6: // BDV
          b = a >> (int)GetComboParam(parameter, a, b, c);
          break;
        case 7: // CDV
          c = a >> (int)GetComboParam(parameter, a, b, c);
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