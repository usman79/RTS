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
    
    public partial class Account
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Account()
        {
            this.accountDetails = new HashSet<accountDetail>();
        }
    
        public int Id { get; set; }
        public int amount_due { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        public int Ward_Id { get; set; }
        public string address { get; set; }
        public int walk_sort { get; set; }
        public string Consumer_Name { get; set; }
        public Nullable<int> last_payment { get; set; }
        public Nullable<System.DateTime> last_payment_date { get; set; }
        public string Account_No { get; set; }
        public int statusId { get; set; }
        public string Account_Type { get; set; }
        public Nullable<System.DateTime> Connection_Date { get; set; }
        public Nullable<int> Current_Demand { get; set; }
        public string propertyNo { get; set; }
        public Nullable<System.DateTime> visit_date { get; set; }
        public Nullable<double> visit_lat { get; set; }
        public Nullable<double> visit_long { get; set; }
        public string remarks { get; set; }
        public string is_assigned { get; set; }
        public Nullable<System.DateTime> assignment_date { get; set; }
        public string Image { get; set; }
        public int partialPayment { get; set; }
        public Nullable<System.DateTime> nextVisit_date { get; set; }
        public int visits { get; set; }
    
        public virtual Ward Ward { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<accountDetail> accountDetails { get; set; }
        public virtual accountStatu accountStatu { get; set; }
    }
}
