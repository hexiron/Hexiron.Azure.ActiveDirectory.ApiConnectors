using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hexiron.Azure.ActiveDirectory.Connectors.Interfaces
{
    public interface IGraphApiConnector
    {
        Task<List<string>> GetMemberGroupsForUser(string userId);
    }
}