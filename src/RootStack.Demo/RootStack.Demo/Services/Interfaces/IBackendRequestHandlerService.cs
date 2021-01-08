using RootStack.Demo.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(BackendRequestHandlerService))]
namespace RootStack.Demo.Services.Interfaces
{
    public interface IBackendRequestHandlerService
    {
        Task<TResult> GetRequest<TResult>(string apiRoute, Dictionary<string, string> parameters = null);
        Task<TResult> PostRequest<TRequest, TResult>(string apiRoute, TRequest request);
    }
}
