using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UC_alert_tool.Models
{
    public class email
    {
        public string FromEmailAddress { get; set; }
        public List<string> Recipients { get; set; }
        public string SMTPServerURL { get; set; }
        public string SMTPUsername { get; set; }
        public string SMTPPassword { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
    }
}