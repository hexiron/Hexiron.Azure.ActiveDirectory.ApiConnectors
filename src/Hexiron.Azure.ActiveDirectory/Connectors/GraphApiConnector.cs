using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Hexiron.Azure.ActiveDirectory.Connectors.Interfaces;
using Hexiron.Azure.ActiveDirectory.Models;
using Microsoft.Extensions.Options;
using Microsoft.CSharp;


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

        public async Task<List<string>> GetMemberGroupsForUser(Guid userId)
        {
            // https://graph.windows.net/odotb2c.onmicrosoft.com/users/f2cd0556-f14f-42ac-aacd-f04889919e08/getMemberGroups?api-version=1.6
            var url = $"{_graphApiUrl}/users/{userId}/getMemberGroups?{_version}";
            var request = new {securityEnabledOnly = true};
            var groups = await _azureAdSecuredApiConnector.Post(url, request, RESOURCE)
                                 .ReceiveJson<GroupMembershipResponse>();
            return groups.Values;
        }

        public async Task<List<User>> GetUsers()
        {
            var url = $"{_graphApiUrl}/users?{_version}";
            var users = await _azureAdSecuredApiConnector.Get<GetUsersResponse>(url, RESOURCE);
            return users.Users;
        }

        public async Task<dynamic> GetUserDynamic(Guid userId)
        {
            var url = $"{_graphApiUrl}/users/{userId}?{_version}";
            var user = await _azureAdSecuredApiConnector.Get<dynamic>(url, RESOURCE);
            return user;
        }

        public async Task<User> GetUser(Guid userId)
        {
            var url = $"{_graphApiUrl}/users/{userId}?{_version}";
            var user = await _azureAdSecuredApiConnector.Get<User>(url, RESOURCE);
            return user;
        }

        public async Task<dynamic> CreateUserAD(User user)
        {
            var url = $"{_graphApiUrl}/users?{_version}";
           
            var userBack = await _azureAdSecuredApiConnector.Post(url, user, RESOURCE)
                                 .ReceiveJson<dynamic>();
            return userBack;
        }

        public async Task UpdateUser(User user)
        {
            var url = $"{_graphApiUrl}/users/{user.ObjectId}?{_version}";

            await _azureAdSecuredApiConnector.Patch(url, user, RESOURCE);
            
        }

        public async Task DeleteUser(Guid userId)
        {
            var url = $"{_graphApiUrl}/users/{userId}?{_version}";
            await _azureAdSecuredApiConnector.Delete(url, RESOURCE);
            
        }

        public async Task ResetUserPassword(Guid userId, PasswordResetPasswordProfile passwordResetPasswordProfile)
        {
            var url = $"{_graphApiUrl}/users/{userId}?{_version}";

            await _azureAdSecuredApiConnector.Patch(url, passwordResetPasswordProfile, RESOURCE);
        }

        public async Task<dynamic> CreateGroup(Group group)
        {
            var url = $"{_graphApiUrl}/groups?{_version}";

            var groupBack = await _azureAdSecuredApiConnector.Post(url, group, RESOURCE)
                                 .ReceiveJson<dynamic>();
            return groupBack;
        }

        public async Task<dynamic> GetGroups()
        {
            var url = $"{_graphApiUrl}/groups?{_version}";

            var group = await _azureAdSecuredApiConnector.Get<dynamic>(url, RESOURCE);
            return group;
        }
        
        public async Task<dynamic> GetGroup(Guid groupId)
        {
            var url = $"{_graphApiUrl}/groups/{groupId}?{_version}";
            return await _azureAdSecuredApiConnector.Get<dynamic>(url, RESOURCE);
        }

        public async Task AddUserToGroup(Guid userId, Guid groupId)
        {
            string variabel = "$links/members";
            var url = $"{_graphApiUrl}/groups/{groupId}/{variabel}?{_version}";
            AddMemberToGroup addMemberToGroup = new AddMemberToGroup($"{_graphApiUrl}/directoryObjects/{userId}");
            await _azureAdSecuredApiConnector.Post(url, addMemberToGroup, RESOURCE );
        }

        public async Task<List<UserUrl>> GetDirectUsersFromGroup(Guid groupId)
        {
            string variabel = "$links/members";
            var url = $"{_graphApiUrl}/groups/{groupId}/{variabel}?{_version}";
            var item = await _azureAdSecuredApiConnector.Get<GetDirectMembersFromGroupResponse>(url, RESOURCE);
            return item.Members;
            
        }

        public async Task UpdateGroup(Group group, Guid groupId)
        {
            var url = $"{_graphApiUrl}/groups/{groupId}?{_version}";
            await _azureAdSecuredApiConnector.Patch(url, group, RESOURCE);
        }

        public async Task DeleteGroup(Guid groupId)
        {
            var url = $"{_graphApiUrl}/groups/{groupId}?{_version}";
            await _azureAdSecuredApiConnector.Delete(url, RESOURCE);
        }

        public async Task DeleteMemberFromGroup(Guid groupId, Guid memberId)
        {
            var url = $"{_graphApiUrl}/groups/{groupId}/$links/members/{memberId}?{_version}";
            await _azureAdSecuredApiConnector.Delete(url, RESOURCE);
        }

        public async Task<List<dynamic>> GetPolicies()
        {
            var url = $"{_graphApiUrl}/policies?{_version}";
            var item = await _azureAdSecuredApiConnector.Get<GetPoliciesResponse>(url, RESOURCE);
            return item.Properties;
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
