using Microsoft.AspNet.Builder;

namespace Kasbah.Core.Admin
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}