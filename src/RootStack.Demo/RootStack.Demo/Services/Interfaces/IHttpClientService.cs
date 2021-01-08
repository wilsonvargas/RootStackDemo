using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RootStack.Demo.Services.Interfaces
{
    public interface IHttpClientService
    {
        Task<TResult> GetRequest<TResult>(string requestUri, CancellationToken cancellationToken, Dictionary<string, string> parameters = null);
        Task<TResult> PostRequest<TContent, TResult>(TContent content, string requestUrl, CancellationToken cancellationToken);
    }
}
