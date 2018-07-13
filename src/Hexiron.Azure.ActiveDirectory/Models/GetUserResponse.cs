using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexiron.Azure.ActiveDirectory.Models
{
    class GetUserResponse
    {
        [JsonProperty(PropertyName = "value")]
        public User User { get; set; }
    }
}
