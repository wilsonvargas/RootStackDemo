using Newtonsoft.Json;
using RootStack.Demo.Exceptions;
using RootStack.Demo.Helpers;
using RootStack.Demo.Services.Interfaces;
using RootStack.Demo.Universal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RootStack.Demo.Services
{
    public class HttpClientService : IDisposable, IHttpClientService
    {
        private HttpClient client;

        public HttpClientService(Uri backendUrl)
        {
            client = new HttpClient() { BaseAddress = backendUrl, Timeout = TimeSpan.FromMilliseconds(UniversalSettings.DefaultTimeout) };
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (client != null)
                {
                    client.Dispose();
                    client = null;
                }
            }
        }

        public async Task<TResult> GetRequest<TResult>(string requestUri, CancellationToken cancellationToken, Dictionary<string, string> parameters = null)
        {
            if (parameters != null)
            {
                requestUri = $"{requestUri}?{string.Join("&", parameters.Select(kvp => $"{kvp.Key}={kvp.Value}"))}";
            }

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            return await SendAsync<TResult>(request, cancellationToken);
        }

        public async Task<TResult> PostRequest<TContent, TResult>(TContent content, string requestUrl, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            HttpContent httpContent = HttpUtility.CreateHttpContent(content);
            request.Content = httpContent;

            return await SendAsync<TResult>(request, cancellationToken);
        }

        private async Task<T> SendAsync<T>(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

                Stream stream = await response.Content.ReadAsStreamAsync();

                if (response.IsSuccessStatusCode)
                {
                    string contentResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(contentResponse);
                }

                string responseContent = await HttpUtility.StreamToStringAsync(stream);
                throw new HttpServiceException { StatusCode = (int)response.StatusCode, Content = responseContent };
            }
            catch (Exception ex)
            {

                throw new Exception();
            }

        }
    }
}
