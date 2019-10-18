using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Npgsql;
using System.Numerics;

using Newtonsoft.Json;
using RestSharp;

namespace ConsoleApp
{
    class Program
    {
        static List<string> vs= new List<string>();
        static void Func()
        {
            string res = "";
            RestClient client = new RestClient("https://localhost:44312/run/423b7b93-953f-47f9-b219-7154668bfac2");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Postman-Token", "a39e65b1-0ee3-4e54-9826-22bd3189a0b1");
            request.AddHeader("cache-control", "no-cache");
            IRestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content);

            client = new RestClient("https://localhost:44312/run/423b7b93-953f-47f9-b219-7154668bfac2");
            request = new RestRequest(Method.POST);
            request.AddHeader("Postman-Token", "a39e65b1-0ee3-4e54-9826-22bd3189a0b1");
            request.AddHeader("cache-control", "no-cache");
            response = client.Execute(request);

            var item = response.Cookies.FirstOrDefault(x => x.Name == "passing_423b7b93-953f-47f9-b219-7154668bfac2");
            
            if (item != null)
            {
                vs.Add(item.Value);
                var index = response.Cookies.IndexOf(item);
                var d = response.Cookies[index].Value;
                //Console.WriteLine(response.Content);


                client = new RestClient("https://localhost:44312/api/run/savequestionnairedata");
                request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "a289fff9-af3b-4b3b-a3da-36a67daf7e47");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW");
                request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"respondentId\"\r\n\r\n" + d + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--", ParameterType.RequestBody);
                response = client.Execute(request);


                client = new RestClient("https://localhost:44312/api/finish");
                request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "8f2ad81e-9003-4679-b1d4-509778a7cabf");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW");
                request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"respondentId\"\r\n\r\n" + d + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"questionnaireId\"\r\n\r\n423b7b93-953f-47f9-b219-7154668bfac2\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"endType\"\r\n\r\nend\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--", ParameterType.RequestBody);
                response = client.Execute(request);
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                Console.WriteLine("Ошибка {0}", vs.Last());
                Console.WriteLine("500");
            }
        }
        static void Main(string[] args)
        {
            #region Determinant

            //int[,] a = new int[3, 3];
            //a[0, 0] = 2;
            //a[0, 1] = 4;
            //a[0, 2] = 3;
            //a[1, 0] = 5;
            //a[1, 1] = 7;
            //a[1, 2] = 8;
            //a[2, 0] = 6;
            //a[2, 1] = 9;
            //a[2, 2] = 1;
            //Determinant det = new Determinant(a);
            //det.PrintMatr(a);
            //det.Calculate();
            //Console.WriteLine(det.Det);

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

            Parallel.For(1, 1000000, (i, p) =>
            {
                Func();
            });

            for (int i = 0; i < 1000000; i++)
            {

            }

            //List<int> nums = new List<int>() { 4, 1, 5, 3, 6, 9, 3 , 2, 1, 8, 6, 10, 7 };
            //List<MyCustomClass> nums = new List<MyCustomClass>() { new MyCustomClass(1), new MyCustomClass(6), new MyCustomClass(2), new MyCustomClass(9), new MyCustomClass(7)};

            //nums.ForEach(x => Console.Write(x.A + " "));
            //Console.WriteLine();

            //Sorts.QuickSort(nums);

            //nums.ForEach(x => Console.Write(x.A + " "));
            //Console.WriteLine();

            //MultipleMatrices multiple = new MultipleMatrices();
            //multiple.UnitMatrix<int>(5).ForEach(x => x.ForEach(y => Console.Write(y)));

            //Neural_network.Training();

            //for (int i = 0; i < 100; i++)
            //{
            //    RequestTo myRequest = new RequestTo("https://localhost:44312/run/423b7b93-953f-47f9-b219-7154668bfac2", "POST", "423b7b93-953f-47f9-b219-7154668bfac2");
            //    string content = myRequest.GetResponse();
            //    Console.WriteLine(content);
            //}

            
            

            //foreach(var vac in result)
            //{
            //    Console.WriteLine($"{vac.Id}");
            //}

            Console.ReadKey();
        }


        class MyCustomClass : IComparable
        {
            public int A { get; private set; }
            public MyCustomClass(int A)
            {
                this.A = A;
            }

            public static bool operator <(MyCustomClass a, MyCustomClass b)
            {
                return a.A < b.A;
            }

            public static bool operator >(MyCustomClass a, MyCustomClass b)
            {
                return a.A > b.A;
            }

            public int CompareTo(object o)
            {
                try
                {
                    if (o is MyCustomClass a)
                        return this.A.CompareTo(a.A);
                    else
                        throw new Exception("Невозможно сравнить два объекта.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return 0;
                }
            }
        }
    }

   
   
}
