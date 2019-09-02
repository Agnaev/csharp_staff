using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Sorts
    {
        public static List<int> Selection(List<int> arr)
        {
            var indexMax = arr.Count;
            for(int i = 0; i < arr.Count; i++)
            {
                var max = GetIndexOfMax(arr, indexMax);
                Swap(arr, indexMax - 1, max);
                indexMax--;
            }
            return arr;
        }

        public static List<int> Bubble(List<int> arr)
        {
            for(int i = 0; i < arr.Count; i++)
            {
                for(int j = arr.Count - 1; j > i; j--)
                {
                    if(arr[j] <= arr[j - 1])
                    {
                        Swap(arr, j, j - 1);
                    }
                }
            }
            return arr;
        }

        public static List<int> Insertion(List<int> arr)
        {
            for(int j = 1; j < arr.Count; j++)
            {
                for(int i = j - 1, key = arr[j]; i >= 0 && arr[i] > key; i--)
                {
                    arr[i + 1] = arr[i];
                    arr[i] = key;
                }
            }
            return arr;
        }

        public static List<int> MergeSort(List<int> list)
        {
            if (list.Count == 2 && list[0] > list[1])
                return Swap(list, 0, 1);

            if (list.Count == 2 || list.Count == 1 || list.Count == 0)
                return list;

            int half = list.Count / 2;
            List<int> firstHalf = list.Take(half).ToList(),
                secondHalf = list.Skip(half).Take(list.Count - half).ToList();

            MergeSort(firstHalf);
            MergeSort(secondHalf);

            list.Clear();
            for (int i = 0, j = 0; i < firstHalf.Count || j < secondHalf.Count;)
            {
                (int, int) minFirst = firstHalf.Count != 0 ? GetMin(firstHalf) : (int.MaxValue, int.MaxValue);
                (int, int) minSecond = secondHalf.Count != 0 ? GetMin(secondHalf) : (int.MaxValue, int.MaxValue);
                if (minFirst.Item2 < minSecond.Item2)
                {
                    list.Add(minFirst.Item2);
                    firstHalf.RemoveAt(minFirst.Item1);
                }
                else
                {
                    list.Add(minSecond.Item2);
                    secondHalf.RemoveAt(minSecond.Item1);
                }
            }
            return list;

        }

        #region private helpers

        private static(int, int) GetMin(List<int> list)
        {
            var result = (0, 0);//index, value
            result.Item2 = list.Min();
            result.Item1 = list.IndexOf(result.Item2);
            return result;
        }

        
        private static int GetIndexOfMax(List<int> arr) => GetIndexOfMax(arr, arr.Count);
        private static int GetIndexOfMax(List<int> arr, int indexOfMax)
        {
            (int, int) max = (arr.ElementAt(0), 0);
            for(int i = 1; i < indexOfMax; i++)
            {
                max = arr[i] > max.Item1 ? (arr.ElementAt(i), i) : max;
            }
            return max.Item2;
        }

        private void Swap<T>(ref T a, ref T b)
        {
            T tmp = a;
            a = b;
            b = tmp;
        }

        private static List<T> Swap<T>(List<T> arr, int a, int b)
        {
            var tmp = arr[a];
            arr[a] = arr[b];
            arr[b] = tmp;
            return arr;
        }
        #endregion
    }
}
