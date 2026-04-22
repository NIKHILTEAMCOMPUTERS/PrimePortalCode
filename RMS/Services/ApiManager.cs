using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace RMS.Services
{
    public class ApiManager
    {
        private readonly string _api;
        private readonly string _authToken;

        public ApiManager(string api, string authToken = "")
        {
            _api = api;
            _authToken = authToken;
        }

        private HttpClient CreateHttpClient()
        {
            var client = new HttpClient();
            if (!string.IsNullOrEmpty(_authToken))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_authToken}");
            }
            return client;
        }

        private async Task<Tuple<HttpStatusCode, string>> SendRequestAsync(HttpMethod method, HttpContent content = null)
        {
            using (var client = CreateHttpClient())
            {
                using (var response = await client.SendAsync(new HttpRequestMessage(method, _api) { Content = content }).ConfigureAwait(false))
                {
                    var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return new Tuple<HttpStatusCode, string>(response.StatusCode, responseContent);
                }
            }
        }

        public async Task<Tuple<HttpStatusCode, string>> Get() => await SendRequestAsync(HttpMethod.Get);

        public async Task<Tuple<HttpStatusCode, string>> PostJson(string jsonBody)
        {
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            return await SendRequestAsync(HttpMethod.Post, content);
        }

        public async Task<Tuple<HttpStatusCode, string>> PostFormData(IEnumerable<KeyValuePair<string, string>> formData)
        {
            var content = new FormUrlEncodedContent(formData);
            return await SendRequestAsync(HttpMethod.Post, content);
        }

        public async Task<Tuple<HttpStatusCode, string>> Put(string jsonBody)
        {
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            return await SendRequestAsync(HttpMethod.Put, content);
        }

        public async Task<Tuple<HttpStatusCode, string>> Delete() => await SendRequestAsync(HttpMethod.Delete);
        public async Task<Tuple<HttpStatusCode, string>> PostFormDatanick(IEnumerable<KeyValuePair<string, string>> formData)
        {
            using (var client = new HttpClient())
            {
                var content = new MultipartFormDataContent();

                foreach (var kvp in formData)
                {
                    content.Add(new StringContent(kvp.Value), kvp.Key);
                }

                try
                {
                    var response = await client.PostAsync(_api, content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    return new Tuple<HttpStatusCode, string>(response.StatusCode, responseContent);
                }
                catch (Exception ex)
                {
                   
                    return new Tuple<HttpStatusCode, string>(HttpStatusCode.InternalServerError, ex.Message);
                }
            }
        }
    }

}

