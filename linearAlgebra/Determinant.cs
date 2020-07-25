using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.linearAlgebra
{
    public abstract class DeterminantFinder
    {
        /// <summary>
        /// Вычисление определителя матрицы matrix.
        /// </summary>
        /// <param name="matrix">Матрица, у которой необходимо вычислить определитель.</param>
        /// <returns>Определитель матрицы.</returns>
        public abstract double Calculate(double[,] matrix);

        /// <summary>
        /// Вычеркивание из метрицы matrix строки line и столбца column.
        /// </summary>
        /// <param name="matrix">Матрица</param>
        /// <param name="deleted_line">Вычеркиваемая строка</param>
        /// <param name="deleted_column">Вычеркиваемый столбец</param>
        /// <returns>Матрица с вычеркнутой строкой и столбцом.</returns>
        protected double[,] Deletion(double[,] matrix, int deleted_line, int deleted_column)
        {
            int length = matrix.GetLength(0);
            double[,] result = new double[length - 1, length - 1];
            for (int line = 0, i = 0; line < length; line++, i++)
            {
                if (line == deleted_line)
                {
                    i--;
                    continue;
                }
                for (int column = 0, j = 0; column < length; column++, j++)
                {
                    if (column == deleted_column)
                    {
                        j--;
                        continue;
                    }

                    result[i, j] = matrix[line, column];
                }
            }
            return result;
        }
    }

    public class Determinant : DeterminantFinder
    {
        private double? Processing(double [,] matrix)
        {

            if (matrix.GetLength(0) == 1)
            {
                return matrix[0, 0];
            }
            else if (matrix.GetLength(0) == 2 && matrix.GetLength(1) == 2)
            {
                return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
            }

            double? result = 0.0;
            double[,] p = new double[matrix.GetLength(0) - 1, matrix.GetLength(1) - 1];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                Deletion(matrix, i, 0);
                result += Math.Pow(-1, i) * matrix[i, 0] * Processing(p);
            }
            return result;
        }

        public override double Calculate(double[,] matrix)
        {
            if(matrix.GetLength(0) != matrix.GetLength(1))
            {
                throw new Exception("Матрица не может быть не квадратной");
            }
            else if(matrix.GetLength(0) == 0)
            {
                throw new Exception("Empty matrix");
            }
            
            return Processing(matrix) ?? 0xffffff;
        }
    }

    public class RecursionCalculateDeterminant : DeterminantFinder
    {
        public override double Calculate(double[,] matrix)
        {
            int length = matrix.GetLength(0);
            if(length != matrix.GetLength(1))
            {
                throw new ArgumentException("Матрица должна быть квадратной.");
            }
            var fns = new Func<double[,], double>[4];

            fns[0] = (double[,] _matrix) => throw new ArgumentException("Пустая матрица.");
            fns[1] = (double[,] _matrix) => _matrix[0, 0];
            fns[2] = (double[,] _matrix) => _matrix[0, 0] * _matrix[1, 1] - _matrix[0, 1] * _matrix[1, 0];
            fns[3] = (double[,] _matrix) => {
                double determinant = 0;
                int fn_pointer;
                for (int i = 0; i < _matrix.GetLength(0); i++)
                {
                    fn_pointer = _matrix.GetLength(0) - 1 > 3 ? 3 : _matrix.GetLength(0) - 1;
                    determinant += Math.Pow(-1, i) * _matrix[0, i] * fns[fn_pointer](Deletion(_matrix, 0, i));
                }
                return determinant;
            };

            
            return fns[length > 3 ? 3 : length](matrix);
        }
    }

    public class ParallelRecursionCalculateDeterminant : DeterminantFinder
    {
        public override double Calculate(double[,] matrix)
        {
            int length = matrix.GetLength(0);
            if (length != matrix.GetLength(1))
            {
                throw new ArgumentException("Матрица должна быть квадратной.");
            }
            Func<double[,], double>[] fns = new Func<double[,], double>[4];
            fns[0] = (double[,] _matrix) => throw new ArgumentException("Пустая матрица.");
            fns[1] = (double[,] _matrix) => _matrix[0, 0];
            fns[2] = (double[,] _matrix) => _matrix[0, 0] * _matrix[1, 1] - _matrix[0, 1] * _matrix[1, 0];
            fns[3] = (double[,] _matrix) =>
            {
                double determinant = 0;
                Parallel.For(0, _matrix.GetLength(0), i =>
                {
                    determinant += Math.Pow(-1, i) * _matrix[0, i] * Calculate(Deletion(_matrix, 0, i));
                });
                return determinant;
            };
            
            return fns[length > 3 ? 3 : length](matrix);
        }
    }
}
