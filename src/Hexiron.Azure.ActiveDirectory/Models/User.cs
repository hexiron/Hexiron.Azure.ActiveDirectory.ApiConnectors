using Newtonsoft.Json;
using System;

namespace Hexiron.Azure.ActiveDirectory.Models
{
    public class User
    {
        public string DisplayName { get; set; }
        
        public string MailNickname { get; set; }
        
        public string GivenName { get; set; }
        public Guid ObjectId { get; set; }
        
        public Boolean AccountEnabled { get; set; }
        
        public string UserPrincipalName { get; set; }
        
        public PasswordProfile PasswordProfile { get; set; }
    }
}
