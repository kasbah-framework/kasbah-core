using Microsoft.AspNet.Builder;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;

namespace Kasbah.Core.Admin
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSingleton<ContentTree.IContentTreeService, ContentTree.Npgsql.ContentTreeService>();
            services.AddSingleton<Events.IEventService, Events.InProcEventService>();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            app.UseDeveloperExceptionPage();

            app.UseMvcWithDefaultRoute();
        }
    }
}