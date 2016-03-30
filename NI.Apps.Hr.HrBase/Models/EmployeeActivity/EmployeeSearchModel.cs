using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using NI.Apps.Hr.Entity.Models;

namespace NI.Apps.Hr.HrBase.Models.EmployeeActivity
{
    public class EmployeeSearchModel
    {
        public string ChineseName { get; set; }
        public string Department { get; set; }
        public string SearchButton { get; set; }
        public IPagedList<Employee> EmployeeList { get; set; }
        public int? Page { get; set; }


    }
}