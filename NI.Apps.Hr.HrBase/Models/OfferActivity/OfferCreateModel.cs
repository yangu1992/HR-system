using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NI.Apps.Hr.HrBase.Models.OfferActivity
{
    public class OfferCreateModel
    {
        public OfferDetailModel Offer { get; set; }
        public PersonelDetailModel Personel { get; set; }
        public ReportDetailModel Report { get; set; }
        public SalaryDetailModel Salary { get; set; }
        public string SaveButton { get; set; }
        public string MgrApproveButton { get; set; }
        public EmailModel Email { get; set; }
    }
}