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
    
    public partial class userToken
    {
        public int userId { get; set; }
        public System.DateTime token_expire_date { get; set; }
        public string token { get; set; }
    }
}
