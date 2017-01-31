using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UC_alert_tool.Models
{
    public class Metadata
    {
    }
    public class StoringMetaData
    {
        public int Id { get; set; }
        [Display(Name = "Product")]
        public int ProductID { get; set; }
        public string Titel { get; set; }
        public string Inhoud { get; set; }
        [Display(Name ="Eigenaar")]
        public int EigenaarID { get; set; }
        [Display(Name = "begindatum")]
        public System.DateTime Begindatum { get; set; }
        [Display(Name = "Einddatum")]
        public Nullable<System.DateTime> Einddatum { get; set; }
        [Display(Name = "Is gesloten")]
        public bool IsGesloten { get; set; }
    }
    public class ProductMetaData
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        [Display(Name ="Helpdesk ID")]
        public int HelpdeskID { get; set; }
    }

}