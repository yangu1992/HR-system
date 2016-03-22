using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NI.Application.HR.HRBase.Models.OfferActivity
{
    public class ReportDetailModel
    {       
        public string ReportLine { get; set; }
        [DataType(DataType.EmailAddress)]
        public string ReportLineEmail { get; set; }
        public string DepartmentMgr { get; set; }
        [DataType(DataType.EmailAddress)]
        public string DepartmentMgrEmail { get; set; }
    }
}