using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    internal class Sorts
    {
        public static List<T> Selection<T>(List<T> list)
        {
            try
            {
                int indexMax = list.Count;
                for (int i = 0; i < list.Count; i++)
                {
                    Swap(list, indexMax - 1, GetIndexOfMax(list, indexMax--));
                }

                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static List<T> Bubble<T>(List<T> list)
        {
            try
            {
                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = list.Count - 1; j > i; j--)
                    {
                        if (Comparer<T>.Default.Compare(list[j], list[j - 1]) <= 0)
                        {
                            Swap(list, j, j - 1);
                        }
                    }
                }

                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static List<T> Bubble<T>(List<T> list, Func<T, T, bool> compare)
        {
            try
            {
                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = list.Count - 1; j > i; j--)
                    {
                        
                        if (compare(list[j], list[j - 1]))
                        {
                            Swap(list, j, j - 1);
                        }
                    }
                }

                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static List<T> Insertion<T>(List<T> list)
        {
            try
            {
                for (int j = 1; j < list.Count; j++)
                {
                    T key = list[j];
                    for (int i = j - 1; i >= 0 && Comparer<T>.Default.Compare(list[i], key) >= 0; i--)
                    {
                        list[i + 1] = list[i];
                        list[i] = key;
                    }
                }
                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        
        public static List<T> Insertion<T>(List<T> list, Func<T, T, bool> compare)
        {
            try
            {
                for (int j = 1; j < list.Count; j++)
                {
                    T key = list[j];
                    for (int i = j - 1; i >= 0 && compare(list[i], key); i--)
                    {
                        list[i + 1] = list[i];
                        list[i] = key;
                    }
                }
                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static List<T> MergeSort<T>(List<T> list)
        {
            try
            {
                if (list.Count == 2 && Comparer<T>.Default.Compare(list[0], list[1]) >= 1)
                {
                    return Swap(list, 0, 1);
                }

                if (list.Count == 2 || list.Count == 1 || list.Count == 0)
                {
                    return list;
                }

                int half = list.Count / 2;
                List<T> firstHalf = list.Take(half).ToList(),
                    secondHalf = list.Skip(half).Take(list.Count - half).ToList();

                MergeSort(firstHalf);
                MergeSort(secondHalf);

                T stub = list.Max();
                list.Clear();
                for (int i = 0, j = 0; i < firstHalf.Count || j < secondHalf.Count;)
                {
                    (int, T) minFirst = firstHalf.Count != 0 ? GetMin<T>(firstHalf) : (int.MaxValue, stub);
                    (int, T) minSecond = secondHalf.Count != 0 ? GetMin<T>(secondHalf) : (int.MaxValue, stub);
                    if (Compare(minFirst.Item2, minSecond.Item2))
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        
        public static List<T> MergeSort<T>(List<T> list, Func<T, T, bool> compare)
        {
            try
            {
                if (list.Count == 2 && compare(list[0], list[1]))
                {
                    return Swap(list, 0, 1);
                }

                if (list.Count == 2 || list.Count == 1 || list.Count == 0)
                {
                    return list;
                }

                int half = list.Count / 2;
                List<T> firstHalf = list.Take(half).ToList(),
                    secondHalf = list.Skip(half).Take(list.Count - half).ToList();

                MergeSort(firstHalf);
                MergeSort(secondHalf);

                T stub = list.Max();
                list.Clear();
                for (int i = 0, j = 0; i < firstHalf.Count || j < secondHalf.Count;)
                {
                    (int, T) minFirst = firstHalf.Count != 0 ? GetMin<T>(firstHalf) : (int.MaxValue, stub);
                    (int, T) minSecond = secondHalf.Count != 0 ? GetMin<T>(secondHalf) : (int.MaxValue, stub);
                    if (compare(minFirst.Item2, minSecond.Item2))
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static List<T> QuickSort<T>(List<T> list)
        {
            try
            {
                if (list.Count == 2 && Compare<T>(list[1], list[0]))
                {
                    return Swap(list, 0, 1);
                }

                if (list.Count == 0 || list.Count == 1 || list.Count == 2)
                {
                    return list;
                }

                T pivot = list[list.Count / 2];
                List<T> firstHalf = new List<T>(),
                    secondHalf = new List<T>();
                for (int i = 0; i < list.Count; i++)
                {
                    if (i != list.Count / 2)
                    {
                        if (Compare(list[i], pivot))
                        {
                            firstHalf.Add(list[i]);
                        }
                        else
                        {
                            secondHalf.Add(list[i]);
                        }
                    }
                }

                QuickSort(firstHalf);
                QuickSort(secondHalf);

                list.Clear();
                list.AddRange(firstHalf);
                list.Add(pivot);
                list.AddRange(secondHalf);

                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        
        public static List<T> QuickSort<T>(List<T> list, Func<T, T, bool> compare)
        {
            try
            {
                if (list.Count == 2 && compare(list[1], list[0]))
                {
                    return Swap(list, 0, 1);
                }

                if (list.Count == 0 || list.Count == 1 || list.Count == 2)
                {
                    return list;
                }

                T pivot = list[list.Count / 2];
                List<T> firstHalf = new List<T>(),
                    secondHalf = new List<T>();
                for (int i = 0; i < list.Count; i++)
                {
                    if (i != list.Count / 2)
                    {
                        if (compare(list[i], pivot))
                        {
                            firstHalf.Add(list[i]);
                        }
                        else
                        {
                            secondHalf.Add(list[i]);
                        }
                    }
                }

                QuickSort(firstHalf);
                QuickSort(secondHalf);

                list.Clear();
                list.AddRange(firstHalf);
                list.Add(pivot);
                list.AddRange(secondHalf);

                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        #region private helpers
        private static bool Compare<T>(T a, T b)
        {
            try
            {
                return Comparer<T>.Default.Compare(a, b) < 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        private static (int, T) GetMin<T>(List<T> list)
        {
            try
            {
                (int, T) result;
                result.Item2 = list.Min<T>();
                result.Item1 = list.IndexOf(result.Item2);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return (-1, list[0]);
            }
        }

        private static int GetIndexOfMax<T>(List<T> list, int count)
        {
            try
            {
                (T, int) max = (list.ElementAt(0), 0);
                for (int i = 0; i < count; i++)
                {
                    max = Comparer<T>.Default.Compare(list[i], max.Item1) >= 0 ? (list[i], i) : max;
                }
                return max.Item2;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
        }

        private void Swap<T>(ref T a, ref T b)
        {
            try
            {
                T tmp = a;
                a = b;
                b = tmp;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static List<T> Swap<T>(List<T> arr, int a, int b)
        {
            try
            {
                T tmp = arr[a];
                arr[a] = arr[b];
                arr[b] = tmp;
                return arr;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        #endregion
    }
}
