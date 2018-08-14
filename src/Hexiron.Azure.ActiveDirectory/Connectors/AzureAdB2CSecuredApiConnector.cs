using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Flurl.Http;
using Hexiron.Azure.ActiveDirectory.Connectors.Interfaces;
using Hexiron.Azure.ActiveDirectory.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace Hexiron.Azure.ActiveDirectory.Connectors
{
    public class AzureAdB2CSecuredApiConnector : IAzureAdB2CSecuredApiConnector
    {
        private readonly ConfidentialClientApplication _confidentialClientApplication;
        private readonly AzureAdB2COptions _azureAdB2COptions;

        public AzureAdB2CSecuredApiConnector(IOptions<AzureAdB2COptions> options, IHttpContextAccessor httpContextAccessor)
        {
            _azureAdB2COptions = options.Value;
            ValidateOptions(options);
            var signedInUserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userTokenCache = new MsalSessionCache(signedInUserId, httpContextAccessor.HttpContext).GetMsalCacheInstance();
            _confidentialClientApplication = new ConfidentialClientApplication(_azureAdB2COptions.ClientId, _azureAdB2COptions.Authority, _azureAdB2COptions.RedirectUri, new ClientCredential(_azureAdB2COptions.ClientSecret), userTokenCache, null);
        }
        public async Task<HttpResponseMessage> Post(string url, object objectToBePosted)
        {
            var token = await GetToken();
            return await url.WithOAuthBearerToken(token.AccessToken).PostJsonAsync(objectToBePosted);
        }

        public async Task<HttpResponseMessage> Put(string url, object objectToBePosted)
        {
            var token = await GetToken();
            return await url.WithOAuthBearerToken(token.AccessToken).PostJsonAsync(objectToBePosted);
        }

        public async Task<T> Get<T>(string url, int requestTimeoutInSec = 60)
        {
            try
            {
                var token = await GetToken();
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

        private async Task<AuthenticationResult> GetToken()
        {
            return await _confidentialClientApplication.AcquireTokenSilentAsync(_azureAdB2COptions.ApiScopes, _confidentialClientApplication.Users.FirstOrDefault(), _azureAdB2COptions.Authority, false);
        }

        private void ValidateOptions(IOptions<AzureAdB2COptions> options)
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
            if (string.IsNullOrEmpty(options?.Value?.RedirectUri))
            {
                validationErrors.Add("ClientSecret", "AzureAD redirectURI is not specified in the settings");
            }
            if (validationErrors.Any())
            {
                var errormessage = "";
                foreach (var validationError in validationErrors)
                {
                    errormessage += $"{validationError.Key}, ";
                }
                throw new ArgumentNullException("The following AzureAdB2CSettings are empty: " + errormessage);
            }
        }
    }
}
