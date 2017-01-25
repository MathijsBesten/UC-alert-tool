using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UC_alert_tool.Models
{
    public class Klant
    {
        public int ID { get; set; }
        public string naam { get; set; }
        public string primaireEmail { get; set; }
        public string secundaireEmail { get; set; }
        public int telefoonnummer { get; set; }
    }
}