using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GoingOnce.Startup))]
namespace GoingOnce
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
