
using Nixill.Collections;
using Nixill.Utils.Extensions;

namespace Nixill.AdventOfCode;

public class Day18 : AdventDay
{
  IntVector2 PlayingFieldSize = (0, 0);

  public override void Run(StreamReader input)
  {
    PlayingFieldSize = // ←──────────────────────────────────────────────────┐
      input // a StreamReader                                                │
      .ReadLine()! // read the first line which'll be something like "70x70" │
      .Split('x') // split to an array ["70", "70"]                          │
      .Select(int.Parse) // parse each element to a number [70, 70]          │
      .Double(); // convert a two-element enumerable to tuple (70, 70)       │
                 // cast (int, int) to an IntVector2 ────────────────────────┘

    int speed = int.Parse(input.ReadLine()!);

    IntVector2[] blocks = input.GetLines()
      .Select(l => (IntVector2)(l.Split(',').Select(int.Parse).Double()))
      .ToArray();

    // Part 1: 1024 blocks fall instantly, nothing else falls
    Part1Number = SimulateGrid(blocks[..1024], 1024);

    // Part 2 guess: All blocks fall at a constant rate
    Part2Number = SimulateGrid(blocks, speed);
  }

  public int SimulateGrid(IntVector2[] blocks, int fallingSpeed)
  {
    Grid<int> steps = new Grid<int>(PlayingFieldSize, int.MaxValue);

    foreach ((IntVector2[] blockfall, int time) in blocks.Chunk(fallingSpeed).WithIndex())
    {
      foreach (IntVector2 block in blockfall)
      {
        steps[block] = time;
      }
    }

    int currentStep = 0;
    HashSet<IntVector2> currentStepQueue = [(0, 0)];
    HashSet<IntVector2> nextStepQueue = [];

    while (currentStepQueue.Count != 0)
    {
      IntVector2 current = currentStepQueue.First();
      currentStepQueue.Remove(current);

      if (current == PlayingFieldSize - (1, 1))
      {
        return currentStep;
      }

      if (steps[current] > currentStep)
      {
        steps[current] = currentStep;
        foreach (IntVector2 next in steps.OrthogonallyAdjacentRefs(current))
        {
          nextStepQueue.Add(next);
        }
      }

      if (currentStepQueue.Count == 0)
      {
        (currentStepQueue, nextStepQueue) = (nextStepQueue, []);
        currentStep++;
      }
    }

    return steps[PlayingFieldSize - (1, 1)];
  }
}