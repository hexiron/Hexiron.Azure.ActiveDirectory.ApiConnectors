using Hexiron.Azure.ActiveDirectory.Connectors;
using Hexiron.Azure.ActiveDirectory.Connectors.Interfaces;
using Hexiron.Azure.ActiveDirectory.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hexiron.Azure.ActiveDirectory.Sample
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IAzureAdSecuredApiConnector, AzureAdSecuredApiConnector>();
            services.AddTransient<IAzureAdB2CSecuredApiConnector, AzureAdB2CSecuredApiConnector>();
            services.AddTransient<IGraphApiConnector, GraphApiConnector>();

            // register Azure AD Settings to be able to use the IOptions pattern via DI
            services.Configure<AzureAdOptions>(_configuration.GetSection("Authentication:AzureAd"));
            // register Azure B2C Settings to be able to use the IOptions pattern via DI
            services.Configure<AzureAdB2COptions>(_configuration.GetSection("Authentication:AzureAdB2C"));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=GraphConnect}/{action=Index}/{id?}");
            });

        }
    }
}
