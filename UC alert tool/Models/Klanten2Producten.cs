//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UC_alert_tool.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Klanten2Producten
    {
        public int ID { get; set; }
        public int KlantID { get; set; }
        public int ProductID { get; set; }
    
        public virtual Klanten Klanten { get; set; }
        public virtual Producten Producten { get; set; }
    }
}
