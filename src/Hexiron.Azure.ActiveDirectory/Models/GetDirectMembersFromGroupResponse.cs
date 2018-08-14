using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexiron.Azure.ActiveDirectory.Models
{
    class GetDirectMembersFromGroupResponse
    {
        [JsonProperty(PropertyName = "value")]
        public List<UserUrl> Members { get; set; }
    }
}
