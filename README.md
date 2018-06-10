# Getting started with ASP.NET Core 2 and Azure AD/B2C OAuth2 flows

[![Build status](https://ci.appveyor.com/api/projects/status/snx0tdnj8930gsr9/branch/master?svg=true)](https://ci.appveyor.com/project/mkeymolen/Hexiron.Azure.ActiveDirectory/branch/master)  [![license](https://img.shields.io/github/license/hexiron/Hexiron.Azure.ActiveDirectory.svg?maxAge=2592000)](https://github.com/hexiron/Hexiron.Azure.ActiveDirectory/blob/master/LICENSE)  [![NuGet](https://img.shields.io/nuget/v/Hexiron.Azure.ActiveDirectory.svg?maxAge=86400)](https://www.nuget.org/packages/Hexiron.Azure.ActiveDirectory/)


Hexiron.Azure.ActiveDirectory has easy to use Azure connectors to connect with Azure AD secured API's using OAuth2 flows.
When accessing an API through one of these connecters, it gets in background an access token from the Microsoft identity provider using ADAL for Azure AD and MSAL for Azure B2C and adds it to the request.

The connectors make use of the .NetCore IOptions pattern so make sure you register them in the startup class. More info see below.

**Features**  
- An Azure AD enabled API connector to access API's secured by Azure AD with caching of the JWT included by using ADAL
- An Azure AD B2C enabled API connector to access API's secured by Azure AD B2C with caching of the JWT included by using MSAL
- An Azure Graph API connector to access easily the Azure Graph API to retrieve Azure AD information with caching of the JWT included

### 1. Create a new ASP.NET Core project ###
In Visual Studio 2017.
### 2. Add dependency in csproj manually or using NuGet ###
Install the latest nuget package:

- Hexiron.Azure.ActiveDirectory

in csproj:

```xml
<PackageReference Include="Hexiron.Azure.ActiveDirectory" Version="x.x.x" />
```

### 3. Make sure you register the settings in the startup class.

You have multiple possibilities to load the settings in the startup class so they can be used by the IOptions pattern in the connectors.  
- Add the settings in you appsettings.json file (and corresponding environment files)
- Add the settings in the Azure web application settings online. Recomended for the secrets, so they are not exposed in source code

See example below:

```json
{
  "AzureAdSettings": {
	"Enabled": true,
    "Tenant": "tentantname.onmicrosoft.com",
    "ClientId": "aaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa",
	"ClientSecret": "fc54rg4d5gx4s5fg5dswrg"
  },
  "AzureB2CSettings": {
	"Enabled": true,
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

```csharp  
	private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        //....

        // register Azure AD Settings to be able to use the IOptions pattern via DI
        services.Configure<AzureAdSettings>(_configuration.GetSection("AzureAdSettings"));
        // register Azure B2C Settings to be able to use the IOptions pattern via DI
        services.Configure<AzureB2CSettings>(_configuration.GetSection("AzureB2CSettings"));

		//....
    }
```

### 4. Register the connectors you want to use via Dependency injenction
In the startup.cs class, register also the AzureConfiguration and the connectors you need:
  
```csharp  
public void ConfigureServices(IServiceCollection services)  
    {  
        //... 
		
		services.AddTransient<IAzureAdSecuredApiConnector, AzureAdSecuredApiConnector>();
        services.AddTransient<IAzureB2CSecuredApiConnector, AzureB2CSecuredApiConnector>();
        services.AddTransient<IGraphApiConnector, GraphApiConnector>();

		//...  
    }  
```

### 5. Use the Connectors via Dependency Injection
In the example below we access the connectors immediately in the controllers, but it is recommended to add a mediator in a real application to abstract controllers from any business logic

```csharp  
public class ExampleController : Controller
{
    private readonly IAzureB2CSecuredApiConnector _azureB2CSecuredApiConnector;

    public ExampleController(IAzureB2CSecuredApiConnector azureB2CSecuredApiConnector)
    {
        _azureB2CSecuredApiConnector = azureAdSecuredApiConnector;
    }

    public async Task<ExampleDto> Index()
    {
        return await _azureB2CSecuredApiConnector.Get<ExampleDto>("http://localhost", "azureResourceId");
    }
}
```