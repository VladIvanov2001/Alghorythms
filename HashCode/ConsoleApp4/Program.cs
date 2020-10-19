using System;
using System.Collections.Generic;

namespace HashTable
{
    // для цепочек переполнения ↓
    class ChainCell
    {
        public int hash;
        public List<int> chain = new List<int>(0);

        public ChainCell(int hash)
        {
            this.hash = hash;
        }
    }
    
    // для линейного зондирования и двойного хеширования ↓

    class Cell
    {
        public int hash;
        public int key = -1;

        public Cell(int hash)
        {
            this.hash = hash;
        }
    }

    class Program
    {
        // для цепочек переполнения ↓
        static string ChainToString(List<int> list)
        {
            string listStr = string.Empty;

            foreach (int key in list)
            {
                listStr += $"{key} ";
            }

            return listStr;
        }

        static int HashFunction(int key, double constant, int tableSize)
        {
            double kAFractionPart = key * constant - Math.Floor(key * constant);
            return (int)(kAFractionPart * tableSize);
        }
        
        // для двойного хеширования ↓

        static int SecondHashFunction(int key, int tableSize)
        {
            // придумать свою (я думаю, можно и эту оставить)
            int remainder = key % tableSize;
            return remainder == 0 ? 1 : remainder;
        }
        
        // для двойного хеширования ↓

        static int DoubledHashFunction(int key, double constant, int tableSize, int i)
        {
            return (HashFunction(key, constant, tableSize) + i * SecondHashFunction(key, tableSize)) % tableSize;
        }

        static int MaxCollision(ChainCell[] ocHashTable)
        {
            int maxCollision = 0;

            foreach (ChainCell cell in ocHashTable)
            {
                if (cell.chain.Count > maxCollision)
                {
                    maxCollision = cell.chain.Count;
                }
            }

            return maxCollision;
        }

        static int[] GetRandomKeysArray(int size, int keyValueRange)
        {
            int[] keysArray = new int[size];
            Random rnd = new Random();
            
            for (int i = 0; i < size; i++)
            {
                keysArray[i] = rnd.Next(0, keyValueRange);
            }

            return keysArray;
        }

        static void Main(string[] args)
        {
            double knutConstant = (Math.Sqrt(5) - 1) * 0.5;
            
            int keysAmount = 10;
            int keyValueRange = 100;
            int tableSize = 15;

            int[] keysArray = GetRandomKeysArray(keysAmount, keyValueRange);
            
            // overflow chains

            ChainCell[] ocHashTable = new ChainCell[tableSize];

            for (int i = 0; i < tableSize; i++)
            {
                ocHashTable[i] = new ChainCell(i);
            }

            foreach (int key in keysArray)
            {
                int hashValue = HashFunction(key, knutConstant, tableSize);

                ocHashTable[hashValue].chain.Add(key);
            }
            
            Console.WriteLine("Цепочка переполнения:");

            foreach (ChainCell cell in ocHashTable)
            {
                Console.WriteLine(cell.hash + ": " + ChainToString(cell.chain));
            }
            
            //my constant

            // тоже надо подобрать свою, я для примера взял 0.83
            double myConstant = 0.83;

            // число P в условии
            int repetitions = 100;
            int knutConstantIsBetterCounter = 0;
            int myConstantIsBetterCounter = 0;

            for (int p = 0; p < repetitions; p++)
            {
                int[] keysArr = GetRandomKeysArray(keysAmount, keyValueRange);
                
                // max collision with knut constant
                
                ChainCell[] knutConstantHashTable = new ChainCell[tableSize];

                for (int i = 0; i < tableSize; i++)
                {
                    knutConstantHashTable[i] = new ChainCell(i);
                }

                foreach (int key in keysArr)
                {
                    int hashValue = HashFunction(key, knutConstant, tableSize);

                    knutConstantHashTable[hashValue].chain.Add(key);
                }
                
                int knutConstantMaxCollision = MaxCollision(ocHashTable);
                
                // max collision with my constant
                
                ChainCell[] myConstantHashTable = new ChainCell[tableSize];

                for (int i = 0; i < tableSize; i++)
                {
                    myConstantHashTable[i] = new ChainCell(i);
                }

                foreach (int key in keysArr)
                {
                    int hashValue = HashFunction(key, myConstant, tableSize);

                    myConstantHashTable[hashValue].chain.Add(key);
                }
                
                int myConstantMaxCollision = MaxCollision(myConstantHashTable);
                
                // drawing conclusions

                if (knutConstantMaxCollision <= myConstantMaxCollision)
                {
                    knutConstantIsBetterCounter++;
                }
                else
                {
                    myConstantIsBetterCounter++;
                }
            }

            Console.WriteLine($"me vs Knut: {myConstantIsBetterCounter} - {knutConstantIsBetterCounter}");

            // linear probing

            Cell[] lpHashTable = new Cell[tableSize];
            
            for (int i = 0; i < tableSize; i++)
            {
                lpHashTable[i] = new Cell(i);
            }
            
            foreach (int key in keysArray)
            {
                int hashValue = HashFunction(key, knutConstant, tableSize);

                while (lpHashTable[hashValue % tableSize].key != -1)
                {
                    hashValue++;
                }

                lpHashTable[hashValue % tableSize].key = key;
            }

            Console.WriteLine("Линейное зондирование:");
            
            foreach (Cell cell in lpHashTable)
            {
                Console.WriteLine($"{cell.hash}: " + (cell.key == -1 ? "" : $"{cell.key}"));
            }
            
            // double hashing
            
            Cell[] dhHashTable = new Cell[tableSize];
            
            for (int i = 0; i < tableSize; i++)
            {
                dhHashTable[i] = new Cell(i);
            }
            
            foreach (int key in keysArray)
            {
                int i = 0;
                int hashValue = DoubledHashFunction(key, knutConstant, tableSize, i);

                while (dhHashTable[hashValue].key != -1)
                {
                    i += 1;
                    hashValue = DoubledHashFunction(key, knutConstant, tableSize, i);
                }

                dhHashTable[hashValue].key = key;
            }
            
            Console.WriteLine("Двойное хеширование:");
            
            foreach (Cell cell in dhHashTable)
            {
                Console.WriteLine($"{cell.hash}: " + (cell.key == -1 ? "" : $"{cell.key}"));
            }
        }
    }
}