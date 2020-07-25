using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Npgsql;
using System.Numerics;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using RestSharp;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Runtime;
using System.Web.Script.Serialization;
using Roll;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Net.NetworkInformation;
using ConsoleApp.linearAlgebra;
using System.Threading;

namespace ConsoleApp
{
    public class A
    {
        public List<double> Data { get; set; }

        public A()
        {
            this.Data = new List<double>();
        }

        public A(List<double> data)
        {
            this.Data = data;
        }

        public static A operator*(A source, double a)
        {
            A res = new A();
            source.Data.ForEach(num =>
            {
                res.Data.Add(num * a);
            });
            return res;
        }


        public static A operator/(A a, double determinator)
        {
            A res = new A();
            a.Data.ForEach(num => res.Data.Add(num / determinator));
            return res;
        }

        public static A operator*(double a, A source) => source * a;
        public static A operator /(double determinator, A a) => a / determinator;

        public static bool operator==(A a, A b) => a.Data == b.Data;
        public static bool operator !=(A a, A b) => a.Data != b.Data;
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            const int COUNT = 10;
            double[,] matrix = new double[COUNT, COUNT];

            Random rand = new Random(10);
            for (int i = 0; i < COUNT; i++)
            {
                for (int j = 0; j < COUNT; j++)
                {
                    matrix[i, j] = rand.NextDouble() * 100;
                }
            }
            ParallelRecursionCalculateDeterminant determinant = new ParallelRecursionCalculateDeterminant();
            RecursionCalculateDeterminant recDeterminant = new RecursionCalculateDeterminant();
            Determinant just_determinant = new Determinant();

            List<Thread> threads = new List<Thread>() {
                new Thread(() => {
                    Stopwatch timer = new Stopwatch();
                    timer.Start();
                    var res = just_determinant.Calculate(matrix);
                    timer.Stop();
                    Console.WriteLine($"Just determinant finder time is {timer.ElapsedMilliseconds / 60} with result {res}");
                }),
                new Thread(() =>
                {
                    Stopwatch timer = new Stopwatch();
                    timer.Start();
                    var res = determinant.Calculate(matrix);
                    timer.Stop();

                    Console.WriteLine($"Sequential calculating time is {timer.ElapsedMilliseconds / 60} with result {res}");
                }),
                new Thread(() =>
                {
                    Stopwatch timer = new Stopwatch();
                    timer.Start();
                    var res = recDeterminant.Calculate(matrix);
                    timer.Stop();

                    Console.WriteLine($"Parallel calculating time is {timer.ElapsedMilliseconds / 60} with result {res}");
                })
            };

            //threads.ForEach(x => x.Start());

            //threads.ForEach(x => x.Join());

            //Permutate<int> list = new Permutate<int>() { 1, 2, 3, 4, 5 };
            //foreach(var per in list.Get())
            //{
            //    Console.WriteLine(string.Join(" ", per));
            //}
            //void print(List<double> data)
            //{
            //    Console.WriteLine(string.Join(" ", data));
            //}
            //void print_a(A data) => print(data.Data);

            //var a = new A(new List<double> { 1, 2, 3, 4, 5, 6 });
            //print_a(a * 10);
            //print_a(a * 10 / 10);

            Console.ReadKey();
        }
    }
}
