using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace UC_alert_tool
{
    public class HangfireBootstrapper : IRegisteredObject
    {
        public static readonly HangfireBootstrapper Instance = new HangfireBootstrapper();

        private readonly object _lockObject = new object();
        private bool _started;

        private BackgroundJobServer _backgroundJobServer;

        private HangfireBootstrapper()
        {
        }

        public void Start()
        {
            lock (_lockObject)
            {
                if (_started) return;
                _started = true;

                HostingEnvironment.RegisterObject(this);

                GlobalConfiguration.Configuration
                    //Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\aspnet-UC alert tool-Hangfire.mdf;Initial Catalog=aspnet-UC alert tool-Hangfire;Integrated Security=True
                    //Data Source=DC-110\ALERTTOOL;Initial Catalog=aspnet-UC alert tool-Hangfire;Integrated Security=True
                    .UseSqlServerStorage(@"Data Source=DC-110\ALERTTOOL;Initial Catalog=aspnet-UC alert tool-Hangfire;Integrated Security=True");
                // Specify other options here

                _backgroundJobServer = new BackgroundJobServer();
            }
        }

        public void Stop()
        {
            lock (_lockObject)
            {
                if (_backgroundJobServer != null)
                {
                    _backgroundJobServer.Dispose();
                }

                HostingEnvironment.UnregisterObject(this);
            }
        }

        void IRegisteredObject.Stop(bool immediate)
        {
            Stop();
        }
    }
}