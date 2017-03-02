using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace UC_alert_tool.Functions.Appsettings
{
    public class Edit
    {
        private static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static bool ChangeExistingValue(string name, string value)
        {
            try
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                config.AppSettings.Settings[name].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                log.Info("User changed the " + name +"  settings - succesfully");
                return true;
            }
            catch (Exception e)
            {
                log.Error("User tried to change " + name + "  settings - error - " + e);
                return false;
            }
        }

    }
}