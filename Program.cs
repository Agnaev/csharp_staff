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

namespace ConsoleApp
{
    public class Program
    {
        class A : IComparable
        {
            public int a { get; set; }
            public A(int a)
            {
                this.a = a;
            }
            public static bool operator <(A a, A b)
            {
                return a.a < b.a;
            }

            public static bool operator >(A a, A b)
            {
                return a.a > b.a;
            }

            int IComparable.CompareTo(object obj)
            {
                //if (obj == null) return 1;
                //A that = obj as A;
                //return this.a.CompareTo(that.a);
                try
                {
                    if (obj is A a)
                    {
                        return this.a.CompareTo(a.a);
                    }
                    throw new Exception($"Cannot compare two values: {obj} and {this.a}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                    return -1;
                }
            }
        }

        public static void Main(string[] args)
        {
            //HttpClient client = new HttpClient();
            //Task.Run(async () => await PostRequest(client));

            //var _enum = new Binnary(4);
            //foreach (var b in _enum)
            //{
            //    Console.WriteLine(b);
            //}
            var matrix = new double[,]
            {
                { 3, -3, -5, 8 },
                { -3, 2, 4, -6 },
                { 2, -5, -7, 5 },
                { -4, 3, 5, -6 },
            };
            RecursionCalculateDeterminant determinant = new RecursionCalculateDeterminant(matrix);
            Console.WriteLine($"Determinant of matrix is equals to {determinant.Calculate()}");
            Console.ReadKey();
        }


        public static async Task PostRequest(HttpClient client)
        {
            var data = await client.PostAsync("https://jsonplaceholder.typicode.com/posts", null);
            Console.WriteLine(data);
        }
    }

    public class Binnary: IEnumerable
    {
        private int Dimension { get; set; }
        public Binnary(int dim)
        {
            this.Dimension = dim;
        }
            

        public IEnumerator GetEnumerator()
        {
            // старт с нуля
            string bin = new string('0', this.Dimension);

            // цикл теста двоичного счётчика 16 bit
            for (int c = 1; c < 32; c *= 2)
            {

                ushort dec = 0x0000;
                ushort k = 0;
                // преобразовать из двоичного в десятичное представление
                for (int i = this.Dimension - 1; i > 0; i--, k++)
                {
                    if (bin[i] == '1')
                        dec |= (ushort)(1 << k);
                }
                dec++;  // счётчик инкремента

                bin = Convert.ToString(dec, 2);
                int len = Dimension - bin.Length;
                while (--len >= 0) // дополняем нулями
                    bin = bin.Insert(0, "0");

                //Console.WriteLine("{0} - {1}", bin, dec);
                if (bin.IndexOf('1') == bin.LastIndexOf('1'))
                {
                    yield return bin;
                }
            }
        }
    }
}
