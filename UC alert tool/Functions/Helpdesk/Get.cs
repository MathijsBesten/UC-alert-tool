using log4net;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using UC_alert_tool.Models;
using SharepointClient = Microsoft.SharePoint.Client;


namespace UC_alert_tool.Functions.Helpdesk
{
    public class Get
    {
        private static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void productDatabaseFunction()
        {
            try
            {
                string url = "http://dc-139.ucsystems.net/sites/ucsystems/"; // site url
                ClientContext client = new ClientContext(url); //client that connects to the sharepoint
                client.Credentials = new NetworkCredential("stagesoftware", "MoeimakerEersteKlas", "ucsystems"); //login details
                SharepointClient.List abbos = client.Web.Lists.GetByTitle("abonnementen"); //table
                SharepointClient.List allProducten = client.Web.Lists.GetByTitle("Productcode"); //producten table

                CamlQuery query = new CamlQuery();// query in caml language
                query.ViewXml = "<view></view>"; // use <view> to get the whole table

                ListItemCollection collListItem = abbos.GetItems(query);
                ListItemCollection productenListItem = allProducten.GetItems(query);

                client.Load(productenListItem,
                items => items.Include(
                    item => item.Id,
                    item => item.DisplayName,
                    item => item.FieldValuesAsHtml // these are the custom fields - artikel / beschrijving / etc.
                ));
                client.Load(collListItem,
                    items => items.Include(
                        item => item.Id,
                        item => item.DisplayName,
                        item => item.FieldValuesAsHtml // these are the custom fields - artikel / beschrijving / etc.
                        ));
                client.ExecuteQuery();

                var listOfArtikelen = new List<string>();
                var listOfProducten = new List<string>();
                var listOfNotInList = new List<string>();
                var listOfProductGroep = new List<string>(); // the main groep - grote groepen
                var listOfProducttype = new List<string>(); // de onderliggende groep - kleine groepen
                foreach (var item in collListItem)
                {
                    string htmlResult = item.FieldValuesAsHtml.FieldValues.ElementAt(5).ToString().Replace("[", "").Replace("]", ""); // artikel
                    var product = htmlResult.Substring(htmlResult.IndexOf(',') + 2);// remove "[artikel, " from the product
                    if (!listOfArtikelen.Contains(product)) // voeg toe aan lijst, als deze nog nog niet in de lijst staat
                    {
                        listOfArtikelen.Add(product);
                    };
                }
                foreach (var item in productenListItem)
                {
                    string productgroepHTML = item.FieldValuesAsHtml.FieldValues.ElementAt(5).ToString().Replace("[", "").Replace("]", ""); // productgroep
                    var productgroep = productgroepHTML.Substring(productgroepHTML.IndexOf(',') + 2);// remove "[artikel, " from the productgoep
                    if (!listOfProductGroep.Contains(productgroep)) // voeg toe aan lijst, als deze nog nog niet in de lijst staat
                    {
                        listOfProductGroep.Add(productgroep);
                    };
                    string producttypeHTML = item.FieldValuesAsHtml.FieldValues.ElementAt(6).ToString().Replace("[", "").Replace("]", ""); // productgroep
                    var producttype = producttypeHTML.Substring(producttypeHTML.IndexOf(',') + 2);// remove "[artikel, " from the productgoep
                    if (!listOfProducttype.Contains(producttype)) // voeg toe aan lijst, als deze nog nog niet in de lijst staat
                    {
                        listOfProducttype.Add(producttype);
                    };

                    string allProductInlisthtml = item.FieldValuesAsHtml.FieldValues.ElementAt(1).ToString().Replace("[", "").Replace("]", ""); // productgroep
                    var allProductInlist = allProductInlisthtml.Substring(allProductInlisthtml.IndexOf(',') + 2);// remove "[artikel, " from the productgoep
                    if (!listOfProducten.Contains(allProductInlist)) // voeg toe aan lijst, als deze nog nog niet in de lijst staat
                    {
                        listOfProducten.Add(allProductInlist);
                    }
                }

                for (int i = 0; i < listOfArtikelen.Count; i++) // een lijst met artiekelen die niet bestaan in de "Productcode" table - deze zullen moeten worden aagemaakt in de "Productcode" tabel
                {
                    if (listOfProducten.Contains(listOfArtikelen[i]))
                    {
                        Console.WriteLine("deze staat niet in de lijst - " + listOfArtikelen[i]);
                        listOfNotInList.Add(listOfArtikelen[i]);
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine("Error : " + e.Message);
            }
        }


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