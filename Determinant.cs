using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class Determinant<T>
    {
        private T[,] Matrix { get; set; }
        public T Det { get; private set; }

        private T TwoByTwo(T [,] m)
        {
            if(m.GetLength(1) != 2 || m.GetLength(0) != 2)
                new Exception("Uncorrect matrix");
            return (dynamic)m[0, 0] * m[1, 1] - (dynamic)m[0, 1] * m[1, 0];
        }

        private void GetMatr(T[,] mas, ref T[,] p, int i, int j, int m = 0)
        {
            int di = 0, dj = 0;
            m = m == 0 ? mas.GetLength(0) - 1 : m - 1;
            for (int ki = 0; ki < m; ki++) { // проверка индекса строки
                if (ki == i) di = 1;
                for (int kj = 0; kj < m; kj++) { // проверка индекса столбца
                    if (kj == j) dj = 1;
                    p[ki, kj] = mas[ki + di, kj + dj];
                }
            }
        }

        private void PrintMatr(T[,] mas, int m = 0)
        {
            m = m == 0 ? mas.GetLength(0) : m;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    Console.Write(j + " ");
                }
                Console.WriteLine();
            }
        }

        private T Processing(T [,] mas)
        {
            T d = (dynamic)0;

            if (mas.GetLength(0) == 1)
            {
                d = mas[0, 0];
            }
            else if (mas.GetLength(0) == 2 && mas.GetLength(1) == 2)
            {
                d = TwoByTwo(mas);
            }
            else
            {
                int j = 0, k = 1;
                T[,] p = new T[mas.GetLength(0) - 1, mas.GetLength(1) - 1];
                for (int i = 0; i < mas.GetLength(0); i++)
                {
                    GetMatr(mas, ref p, i, 0, mas.GetLength(0));
                    //Console.WriteLine(mas[i, j]);
                    //PrintMatr(p, p.GetLength(0));
                    d += (dynamic)k * mas[i, 0] * Processing(p);
                    k = -k;
                }
            }
            return d;
        }

        public Determinant(T[,] Matrix)
        {
            this.Matrix = Matrix;
        }

        public T Calculate()
        {
            if(Matrix.GetLength(0) != Matrix.GetLength(1))
            {
                throw new Exception("Матрица не может быть не квадратной");
            }
            else if(Matrix.GetLength(0) == 0)
            {
                throw new Exception("Empty matrix");
            }
            
            return Det = Processing(Matrix);
        }
    }

    public class RecursionCalculateDeterminant
    {
        /// <summary>
        /// Результат вычисления определителя матрицы.
        /// </summary>
        public double? Determinant { get; private set; } = null;
        /// <summary>
        /// Вычисление определителя матрицы matrix.
        /// </summary>
        /// <param name="matrix">Матрица, у которой необходимо вычислить определитель.</param>
        /// <returns>Определитель матрицы.</returns>
        public double Calculate(double[,] matrix)
        {
            int length = matrix.GetLength(0);
            if(length != matrix.GetLength(1))
            {
                throw new ArgumentException("Матрица должна быть квадратной.");
            }
            return new Func<double>[]
            {
                () => throw new ArgumentException("Пустая матрица."),
                () => matrix[0, 0],
                () => matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0],
                () => {
                    double determinant = 0;
                    for (int i = 0; i < length; i++)
                    {
                        determinant += (Math.Pow(-1, i) < 0 ? -1 : 1) * matrix[0, i] * Calculate(Deletion(matrix, 0, i));
                    }
                    this.Determinant = determinant;
                    return determinant;
                }
            }[length > 3 ? 3 : length]();
        }

        /// <summary>
        /// Вычеркивание из метрицы matrix строки line и столбца column.
        /// </summary>
        /// <param name="matrix">Матрица</param>
        /// <param name="line">Вычеркиваемая строка</param>
        /// <param name="column">Вычеркиваемый столбец</param>
        /// <returns>Матрица с вычеркнутой строкой и столбцом.</returns>
        private double[,] Deletion(double[,] matrix, int line, int column)
        {
            int length = matrix.GetLength(0);
            double[,] result = new double[length - 1, length - 1];
            for(int _ = 0, i = 0; _ < length; _++, i++)
            {
                if(_ == line)
                {
                    i--;
                    continue;
                }
                for(int __ = 0, j = 0; __ < length; __++, j++)
                {
                    if(__ == column)
                    {
                        j--;
                        continue;
                    }

                    result[i, j] = matrix[_, __];
                }
            }
            return result;
        }
    }
}
