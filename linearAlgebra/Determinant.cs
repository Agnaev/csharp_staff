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

        protected bool CheckMatrix(double[,] matrix, out Exception error)
        {
            error = null;
            if (matrix.GetLength(0) != matrix.GetLength(1))
            {
                error = new Exception("Матрица должна быть квадратной.");
                return true;
            }
            else if (matrix.GetLength(0) == 0)
            {
                error = new Exception("Empty matrix");
                return true;
            }
            return false;
        }

        protected Func<double[,], double>[] fns = new Func<double[,], double>[4] {
            (double[,] _matrix) => throw new ArgumentException("Пустая матрица."),
            (double[,] _matrix) => _matrix[0, 0],
            (double[,] _matrix) => _matrix[0, 0] * _matrix[1, 1] - _matrix[0, 1] * _matrix[1, 0],
            null
        };

        protected int GetFunctionPointer(int matrix_length)
        {
            return matrix_length > 3 ? 3 : matrix_length;
        }
    }

    public class Determinant : DeterminantFinder
    {
        private double? Processing(double [,] matrix)
        {
            if(this.CheckMatrix(matrix, out Exception error))
            {
                throw error;
            }

            if (matrix.GetLength(0) == 1)
            {
                return fns[1](matrix);
            }
            else if (matrix.GetLength(0) == 2 && matrix.GetLength(1) == 2)
            {
                return fns[2](matrix);
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
            if(this.CheckMatrix(matrix, out Exception error))
            {
                throw error;
            }
            
            return Processing(matrix) ?? 0xffffff;
        }
    }

    public class RecursionCalculateDeterminant : DeterminantFinder
    {
        public override double Calculate(double[,] matrix)
        {
            if(this.CheckMatrix(matrix, out Exception error))
            {
                throw error;
            }
            
            fns[3] = (double[,] _matrix) => {
                double determinant = 0;
                for (int i = 0; i < _matrix.GetLength(0); i++)
                {
                    determinant += Math.Pow(-1, i) * _matrix[0, i] * this.fns[this.GetFunctionPointer(_matrix.GetLength(0) - 1)](this.Deletion(_matrix, 0, i));
                }
                return determinant;
            };

            
            return fns[this.GetFunctionPointer(matrix.GetLength(0))](matrix);
        }
    }

    public class ParallelRecursionCalculateDeterminant : DeterminantFinder
    {
        public override double Calculate(double[,] matrix)
        {
            if(this.CheckMatrix(matrix, out Exception error))
            {
                throw error;
            }
            
            fns[3] = (double[,] _matrix) =>
            {
                double determinant = 0;
                Parallel.For(0, _matrix.GetLength(0), i =>
                {
                    determinant += Math.Pow(-1, i) * _matrix[0, i] * this.Calculate(this.Deletion(_matrix, 0, i));
                });
                return determinant;
            };
            
            return fns[this.GetFunctionPointer(matrix.GetLength(0))](matrix);
        }
    }
}
