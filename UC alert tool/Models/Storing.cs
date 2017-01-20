using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UC_alert_tool.Models
{
    public class Storing
    {
        public int ID { get; set; }
        public int productID { get; set; }
        public string Titel { get; set; }
        public int eigenaarID { get; set; }
        public DateTime begindatum { get; set; }
        public DateTime einddatum { get; set; }
        public bool isGesloten { get; set; }
    }
}