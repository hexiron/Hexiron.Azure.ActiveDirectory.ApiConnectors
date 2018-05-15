using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hexiron.Azure.ActiveDirectory.Connectors.Interfaces
{
    public interface IAzureB2CSecuredApiConnector
    {
        Task<HttpResponseMessage> Post(string url, Object objectToBePosted);
        Task<HttpResponseMessage> Put(string url, Object objectToBePosted);
        Task<T> Get<T>(string url, int requestTimeoutInSec = 60);
    }
}