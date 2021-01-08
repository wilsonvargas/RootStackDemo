using RootStack.Demo.Services.Interfaces;
using RootStack.Demo.Universal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace RootStack.Demo.Services
{
    public class BackendRequestHandlerService : IBackendRequestHandlerService
    {
        private HttpClientService httpServiceClient;
        public async Task<TResult> GetRequest<TResult>(string apiRoute, Dictionary<string, string> parameters = null)
        {
            Task<TResult> getFunction(CancellationToken token) => httpServiceClient.GetRequest<TResult>(apiRoute, token, parameters);
            return await SendRequest(apiRoute, getFunction, parameters);
        }

        public async Task<TResult> PostRequest<TRequest, TResult>(string apiRoute, TRequest request)
        {
            Task<TResult> postFunction(CancellationToken token) => httpServiceClient.PostRequest<TRequest, TResult> (request, apiRoute, token);
            return await SendRequest(apiRoute, postFunction);
        }

        private async Task<TResult> SendRequest<TResult>(string apiRoute, Func<CancellationToken, Task<TResult>> func, Dictionary<string, string> requestProperties = null)
        {            
            InitializeHttpClient();
            CancellationTokenSource cancelSource = new CancellationTokenSource();
            try
            {
                Task<TResult> getResponseTask = func(cancelSource.Token);
                if (await Task.WhenAny(getResponseTask, Task.Delay(UniversalSettings.DefaultTimeout)) == getResponseTask)
                {
                    return getResponseTask.Result;
                }
                else
                {
                    return default;
                }
            }
            catch (Exception e)
            {
                return default;
            }
            finally
            {
                cancelSource.Cancel(false);
                cancelSource.Dispose();
            }
        }

        private void InitializeHttpClient()
        {
            if (httpServiceClient == null)
            {
                httpServiceClient = new HttpClientService(new Uri(UniversalSettings.BackendUrl));
            }
        }
    }
}
