//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RTS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AFI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AFI()
        {
            this.Wards = new HashSet<Ward>();
        }
    
        public int Id { get; set; }
        public string name { get; set; }
        public int ticket { get; set; }
        public string phone { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        public string password { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ward> Wards { get; set; }
    }
}
