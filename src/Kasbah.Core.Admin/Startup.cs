using System.Linq;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Mvc.Formatters;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Newtonsoft.Json.Serialization;

namespace Kasbah.Core.Admin
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
                    options.AddPolicy("allowSingleOrigin", builder => builder.WithOrigins("http://localhost:3000")));

            services.AddMvc((options) =>
            {
                var formatter = options.OutputFormatters.SingleOrDefault(f => f is JsonOutputFormatter) as JsonOutputFormatter;

                formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddSingleton<ContentTree.IContentTreeService, ContentTree.Npgsql.ContentTreeService>();
            services.AddSingleton<Events.IEventService, Events.InProcEventService>();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            app.UseCors("allowSingleOrigin");

            app.UseDeveloperExceptionPage();

            app.UseMvcWithDefaultRoute();
        }
    }
}