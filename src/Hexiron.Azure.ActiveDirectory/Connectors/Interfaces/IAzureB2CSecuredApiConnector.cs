using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hexiron.Azure.ActiveDirectory.Connectors.Interfaces
{
    public interface IAzureAdB2CSecuredApiConnector
    {
        Task<HttpResponseMessage> Post(string url, Object objectToBePosted);
        Task<HttpResponseMessage> Put(string url, Object objectToBePutted);
        Task<T> Get<T>(string url, int requestTimeoutInSec = 60);

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