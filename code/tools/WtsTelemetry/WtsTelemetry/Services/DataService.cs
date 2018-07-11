using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WtsTelemetry.Services
{
    public static class DataService
    {
        private const string URL = "https://api.applicationinsights.io/v1/apps/{0}/query?query={1}";

        private static readonly string AppId = Environment.GetEnvironmentVariable("AppId");
        private static readonly string ApiKey = Environment.GetEnvironmentVariable("ApiKey");

        public static string GetData(string query)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("x-api-key", ApiKey);

            var req = string.Format(URL, AppId, query);
            HttpResponseMessage response = client.GetAsync(req).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                return response.ReasonPhrase;
            }
        }        
    }
}
