using System.Web;
using System.Web.Mvc;
using NI.Apps.Hr.HrBase.Filters;

namespace NI.Apps.Hr.HrBase
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new CustomAuthAttribute());
        }
    }
}