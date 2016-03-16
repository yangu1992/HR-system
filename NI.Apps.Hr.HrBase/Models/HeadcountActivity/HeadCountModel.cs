using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NI.Application.HR.HRBase.Models.HeadcountActivity
{
    public class HeadCountModel
    {
        public int Code { get; set; }
        public string Position { get; set; }
        public string Number { get; set; }
        public string CostCenter { get; set; }
        public string Department { get; set; }
        public string InternalLevel { get; set; }
    }
}