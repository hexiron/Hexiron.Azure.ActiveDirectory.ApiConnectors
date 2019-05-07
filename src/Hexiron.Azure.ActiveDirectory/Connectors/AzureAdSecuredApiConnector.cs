using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly Dictionary<string, string> _defaultHeaders;
        public AzureAdSecuredApiConnector(IOptions<AzureAdOptions> options)
        {
            ValidateOptions(options);
            _authenticationContext = new AuthenticationContext(options.Value.Authority);
            _clientCredential = new ClientCredential(options.Value.ClientId, options.Value.ClientSecret);
            _defaultHeaders = new Dictionary<string, string>();
        }

        public async Task<HttpResponseMessage> Post(string url, object objectToBePosted, string azureResourceId)
        {
            var token = await _authenticationContext.AcquireTokenAsync(azureResourceId, _clientCredential);
            return await url.WithOAuthBearerToken(token.AccessToken)
                    .WithHeaders(_defaultHeaders)
                    .PostJsonAsync(objectToBePosted);
        }

        public async Task<T> Put<T>(string url, object objectToBePosted, string azureResourceId, int requestTimeoutInSec = 60)
        {
            var token = await _authenticationContext.AcquireTokenAsync(azureResourceId, _clientCredential);
            return await url.WithOAuthBearerToken(token.AccessToken)
                .WithTimeout(requestTimeoutInSec)
                .WithHeaders(_defaultHeaders)
                .PutJsonAsync(objectToBePosted)
                .ReceiveJson<T>();
        }

        public async Task<T> Get<T>(string url, string azureResourceId, int requestTimeoutInSec = 60)
        {
                var token = await _authenticationContext.AcquireTokenAsync(azureResourceId, _clientCredential);
                return await url.WithOAuthBearerToken(token.AccessToken)
                    .WithTimeout(requestTimeoutInSec)
                    .WithHeaders(_defaultHeaders)
                    .GetJsonAsync<T>();
        }
        public async Task<HttpResponseMessage> Delete(string url, string azureResourceId)
        {
            var token = await _authenticationContext.AcquireTokenAsync(azureResourceId, _clientCredential);
            return await url.WithOAuthBearerToken(token.AccessToken).DeleteAsync();
        }

        public void AddDefaultHeader(string name, string value)
        {
            _defaultHeaders.Add(name,value);
        }
        public void AddDefaultHeaders(IDictionary<string,string> defaultHeaders)
        {
            foreach (var header in defaultHeaders)
            {
                _defaultHeaders.Add(header.Key, header.Value);
            }
        }

        private void ValidateOptions(IOptions<AzureAdOptions> options)
        {
            var validationErrors = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(options?.Value?.Authority))
            {
                validationErrors.Add("Authority", "AzureAD authority is not specified in the settings");
            }
            if (string.IsNullOrEmpty(options?.Value?.ClientId))
            {
                validationErrors.Add("ClientId", "AzureAD clientId is not specified in the settings");
            }
            if (string.IsNullOrEmpty(options?.Value?.ClientId))
            {
                validationErrors.Add("ClientSecret", "AzureAD clientSecret is not specified in the settings");
            }
            if (validationErrors.Any())
            {
                var errormessage = "";
                foreach (var validationError in validationErrors)
                {
                    errormessage += $"{validationError.Key}, ";
                }
                throw new ArgumentNullException("The following azureAdSettings are empty: " + errormessage);
            }
        }
    }
}
