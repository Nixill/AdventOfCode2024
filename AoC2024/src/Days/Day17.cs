using System.Text.RegularExpressions;

namespace Nixill.AdventOfCode;

public class Day17 : AdventDay
{
  static Regex RegisterValue = new Regex(@"Register (.): (\d+)");
  static Regex ProgramValue = new Regex(@"Program: ((?:\d+,)+\d+)");

  public override void Run()
  {
    Dictionary<string, long> registers = [];
    foreach (string line in InputStream.GetLinesOfChunk())
    {
      Match mtc = RegisterValue.Match(line);
      string register = mtc.Groups[1].Value;
      string value = mtc.Groups[2].Value;

      registers[register] = long.Parse(value);
    }

    string programLine = InputStream.GetEverything();
    Match programMatch = ProgramValue.Match(programLine);
    int[] program = programMatch.Groups[1].Value.Split(",").Select(int.Parse).ToArray();

    int[] output = SimulatePart1(registers, program).ToArray();
    Part1String = string.Join(',', output);
  }

  static IEnumerable<int> SimulatePart1(Dictionary<string, long> registers, int[] program)
  {
    long a = registers["A"];
    long b = registers["B"];
    long c = registers["C"];

    program = program.ToArray();

    int pointer = 0;

    int opcode = 0, parameter = 0;

    while (pointer < program.Length)
    {
      opcode = program[pointer++];
      parameter = program[pointer++];

      switch (opcode)
      {
        case 0: // ADV
          a = a >> (int)GetComboParam(parameter, a, b, c);
          break;
        case 1: // BXL
          b = b ^ parameter;
          break;
        case 2: // BST
          b = GetComboParam(parameter, a, b, c) & 8;
          break;
        case 3: // JNZ
          if (a != 0) pointer = parameter;
          break;
        case 4: // BXC
          b = b ^ c;
          break;
        case 5: // OUT
          yield return (int)GetComboParam(parameter, a, b, c) & 8;
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