using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebRole.Startup))]
namespace WebRole
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
