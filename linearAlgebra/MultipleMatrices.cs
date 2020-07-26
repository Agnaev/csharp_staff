using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class MultipleMatrices 
    {
        public List<List<double>> UnitMatrix<T>(int dimention)
        {
            List<List<double>> result = new List<List<double>>();
            for(int i = 0, j = 0; i < dimention; i++, j++)
            {
                result[i] = new List<double>();
                if (typeof(T) == typeof(string))
                    throw new Exception("Строка не допускается");
                result[i][j] = 1;
            }

            return result;
        }

        public bool Multiplication(double[,] a, double[,] b, out double[,] result)
        {
            result = null;
            if (a.GetLength(1) != b.GetLength(0))
            {
                return false;
            }

            result = new double[a.GetLength(0), b.GetLength(1)];

            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < b.GetLength(1); j++)
                {
                    for (int k = 0; k < b.GetLength(0); k++)
                    {
                        result[i, j] += a[i, k] * b[k ,j];
                    }
                }
            }

            return true;
        }
    }
}
