using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UC_alert_tool.Models
{
    public class Metadata
    {
    }
    public class StoringMetaData
    {
        public int Id { get; set; }
        [Display(Name = "product")]
        public IEnumerable<SelectListItem> ProductID { get; set; }
        [Required]
        public string Titel { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Inhoud { get; set; }
        [Display(Name ="eigenaar")]
        public int EigenaarID { get; set; }
        [Required]
        [Display(Name = "begindatum")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime Begindatum { get; set; }
        [Required]
        [Display(Name = "begintijd")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public System.DateTime Begintijd { get; set; }
        [Display(Name = "einddatum")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Einddatum { get; set; }
        [Display(Name = "eindtijd")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public System.DateTime Eindtijd { get; set; }
        [Display(Name = "Is gesloten")]
        public bool IsGesloten { get; set; }
    }
    public class ProductMetaData
    {
        public int Id { get; set; }
        public string Naam { get; set; }
    }

}