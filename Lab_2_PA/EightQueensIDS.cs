public class EightQueensIDS
{
    private readonly Random _random = new();
    private readonly int _maxDepth;

    public EightQueensIDS(int maxDepth = 8)
    {
        _maxDepth = maxDepth;
    }

    public void Solve()
    {
        int[] initialState = GenerateRandomStartState();
        Console.WriteLine("Initial State: " + string.Join(", ", initialState));

        int steps = 0;
        int deadEnds = 0;
        int generatedStates = 0;
        int maxMemoryUsage = 0;

        for (int depth = 0; depth <= _maxDepth; depth++)
        {
            int[] result = DepthLimitedSearch(initialState, depth, ref steps, ref deadEnds, ref generatedStates, ref maxMemoryUsage);
            if (result != null)
            {
                Console.WriteLine($"Solution found on depth {depth}");
                PrintBoard(result);
                PrintStatistics(steps, deadEnds, generatedStates, maxMemoryUsage);
                return;
            }
            Console.WriteLine($"Solution not found on depth {depth}");
        }
    }

    private int[] DepthLimitedSearch(int[] state, int limit, ref int steps, ref int deadEnds, ref int generatedStates, ref int maxMemoryUsage)
    {
        var stack = new Stack<(int[] state, int depth)>();
        var visited = new HashSet<string>();
        stack.Push((state, 0));
        visited.Add(GetStateKey(state));

        maxMemoryUsage = Math.Max(maxMemoryUsage, stack.Count + visited.Count);

        while (stack.Count > 0)
        {
            var (currentState, depth) = stack.Pop();
            int h = Heuristic(currentState);

            if (h == 0) // Solution found
                return currentState;

            if (depth < limit)
            {
                bool deadEnd = true;

                for (int i = 0; i < currentState.Length; i++)
                {
                    for (int j = 0; j < currentState.Length; j++)
                    {
                        steps++;
                        if (currentState[i] != j)
                        {
                            int[] neighbor = (int[])currentState.Clone();
                            neighbor[i] = j;

                            string neighborKey = GetStateKey(neighbor);
                            if (!visited.Contains(neighborKey))
                            {
                                stack.Push((neighbor, depth + 1));
                                visited.Add(neighborKey);
                                generatedStates++;
                                deadEnd = false;
                            }
                        }
                    }
                }

                if (deadEnd)
                    deadEnds++;
            }

            maxMemoryUsage = Math.Max(maxMemoryUsage, stack.Count + visited.Count);
        }
        return null!; // No solution found within this depth
    }

    private int Heuristic(int[] state)
    {
        int h = 0;
        int[] rowConflict = new int[state.Length];
        int[] diagConflict1 = new int[state.Length * 2];
        int[] diagConflict2 = new int[state.Length * 2];

        for (int i = 0; i < state.Length; i++)
        {
            rowConflict[state[i]]++;
            diagConflict1[i + state[i]]++;
            diagConflict2[i - state[i] + state.Length - 1]++;
        }

        for (int i = 0; i < state.Length; i++)
        {
            if (rowConflict[state[i]] > 1) h += rowConflict[state[i]] - 1;
            if (diagConflict1[i + state[i]] > 1) h += diagConflict1[i + state[i]] - 1;
            if (diagConflict2[i - state[i] + state.Length - 1] > 1) h += diagConflict2[i - state[i] + state.Length - 1] - 1;
        }

        return h;
    }

    private int[] GenerateRandomStartState()
    {
        int[] state = new int[8];
        for (int i = 0; i < state.Length; i++)
        {
            state[i] = _random.Next(0, 8);
        }
        return state;
    }

    private string GetStateKey(int[] state)
    {
        return string.Join(",", state);
    }

    private void PrintStatistics(int steps, int deadEnds, int generatedStates, int maxMemoryUsage)
    {
        Console.WriteLine("\nStatistics:");
        Console.WriteLine($"Steps: {steps}");
        Console.WriteLine($"Dead ends: {deadEnds}");
        Console.WriteLine($"Generated states: {generatedStates}");
        Console.WriteLine($"Max memory usage (unique states): {maxMemoryUsage}");
    }

    private static void PrintBoard(int[] board)
    {
        int size = board.Length;

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                Console.Write(board[row] == col ? "Q " : ". ");
            }
            Console.WriteLine();
        }
    }
}
