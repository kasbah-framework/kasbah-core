using System.Linq;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace Kasbah.Core.Admin
{
    public class Startup
    {
        #region Public Methods

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHostingEnvironment hostingEnv)
        {
            loggerFactory.AddConsole();

            app.UseCors("default");

            app.UseDeveloperExceptionPage();

            app.UseMvcWithDefaultRoute();

            app.ApplicationServices.GetService<Index.IndexService>().Noop();

            app.UseHtml5UrlMode(hostingEnv, loggerFactory, "dist", "/index.html");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("default", builder => {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowAnyOrigin();
                });

                options.DefaultPolicyName = "default";
            });

            services.AddMvc((options) =>
            {
                var formatter = options.OutputFormatters.SingleOrDefault(f => f is JsonOutputFormatter) as JsonOutputFormatter;

                formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddSingleton<ContentTree.IContentTreeProvider, ContentTree.Npgsql.NpgsqlContentTreeProvider>();
            services.AddSingleton<ContentTree.ContentTreeService>();

            services.AddSingleton<Events.IEventBusProvider, Events.InProcEventBusProvider>();
            services.AddSingleton<Events.EventService>();

            services.AddSingleton<Index.IIndexProvider, Index.Solr.SolrIndexProvider>();
            services.AddSingleton<Index.IndexService>();
        }

        #endregion
    }
}
