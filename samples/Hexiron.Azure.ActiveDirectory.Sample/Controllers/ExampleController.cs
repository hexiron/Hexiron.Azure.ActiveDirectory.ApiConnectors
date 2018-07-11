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
        private readonly AzureAd _azureAd;

        public ExampleController(IAzureAdSecuredApiConnector azureAdSecuredApiConnector, IOptions<AzureAd> azureSettingsAccessor)
        {
            _azureAdSecuredApiConnector = azureAdSecuredApiConnector;
            _azureAd = azureSettingsAccessor.Value;
        }

        public async Task<ExampleDto> Index()
        {
            _azureAdSecuredApiConnector.AddDefaultHeader("custom-key","value");
            var clientId = _azureAd.ClientId;
            return await _azureAdSecuredApiConnector.Get<ExampleDto>("http://localhost", "azureResourceId");
        }
    }
}
