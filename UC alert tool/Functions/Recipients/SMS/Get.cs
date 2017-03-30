using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UC_alert_tool.Models;

namespace UC_alert_tool.Functions.Recipients.SMS
{
    public class Get
    {
        private static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static alertDatabaseEntities db = new alertDatabaseEntities();

        public static int getTotalCountRecipients(string productName)
        {
            //receive all recipients form db
            var allProducts = db.Producten.ToList();
            var selectedProduct = allProducts.SingleOrDefault(product => product.Naam == productName);
            var selectedProductID = selectedProduct.Id;
            var allRecipients = selectedProduct.Klanten2Producten;
            var allRecipientsOnlySMSNumber = new List<string>();

            //fill list with people who wants to receive sms message
            foreach (var item in allRecipients)
            {
                string email = item.Klanten.Telefoonnummer;
                if (!string.IsNullOrWhiteSpace(email))
                {
                    allRecipientsOnlySMSNumber.Add(item.Klanten.Telefoonnummer);
                }
            }
            return allRecipientsOnlySMSNumber.Count;
        }
    }
}