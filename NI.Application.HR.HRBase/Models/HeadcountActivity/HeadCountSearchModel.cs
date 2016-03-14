using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using NI.Apps.Hr.Entity.Models;

namespace NI.Application.HR.HRBase.Models.HeadcountActivity
{
    public class HeadCountSearchModel
    {
        public int? Code { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public IPagedList<HeadCount> HeadCountList { get; set; }
        public string SearchButton { get; set; }
        public int? Page { get; set; }
    }
}