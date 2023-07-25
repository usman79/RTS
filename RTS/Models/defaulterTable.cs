using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTS.Models
{
    public class defaulterTable
    {
        public string town { get; set; }
        public int totalAccounts { get; set; }
        public int recoveredAccounts { get; set; }
        public int totalAmount { get; set; }
        public int recoveredAmount { get; set; }
    }
}