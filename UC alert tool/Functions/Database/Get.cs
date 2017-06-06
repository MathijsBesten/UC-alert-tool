using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UC_alert_tool.Models;

namespace UC_alert_tool.Functions.Database
{
    public class Get
    {
        public static List<Producten> getProductenFromGroupOrType(string ID)
        {
            alertDatabaseEntities db = new alertDatabaseEntities();
            var allProducts = new List<Producten> ();
            List<Producten> allProductenFromGroup = new List<Producten>();
            string selectedProduct = ID.Substring(1); // the selectlist is always in order as in the database - minus 1 because the list startes with a 1 instaid of a 0
            int id = int.Parse(selectedProduct);
            if (ID.Contains("g")) //group
            {
                var productgroup = db.Productgroep.SingleOrDefault(p => p.Id == id);
                foreach (var item in productgroup.Producten)
                {
                    allProducts.Add(item);
                }
            }
            else
            {
                var productgroup = db.Producttype.SingleOrDefault(p => p.Id == id);
                foreach (var item in productgroup.Producten)
                {
                    allProducts.Add(item);
                }
            }
            return allProducts;
        }
    }
}