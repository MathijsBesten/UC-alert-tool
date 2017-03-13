using Hangfire;
using Microsoft.Owin;
using Owin;
using System;
using System.Configuration;

[assembly: OwinStartupAttribute(typeof(UC_alert_tool.Startup))]
namespace UC_alert_tool
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            GlobalConfiguration.Configuration.UseSqlServerStorage("HangfireConnection",new Hangfire.SqlServer.SqlServerStorageOptions { QueuePollInterval = TimeSpan.FromSeconds(1) }) ;
            app.UseHangfireServer();
        }
    }
}
