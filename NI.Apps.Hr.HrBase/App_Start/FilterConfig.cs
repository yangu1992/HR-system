using System.Web;
using System.Web.Mvc;

namespace NI.Apps.Hr.HrBase
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}