using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp.linearAlgebra
{
    class CramersProblem
    {
        #region public data
        public double? Determinant { get; private set; } = null;
        public double[,] A { get; private set; }
        public double[] B { get; private set; }
        public double?[] Result { get; private set; }
        #endregion

        #region private data
        private ParallelRecursionCalculateDeterminant determinantFinder = new ParallelRecursionCalculateDeterminant();
        #endregion

        public CramersProblem(double[,] A, double[] B)
        {
            this.A = A;
            this.B = B;

            this.Result = new double?[A.GetLength(0)];

            this.Determinant = determinantFinder.Calculate(this.A);
        }

        public double?[] Solve()
        {
            if(this.Determinant == 0 || this.Determinant == null)
            {
                return null;
            }
            Parallel.For(0, this.A.GetLength(0), i =>
            {
                this.Result[i] = this.determinantFinder.Calculate(ReplaceColumn(i));
            });
            return this.Result;
        }

        private double[,] ReplaceColumn(int replacementLine)
        {
            var a = this.A.Clone() as double[,];
            for(int i = 0; i < this.A.GetLength(0); i++)
            {
                a[i, replacementLine] = this.B[i];
            }
            return a;
        }
    }
}
