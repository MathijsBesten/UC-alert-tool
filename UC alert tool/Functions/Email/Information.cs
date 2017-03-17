using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UC_alert_tool.Models;

namespace UC_alert_tool.Functions.Email
{
    public class Information
    {
        private static alertDatabaseEntities db = new alertDatabaseEntities();
        public static List<int> GetCountOfEmailRecipients() // will loop over all products and gets the total Recipients count for each product
        {
            var allProducts = db.Producten.ToList();
            var productCountList = new List<int>();// this list has the same order as the 'default' product list
            foreach (var product in allProducts)
            {
                int customersOnThisProduct = 0;
                foreach (var klant in product.Klanten2Producten)
                {
                    if (!string.IsNullOrWhiteSpace(klant.Klanten.PrimaireEmail) || !string.IsNullOrWhiteSpace(klant.Klanten.SecundaireEmail ))
                    {
                        customersOnThisProduct++;
                    }
                }
                productCountList.Add(customersOnThisProduct);
            }

            return productCountList;
        }
    }
}