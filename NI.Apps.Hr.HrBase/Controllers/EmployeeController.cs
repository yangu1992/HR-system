using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using NI.Apps.Hr.HrBase.Models.EmployeeActivity;
using NI.Apps.Hr.Service;
using NI.Apps.Hr.Service.Interface;
using NI.Apps.Hr.Repository;
using NI.Apps.Hr.Repository.Interface;
using NI.Apps.Hr.Entity;
using NI.Apps.Hr.HrBase.Filters;
using NI.Apps.Hr.HrBase.BusinessRules;

namespace NI.Apps.Hr.HrBase.Controllers
{
    public class EmployeeController : Controller
    {
        //
        // GET: /Employee/
        private IEmployeeService _employeeService;
        private IEmployeeRepository _employeeRepository;
        private IEmployeeService EmployeeService
        {
            get { return _employeeService ?? (_employeeService = new EmployeeService()); }
        }
        public IEmployeeRepository EmployeeRepository
        {
            get { return _employeeRepository ?? (_employeeRepository = new EmployeeRepository()); }
        }

        [CustomAuthorize(RoleName = "OfferSpecialist")]
        public ActionResult Search(EmployeeSearchModel model)
        {
            ViewBag.User = Utility.getCurrentUserName(Utility.CurrentUser);

            int pageSize = 14;
            int pageNumber = (model.Page ?? 1);

            if (!string.IsNullOrEmpty(model.SearchButton) || model.Page.HasValue)
            {
                model.EmployeeList = this.EmployeeService.FindEmployees(model.ChineseName, model.Department).ToPagedList(pageNumber, pageSize);

                if (model.EmployeeList == null || model.EmployeeList.Count == 0)
                {
                    TempData["AlertMessage"] = "No results matches !";
                }
            }
            
            return View("EmployeeSearch", model);
        }

        public JsonResult GetDepartments()
        {       
            var db = new HrDbContext();
            //department active
             var result = from e in db.Table_Department
                             select new { e.Department_Name };
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDetialInfo(int EmpID){
            ViewBag.User = Utility.getCurrentUserName(Utility.CurrentUser);

            return View("EmployeeDetail");
        }

    }
}
