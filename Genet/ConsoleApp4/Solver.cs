using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp4
{
    public class Solver
    {
        private const int MinValue = -300;
        private const int MaxValue = 300;
        private const int Population = 1000;

        private static readonly Random Rand = new Random();

        private readonly int[] _powers;//массив для степеней
        private readonly int _result;//число в конце

        public Solver(int[] powers, int result)
        {
            _powers = powers;
            _result = result;
        }

        public Vector Solve()//отбор человека
        {
            var vectors = GetStartPopulation();//генерация векторов с рандомными координамтами
            var solution = SolutionFrom(vectors);//вектор, который подходит или null

            while (solution == null)
            {
                vectors = Replace(vectors);
                solution = SolutionFrom(vectors);
            }

            return solution;
        }

        //отклонение
        public long Deviation(Vector vector) => Math.Abs(Pow(vector.X, _powers[0]) * Pow(vector.Y, _powers[1]) * Pow(vector.Z, _powers[2]) * Pow(vector.U, _powers[3]) * Pow(vector.W, _powers[4]) +
            Pow(vector.X, _powers[5]) * Pow(vector.Y, _powers[6]) * Pow(vector.Z, _powers[7]) * Pow(vector.U, _powers[8]) * Pow(vector.W, _powers[9]) +
            Pow(vector.X, _powers[10]) * Pow(vector.Y, _powers[11]) * Pow(vector.Z, _powers[12]) * Pow(vector.U, _powers[13]) * Pow(vector.W, _powers[14]) +
            Pow(vector.X, _powers[15]) * Pow(vector.Y, _powers[16]) * Pow(vector.Z, _powers[17]) * Pow(vector.U, _powers[18]) * Pow(vector.W, _powers[19]) +
            Pow(vector.X, _powers[20]) * Pow(vector.Y, _powers[21]) * Pow(vector.Z, _powers[22]) * Pow(vector.U, _powers[23]) * Pow(vector.W, _powers[24]) - _result);

        private List<Vector> Select(List<Vector> vectors)
        {
            var selectedVectors = new List<Vector>();//берет из популяции векторы, которые станут родителями
            var n = vectors.Count;
            n -= n % 2;//делим по парам
            for (int i = 0; i < n; i += 2)
            {//турнирная селекция
                var participant1 = vectors.ElementAt(i);
                var participant2 = vectors.ElementAt(i + 1);
                selectedVectors.Add(Deviation(participant1) < Deviation(participant2) ? participant1 : participant2);
            }

            return selectedVectors;
        }

        private List<Vector> Crossover(List<Vector> vectors)
        {
            var children = new List<Vector>();//скрещивание. Берем родителей, получаются потомки
            var n = vectors.Count;
            n -= n % 2;//нужна пара родителей для ребенка(если нечетное количество - откидываем)

            for (int i = 0; i < n; i += 2)
            {
                var parent1 = vectors.ElementAt(i);
                var parent2 = vectors.ElementAt(i + 1);
                children.Add(new Vector
                {//подкидываем монетку
                    U = RandomBool(0.5) ? parent1.U : parent2.U,
                    W = RandomBool(0.5) ? parent1.W : parent2.W,
                    X = RandomBool(0.5) ? parent1.X : parent2.X,
                    Y = RandomBool(0.5) ? parent1.Y : parent2.Y,
                    Z = RandomBool(0.5) ? parent1.Z : parent2.Z
                });
            }

            return children;
        }

        private List<Vector> Mutate(List<Vector> vectors)
        {
            var mutatedVectors = new List<Vector>();//список мутированных детей
            var orderedVectors = vectors.OrderBy(Deviation);//список отсотрированных по отклонению детей
            var n = vectors.Count;

            var bestFitMutationProbability = 0.2;
            var worstFitMutationProbability = 0.7;

            //половина худших - 0.7, половина лучших - 0.2
            mutatedVectors.AddRange(orderedVectors.Take(n / 2).Select(vector => new Vector
            {
                U = RandomBool(bestFitMutationProbability) ? Rand.Next(MinValue, MaxValue) : vector.U,//мутируем ген с 0.2
                W = RandomBool(bestFitMutationProbability) ? Rand.Next(MinValue, MaxValue) : vector.W,//мутация гена - замена на рандомное число оот -300 до 300
                X = RandomBool(bestFitMutationProbability) ? Rand.Next(MinValue, MaxValue) : vector.X,
                Y = RandomBool(bestFitMutationProbability) ? Rand.Next(MinValue, MaxValue) : vector.Y,
                Z = RandomBool(bestFitMutationProbability) ? Rand.Next(MinValue, MaxValue) : vector.Z
            }));

            mutatedVectors.AddRange(orderedVectors.TakeLast(n - n / 2).Select(vector => new Vector
            {
                U = RandomBool(worstFitMutationProbability) ? Rand.Next(MinValue, MaxValue) : vector.U,//c худшими так же
                W = RandomBool(worstFitMutationProbability) ? Rand.Next(MinValue, MaxValue) : vector.W,
                X = RandomBool(worstFitMutationProbability) ? Rand.Next(MinValue, MaxValue) : vector.X,
                Y = RandomBool(worstFitMutationProbability) ? Rand.Next(MinValue, MaxValue) : vector.Y,
                Z = RandomBool(worstFitMutationProbability) ? Rand.Next(MinValue, MaxValue) : vector.Z
            }));

            return mutatedVectors;
        }

        private List<Vector> Replace(List<Vector> pastGeneration)
        {
            var nextGeneration = Mutate(Crossover(Select(pastGeneration)));//из прошлого поколения и из потомков выбираем особоей, которые пойдут в следующее поколоение

            while (nextGeneration.Count < Population)
            {
                Shuffle(pastGeneration);//рандомно перемешивает поколоение предыдушее для разного потомства
                nextGeneration.AddRange(Mutate(Crossover(Select(pastGeneration))));
            }

            return nextGeneration.OrderBy(Deviation).Take(Population).ToList();//сортируем по отклоению и берем количество популяции
        }

        private List<Vector> GetStartPopulation() => Enumerable.Range(0, Population)
            .Select(_ => Vector.Generate(Rand, MinValue, MaxValue))
            .ToList();

        private long Pow(long x, int pow)
        {
            long result = 1;
            for (int i = 0; i < pow; i++) result *= x;

            return result;
        }
#nullable enable
        private Vector? SolutionFrom(List<Vector> orderedVectors)
        {
            var bestVector = orderedVectors.First();
            return Deviation(bestVector) > 0 ? null : bestVector;
        }
#nullable disable

        private bool RandomBool(double trueProbability) => Rand.NextDouble() < trueProbability;

        private void Shuffle(List<Vector> vectors)
        {
            int n = vectors.Count;
            while (n > 1)
            {
                n--;
                int k = Rand.Next(n + 1);
                (vectors[k], vectors[n]) = (vectors[n], vectors[k]);
            }
        }
    }
}
