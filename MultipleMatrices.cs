using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class MultipleMatrices
    {
        public List<List<T>> UnitMatrix<T>(int dimention)
        {
            List<List<T>> result = new List<List<T>>();
            for(int i = 0, j = 0; i < dimention; i++, j++)
            {
                result[i] = new List<T>();
                if (typeof(T) == typeof(string))
                    throw new Exception("Строка не допускается");
                result[i][j] = (T)Convert.ChangeType(1, typeof(T));
            }

            return result;
        }

        public bool multiplication<T>(T[,] a, T[,] b, T[,] c)
        {
            if(a.GetLength(1) != b.GetLength(0))
                return false;

            

            return true;
        }

        //private bool multiplicationTwoArrays<T>(T[] a, T[] b, T[] res)
        //{
        //    if (a.Length != b.Length)
        //        return false;
        //    for(int i = 0; i < a.Length; i++)
        //    {
        //        res[i] = a[i] * b[i];
        //    }
        //}

        //private T multiplicationT<T>(T a, T b)
        //{
        //    return Operator<T>.Multiply(a, b);
        //}
    }
}
