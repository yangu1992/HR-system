
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NI.Apps.Hr.Entity.Models;
using PagedList;

namespace NI.Apps.Hr.HrBase.Models.HeadcountActivity
{
    public class HeadCountDetailModel
    {
        public HeadCount headcount { get; set; }
        public IPagedList<Offer> offers { get; set; }

        
    }
}