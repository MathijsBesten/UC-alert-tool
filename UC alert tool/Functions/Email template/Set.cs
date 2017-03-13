using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using UC_alert_tool.Models;

namespace UC_alert_tool.Functions.Email_template
{
    public class Set
    {
        private static alertDatabaseEntities db = new alertDatabaseEntities();

        public static bool MakeEmailTemplateDirIfNotExistsAndReturnResult()
        {
            string path = db.Settings.Single(s => s.Setting == "SignatureDirectory").Value;
            bool pathExist = Directory.Exists(path);
            if (!pathExist)
            {
                try
                {
                    Directory.CreateDirectory(path);
                    return true;
                }
                catch (Exception)
                {

                    return false;// the path did not exists and could not be created
                }
            }
            else
            {
                return true;
            }
        }
        public static bool saveLogoPicture(HttpPostedFileBase picture)
        {
            if (MakeEmailTemplateDirIfNotExistsAndReturnResult()) //will return true, if the folder does exists
            {
                string path = db.Settings.Single(s => s.Setting == "SignaturDirectory").Value;
                string filename = db.Settings.Single(s => s.Setting == "SignatureImage").Value; 
                string fullPath = Path.Combine(path, filename);
                picture.SaveAs(path);
                return true;
            }
            else
            {
                return false;
            }
        } 
    }
}