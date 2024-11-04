namespace Lab_2_PA
{
    class Program
    {
        public static void Main(string[] args)
        {
            EightQueensAStar eightQueensAStar = new EightQueensAStar();
            Console.WriteLine("A* algorithm:");
            eightQueensAStar.Solve();
            
            EightQueensIDS eightQueensIDS = new EightQueensIDS();
            Console.WriteLine("IDS algorithm:");
            eightQueensIDS.Solve();
        }
    }
}

