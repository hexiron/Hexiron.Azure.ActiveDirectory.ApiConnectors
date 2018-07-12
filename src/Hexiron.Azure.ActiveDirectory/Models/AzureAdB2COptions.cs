namespace Hexiron.Azure.ActiveDirectory.Models
{
    public class AzureAdB2C
    {
        //public const string POLICY_AUTHENTICATION_PROPERTY = "Policy";
        private readonly string _azureAdB2CInstance;

        public AzureAdB2C()
        {
            _azureAdB2CInstance = "https://login.microsoftonline.com/tfp";
        }
        public bool Enabled { get; set; }

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public string Tenant { get; set; }
        public string SignUpSignInPolicyId { get; set; }
        //public string SignInPolicyId { get; set; }
        //public string SignUpPolicyId { get; set; }basix
        //public string ResetPasswordPolicyId { get; set; }
        //public string EditProfilePolicyId { get; set; }

        public string RedirectUri { get; set; }
        public string Authority => $"{Domain}/{SignUpSignInPolicyId}/v2.0";
        public string Domain => $"{_azureAdB2CInstance}/{Tenant}";
        public string ScopePrefix { get; set; }

        // a space seperated list of necessary scopes for accessing the api 
        public string ApiScopes { get; set; }

    }
}
