using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using NI.Apps.Hr.Entity.Models;

namespace NI.Application.HR.HRBase.Models.OfferActivity
{
    public class OfferSearchModel
    {
        public string ChineseName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Status { get; set; }
        public string SearchButton { get; set; }

        public IPagedList<Offer> OfferList { get; set; }
        public int? Page { get; set; }
    }
}