using System.Collections.Generic;
using System.Linq;

namespace Hexiron.Azure.ActiveDirectory.Models
{
    public class AzureAdB2COptions
    {
        public const string POLICY_AUTHENTICATION_PROPERTY = "Policy";
        private readonly string _azureAdB2CInstance;

        public AzureAdB2COptions()
        {
            _azureAdB2CInstance = "https://login.microsoftonline.com/tfp";
        }
        public bool Enabled { get; set; }

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Tenant { get; set; }
        public string DefaultPolicy { get; set; }
        public string ResetPasswordPolicyId { get; set; }
        public string RedirectUri { get; set; }
        public string Authority => $"{Domain}/{DefaultPolicy}/v2.0";
        public string Domain => $"{_azureAdB2CInstance}/{Tenant}";

        public string[] Scopes { get; set; }

    }
}
