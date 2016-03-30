using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NI.Apps.Hr.Service;
using NI.Apps.Hr.Service.Interface;
using NI.Application.HR.HRBase.Models.OfferActivity;
using PagedList;
using NI.Apps.Hr.HrBase.Filters;
using NI.Apps.Hr.HrBase.BusinessRules;

namespace NI.Apps.Hr.HrBase.Controllers
{
    [CustomAuthorize(RoleName = "OfferSpecialist")]
    public class HomeController : Controller
    {
        private IOfferService _offerService;
        public IOfferService OfferService
        {
            get { return _offerService ?? (_offerService = new OfferService()); }
        }
        //
        // GET: /Home/
        //[Authentication]
        
        public ActionResult Index(OfferSearchModel model)
        {
            ViewBag.User = Utility.getCurrentUserName(Utility.CurrentUser);

            int pageSize = 14;
            int pageNumber = (model.Page ?? 1);

            model.OfferList = this.OfferService.FindOffers(model.ChineseName, new DateTime(), DateTime.Now, model.Status).ToPagedList(pageNumber, pageSize);

            if (model.OfferList == null || model.OfferList.Count == 0)
            {
                TempData["AlertMessage"] = "No results matches !";
            }    
            
            return View("~/Views/Offer/OfferSearch.cshtml", model);
        }

    }
}
