using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hexiron.Azure.ActiveDirectory.Models
{
    public class UserB2C : User
    {
        public List<SignInName> SignInNames { get; set; }
        
        public string CreationType { get; set; }
    }
}
