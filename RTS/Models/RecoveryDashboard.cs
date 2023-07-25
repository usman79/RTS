using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTS.Models
{
    public class RecoveryDashboard
    {
        public ConSummary ConSummary { get; set; }
        public List<RecoveryProgress> RPList { get; set; }
        public List<defaulterTable> dtList { get; set; }
        public string marquee { get; set; }
    }
}