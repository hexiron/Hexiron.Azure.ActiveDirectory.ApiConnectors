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
        public string RedirectUri { get; set; }
        public string Authority => $"{Domain}/{DefaultPolicy}/v2.0";
        public string Domain => $"{_azureAdB2CInstance}/{Tenant}";

        public string ScopePrefix { get; set; }

        public string[] Scopes { get; set; }
        // a space seperated list of necessary scopes for accessing the api 
        public string[] ApiScopes
        {
            get
            {   
                var scopeList = new List<string>();
                if (Scopes != null)
                {
                    if (!string.IsNullOrEmpty(ScopePrefix))
                    {
                        if (ScopePrefix.EndsWith("/"))
                        {
                            ScopePrefix = ScopePrefix.Remove(ScopePrefix.Length - 1, 1);
                        }
                        foreach (var scope in Scopes)
                        {
                            if (scope.ToLower().StartsWith("http"))
                            {
                                scopeList.Add(scope);
                            }
                            else
                            {

                                scopeList.Add($"{ScopePrefix}/{scope}");
                            }
                        }
                    }
                    else
                    {
                        return Scopes;
                    }
                }
                
                return scopeList.ToArray();
            }
        }

    }
}
