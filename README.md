# Getting started with ASP.NET Core 2 and Azure AD/B2C OAuth2 flows

Hexiron.Azure.ActiveDirectory has easy to use Azure connectors to connect with Azure AD secured API's using OAuth2 flows.
When accessing an API through one of these connecters, it gets in background an access token from the Microsoft identity provider and adds it to the request.

**Features**  
- A static settings loader that creates a configuration object to be used by the AspNetCore Options pattern (IOptions<AuthenticationSettings>) and dependency injection
- An Azure AD enabled API connector to access API's secured by Azure AD with caching of the JWT included
- An Azure AD B2C enabled API connector to access API's secured by Azure AD B2C with caching of the JWT included
- An Azure Graph API connector to access easily the Azure Graph API to retrieve Azure AD information with caching of the JWT included

### 1. Create a new ASP.NET Core project ###
In Visual Studio 2017.
### 2. Add dependency in csproj manually or using NuGet ###
Install the latest:

- Hexiron.AspNetCore.Authentication.AzureAdMixed 

in csproj:

```xml
<PackageReference Include="Hexiron.AspNetCore.Authentication.AzureAdMixed" Version="x.x.x" />
```

### 3. Create an azureauthenticationsettings.json file. 
Create azureauthenticationsettings.json (lowercase all) file in the root of your project.  
In this file, you need to fill in the Azure settings from your Azure AD tenant(s).
We use the following example:

```json
{
  "Enabled": true,
  "AzureAdSettings": {
    "Tenant": "tentantname.onmicrosoft.com",
    "ClientId": "aaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa"
  },
  "AzureB2CSettings": {
    "ClientId": "aaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa",
    "Tenant": "tentantname.onmicrosoft.com",
    "SignUpSignInPolicyId": "defined_Policy_from_Azure",
    "ResetPasswordPolicyId": "defined_Policy_from_Azure",
    "EditProfilePolicyId": "defined_Policy_from_Azure",
    "RedirectUri": "https://.../signin-oidc",
    "ClientSecret": "secret",
	"ScopePrefix": "https://myb2c.onmicrosoft.com/my-api/",
    "ApiScopes": "read:companies write:companies" 
  }
}
```

### 4. Register the serivices for the middleware
In the startup.cs class, register the AzureConfiguration and the connectors you need:
  
```csharp  
public void ConfigureServices(IServiceCollection services)  
    {  
        //...  
		services.AddTransient<IAzureAdSecuredApiConnector, AzureAdSecuredApiConnector>();
        services.AddTransient<IAzureB2CSecuredApiConnector, AzureB2CSecuredApiConnector>();
        services.AddTransient<IGraphApiConnector, GraphApiConnector>();

        var azureConfiguration = AzureSettingsLoader.LoadAzureAdConfiguration(_environment);
        // register Azure Settings to be able to use the IOptions pattern via DI
        services.Configure<AzureAuthenticationSettings>(azureConfiguration);
		//...  
    }  
```

Make sure you can inject the IHostingEnvironment interface. This is needed to load the correct settingsfile. You can inject the IHostingEnvironmnet in the startup.cs class by using property injection. The default WebhostBuilder from AspNetCore has already registered the implementation for you.  
Also specify the assembly where your controllers are situated so it can load the correct Authorization policies from you controllers.


```csharp  
private readonly IHostingEnvironment _environment;
        public Startup(IHostingEnvironment environment)
        {
            _environment = environment;
        }
```
### 5. Use the Connectors via Dependency Injection
In the example below we access the connectors immediately in the controllers, but it is recommended to add a mediator in a real application to abstract controllers from any business logic

```csharp  
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
            return await _azureAdSecuredApiConnector.Get<ExampleDto>("http://localhost", "azureResourceId");
        }
    }
```