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
        private readonly AzureB2CSettings _azureB2CSettings;

        public ExampleController(IAzureAdSecuredApiConnector azureAdSecuredApiConnector, IOptions<AzureAuthenticationSettings> azureSettingsAccessor)
        {
            _azureAdSecuredApiConnector = azureAdSecuredApiConnector;
            _azureB2CSettings = azureSettingsAccessor.Value.AzureB2CSettings;
        }

        public async Task<ExampleDto> Index()
        {
            _azureAdSecuredApiConnector.AddDefaultHeader("custom-key","value");
            return await _azureAdSecuredApiConnector.Get<ExampleDto>("http://localhost", "azureResourceId");
        }
    }
}
