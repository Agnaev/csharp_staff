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

            public static bool operator>(A a, A b)
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
                    if(obj is A a)
                    {
                        return this.a.CompareTo(a.a);
                    }
                    throw new Exception($"Cannot compare two values: {obj} and {this.a}");
                }
                catch(Exception e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                    return -1;
                }
            }
        }

        public static void Main(string[] args)
        {
            //JsonDeserializer.LocalMain();

            //List<A> list = new List<A>() { new A(4), new A(2), new A(6), new A(1), new A(6), new A(9), new A(6), new A(4), new A(1), new A(2) };
            //Sorts.Insertion(list, (A a, A b) =>  a > b );

            //list.ForEach(c => Console.WriteLine(c.a + " "));

            //new BackPropagation(new LearningAlgorithmConfig()).Train();
            Roll<string> roll = new Roll<string>() { "lol", "kek", "cheburek", "arbidol" };
            //foreach(var i in roll)
            //{
            //    Console.WriteLine(i);
            //}

            roll.ForEach(elem =>
            {
                Console.WriteLine(elem);
            });



            Console.ReadKey();
        }
    }
}
