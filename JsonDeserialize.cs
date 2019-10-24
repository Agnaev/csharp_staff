using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ConsoleApp
{
    class JsonDeserialize
    {
        public class Responce
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

        //class vacancies
        //{
        //    public List<vacancy> vacancies { get; set; }
        //}

        public class vacancy_container
        {
            public Guid id { get; set; }
            public string source { get; set; }
            public region region { get; set; }
            public company company { get; set; }
            public DateTime creation_date { get; set; }
            public DateTime modify_date { get; set; }
            public string salary { get; set; }
            public int salary_min { get; set; }
            public int salary_max { get; set; }
            public string job_name { get; set; }
            public string vac_url { get; set; }
            public string employment { get; set; }
            public string schedule { get; set; }
            public string duty { get; set; }
            public category category { get; set; }
            public requirement requirement { get; set; }
            public addresses addresses { get; set; }
            public string currency { get; set; }

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
            //[DataMember(Name = "hr-agency")]
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
            public int experience { get; set; }
        }

        public class addresses
        {
            public string address { get; set; }
            public string lng { get; set; }
            public string lat { get; set; }
        }
    }
}
