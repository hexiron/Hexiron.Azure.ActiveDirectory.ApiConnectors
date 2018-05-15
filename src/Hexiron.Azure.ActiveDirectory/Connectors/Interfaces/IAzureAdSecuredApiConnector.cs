using System;
using System.Net.Http;
using System.Threading.Tasks;
namespace Hexiron.Azure.ActiveDirectory.Connectors.Interfaces
{
    public interface IAzureAdSecuredApiConnector
    {
        Task<HttpResponseMessage> Post(string url, Object objectToBePosted, string azureResourceId);
        Task<T> Put<T>(string url, Object objectToBePosted, string azureResourceId, int requestTimeoutInSec = 60);
        Task<T> Get<T>(string url, string azureResourceId, int requestTimeoutInSec = 60);
        Task<HttpResponseMessage> Delete(string url, string azureResourceId);
    }
}
