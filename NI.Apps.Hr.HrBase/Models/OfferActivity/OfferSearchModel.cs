using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using NI.Apps.Hr.Entity.Models;
using System.ComponentModel.DataAnnotations;

namespace NI.Apps.Hr.HrBase.Models.OfferActivity
{
    public class OfferSearchModel
    {
        public OfferSearchModel() {
            this.FromDate = DateTime.Today;
            this.ToDate = DateTime.Today;
        }
        public string ChineseName { get; set; }
        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Status { get; set; }
        public string SearchButton { get; set; }

        public IPagedList<Offer> OfferList { get; set; }
        public int? Page { get; set; }
    }
}