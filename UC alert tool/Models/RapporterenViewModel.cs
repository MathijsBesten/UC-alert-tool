using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UC_alert_tool.Models
{
    public class rapporterenMetEmail
    {
        [Required]
        [Display(Name = "Titel storing")]
        public string Titel { get; set; }
        [Required]
        [Display(Name = "Storing omschrijving")]
        public string description { get; set; }
        [Display(Name = "product")]
        public int ProductID { get; set; }
        [Required]
        [Display(Name = "begindatum")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime Begindatum { get; set; }
        [Required]
        [Display(Name = "begintijd")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH\\:mm}", ApplyFormatInEditMode = true)]
        public System.DateTime Begintijd { get; set; }
        [Display(Name = "einddatum")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Einddatum { get; set; }
        [Display(Name = "eindtijd")]
        [DataType(DataType.Time)]
        public System.DateTime Eindtijd { get; set; }
        [Display(Name = "Is gesloten")]
        public bool IsGesloten { get; set; }
        [Required]
        [Display(Name = "Email onderwerp")]
        public string emailtitle { get; set; }
        [Required]
        [Display(Name = "Email bericht")]
        public string emailbody { get; set; }
    }
}