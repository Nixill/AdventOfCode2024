using Nixill.Collections;
using Nixill.Utils.Extensions;

namespace Nixill.AdventOfCode;

public class Day18 : AdventDay
{
  List<IntVector2> BlocksToFall = [];
  Grid<char> PlayingField = [];
  IntVector2 LastBlockFallen = (-1, -1);
  IntVector2 PlayingFieldSize;

  public override void Run(StreamReader input)
  {
    PlayingFieldSize = // ←──────────────────────────────────────────────────┐
      input // a StreamReader                                                │
      .ReadLine()! // read the first line which'll be something like "70x70" │
      .Split('x') // split to an array ["70", "70"]                          │
      .Select(int.Parse) // parse each element to a number [70, 70]          │
      .Double(); // convert a two-element enumerable to tuple (70, 70)       │
                 // cast (int, int) to an IntVector2 ────────────────────────┘

    PlayingField = new Grid<char>(PlayingFieldSize, '.');

    int part1Blocks = int.Parse(input.ReadLine()!);
    // int speed = int.Parse(input.ReadLine()!);

    BlocksToFall = input.GetLines()
      .Select(l => (IntVector2)(l.Split(',').Select(int.Parse).Double()))
      .ToList();

    foreach (int i in Enumerable.Range(0, part1Blocks))
    {
      PlayingField[BlocksToFall.Pop()] = '#';
    }

    Part1Number = FindPaths();

    int zero = 0; // this line exists to have a place I can put a logpoint
    zero = zero + 0; // and this line exists to kill a warning.

    while (BlocksToFall.Count > 0)
    {
      LastBlockFallen = BlocksToFall.Pop();
      PlayingField[LastBlockFallen] = '#';

      if (PlayingField[LastBlockFallen] == 'O')
      {
        ResetPaths();
        int test = FindPaths();
        if (test == int.MaxValue)
        {
          Part2String = $"{LastBlockFallen.X},{LastBlockFallen.Y}";
          return;
        }
      }
    }

    Part2String = "Not found.";
  }

  public int FindPaths()
  {
    Grid<int> steps = new Grid<int>(PlayingFieldSize, iv2 => PlayingField[iv2] == '#' ? -1 : int.MaxValue);

    int currentStep = 0;
    HashSet<IntVector2> currentStepQueue = [(0, 0)];
    HashSet<IntVector2> nextStepQueue = [];

    int answer = int.MaxValue;

    while (currentStepQueue.Count > 0)
    {
      IntVector2 current = currentStepQueue.First();
      currentStepQueue.Remove(current);

      if (steps[current] > currentStep)
      {
        steps[current] = currentStep;
        foreach (IntVector2 next in steps.OrthogonallyAdjacentRefs(current))
        {
          nextStepQueue.Add(next);
        }
      }

      if (current == PlayingFieldSize - (1, 1))
      {
        answer = currentStep;
        break;
      }

      if (currentStepQueue.Count == 0)
      {
        if (nextStepQueue.Count == 0) return int.MaxValue;
        (currentStepQueue, nextStepQueue) = (nextStepQueue, []);
        currentStep++;
      }
    }

    currentStepQueue = [PlayingFieldSize - (1, 1)];
    nextStepQueue = [];

    while (currentStepQueue.Count > 0)
    {
      IntVector2 current = currentStepQueue.First();
      currentStepQueue.Remove(current);

      if (steps[current] == currentStep)
      {
        PlayingField[current] = 'O';
        // eh, it can be either grid here
        foreach (IntVector2 next in steps.OrthogonallyAdjacentRefs(current))
        {
          nextStepQueue.Add(next);
        }
      }

      if (currentStepQueue.Count == 0)
      {
        if (nextStepQueue.Count == 0) break;
        (currentStepQueue, nextStepQueue) = (nextStepQueue, []);
        currentStep--;
      }
    }

    return answer;
  }

  public void ResetPaths()
  {
    foreach ((char item, IntVector2 position) in PlayingField.Flatten())
    {
      if (item == 'O') PlayingField[position] = 'X';
    }
  }
}