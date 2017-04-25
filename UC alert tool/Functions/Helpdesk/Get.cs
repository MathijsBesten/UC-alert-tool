using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UC_alert_tool.Models;


namespace UC_alert_tool.Functions.Helpdesk
{
    public class Get
    {
        public static void getAllUsers()
        {
            using (var db = new Models.supportcenterEntities())
            {;
                Console.WriteLine("");         
                var usersWithAlert = db.Requester_Fields.Where(u => u.UDF_CHAR1 == "Ja");
                var allDepartmentuserID = usersWithAlert.Select(u => u.DEPARTMENTUSERID).ToList<long>();
                var allContactInfo = new List<AaaContactInfo>();
                var allNewContacts = new List<Klanten>();
                foreach (long departmentUserID in allDepartmentuserID)
                {
                    var departmentuser = db.DepartmentUser.First(u => u.DEPARTMENTUSERID == departmentUserID);
                    var userID = departmentuser.USERID;
                    var aaaUserItem = db.AaaUser.First(u => u.USER_ID == userID);
                    allContactInfo.Add(aaaUserItem.AaaContactInfo.First());
                }
                
                Console.Write("");
                Console.WriteLine("");
            }
        }
    }
}