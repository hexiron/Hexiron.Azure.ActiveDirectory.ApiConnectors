using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexiron.Azure.ActiveDirectory.Models
{
    public class SignInName
    {
        [JsonProperty(PropertyName = "type") ]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }
}
