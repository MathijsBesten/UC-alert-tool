using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UC_alert_tool.Models;


namespace UC_alert_tool.Functions.Helpdesk
{
    public class Get
    {
        private static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
                    try
                    {
                        Klanten klant = new Klanten()
                        {
                            PrimaireEmail = aaaUserItem.AaaContactInfo.First().EMAILID,
                            Telefoonnummer = aaaUserItem.AaaContactInfo.First().MOBILE,
                            debiteurnummer = aaaUserItem.AaaContactInfo.First().AaaUser.First().SDUser.Customer_Requester.First().Customer.AaaOrganization.Customer.Department_Account.First().Customer_Fields.UDF_LONG1.ToString(),
                            Naam = aaaUserItem.AaaContactInfo.First().EMAILID
                        };
                        allNewContacts.Add(klant);
                    }
                    catch (Exception e)
                    {
                        log.Error("Could not add user \'" +aaaUserItem.AaaContactInfo.First().EMAILID + " \' to the database, user has probably no company assigned to him/her");
                        log.Error("Error: " + e);
                    }

                }
                
                Console.Write("");
                Console.WriteLine("");
            }
        }
    }
}