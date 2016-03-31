using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NI.Apps.Hr.HrBase.Models.OfferActivity;
using NI.Apps.Hr.Repository;
using NI.Apps.Hr.Repository.Interface;
using System.Web.Routing;

namespace NI.Apps.Hr.HrBase.Filters
{
    public class CheckEmailFromAttribute : FilterAttribute, IActionFilter
    {
        private IEmployeeRepository _employeeRepository;
        public IEmployeeRepository EmployeeRepository
        {
            get { return _employeeRepository ?? (_employeeRepository = new EmployeeRepository()); }
        }
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
            string from;
            string pwd;

            int offerID = 0;
            if (filterContext.ActionParameters.ContainsKey("model"))
            {
                var model= filterContext.ActionParameters["model"] as OfferCreateModel;
                offerID = model.Offer.ID;
                //create offer process
                var email = model.Email;
                from = email.FromEmail;              
            }
            else {
                from = filterContext.ActionParameters["FromEmail"].ToString();
                offerID = int.Parse(filterContext.ActionParameters["OfferID"].ToString());
            }

            pwd = this.EmployeeRepository.getEmailPassword(from);
            if (string.IsNullOrEmpty(pwd))
            {
                var routeValues = new RouteValueDictionary(new
                {
                    controller = "Offer",
                    action = "GoToNewestPage",
                    OfferID = offerID
                });

                filterContext.Result = new RedirectToRouteResult(routeValues);
            }
            else {
                filterContext.HttpContext.Response.Write(string.Format("<div>right</div>"));
            }
        }
    }
}