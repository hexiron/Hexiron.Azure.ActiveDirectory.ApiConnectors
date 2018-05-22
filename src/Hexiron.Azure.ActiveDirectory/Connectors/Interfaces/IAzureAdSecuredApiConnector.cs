using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Add a default header which is added to each request executed by the connector
        /// </summary>
        /// <param name="name">The name of the header</param>
        /// <param name="value">The value</param>
        void AddDefaultHeader(string name, string value);

        /// <summary>
        /// Add a list of default header which are added to each request executed by the connector
        /// </summary>
        /// <param name="defaultHeaders">A list of default headers</param>
        void AddDefaultHeaders(IDictionary<string, string> defaultHeaders);
    }
}
