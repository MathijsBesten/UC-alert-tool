using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UC_alert_tool.Startup))]
namespace UC_alert_tool
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
