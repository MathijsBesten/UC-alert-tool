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
    
    public partial class Klanten
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Klanten()
        {
            this.Klanten2Producten = new HashSet<Klanten2Producten>();
        }
    
        public int Id { get; set; }
        public string Naam { get; set; }
        public string PrimaireEmail { get; set; }
        public string SecundaireEmail { get; set; }
        public string Telefoonnummer { get; set; }
        public Nullable<int> ProductID { get; set; }
        public string debiteurnummer { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Klanten2Producten> Klanten2Producten { get; set; }
    }
}
