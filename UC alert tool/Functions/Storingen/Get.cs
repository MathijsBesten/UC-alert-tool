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
            alertDatabaseEntities db = new alertDatabaseEntities();
            List<Models.Storingen> AllStoringen = db.Storingen.ToList();
            var ThisWeeksStoringen = AllStoringen.Where((s => s.IsGesloten == false || (s.Einddatum.Value.Year == DateTime.Now.Year && s.Einddatum.Value.DayOfYear + 7 >= DateTime.Now.DayOfYear))).ToList<Models.Storingen>();
            return ThisWeeksStoringen;
        }
    }
}