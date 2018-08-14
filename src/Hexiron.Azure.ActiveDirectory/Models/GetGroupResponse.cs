using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexiron.Azure.ActiveDirectory.Models
{
    public class GetGroupResponse
    {
        [JsonProperty(PropertyName = "value")]
        public List<Group> Groups { get; set; }
    }
}
