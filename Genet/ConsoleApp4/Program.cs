using System;

namespace ConsoleApp4
{
    public class Program
    {
        public static void Main(string[] args)
        {
            PrintSolution(
                new Solver(new[] { 1, 1, 2, 1, 1, 0, 0, 1, 0, 0, 2, 2, 0, 0, 0, 0, 2, 0, 0, 1, 0, 2, 2, 1, 0 }, -50));
            PrintSolution(
                new Solver(new[] { 0, 2, 1, 1, 0, 0, 0, 1, 1, 0, 0, 1, 0, 2, 2, 1, 2, 2, 1, 1, 1, 0, 0, 0, 0 }, 13));
        }

        private static void PrintSolution(Solver solver)
        {
            var solution = solver.Solve();
            Console.WriteLine($"Solution: X = {solution.X}, Y = {solution.Y}, Z = {solution.Z}, U = {solution.U}, W = {solution.W}");
            Console.WriteLine($"Solution check: deviation = {solver.Deviation(solution)}");
        }
    }
}
