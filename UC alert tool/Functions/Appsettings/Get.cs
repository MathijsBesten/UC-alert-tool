using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UC_alert_tool.Models;

namespace UC_alert_tool.Functions.Appsettings
{

    public class Get
    {
        private static alertDatabaseEntities db = new alertDatabaseEntities();
        public static string setting(string nameOfSetting)
        {
            return db.Settings.Single(s => s.Setting == nameOfSetting).Value;
        }
    }
}