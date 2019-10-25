using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleApp
{

    internal static class Helper
    {
        public static IEnumerable<JToken> AllChildren(JToken json)
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
        public async static Task<string> GetContentAsync()
        {
            string result = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                result = await client.GetStringAsync(GetLink(0, 1));
            }
            return result;
        }

        public async static Task<string> GetContentAsync(int offset)
        {
            string result = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                result = await client.GetStringAsync(GetLink(offset));
            }
            return result;
        }

        public async static Task<string> GetContentAsync(int offset, int limit)
        {
            string result = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                result = await client.GetStringAsync(GetLink(offset, limit));
            }
            return result;
        }

        public async static Task<string> GetContentAsync(string link)
        {
            string result = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                result = await client.GetStringAsync(link);
            }
            return result;
        }

        public static string GetLink(int offset, int limit = 100)
        {
            return $"http://opendata.trudvsem.ru/api/v1/vacancies/region/15?offset={offset}&limit={limit}";
        }
    }
    public class JsonDeserializer
    {
        public static void LocalMain()
        {
            List<vacancy> vacancies = new List<vacancy>();
            int countOfVacancies = JsonConvert.DeserializeObject<Responce>(Task.Run(async () => await Helper.GetContentAsync(0)).Result).meta.total;
            int countOfPages = countOfVacancies / 100 + 1;

            for (int offset = 0; offset < countOfPages; offset++)
            {
                string json = Task.Run(async () => await Helper.GetContentAsync(offset)).Result;
                var resultObjects = Helper.AllChildren(JObject.Parse(json))
                    .First(c => c.Type == JTokenType.Array && c.Path.Contains("vacancies"))
                    .Children<JObject>();

                foreach (JObject result in resultObjects)
                {
                    foreach (JProperty property in result.Properties())
                    {
                        vacancy vacancy = JsonConvert.DeserializeObject<vacancy>(property.Value.ToString());
                        vacancies.Add(vacancy);
                        Console.WriteLine(property.Name + " " + property.Value);
                    }
                }
            }
            Console.WriteLine($"{countOfPages}\n{countOfVacancies}");
        }
        private class Responce
        {
            public string status { get; set; }
            public request request { get; set; }
            public meta meta { get; set; }
            [JsonProperty("results")]
            public results results { get; set; }
        }

        public class request
        {
            public string api { get; set; }
        }

        public class meta
        {
            public int total { get; set; }
            public int limit { get; set; }
        }

        public class results
        {
            [JsonProperty("vacancies")]
            public List<vacancy> vacancies { get; set; }
        }

        
        public class vacancy
        {
            [JsonProperty("id")]
            public Guid id { get; set; }
            [JsonProperty("source")]
            public string source { get; set; }
            [JsonProperty("region")]
            public region region { get; set; }
            [JsonProperty("company")]
            public company company { get; set; }
            [JsonProperty("creation-date")]
            public DateTime creation_date { get; set; }
            [JsonProperty("modify-date")]
            public DateTime modify_date { get; set; }
            [JsonProperty("salary")]
            public string salary { get; set; }
            [JsonProperty("salary_min")]
            public int salary_min { get; set; }
            [JsonProperty("salary_max")]
            public int salary_max { get; set; }
            [JsonProperty("job-name")]
            public string job_name { get; set; }
            [JsonProperty("vac_url")]
            public string vac_url { get; set; }
            [JsonProperty("employment")]
            public string employment { get; set; }
            [JsonProperty("schedule")]
            public string schedule { get; set; }
            [JsonProperty("duty")]
            public string duty { get; set; }
            public category category { get; set; }
            [JsonProperty("requirement")]
            public requirement requirement { get; set; }
            [JsonProperty("addresses")]
            public addresses addresses { get; set; }
            [JsonProperty("currency")]
            public string currency { get; set; }
        }

        public class region
        {
            public string region_code { get; set; }
            public string name { get; set; }
        }

        public class company
        {
            public string ogrn { get; set; }
            public string site { get; set; }
            public string companycode { get; set; }
            [JsonProperty("hr-agency")]
            public bool hr_agency { get; set; }
            public string inn { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string url { get; set; }
        }

        public class category
        {
            public string specialisation { get; set; }
        }

        public class requirement
        {
            public string qualification { get; set; }
            public string experience { get; set; }
        }

        public class addresses
        {
            public List<address> address { get; set; }
        }

        public class address
        {
            public string location { get; set; }
            public string lng { get; set; }
            public string lat { get; set; }
        }
    }
}
