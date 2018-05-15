using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http;
using Hexiron.Azure.ActiveDirectory.Connectors.Interfaces;
using Hexiron.Azure.ActiveDirectory.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Hexiron.Azure.ActiveDirectory.Connectors
{
    public class AzureAdSecuredApiConnector : IAzureAdSecuredApiConnector
    {
        private readonly AuthenticationContext _authenticationContext;
        private readonly ClientCredential _clientCredential;
        public AzureAdSecuredApiConnector(IOptions<AzureAuthenticationSettings> options)
        {
            _authenticationContext = new AuthenticationContext(options.Value.AzureAdSettings.Authority);
            _clientCredential = new ClientCredential(options.Value.AzureAdSettings.ClientId, options.Value.AzureAdSettings.ClientSecret);

        }
        public async Task<HttpResponseMessage> Post(string url, object objectToBePosted, string azureResourceId)
        {
            var token = await _authenticationContext.AcquireTokenAsync(azureResourceId, _clientCredential);
            return await url.WithOAuthBearerToken(token.AccessToken).PostJsonAsync(objectToBePosted);
        }

        public async Task<T> Put<T>(string url, object objectToBePosted, string azureResourceId, int requestTimeoutInSec = 60)
        {
            var token = await _authenticationContext.AcquireTokenAsync(azureResourceId, _clientCredential);
            return await url.WithOAuthBearerToken(token.AccessToken)
                .WithTimeout(requestTimeoutInSec)
                .PutJsonAsync(objectToBePosted)
                .ReceiveJson<T>();
        }

        public async Task<T> Get<T>(string url, string azureResourceId, int requestTimeoutInSec = 60)
        {
            try
            {
                var token = await _authenticationContext.AcquireTokenAsync(azureResourceId, _clientCredential);
                return await url.WithOAuthBearerToken(token.AccessToken)
                    .WithTimeout(requestTimeoutInSec)
                    .GetJsonAsync<T>();
            }
            catch (FlurlHttpException ex)
            {
                var statusCode = ex.Call?.Response?.StatusCode;
                if (statusCode != null && statusCode == HttpStatusCode.NotFound)
                {
                    return default(T);
                }
                // no need for stacktrace in logging so just throw ex instead of throw
                throw ex;
            }
        }
        public async Task<HttpResponseMessage> Delete(string url, string azureResourceId)
        {
            var token = await _authenticationContext.AcquireTokenAsync(azureResourceId, _clientCredential);
            return await url.WithOAuthBearerToken(token.AccessToken).DeleteAsync();
        }
    }
}
