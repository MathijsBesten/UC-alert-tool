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
        [Display(Name = "product")]
        public int ProductID { get; set; }
        public string Titel { get; set; }
        public string Inhoud { get; set; }
        [Display(Name ="eigenaar")]
        public int EigenaarID { get; set; }
        [Display(Name = "begindatum")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Begindatum { get; set; }
        [Display(Name = "begintijd")]
        [DataType(DataType.Time)]
        public System.DateTime Begintijd { get; set; }
        [Display(Name = "Einddatum")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Einddatum { get; set; }
        [Display(Name = "eindtijd")]
        [DataType(DataType.Time)]
        public System.DateTime Eindtijd { get; set; }
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