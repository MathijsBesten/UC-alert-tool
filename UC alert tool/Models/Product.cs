using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace UC_alert_tool.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string naam { get; set; }
        public int helpdeskID { get; set; }
    }
    public class ProductDBContext : DbContext
    {
        public DbSet<Product> Producten { get; set; }
    }
}