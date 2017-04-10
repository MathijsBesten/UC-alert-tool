using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UC_alert_tool.Models;

namespace UC_alert_tool.Functions.Appsettings
{

    public class Get
    {
        public static string setting(string nameOfSetting)
        {
            using (var db = new alertDatabaseEntities())
            {
                return db.Settings.Single(s => s.Setting == nameOfSetting).Value;
            }
        }
    }
}