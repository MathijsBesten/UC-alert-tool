using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UC_alert_tool.Models
{
    public class SMS
    {
        public string server { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public List<string> Recipients { get; set; }
        public string text { get; set; }

    }
}