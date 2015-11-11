using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Alertsense.Azure.Lumberjack.Startup))]
namespace Alertsense.Azure.Lumberjack
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
