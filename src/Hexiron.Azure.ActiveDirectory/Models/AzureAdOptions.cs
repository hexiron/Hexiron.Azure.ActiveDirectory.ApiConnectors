namespace Hexiron.Azure.ActiveDirectory.Models
{
    public class AzureAdOptions
    {
        private readonly string _azureAdInstance;

        public AzureAdOptions()
        {
            _azureAdInstance = "https://login.microsoftonline.com";
        }
        public string ClientSecret { get; set; }
        public string ClientId { get; set; }
        public string Tenant { get; set; }
        public string Authority => $"{_azureAdInstance}/{Tenant}";
        public bool Enabled { get; set; }
    }
}
