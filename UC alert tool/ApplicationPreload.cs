using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hangfire;

namespace UC_alert_tool
{
    public class ApplicationPreload : System.Web.Hosting.IProcessHostPreloadClient
    {
        public void Preload(string[] parameters)
        {
            HangfireBootstrapper.Instance.Start();
        }
    }
}