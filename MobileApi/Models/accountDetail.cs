//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MobileApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class accountDetail
    {
        public int id { get; set; }
        public System.DateTime visit_date { get; set; }
        public Nullable<double> visit_lat { get; set; }
        public Nullable<double> visit_long { get; set; }
        public string remarks { get; set; }
        public int accountId { get; set; }
        public string is_assigned { get; set; }
        public Nullable<System.DateTime> assignment_date { get; set; }
    
        public virtual Account Account { get; set; }
    }
}