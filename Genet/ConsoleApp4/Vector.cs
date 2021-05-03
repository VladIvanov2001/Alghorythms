using System;

namespace ConsoleApp4
{
    public class Vector
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int U { get; set; }
        public int W { get; set; }

        public static Vector Generate(Random rand, int minValue, int maxValue) => new Vector
        {
            X = rand.Next(minValue, maxValue),
            Y = rand.Next(minValue, maxValue),
            Z = rand.Next(minValue, maxValue),
            U = rand.Next(minValue, maxValue),
            W = rand.Next(minValue, maxValue)
        };
    }
}
