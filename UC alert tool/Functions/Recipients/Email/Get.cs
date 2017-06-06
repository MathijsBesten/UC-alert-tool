using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UC_alert_tool.Models;

namespace UC_alert_tool.Functions.Recipients.Email
{
    public class Get
    {
        private static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static alertDatabaseEntities db = new alertDatabaseEntities();
        public static int getTotalCountRecipients(string productInputName, bool useProducttype)
        {
            //receive all recipients form db
            var allProducts = db.Producten.ToList();
            var allProductsThatContainsK2P = new List<Producten>();
            var allProductGroepen = db.Productgroep;
            var allproducttypes = db.Producttype;
            List<Producten> alleSelectedProducts = new List<Producten>();
            if (useProducttype == true)
            {
                Producttype selectedProducttype = allproducttypes.SingleOrDefault(p => p.Producttypenaam == productInputName);
                alleSelectedProducts = selectedProducttype.Producten.ToList();
            }
            else // use productgroepen
            {
                Productgroep selectedProductgroep = allProductGroepen.SingleOrDefault(p => p.Naam == productInputName);
                alleSelectedProducts = selectedProductgroep.Producten.ToList();
            }
            allProductsThatContainsK2P = alleSelectedProducts.Where(b => b.Klanten2Producten.Count != 0).ToList();
            List<int> klantenThatWantsMessageIDS = new List<int>();
            foreach (var item in allProductsThatContainsK2P)
            {
                foreach (var user in item.Klanten2Producten)
                {
                    if (!klantenThatWantsMessageIDS.Contains(user.KlantID))
                    {
                        klantenThatWantsMessageIDS.Add(user.KlantID);
                    }
                }
            }
            var allRecipientsOnlyEmail = new List<string>();
            klantenThatWantsMessageIDS.Sort();
            ////fill list with people who wants to receive sms message
            foreach (var item in klantenThatWantsMessageIDS)
            {
                string email = db.Klanten.First(u => u.Id == item).PrimaireEmail;
                if (!string.IsNullOrWhiteSpace(email))
                {
                    allRecipientsOnlyEmail.Add(email);
                }
            }
            return allRecipientsOnlyEmail.Count;
        }
    }
}