using Hexiron.Azure.ActiveDirectory.Connectors;
using Hexiron.Azure.ActiveDirectory.Connectors.Interfaces;
using Hexiron.Azure.ActiveDirectory.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Hexiron.Azure.ActiveDirectory.Sample
{
    public class Startup
    {
        private readonly IHostingEnvironment _environment;
        public Startup(IHostingEnvironment environment)
        {
            _environment = environment;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IAzureAdSecuredApiConnector, AzureAdSecuredApiConnector>();
            services.AddTransient<IAzureB2CSecuredApiConnector, AzureB2CSecuredApiConnector>();
            services.AddTransient<IGraphApiConnector, GraphApiConnector>();

            var azureConfiguration = AzureSettingsLoader.LoadAzureAdConfiguration(_environment);
            // register Azure Settings to be able to use the IOptions pattern via DI
            services.Configure<AzureAuthenticationSettings>(azureConfiguration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
