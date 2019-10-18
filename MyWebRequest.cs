using System;
using System.Net;
using System.IO;
using System.Text;

namespace ConsoleApp
{
    public class RequestTo
    {
        private WebRequest request;
        private Stream dataStream;

        public string Status { get; set; }

        public RequestTo(string url)
        {
            request = WebRequest.Create(url);
        }

        public RequestTo(string url, string method)
            : this(url)
        {

            if (method.ToLower().Equals("get") || method.ToLower().Equals("post"))
            {
                request.Method = method;
            }
            else
            {
                throw new Exception("Invalid Method Type");
            }
        }

        public RequestTo(string url, string method, string data) : this(url, method)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(data);

            request.ContentType = "application/x-www-form-urlencoded";

            request.ContentLength = byteArray.Length;
            using (dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }
        }

        public string GetResponse()
        {
            string responseFromServer = string.Empty;

            using (WebResponse response = request.GetResponse())
            {
                Status = (response as HttpWebResponse).StatusDescription;
                using (dataStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(dataStream))
                    {
                        responseFromServer = reader.ReadToEnd();
                    }
                }
            }
            return responseFromServer;
        }
    }
}
