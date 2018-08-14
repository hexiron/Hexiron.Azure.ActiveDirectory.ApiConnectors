using Hexiron.Azure.ActiveDirectory.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hexiron.Azure.ActiveDirectory.Connectors.Interfaces
{
    public interface IGraphApiConnector
    {
        Task<List<string>> GetMemberGroupsForUser(Guid userId);
        Task<List<User>> GetUsers();
        Task<dynamic> GetUserDynamic(Guid userId);
        Task<User> GetUser(Guid userId);
        Task<dynamic> CreateUserAD(User user);
        Task UpdateUser(User user);
        Task DeleteUser(Guid userId);
        Task ResetUserPassword(Guid userId, PasswordResetPasswordProfile passwordResetPasswordProfilerofile);
        Task<dynamic> CreateGroup(Group group);
        Task<dynamic> GetGroups();
        Task<dynamic> GetGroup(Guid groupId);
        Task AddUserToGroup(Guid userId, Guid groupId);
        Task<List<UserUrl>> GetDirectUsersFromGroup(Guid groupId);
        Task UpdateGroup(Group group, Guid groupId);
        Task DeleteGroup(Guid groupId);
        Task DeleteMemberFromGroup(Guid groupId, Guid memberId);
        Task<List<dynamic>> GetPolicies();
    }
}