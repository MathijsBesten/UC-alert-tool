using Hangfire;
using Hangfire.Dashboard;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;

[assembly: OwinStartupAttribute(typeof(UC_alert_tool.Startup))]
namespace UC_alert_tool
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            GlobalConfiguration.Configuration.UseSqlServerStorage("HangfireConnection",new Hangfire.SqlServer.SqlServerStorageOptions { QueuePollInterval = TimeSpan.FromMinutes(1) }) ;
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 2 });
            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                AuthorizationFilters = new[] { new HangfireAuthorizationFilter() }
            });
            int syncinterval = int.Parse(Functions.Appsettings.Get.setting("DatabaseUpdateTimer"));
            RecurringJob.AddOrUpdate("Database updaten", () => Functions.Helpdesk.Get.ClearDatabaseAndSync(), Cron.HourInterval(syncinterval));
            RecurringJob.AddOrUpdate("I am Alive email",() => Functions.Webserver.FeedbackMessage.sendIamAliveEmail(), Cron.Daily);
        }
    }
    public class HangfireAuthorizationFilter : IAuthorizationFilter
    {
        public bool Authorize(IDictionary<string, object> owinEnvironment)
        {
            var context = new OwinContext(owinEnvironment);
            return context.Authentication.User.Identity.IsAuthenticated;
        }
    }
}
