using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexiron.Azure.ActiveDirectory.Models
{
    public class Group
    {
        public Guid ObjectId { get; set; }

        public string Description { get; set; }

        public string DisplayName { get; set; }

        public string MailNickname { get; set; }

        public bool MailEnabled { get; set; }

        public bool SecurityEnabled
        {
            get
            {
                return true;
            }
        }
    }
}

