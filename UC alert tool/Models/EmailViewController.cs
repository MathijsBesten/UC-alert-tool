using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UC_alert_tool.Models
{
    public class email
    {
        [Required]
        public string FromEmailAddress { get; set; }
        [Required]
        public List<string> Recipients { get; set; }
        [Required]
        public string SMTPServerURL { get; set; }
        public string SMTPUsername { get; set; }
        public string SMTPPassword { get; set; }
        [Required]
        public string EmailSubject { get; set; }
        [Required]
        public string EmailBody { get; set; }
    }
}