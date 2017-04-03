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
        public static string getTotalCountRecipients(string productName)
        {
            //receive all recipients form db
            var allProducts = db.Producten.ToList();
            var selectedProduct = allProducts.SingleOrDefault(product => product.Naam == productName);
            var selectedProductID = selectedProduct.Id;
            var allRecipients = selectedProduct.Klanten2Producten;
            var allRecipientsOnlyEmail = new List<string>();

            //fill list with people who wants to receive sms message
            foreach (var item in allRecipients)
            {
                string email = item.Klanten.PrimaireEmail;
                if (!string.IsNullOrWhiteSpace(email))
                {
                    allRecipientsOnlyEmail.Add(item.Klanten.PrimaireEmail);
                }
                else
                {
                    string emailSecondary = item.Klanten.SecundaireEmail;
                    if (!string.IsNullOrWhiteSpace(emailSecondary))
                    {
                        allRecipientsOnlyEmail.Add(item.Klanten.SecundaireEmail);
                    }
                }
            }
            return allRecipientsOnlyEmail.Count.ToString();
        }
    }
}