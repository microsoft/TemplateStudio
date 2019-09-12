using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Param_RootNamespace.Core.Services
{
    // This class provides a wrapper around common functionality for HTTP actions.
    // Learn more at https://docs.microsoft.com/windows/uwp/networking/httpclient
    public class HttpDataService
    {
        private readonly Dictionary<string, object> responseCache;
        private HttpClient client;

        public HttpDataService(string defaultBaseUrl = "")
        {
            client = new HttpClient();

            if (!string.IsNullOrEmpty(defaultBaseUrl))
            {
                client.BaseAddress = new Uri($"{defaultBaseUrl}/");
            }

            responseCache = new Dictionary<string, object>();
        }

        public async Task<T> GetAsync<T>(string uri, bool forceRefresh = false)
        {
            T result = default(T);

            // The responseCache is a simple store of past responses to avoid unnecessary requests for the same resource.
            // Feel free to remove it or extend this request logic as appropraite for your app.
            if (forceRefresh || !responseCache.ContainsKey(uri))
            {
                var json = await client.GetStringAsync(uri);
                result = await Task.Run(() => JsonConvert.DeserializeObject<T>(json));

                if (responseCache.ContainsKey(uri))
                {
                    responseCache[uri] = result;
                }
                else
                {
                    responseCache.Add(uri, result);
                }
            }
            else
            {
                result = (T)responseCache[uri];
            }

            return result;
        }

        public async Task<bool> PostAsync<T>(string uri, T item)
        {
            if (item == null)
            {
                return false;
            }

            var serializedItem = JsonConvert.SerializeObject(item);
            var buffer = Encoding.UTF8.GetBytes(serializedItem);
            var byteContent = new ByteArrayContent(buffer);

            var response = await client.PostAsync(uri, byteContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PostAsJsonAsync<T>(string uri, T item)
        {
            if (item == null)
            {
                return false;
            }

            var serializedItem = JsonConvert.SerializeObject(item);

            var response = await client.PostAsync(uri, new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PutAsync<T>(string uri, T item)
        {
            if (item == null)
            {
                return false;
            }

            var serializedItem = JsonConvert.SerializeObject(item);
            var buffer = Encoding.UTF8.GetBytes(serializedItem);
            var byteContent = new ByteArrayContent(buffer);

            var response = await client.PutAsync(uri, byteContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PutAsJsonAsync<T>(string uri, T item)
        {
            if (item == null)
            {
                return false;
            }

            var serializedItem = JsonConvert.SerializeObject(item);

            var response = await client.PutAsync(uri, new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(string uri)
        {
            var response = await client.DeleteAsync(uri);

            return response.IsSuccessStatusCode;
        }
    }
}
