using System;
using System.Collections;
using System.Linq;

namespace BinarySearch
{
    class Program
    {
        static int BinarySearch(int[] arr, int left, int right, int needValue, ref int comparisionCount) 
        { 
            if (right >= left) { 
                int mid = (right + left) / 2;
                if (arr[mid] == needValue)
                {
                    comparisionCount++;
                    return mid;
                }
                else if (arr[mid] > needValue)
                {
                    comparisionCount++;
                    return BinarySearch(arr, left, mid - 1, needValue, ref comparisionCount);
                }
                else
                {
                    comparisionCount++;
                    return BinarySearch(arr, mid + 1, right, needValue, ref comparisionCount);
                }
            }
            return -1; 
        }

        static int InterpolationSearch(int[] arr, int start, int end, int needValue, ref int comparisionCount) 
        { 
            if (end >= start) { 
                int mid = start + ((end - start) * (needValue - arr[0])) / (arr[end] - arr[0]);
                if (arr[mid] == needValue)
                {
                    comparisionCount++;
                    return mid;
                }
                else if (arr[mid] > needValue)
                {
                    comparisionCount++;
                    return InterpolationSearch(arr, start, mid - 1, needValue, ref comparisionCount);
                }
                else
                {
                    comparisionCount++;
                    return InterpolationSearch(arr, mid + 1, end, needValue, ref comparisionCount);
                }
            }
            return -1; 
        }
        
        static void Main(string[] args)
        {            
            const int sizeOfArray = 100000, minRange = 0, maxRange = 100000; 
            int[] arr = new int[sizeOfArray];
            const int maxSize = sizeOfArray - 1, minSize = 0;
            Random rand = new Random();
            for (int i = 0; i < sizeOfArray; i++)
            {
                arr[i] = rand.Next(minRange, maxRange);
            }

            Array.Sort(arr);
            int comparisonCountForBinarySearch = 0;
            int comparisonCountForInterpolationSearch = 0;
            const int needValue = 169;
            if (!arr.Contains(needValue))
            {
                throw new ArgumentException($"This array doesn't contain your number {needValue}");
            }

            foreach (int elem in arr)
            {
                Console.Write($"{elem} ");
            }

            Console.WriteLine("");
            int resultForBin = BinarySearch(arr, minSize, maxSize, needValue, ref comparisonCountForBinarySearch); 
            int resultForInt = InterpolationSearch(arr, minSize, maxSize, needValue, ref comparisonCountForInterpolationSearch); 
            Console.WriteLine($"{comparisonCountForBinarySearch} - количество сравнений в бинарном поиске");
            Console.WriteLine($"{resultForBin} - номер элемента в массиве при бинарном поиске");
            Console.WriteLine($"{comparisonCountForInterpolationSearch} - количество сравнений в интерполяционном поиске");
            Console.WriteLine($"{resultForInt} - номер элемента в массиве при интерполяционном поиске");
        }
    }
}