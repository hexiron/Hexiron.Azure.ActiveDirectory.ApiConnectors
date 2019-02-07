# Connect with API's using the Azure AD or Azure AD B2C Identity provider and OAuth2 flows

[![Build status](https://ci.appveyor.com/api/projects/status/snx0tdnj8930gsr9/branch/master?svg=true)](https://ci.appveyor.com/api/projects/status/snx0tdnj8930gsr9/branch/master?svg=true)  [![license](https://img.shields.io/github/license/hexiron/Hexiron.Azure.ActiveDirectory.svg?maxAge=2592000)](https://github.com/hexiron/Hexiron.Azure.ActiveDirectory/blob/master/LICENSE)  [![NuGet](https://img.shields.io/nuget/v/Hexiron.Azure.ActiveDirectory.svg?maxAge=86400)](https://www.nuget.org/packages/Hexiron.Azure.ActiveDirectory/)

### Attention ###
If you are looking how to authenticate with Azure AD or Azure AD B2C and/or accept JWT tokens from Azure AD or Azure AD B2C, please go to [Hexiron.AspNetCore.Authentication.AzureAdMixed](https://github.com/hexiron/Hexiron.AspNetCore.Authentication.AzureAdMixed).  

## What is it about? ##

This library has easy to use API connector clients to connect with API's which are using the Azure AD or Azure AD B2C identity provider and its OAuth2 flows.
When accessing an API through one of the connectors described below, it gets in background an access token from the Microsoft identity provider using ADAL for Azure AD and MSAL for Azure B2C and adds it automatically in background to the request.  
You don't need to worry about getting, storing and maintaining the access token. This library does it for you.

The connectors make use of the .NetCore IOptions pattern so make sure you register them in the startup class. More info see below.

## Features ##
- An **Azure AD enabled API connector** to access API's secured by Azure AD with caching of the JWT included by using ADAL  
- An **Azure AD B2C enabled API connector** to access API's secured by Azure AD B2C with caching of the JWT included by using MSAL  
- An **Azure Graph API connector** to access easily the Azure Graph API to retrieve Azure AD information with caching of the JWT included (implicitly using ADAL)  

### Azure Graph API Connector features 
- GetMemberGroups : get all groups for the specified userid

## How to use:
### 1. Create a new ASP.NET Core project ###
In Visual Studio 2017.
### 2. Add dependency in csproj manually or using NuGet ###
Install the latest nuget package of "Hexiron.Azure.ActiveDirectory"

in csproj:
```xml
<PackageReference Include="Hexiron.Azure.ActiveDirectory" Version="x.x.x" />
```

### 3. Make sure you register the settings in the startup class.

You have multiple possibilities to load the settings in the startup class so they can be used by the IOptions pattern in the connectors.  
- Add the settings in you appsettings.json file (and corresponding environment files)
- Add the settings in the web applications settings in Azure. This is recommended for secrets, so they are not exposed in source code

See example below:
```json
{
  "Authentication":{
	"AzureAd": {
		"Enabled": true,
		"Tenant": "tentantname.onmicrosoft.com",
		"ClientId": "aaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa",
		"ClientSecret": "fc54rg4d5gx4s5fg5dswrg"
		},
	"AzureAdB2C": {
		"Enabled": true,
		"ClientId": "aaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa",
		"Tenant": "tentantname.onmicrosoft.com",
		"SignUpSignInPolicyId": "defined_Policy_from_Azure",
		"ResetPasswordPolicyId": "defined_Policy_from_Azure",
		"EditProfilePolicyId": "defined_Policy_from_Azure",
		"RedirectUri": "https://.../signin-oidc",
		"ClientSecret": "secret",
		"ApiScopes": "read:companies write:companies" 
	}
  }
}
```
Register the configuration settings to be able to use the IOptions pattern and dependency injection.

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
    services.Configure<AzureAdOptions>(_configuration.GetSection("Authentication:AzureAd"));
    // register Azure B2C Settings to be able to use the IOptions pattern via DI
    services.Configure<AzureAdB2COptions>(_configuration.GetSection("Authentication:AzureAdB2C"));
	//....
}
```

### 4. Register the connectors you want to use via Dependency injenction
In the startup.cs class, register the connectors you need.  
If you want to register the GraphApiConnector, you also need to register the IAzureAdSecuredApiConnector as the GraphApiConnector uses this via constructor injection.  
When using the AzureAdB2CSecuredApiConnector, don't forget to register the HttpContextAccessor   
```csharp  
public void ConfigureServices(IServiceCollection services)  
{  
    //... 
	services.AddTransient<IAzureAdSecuredApiConnector, AzureAdSecuredApiConnector>();

    services.AddTransient<IAzureAdB2CSecuredApiConnector, AzureAdB2CSecuredApiConnector>();
	services.AddHttpContextAccessor(); //aspnetcore 2.1
	services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //aspnetcore 2.0

    services.AddTransient<IGraphApiConnector, GraphApiConnector>();
	//...  
}  
```

### 5. Use the Connectors via Dependency Injection
In the example below we access the connectors immediately in the controllers, but it is recommended to add a mediator in a real application to abstract controllers from any business logic

```csharp
public class ExampleController : Controller
{
	private readonly IAzureAdB2CSecuredApiConnector _AzureAdB2CSecuredApiConnector;
    public ExampleController(IAzureAdB2CSecuredApiConnector azureAdB2CSecuredApiConnector)
	{
	    _AzureAdB2CSecuredApiConnector = azureAdB2CSecuredApiConnector;
	}
	public async Task<ExampleDto> Index()
	{
	    return await _AzureAdB2CSecuredApiConnector.Get<ExampleDto>("http://localhost", "azureResourceId");
	}
}
```