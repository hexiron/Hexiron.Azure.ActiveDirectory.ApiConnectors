namespace Hexiron.Azure.ActiveDirectory.Models
{
    public class AzureAuthenticationSettings
    {
        public bool Enabled { get; set; }
        public AzureB2CSettings AzureB2CSettings { get; set; }
        public AzureAdSettings AzureAdSettings { get; set; }
    }
}
