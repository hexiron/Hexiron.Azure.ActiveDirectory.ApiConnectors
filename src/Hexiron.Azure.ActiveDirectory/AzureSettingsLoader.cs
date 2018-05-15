using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Hexiron.Azure.ActiveDirectory
{
    public static class AzureSettingsLoader
    {
        public static IConfigurationRoot LoadAzureAdConfiguration(IHostingEnvironment hostingEnvironment)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("azureauthenticationsettings.json")
                .AddJsonFile($"azureauthenticationsettings.{environment}.json", optional: true);
            return builder.Build();
        }
    }
}
