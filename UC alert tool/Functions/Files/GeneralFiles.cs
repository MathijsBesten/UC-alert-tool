using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace UC_alert_tool.Functions.Files
{
    public class GeneralFiles
    {
        public static void MakeProjectFileFolderIfItNotExist()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), ConfigurationManager.AppSettings["FileDirectory"]);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static void MakeSignatureFolderIfItNotExist()
        {
            MakeProjectFileFolderIfItNotExist();
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), ConfigurationManager.AppSettings["FileDirectory"], ConfigurationManager.AppSettings["SignatureDirectory"]);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}