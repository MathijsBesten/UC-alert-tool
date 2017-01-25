using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace UC_alert_tool.Models
{
    public class AlertToolContext : DbContext 
    {
        public AlertToolContext(): base()
        {

        }
        public DbSet<Storing> Storingen { get; set; }
        public DbSet<Product> Producten { get; set; }
        public DbSet<Klant> Klanten { get; set; }
    }
}