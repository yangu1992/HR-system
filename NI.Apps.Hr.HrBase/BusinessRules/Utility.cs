using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NI.Apps.Hr.Entity;
using System.Data.Entity.Core.Objects;

namespace NI.Apps.Hr.HrBase.BusinessRules
{
    public class Utility
    {
        private static string userKey = "HrSystem.CurrentUser";
        private static string adminKey = "HrSystem.Admin";

        public static string CurrentUser
        {
            get
            {       
                HttpContext.Current.Session[userKey] = HttpContext.Current.User.Identity.Name;
                return (string)HttpContext.Current.Session[userKey];
                //return @"apac\yqian";
            }
        }

        

        public static void RefreshCurrentUser()
        {
            HttpContext.Current.Session[userKey] = HttpContext.Current.User.Identity.Name;
        }

        public static void RefreshAdmin()
        {
           // HttpContext.Current.Session[adminKey] = GetAuthorization(HttpContext.Current.User.Identity.Name);
        }



        internal static dynamic getCurrentUserName(string domainLogin)
        {
            string user = domainLogin.ToLower();
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

            using (var db = new HrDbContext())
            {
                ObjectParameter p = new ObjectParameter("returnVal", typeof(string));
                db.Proc_GetUserName(p,user);
                string result = (p.Value==DBNull.Value)?string.Empty:p.Value.ToString();

                return result;
            }
        }

        internal static dynamic getCurrentUserEmail(string domainLogin)
        {
            string user = domainLogin.ToLower();
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

            using (var db = new HrDbContext())
            {
                ObjectParameter p = new ObjectParameter("returnVal", typeof(string));
                db.Proc_GetUserEmail(p, user);
                string result = (p.Value == DBNull.Value) ? string.Empty : p.Value.ToString();

                return result;
            }
        }
    }
}