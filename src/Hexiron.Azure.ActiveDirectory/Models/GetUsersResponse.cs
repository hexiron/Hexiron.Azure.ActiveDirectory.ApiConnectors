using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexiron.Azure.ActiveDirectory.Models
{
    public class GetUsersResponse
    {
        [JsonProperty(PropertyName = "value")]
        public List<User> Users { get; set; }
    }
}
