using Owin;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(BI.Web.Startup))]

namespace BI.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}