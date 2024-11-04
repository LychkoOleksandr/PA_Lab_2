public class EightQueensAStar
{
    private Random random = new Random();

    public void Solve()
    {
        // Generate a random initial state
        int[] initialState = GenerateRandomStartState();
        Console.WriteLine("Initial State: " + string.Join(", ", initialState));

        // Initialize A* search variables
        var openSet = new PriorityQueue<(int[] state, int g, int h), int>();
        var visited = new HashSet<string>();
        var deadEndStates = new HashSet<string>();

        int initialH = Heuristic(initialState);
        openSet.Enqueue((initialState, 0, initialH), initialH);
        visited.Add(GetStateKey(initialState));

        // Statistics variables
        int steps = 0;
        int deadEnds = 0;
        int generatedStates = 0;
        int maxMemoryUsage = 0;

        // A* search process
        while (openSet.Count > 0)
        {
            var (currentState, g, h) = openSet.Dequeue();
            steps++;
            maxMemoryUsage = Math.Max(maxMemoryUsage, openSet.Count + visited.Count);

            // Check if the goal state is reached
            if (h == 0)
            {
                Console.WriteLine("Solution Found!");
                PrintBoard(currentState);
                PrintStatistics(steps, deadEnds, generatedStates, maxMemoryUsage);
                return;
            }

            bool deadEnd = true;

            // Generate neighbors by moving each queen in each column to a new row
            for (int i = 0; i < currentState.Length; i++)
            {
                for (int j = 0; j < currentState.Length; j++)
                {
                    steps++;

                    if (currentState[i] != j)
                    {
                        int[] neighbor = (int[])currentState.Clone();
                        neighbor[i] = j;
                        generatedStates++;
                        string neighborKey = GetStateKey(neighbor);
                        if (!visited.Contains(neighborKey))
                        {
                            int newH = Heuristic(neighbor);
                            openSet.Enqueue((neighbor, g + 1, newH), g + 1 + newH * 2); 
                            visited.Add(neighborKey);
                            deadEnd = false;
                        }
                    }
                }
            }

            // Track dead ends if no promising neighbors found
            if (deadEnd)
            {
                deadEnds++;
                deadEndStates.Add(GetStateKey(currentState));
            }
        }

        Console.WriteLine("Solution not found!");
        PrintStatistics(steps, deadEnds, generatedStates, maxMemoryUsage);
    }

    private int Heuristic(int[] state)
    {
        int h = 0;
        int[] rowConflict = new int[state.Length];
        int[] diagConflict1 = new int[state.Length * 2];
        int[] diagConflict2 = new int[state.Length * 2];

        // Track conflicts
        for (int i = 0; i < state.Length; i++)
        {
            rowConflict[state[i]]++;
            diagConflict1[i + state[i]]++;
            diagConflict2[i - state[i] + state.Length - 1]++;
        }

        // Count conflicts in each row and diagonal
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
            state[i] = random.Next(0, 8);
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
