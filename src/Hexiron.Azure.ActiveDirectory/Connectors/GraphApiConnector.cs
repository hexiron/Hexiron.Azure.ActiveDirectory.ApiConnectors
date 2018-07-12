using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Hexiron.Azure.ActiveDirectory.Connectors.Interfaces;
using Hexiron.Azure.ActiveDirectory.Models;
using Microsoft.Extensions.Options;

namespace Hexiron.Azure.ActiveDirectory.Connectors
{
    public class GraphApiConnector : IGraphApiConnector
    {
        private readonly IAzureAdSecuredApiConnector _azureAdSecuredApiConnector;
        private readonly string _graphApiUrl;
        private readonly string _version;
        private const string RESOURCE = "https://graph.windows.net";

        public GraphApiConnector(IAzureAdSecuredApiConnector azureAdSecuredApiConnector, IOptions<AzureAdOptions> options)
        {
            ValidateOptions(options);
            _azureAdSecuredApiConnector = azureAdSecuredApiConnector;
            _graphApiUrl = $"https://graph.windows.net/{options.Value.Tenant}";
            _version = "api-version=1.6";
        }

        public async Task<List<string>> GetMemberGroupsForUser(string userId)
        {
            // https://graph.windows.net/odotb2c.onmicrosoft.com/users/f2cd0556-f14f-42ac-aacd-f04889919e08/getMemberGroups?api-version=1.6
            var url = $"{_graphApiUrl}/users/{userId}/getMemberGroups?{_version}";
            var request = new {securityEnabledOnly = true};
            var groups = await _azureAdSecuredApiConnector.Post(url, request, RESOURCE)
                                 .ReceiveJson<GroupMembershipResponse>();
            return groups.Values;
        }

        private void ValidateOptions(IOptions<AzureAdOptions> options)
        {
            var validationErrors = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(options?.Value?.Tenant))
            {
                validationErrors.Add("Tenant", "AzureAD authority is not specified in the settings");
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
