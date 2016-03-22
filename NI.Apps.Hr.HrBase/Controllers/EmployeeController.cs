using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NI.Apps.Hr.HrBase.Models.EmployeeActivity;

namespace NI.Apps.Hr.HrBase.Controllers
{
    public class EmployeeController : Controller
    {
        //
        // GET: /Employee/

        public ActionResult Search(EmployeeSearchModel model)
        {
            return View();
        }

    }
}
