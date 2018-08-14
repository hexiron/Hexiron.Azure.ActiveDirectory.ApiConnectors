using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexiron.Azure.ActiveDirectory.Models
{
    public class GetPoliciesResponse
    {
        [JsonProperty(PropertyName = "value")]
        public List<dynamic> Properties { get; set; }
    }
}
