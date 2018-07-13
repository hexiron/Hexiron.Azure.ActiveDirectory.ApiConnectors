using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexiron.Azure.ActiveDirectory.Models
{
    public class PasswordResetPasswordProfile
    {
        private PasswordProfile passwordProfile;
        [JsonProperty(PropertyName = "passwordProfile")]
        public PasswordProfile PasswordProfile
        {
            get
            {
                return passwordProfile;
            }
            set
            {
                passwordProfile = new PasswordProfile();
                passwordProfile.ForceChangePasswordNextLogin = true;
                passwordProfile.Password = "123NieuwPasswoord";
            }
        }

    }
}
