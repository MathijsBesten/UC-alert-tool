using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UC_alert_tool.Models
{
    public class rapporterenMetEmail : Storingen
    {
        [Required]
        [Display(Name = "Email onderwerp")]
        public string emailtitle { get; set; }
        [Required]
        [Display(Name = "Email bericht")]
        public string emailbody { get; set; }
    }
    public class rapporterenMetSMS : Storingen
    {
        [Required]
        [Display(Name = "sms bericht")]
        public string smsbericht { get; set; }
    }
    public class rapporterenMetEmailenSMS : Storingen
    {
        [Required]
        [Display(Name = "Email onderwerp")]
        public string emailtitle { get; set; }
        [Required]
        [Display(Name = "Email bericht")]
        public string emailbody { get; set; }
        [Required]
        [Display(Name = "sms bericht")]
        public string smsbericht { get; set; }
    }
}