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
        public static SelectList productgroepsAndProducttypes()
        {
            alertDatabaseEntities db = new alertDatabaseEntities();
            List<SelectListItem> ListWithAllTypes = new List<SelectListItem>();
            ListWithAllTypes.Add(new SelectListItem() { Text = "---Productgroepen---", Value = "header" });
            foreach (var item in db.Productgroep)
            {
                ListWithAllTypes.Add(new SelectListItem() { Text = item.Naam, Value = "g" + item.Id.ToString() }); //g is to define groups
            }
            ListWithAllTypes.Add(new SelectListItem() { Text = "---Producttypes---", Value = "header" });
            foreach (var item in db.Producttype)
            {
                ListWithAllTypes.Add(new SelectListItem() { Text = item.Producttypenaam, Value = "t" + item.Id.ToString() }); //t is to define types
            }
            return new SelectList(ListWithAllTypes, "Value", "Text");
        }
    }
}