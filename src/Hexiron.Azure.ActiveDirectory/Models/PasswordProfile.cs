using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexiron.Azure.ActiveDirectory.Models
{
    public class PasswordProfile
    {
        public string Password { get;set; }
        public Boolean ForceChangePasswordNextLogin { get; set; }
    }
}
