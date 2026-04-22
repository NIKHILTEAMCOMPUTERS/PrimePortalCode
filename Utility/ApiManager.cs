using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace RMS.Client.Utility
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
            client.Timeout = TimeSpan.FromSeconds(300); // Increased timeout from default 100s to 300s
            if (!string.IsNullOrEmpty(_authToken))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_authToken}");
            }
            return client;
        }

        public async Task<Tuple<HttpStatusCode, string>> SendRequestAsync(HttpMethod method, HttpContent content = null)
        {
            try
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
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                // Handle timeout specifically
                return new Tuple<HttpStatusCode, string>(HttpStatusCode.RequestTimeout, "The request timed out.");
            }
            catch (HttpRequestException ex)
            {
                // Handle socket/network exceptions
                return new Tuple<HttpStatusCode, string>(HttpStatusCode.ServiceUnavailable, $"Network error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle other unexpected exceptions
                return new Tuple<HttpStatusCode, string>(HttpStatusCode.InternalServerError, $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<Tuple<HttpStatusCode, string>> Get(HttpContent content = null) => await SendRequestAsync(HttpMethod.Get, content);

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
        public async Task<Tuple<HttpStatusCode, string>> PostWithFiles(string jsonBody, IFormFileCollection Files, string jsonName = "jasonData")
        {
            using (var content = new MultipartFormDataContent())
            {
                if(Files.Count > 0) { AddFilesToContent(Files, content); }
                
                AddJsonToContent(jsonBody, content, jsonName);

                return await SendRequestAsync(HttpMethod.Post, content);
            }
        }

        public async Task<Tuple<HttpStatusCode, string>> PutWithFiles(string jsonBody, IFormFileCollection Files, string jsonName = "jasonData")
        {
            using (var content = new MultipartFormDataContent())
            {
                if (Files.Count > 0) { AddFilesToContent(Files, content); }

                AddJsonToContent(jsonBody, content, jsonName);

                return await SendRequestAsync(HttpMethod.Put, content);
            }
        }

        private void AddFilesToContent(IFormFileCollection Files, MultipartFormDataContent content)
        {
            if (Files != null && Files.Count > 0)
            {
                foreach (var file in Files)
                {
                    content.Add(CreateFileContent(file.OpenReadStream(), file.FileName, file.ContentType));
                }
            }
        }
        private StreamContent CreateFileContent(Stream stream, string fileName, string contentType)
        {
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "files",
                FileName = fileName
            };
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            return fileContent;
        }
        private void AddJsonToContent(string jsonBody, MultipartFormDataContent content, string name)
        {
            if (!string.IsNullOrEmpty(jsonBody))
            {
                content.Add(new StringContent(jsonBody, Encoding.UTF8, "application/json"), $"\"{name}\"");
            }
        }

        public async Task<Tuple<HttpStatusCode, string>> Delete() => await SendRequestAsync(HttpMethod.Delete);


        public async Task<Tuple<HttpStatusCode, string>> CustomPost(string jsonBody, IFormFile emailAttachment, IFormFile proposalAttachment, IFormFile poAttachment, IFormFile costAttachment, string jsonName = "jasonData")
        {
            using (var content = new MultipartFormDataContent())
            {
                if (emailAttachment != null)
                {
                    content.Add(new StreamContent(emailAttachment.OpenReadStream())
                    {
                        Headers =
                {
                    ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "Emailattachment",
                        FileName = emailAttachment.FileName
                    }
                }
                    });
                }

                if (proposalAttachment != null)
                {
                    content.Add(new StreamContent(proposalAttachment.OpenReadStream())
                    {
                        Headers =
                {
                    ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "Proposalattachment",
                        FileName = proposalAttachment.FileName
                    }
                }
                    });
                }

                if (poAttachment != null)
                {
                    content.Add(new StreamContent(poAttachment.OpenReadStream())
                    {
                        Headers =
                {
                    ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "Poattachment",
                        FileName = poAttachment.FileName
                    }
                }
                    });
                }

                if (costAttachment != null)
                {
                    content.Add(new StreamContent(costAttachment.OpenReadStream())
                    {
                        Headers =
                {
                    ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "Costattachment",
                        FileName = costAttachment.FileName
                    }
                }
                    });
                }

                AddJsonToContent(jsonBody, content, jsonName);

                return await SendRequestAsync(HttpMethod.Post, content);
            }
        }
        public async Task<Tuple<HttpStatusCode, string>> CustomPut(string jsonBody, IFormFile emailAttachment, IFormFile proposalAttachment, IFormFile poAttachment, IFormFile costAttachment, string jsonName = "jasonData")
        {
            using (var content = new MultipartFormDataContent())
            {
                if (emailAttachment != null)
                {
                    content.Add(new StreamContent(emailAttachment.OpenReadStream())
                    {
                        Headers =
                {
                    ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "Emailattachment",
                        FileName = emailAttachment.FileName
                    }
                }
                    });
                }

                if (proposalAttachment != null)
                {
                    content.Add(new StreamContent(proposalAttachment.OpenReadStream())
                    {
                        Headers =
                {
                    ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "Proposalattachment",
                        FileName = proposalAttachment.FileName
                    }
                }
                    });
                }

                if (poAttachment != null)
                {
                    content.Add(new StreamContent(poAttachment.OpenReadStream())
                    {
                        Headers =
                {
                    ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "Poattachment",
                        FileName = poAttachment.FileName
                    }
                }
                    });
                }

                if (costAttachment != null)
                {
                    content.Add(new StreamContent(costAttachment.OpenReadStream())
                    {
                        Headers =
                {
                    ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "Costattachment",
                        FileName = costAttachment.FileName
                    }
                }
                    });
                }

                AddJsonToContent(jsonBody, content, jsonName);

                return await SendRequestAsync(HttpMethod.Put, content);
            }
        }

    }

}
