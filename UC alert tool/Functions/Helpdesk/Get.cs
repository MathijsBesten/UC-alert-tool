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
        public static void ClearDatabaseAndSync()
        {
            log.Info("All data in database will refresh now");
            log.Info("All tables will be cleared to 0 rows");
            using (var db = new alertDatabaseEntities())
            {
                try
                {
                    db.Database.ExecuteSqlCommand("DELETE FROM [Klanten2Producten]");
                    db.Database.ExecuteSqlCommand("DELETE FROM [Producten]");
                    db.Database.ExecuteSqlCommand("DELETE FROM [Producttype]");
                    db.Database.ExecuteSqlCommand("DELETE FROM [Productgroep]");
                    db.Database.ExecuteSqlCommand("DELETE FROM [Klanten]");
                }
                catch (Exception e)
                {
                    log.Error("There was a error while clearing the tables - Please read the full errormessage below");
                    log.Error("Error: " + e.Message);
                    throw e;
                }
                log.Info("All data from the tables: Klanten2Producten,Producten,Producttype,Productgroep and Klanten are cleared");
                getAllUsers();
                getAllProductgroepen();
                getAllProducttype();
                getAllProducten();
                getAllKlanten2Producten();

            }
        }

        public static ListItemCollection GetItemsFromSharepointDatabaseFunction(bool getAbbos)
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
                if (getAbbos)
                {
                    return collListItem;
                }
                else
                {
                    return productenListItem;
                }
            }
            catch (Exception e)
            {
                log.Error("There was a error while trying to receive data from the sharepoint database - check the line below for the error message");
                log.Error("Error message: " + e.Message);
                return null;
            }
        }
        public static void getAllUsers()
        {
            var allNewContacts = new List<Klanten>();
            using (var db = new Models.supportcenterEntities())
            {
                Console.WriteLine("");
                var usersWithAlert = db.Requester_Fields.Where(u => u.UDF_CHAR1 == "Ja");
                var allDepartmentuserID = usersWithAlert.Select(u => u.DEPARTMENTUSERID).ToList<long>();
                var allContactInfo = new List<AaaContactInfo>();
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
                        log.Error("Could not add user \'" + aaaUserItem.AaaContactInfo.First().EMAILID + " \' to the database, user has probably no company assigned to him/her");
                        log.Error("Error: " + e);
                    }

                }

                Console.Write("");
            }
            using (var db = new alertDatabaseEntities())
            {
                //db.Database.ExecuteSqlCommand("delete * from dbo.Klanten");
                foreach (var item in allNewContacts)
                {
                    if (!string.IsNullOrEmpty(item.Naam)) // check if there is a email in the item
                    {
                        db.Klanten.Add(item);
                    }
                }
                db.SaveChanges();
                log.Info("synced all the 'klanten' in dbo.Klanten");
            }
        }

        public static void getAllProductgroepen()
        {
            var allItems = GetItemsFromSharepointDatabaseFunction(false);
            var listOfProductGroep = new List<Productgroep>(); // the main groep - grote groepen
            var listOfProductsForCheck = new List<string>(); // a list to check if new item is a duplicate
            foreach (var item in allItems)
            {
                string productgroepHTML = item.FieldValuesAsHtml.FieldValues.ElementAt(5).ToString().Replace("[", "").Replace("]", ""); // productgroep
                var productgroepAsString = productgroepHTML.Substring(productgroepHTML.IndexOf(',') + 2);// remove "[artikel, " from the productgoep
                var productgroep = new Productgroep() { Naam = productgroepAsString };
                if (!listOfProductsForCheck.Contains(productgroep.Naam)) // voeg toe aan lijst, als deze nog nog niet in de lijst staat
                {
                    listOfProductGroep.Add(productgroep);
                    listOfProductsForCheck.Add(productgroep.Naam);
                };
            }
            using (var db = new alertDatabaseEntities())
            {

                //db.Database.ExecuteSqlCommand("delete * from dbo.Klanten");
                foreach (var item in listOfProductGroep)
                {

                    if (!string.IsNullOrEmpty(item.Naam)) // check if there is a email in the item
                    {
                        db.Productgroep.Add(item);
                    }
                }
                db.SaveChanges();
                log.Info("synced all the 'productgroepen' in dbo.productgroep");
            }
        }

        public static void getAllProducttype()
        {
            var allItems = GetItemsFromSharepointDatabaseFunction(false);
            var listOfProducttype = new List<Producttype>(); // the main groep - grote groepen
            var listOfProductsForCheck = new List<string>(); // a list to check if new item is a duplicate
            var listOfProductGroepen = new List<string>(); // contains all the productgroepen at the index
            foreach (var item in allItems)
            {
                string producttypeHTML = item.FieldValuesAsHtml.FieldValues.ElementAt(6).ToString().Replace("[", "").Replace("]", ""); // productgroep
                var producttypeAsString = producttypeHTML.Substring(producttypeHTML.IndexOf(',') + 2);// remove "[artikel, " from the productgoep
                string productgroepHTML = item.FieldValuesAsHtml.FieldValues.ElementAt(5).ToString().Replace("[", "").Replace("]", ""); // productgroep
                var productgroepAsString = productgroepHTML.Substring(productgroepHTML.IndexOf(',') + 2);// remove "[artikel, " from the productgoep
                var producttype = new Producttype() {
                    Producttypenaam = producttypeAsString
                };
                if (!listOfProductsForCheck.Contains(producttype.Producttypenaam)) // voeg toe aan lijst, als deze nog nog niet in de lijst staat
                {
                    listOfProducttype.Add(producttype);
                    listOfProductsForCheck.Add(producttype.Producttypenaam);
                    listOfProductGroepen.Add(productgroepAsString); // add productgroep on index for later in this function
                };
            }
            using (var db = new alertDatabaseEntities())
            {

                //db.Database.ExecuteSqlCommand("delete * from dbo.Klanten");
                int count = 0;
                foreach (var item in listOfProducttype)
                {

                    if (!string.IsNullOrEmpty(item.Producttypenaam)) // check if there is a email in the item
                    {
                        string productgroepname = listOfProductGroepen[count];
                        int productgroepindex = db.Productgroep.SingleOrDefault(g => g.Naam == productgroepname).Id; //hierzo - geen resultaten in de database :(
                        item.ProductMainID = productgroepindex;
                        db.Producttype.Add(item);
                    }
                    count++;
                }
                db.SaveChanges();
                log.Info("synced all the 'producttypes' in dbo.producttype");
            }
        }
        public static void getAllProducten()
        {
            var allItems = GetItemsFromSharepointDatabaseFunction(false);
            var listOfProducten = new List<Producten>(); // the main groep - grote groepen
            var listOfProductsForCheck = new List<string>(); // a list to check if new item is a duplicate
            var listOfProducttypes = new List<string>(); // contains all the productgroepen at the index
            foreach (var item in allItems)
            {
                string productHTML = item.FieldValuesAsHtml.FieldValues.ElementAt(1).ToString().Replace("[", "").Replace("]", ""); // productgroep
                var productAsString = productHTML.Substring(productHTML.IndexOf(',') + 2);// remove "[artikel, " from the productgoep
                string producttypeasHTML = item.FieldValuesAsHtml.FieldValues.ElementAt(6).ToString().Replace("[", "").Replace("]", ""); // productgroep
                var productgroepAsString = producttypeasHTML.Substring(producttypeasHTML.IndexOf(',') + 2);// remove "[artikel, " from the productgoep
                var product = new Producten()
                {
                    Naam = productAsString
                };
                if (!listOfProductsForCheck.Contains(product.Naam)) // voeg toe aan lijst, als deze nog nog niet in de lijst staat
                {
                    listOfProducten.Add(product);
                    listOfProductsForCheck.Add(product.Naam);
                    listOfProducttypes.Add(productgroepAsString); // add productgroep on index for later in this function
                };
            }
            using (var db = new alertDatabaseEntities())
            {

                //db.Database.ExecuteSqlCommand("delete * from dbo.Klanten");
                int count = 0;
                foreach (var item in listOfProducten)
                {

                    if (!string.IsNullOrEmpty(item.Naam)) // check if there is a email in the item
                    {
                        string producttypename = listOfProducttypes[count];
                        Producttype producttype = db.Producttype.SingleOrDefault(g => g.Producttypenaam == producttypename);
                        int producttypeindex = producttype.Id; // ID of type product - also contains the group id
                        int productgroupindex = producttype.ProductMainID;
                        item.ProducttypeID = producttypeindex;
                        item.ProductgroepID = productgroupindex;
                        db.Producten.Add(item);
                    }
                    count++;
                }
                db.SaveChanges();
                log.Info("synced all the 'producten' in dbo.producten");
            }
        }
        public static void getAllKlanten2Producten()
        {
            var allItems = GetItemsFromSharepointDatabaseFunction(true); // for abbo table
            List<simpleProducten> simpleProductenList  = new List<simpleProducten>(); // searching will be on index - when index is found get item from allitems from this index
            using (var db = new alertDatabaseEntities())
            {
                List<klanten2productenModeler> debuteurenScafholder = new List<klanten2productenModeler>();
                List<int> listofdebuteuren = new List<int>();
                foreach (var item in db.Klanten.ToList())
                {
                    int debuteurennummer;
                    int.TryParse(item.debiteurnummer, out debuteurennummer); // null will return 0
                    listofdebuteuren.Add(debuteurennummer);
                    klanten2productenModeler model = new klanten2productenModeler()
                    {
                        debuteurennummer = debuteurennummer,
                        klantID = item.Id
                    };
                    debuteurenScafholder.Add(model);
                }
                foreach (var item in allItems) // searching will be done with allitemsasstring and this index will be used for the allitems list
                {
                    simpleProducten product = new simpleProducten();
                    string artikel = Formatting.SharepointFormatting.getTextFromSharepointHTML(item.FieldValuesAsHtml.FieldValues.ElementAt(5).ToString());
                    string aktief = Formatting.SharepointFormatting.getTextFromSharepointHTML(item.FieldValuesAsHtml.FieldValues.ElementAt(17).ToString());
                    string debuteurennummerAsString = Formatting.SharepointFormatting.getTextFromSharepointHTML(item.FieldValuesAsHtml.FieldValues.ElementAt(21).ToString()).Replace(".", "");
                    if (debuteurennummerAsString != "")
                    {
                        int debuteurennummer = int.Parse(debuteurennummerAsString);
                        product.debuteurennummer = debuteurennummer;
                    }
                    product.aktief = aktief;
                    product.artikel = artikel;
                    simpleProductenList.Add(product);
                }
                List<string> listOfMissingProducts = new List<string>();
                foreach (var item in debuteurenScafholder)
                {
                    List<int> actieveArtikelen = new List<int>();
                    var productenVanKlant = simpleProductenList.FindAll(i => (i.debuteurennummer == item.debuteurennummer) && (i.aktief == "Aktief" || i.aktief == "Opgezegd"));
                    int countOfaddedProducts = 0;
                    foreach (var productVanKlant in productenVanKlant)
                    {
                        Klanten2Producten k2pEntity = new Klanten2Producten
                        {
                            KlantID = item.klantID,
                        };
                        try
                        {
                            int productID;
                            productID = db.Producten.FirstOrDefault(x => x.Naam == productVanKlant.artikel).Id;
                            k2pEntity.ProductID = productID;
                            if (productID != 0)
                            {
                                db.Klanten2Producten.Add(k2pEntity);
                            }
                            countOfaddedProducts++;
                        }
                        catch (Exception e)
                        {
                            if (!listOfMissingProducts.Contains(productVanKlant.artikel))
                            {
                                listOfMissingProducts.Add(productVanKlant.artikel);
                            }
                        }
                    }
                    if (countOfaddedProducts == 0)
                    {
                        log.Error("User with debiteurennummer '" + item.debuteurennummer + "' will not get a alert - There where no products connected to the user - check if all products are in the sharepoint database");
                    }
                    Console.WriteLine();
                }
                if (listOfMissingProducts.Count != 0)
                {
                    listOfMissingProducts.Sort();
                    foreach (var item in listOfMissingProducts)
                    {
                        log.Error("Missing product in productlist - Please add '" + item + "' to the sharepoint producten database");
                    }
                }
                db.SaveChanges();
                log.Info("synced all klanten2producten");
                Console.WriteLine();
            }
        }
    }
}