using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NI.Apps.Hr.HrBase.BusinessRules;
using NI.Apps.Hr.Entity;
using System.Data.Entity.Core.Objects;

namespace NI.Apps.Hr.HrBase.Filters
{
    public class CustomAuthorizeAttribute : FilterAttribute, IActionFilter
    {
        public string RoleName { get; set; }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string user = Utility.CurrentUser.ToLower();
            if (user.StartsWith("ni"))
            {
                user = user.Substring(3);
            }
            else if (user.StartsWith("apac"))
            {
                user = user.Substring(5);
            }
            else
            {
                //start with apac
            }
            //redirect if not authenticated 
            
            if (!checkAccess(user,RoleName)){

                filterContext.Result = new ViewResult
                {
                    ViewName = "NotAuthorized"
                };
            }
        }

        private bool checkAccess(string DomainLogin, string RoleName)
        {
            using (var db = new HrDbContext()) {
                ObjectParameter p = new ObjectParameter("returnVal", typeof(int));
                db.Proc_GetRoleName(p, DomainLogin, RoleName);
                int result = (int)p.Value;

                if (result > 0)
                    return true;

                return false;
            }
        }
    }
}