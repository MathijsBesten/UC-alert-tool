using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UC_alert_tool.Models;

namespace UC_alert_tool.Functions.Storingen
{
    public class Get
    {
        public static List<Models.Storingen> AllStoringenFromLastWeek()
        {
            using (var db = new alertDatabaseEntities())
            {
                List<Models.Storingen> AllStoringen = db.Storingen.ToList();
                int daysInHistory = int.Parse(Functions.Appsettings.Get.setting("OldMessageTime"));
                var ThisWeeksStoringen = AllStoringen.Where((s => s.IsGesloten == false || (s.Einddatum.Value.DayOfYear + daysInHistory >= DateTime.Now.DayOfYear && s.Einddatum.Value.Year == DateTime.Now.Year))).ToList<Models.Storingen>();
                return ThisWeeksStoringen;
            }
        }
    }
}