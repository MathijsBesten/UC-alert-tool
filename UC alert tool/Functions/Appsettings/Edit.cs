using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using UC_alert_tool.Models;

namespace UC_alert_tool.Functions.Appsettings
{
    public class Edit
    {
        private static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static bool ChangeExistingValue(string name, string value) // this function will edit a value in the "dbo.settings" table
        {
            try
            {
                using (var db = new alertDatabaseEntities())
                {
                    var ChangeItem = db.Settings.Single(s => s.Setting == name);
                    ChangeItem.Value = value;
                    db.SaveChanges();
                    log.Info("User changed the " + name + "  settings - succesfully");
                    return true;
                }
            }
            catch (Exception e)
            {
                log.Error("User tried to change " + name + "  settings - error - " + e);
                return false;
            }
        }

    }
}