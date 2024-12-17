using System.Text.RegularExpressions;

namespace Nixill.AdventOfCode;

public class Day17A : AdventDay
{
  static Regex RegisterValue = new Regex(@"Register (.): (\d+)");
  static Regex ProgramValue = new Regex(@"Program: ((?:\d+,)+\d+)");

  public override void Run(StreamReader input)
  {
    Dictionary<string, long> registers = [];
    foreach (string line in input.GetLinesOfChunk())
    {
      Match mtc = RegisterValue.Match(line);
      string register = mtc.Groups[1].Value;
      string value = mtc.Groups[2].Value;

      registers[register] = long.Parse(value);
    }

    string programLine = input.GetEverything();
    Match programMatch = ProgramValue.Match(programLine);
    int[] program = programMatch.Groups[1].Value.Split(",").Select(int.Parse).ToArray();

    int[] output = Run(registers, program).ToArray();
    Part1String = string.Join(',', output);

    // if (SkipPart2) return;

    // for (int a = 0; true; a++)
    // {
    //   registers["A"] = a;

    //   int i = 0;
    //   foreach (int o in Run(registers, program))
    //   {
    //     if (i >= program.Length) goto nextA;
    //     if (o != program[i]) goto nextA;
    //     i++;
    //   }

    //   if (i < program.Length) goto nextA;

    //   Part2Number = a;
    //   break;

    // nextA:;
    // }
  }

  static IEnumerable<int> Run(Dictionary<string, long> registers, int[] program)
  {
    long a = registers["A"];
    long b = registers["B"];
    long c = registers["C"];

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