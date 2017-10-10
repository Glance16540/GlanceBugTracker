using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GlanceBugTracker.Startup))]
namespace GlanceBugTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
