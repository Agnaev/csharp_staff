using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Npgsql;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Determinant
            /*
             int [,]a = new int[3, 3];
            a[0, 0] = 2;
            a[0, 1] = 4;
            a[0, 2] = 3;
            a[1, 0] = 5;
            a[1, 1] = 7;
            a[1, 2] = 8;
            a[2, 0] = 6;
            a[2, 1] = 9;
            a[2, 2] = 1;
            Determinant det = new Determinant(a);
            det.PrintMatr(a);
            det.Calculate();
            Console.WriteLine(det.Det);
             */
            #endregion

            #region connect to db
            /*
            string connectionString = "Host=localhost;Port=5432;User Id=postgres;Password=123456;Database=Permutations";
            using(var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand("delete from data", conn))
                {
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = new NpgsqlCommand("INSERT INTO data (symbols) VALUES ", conn))
                {
                    for(int i= 0; i<10; i++)
                    {
                        cmd.CommandText += ", (" + i + ")";
                    }
                    cmd.ExecuteNonQuery();
                }

                using(var cmd = new NpgsqlCommand("SELECT * FROM data as x1, data as x2, data as x3", conn))
                {
                    using(var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.Write(reader.GetString(0));
                        };
                        Console.WriteLine();
                    }
                }

                using (var cmd = new NpgsqlCommand("SELECT symbols FROM data", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(reader.GetString(0));
                        }
                    }
                }
            }*/
            #endregion

            List<int> nums = new List<int>() { 1, 5, 3, 6, 9, 3 , 2, 1, 8, 6, 10, 7 };

            nums.ForEach(x => Console.Write(x + " "));
            Console.WriteLine();

            Sorts.MergeSort(nums);

            nums.ForEach(x => Console.Write(x + " "));
            Console.WriteLine();

            Console.ReadKey();
        }
    }
}
