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


namespace ConsoleApp
{
    class Program
    {
        public static string json = @"
			{
  				""success"": true,
  				""message"":
                [
                    {
                        status: ""OK""
                    },
                    {
                        status: ""Lol""
                    }
                ],
			  	""types"": 
			  	[
					{
				  		""name"": ""A5EF3-ASR"",
				  		""title"": ""ITIL Foundation Plus Cloud Introduction"",
				  		""classroomDeliveryMethod"": ""Self-paced Virtual Class"",
				  		""descriptions"": {
							""EN"": {
					  			""description"": ""some Text null"",
					  			""overview"": null,
					  			""abstract"": ""Some other text"",
					  			""prerequisits"": null,
					  			""objective"": null,
					  			""topic"": null
							}
				  		},
				  		""lastModified"": ""2014-10-08T08:37:43Z"",
				  		""created"": ""2014-04-28T11:23:12Z""
					},
					{
				  		""name"": ""A4DT3-ASR"",
				  		""title"": ""ITIL Foundation eLearning Course + Exam"",
				  		""classroomDeliveryMethod"": ""Self-paced Virtual Class"",
				  		""descriptions"": {
							""EN"": {
					  			""description"": ""some Text"",
					  			""overview"": null,
					  			""abstract"": ""abstract test"",
					  			""prerequisits"": null,
					  			""objective"": null,
					  			""topic"": null
							}
				  		},
				  		""lastModified"": ""2014-10-09T08:37:43Z"",
				  		""created"": ""2014-04-29T11:23:12Z""
					}
				]
			}";
        static void Main(string[] args)
        {
            //Myscheduler.intervalinseconds(17, 40, 10, () =>
            //{
            //    console.writeline("hello");
            //});

            //var responce = new RequestTo("http://opendata.trudvsem.ru/api/v1/vacancies/region/15?offset=0&limit=1").GetResponse()
            //    .Replace("creation-date", "creation_date").Replace("modify-date", "modify_date").Replace("job-name", "job_name")
            //    .Replace("hr-agency", "hr_agency");
            //Responce result = JsonConvert.DeserializeObject<Responce>(responce);


            //using (WebClient client = new WebClient())
            //{
            //    string rawJson = client.DownloadString(Helper.GetLink(0, 1));
            //    Responce vacancies = JsonConvert.DeserializeObject<Responce>(rawJson);
            //    Console.WriteLine(vacancies);
            //}

            string json = Task.Run(async () => await Helper.getcontentasync()).Result;
            int offset = JsonConvert.DeserializeObject<JsonDeserialize.Responce>(json).meta.total / 100 + 1;
            List<JsonDeserialize.vacancy_container> vacancies = new List<JsonDeserialize.vacancy_container>();
            for(int page = 1; page <= offset; page++)
            {
                var resultObjects = AllChildren(JObject.Parse(json))
                    .First(c => c.Type == JTokenType.Array && c.Path.Contains("vacancies"))
                    .Children<JObject>();

                foreach (JObject result in resultObjects)
                {
                    //foreach (JProperty property in result.Properties())
                    //{
                    string currentVacancy = result.ToString()
                        .Replace("creation-date", "creation_date").Replace("modify-date", "modify_date").Replace("job-name", "job_name")
                        .Replace("hr-agency", "hr_agency"); ;//.Insert(0, "{") + "}";
                        JsonDeserialize.vacancy_container vac = JsonConvert.DeserializeObject<JsonDeserialize.vacancy_container>(currentVacancy);
                        //JsonDeserialize.vacancy vac = JsonSerializer.
                        vacancies.Add(vac);
                    //}
                }
                json = Task.Run(async () => await Helper.getcontentasync(page)).Result;
            }
            Console.WriteLine(offset + " " + JsonConvert.DeserializeObject<JsonDeserialize.Responce>(json).meta.total);
            Console.ReadKey();
        }
        private static IEnumerable<JToken> AllChildren(JToken json)
        {
            foreach (var c in json.Children())
            {
                yield return c;
                foreach (var cc in AllChildren(c))
                {
                    yield return cc;
                }
            }
        }
    }

    static class Helper
    {
        public async static Task<string> getcontentasync() => 
            await new HttpClient().GetStringAsync(GetLink(0));

        public async static Task<string> getcontentasync(int offset) =>
            await new HttpClient().GetStringAsync(GetLink(offset));

        public static string GetLink(int offset, int limit = 100) =>
            $"http://opendata.trudvsem.ru/api/v1/vacancies/region/15?offset={offset}&limit={limit}";
    }
}
