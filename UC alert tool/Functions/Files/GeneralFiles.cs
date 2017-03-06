using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace UC_alert_tool.Functions.Files
{
    public class GeneralFiles
    {
        public static void MakeSignatureFolderIfItNotExist()
        {
            string path = HostingEnvironment.MapPath(ConfigurationManager.AppSettings["SignatureDirectory"]);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}