using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebRoleProject.Startup))]
namespace WebRoleProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
