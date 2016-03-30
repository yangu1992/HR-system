using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NI.Apps.Hr.Entity.Models
{
    public class HeadCount
    {
        public int ID { get; set; }
        public int Code { get; set; }
        public string Position { get; set; }
        public int Number { get; set; }
        public string CostCenter { get; set; }
        public string Department { get; set; }
        public string InternalLevel { get; set; }
        public string CreateBtn { get; set; }
        public bool? isExisted { get; set; }
    }
}