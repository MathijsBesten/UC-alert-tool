using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace UC_alert_tool.Functions.Email_template
{
    public class Set
    {
        public static bool MakeEmailTemplateDirIfNotExistsAndReturnResult()
        {
            string path = ConfigurationManager.AppSettings["SignatureDirectory"];
            bool pathExist = Directory.Exists(path);
            if (!pathExist)
            {
                try
                {
                    Directory.CreateDirectory(path);
                    return true;
                }
                catch (Exception e)
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
                string path = ConfigurationManager.AppSettings["SignatureDirectory"];
                string filename = ConfigurationManager.AppSettings["SignatureImage"];
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