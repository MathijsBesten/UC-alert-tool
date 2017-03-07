using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using UC_alert_tool.Models;

namespace UC_alert_tool.Functions.Files
{
    public class GeneralFiles
    {
        private static alertDatabaseEntities db = new alertDatabaseEntities();

        public static void MakeSignatureFolderIfItNotExist()
        {
            string path = HostingEnvironment.MapPath(db.Settings.Single(s => s.Setting == "SignatureDirectory").Value);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}