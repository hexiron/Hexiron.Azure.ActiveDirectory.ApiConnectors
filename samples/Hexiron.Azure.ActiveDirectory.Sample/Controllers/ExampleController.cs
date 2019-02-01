using System.Threading.Tasks;
using Hexiron.Azure.ActiveDirectory.Connectors.Interfaces;
using Hexiron.Azure.ActiveDirectory.Models;
using Hexiron.Azure.ActiveDirectory.Sample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Hexiron.Azure.ActiveDirectory.Sample.Controllers
{
    public class ExampleController : Controller
    {
        private readonly IAzureAdSecuredApiConnector _azureAdSecuredApiConnector;
        private readonly AzureAdOptions _azureAdOptions;
        private readonly AzureAdB2COptions _azureAdB2COptions;

        public ExampleController(IAzureAdSecuredApiConnector azureAdSecuredApiConnector, IOptions<AzureAdOptions> azureSettingsAccessor, IOptions<AzureAdB2COptions> azureB2CSettingsAccessor)
        {
            _azureAdSecuredApiConnector = azureAdSecuredApiConnector;
            _azureAdOptions = azureSettingsAccessor.Value;
            _azureAdB2COptions = azureB2CSettingsAccessor.Value;
        }

        public async Task<ExampleDto> Index()
        {
            _azureAdSecuredApiConnector.AddDefaultHeader("custom-key","value");
            var clientId = _azureAdOptions.ClientId;
            var scopes = _azureAdB2COptions.Scopes;
            return await _azureAdSecuredApiConnector.Get<ExampleDto>("http://localhost", "azureResourceId");
        }
    }
}
