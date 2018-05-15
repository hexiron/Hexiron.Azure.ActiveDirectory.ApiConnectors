using System.Collections.Generic;
using Newtonsoft.Json;

namespace Hexiron.Azure.ActiveDirectory.Models
{
    public class GroupMembershipResponse
    {
        [JsonProperty(PropertyName = "value")]
        public List<string> Values { get; set; }
    }
}
