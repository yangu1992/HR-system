using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NI.Apps.Hr.Service;
using NI.Apps.Hr.Service.Interface;
using NI.Application.HR.HRBase.Models.HeadcountActivity;
using NI.Apps.Hr.Entity;
using NI.Apps.Hr.Entity.Models;
using PagedList;
using NI.Apps.Hr.HrBase.Models.HeadcountActivity;

namespace NI.Application.HR.HRBase.Controllers.HeadCountController
{
    public class HeadCountController : Controller
    {
        private IHeadCountService _headcountService;
        private IOfferService _offerService;
        private int pageSize = 14;

        public IHeadCountService HeadCountService
        {
            get { return _headcountService ?? (_headcountService = new HeadCountService()); }
        }
        public IOfferService OfferService
        {
            get { return _offerService ?? (_offerService = new OfferService()); }
        }

        public HeadCountController()
        {
            //this.repository = headcountRepository;
        }

        public ActionResult Search(HeadCountSearchModel model) {
            int pageSize = 14;
            int pageNumber = (model.Page ?? 1);
            if (!string.IsNullOrEmpty(model.SearchButton) || model.Page.HasValue) {
                model.HeadCountList = this.HeadCountService.FindHeadCounts(model.Code, model.Position, model.Department).ToPagedList(pageNumber, pageSize);

                if (model.HeadCountList == null || model.HeadCountList.Count == 0)
                {
                    TempData["AlertMessage"] = "No results matches !";
                }
            }

            SpinnerService service = new SpinnerService();
            ViewData["Department"] = new SelectList(service.GetDepartmentList());
            return View("HeadCountSearch",model);
        }

        public ActionResult Create()
        {
            SpinnerService service = new SpinnerService();
            ViewData["CostCenter"] = new SelectList(service.GetCostCenterList());
            ViewData["Department"] = new SelectList(service.GetDepartmentList());
            ViewData["InternalLevel"] = new SelectList(service.GetInternalLevelList());

            return View("HeadCountCreate");
        }

        [HttpPost]
        public ActionResult Create(HeadCountCreateModel headcount)
        {
            Table_Headcount newHC = new Table_Headcount { 
                Headcount_ID=headcount.ID,
                Headcount_Code=headcount.Code,
                Headcount_Position=headcount.Position,
                Headcount_Number=headcount.Number,
                Headcount_CostCenter=headcount.CostCenter,
                Headcount_Department=headcount.Department,
                Headcount_InternalLevel=headcount.InternalLevel
            };

            Table_Headcount tmp = this.HeadCountService.AddHeadCount(newHC);
            
            return RedirectToAction("ShowDetail", "HeadCount", new { HeadcountCode = tmp.Headcount_Code });
               // return View("test", result);      
        }

        public ActionResult ShowDetail(string CreateBtn, int HeadcountCode, int? Page=1)
        {
            HeadCount hc = null;

            Table_Headcount searchResult = this.HeadCountService.FindHeadCountByCode(HeadcountCode);
            hc = new HeadCount
            {
                ID = searchResult.Headcount_ID,
                Code = searchResult.Headcount_Code,
                Position = searchResult.Headcount_Position,
                Number = searchResult.Headcount_Number ?? 0,
                CostCenter = searchResult.Headcount_CostCenter,
                Department = searchResult.Headcount_Department,
                InternalLevel = searchResult.Headcount_InternalLevel
            };    
            
          
            int pageNumber = Page??1;
            IEnumerable<Offer> offers = this.OfferService.FindOffersByHcCode(hc.Code);

            HeadCountDetailModel HcDetail = new HeadCountDetailModel { headcount = hc, offers = offers.ToPagedList(pageNumber, pageSize) };

            return View("HeadCountDetail", HcDetail);
        }


        [HttpGet]
        public PartialViewResult Edit(int hcID) {
            Table_Headcount searchResult = this.HeadCountService.FindHeadCountByID(hcID);

            HeadCount hc= new HeadCount
            {
                ID=hcID,
                Code = searchResult.Headcount_Code,
                Position = searchResult.Headcount_Position,
                Number = searchResult.Headcount_Number??0,
                CostCenter = searchResult.Headcount_CostCenter,
                Department = searchResult.Headcount_Department,
                InternalLevel = searchResult.Headcount_InternalLevel
            };

            IEnumerable<Offer> offers = this.OfferService.FindOffersByHcCode(hc.Code);
    
            HeadCountDetailModel model = new HeadCountDetailModel { 
                headcount = hc, 
                offers = offers.ToPagedList(1,pageSize) };

            SpinnerService service = new SpinnerService();
            ViewData["headcount.CostCenter"] = new SelectList(service.GetCostCenterList(),hc.CostCenter);
            ViewData["headcount.Department"] = new SelectList(service.GetDepartmentList(),hc.Department);
            ViewData["headcount.InternalLevel"] = new SelectList(service.GetInternalLevelList(),hc.InternalLevel);

            return PartialView("~/Views/Shared/_HeadCountEditPartial.cshtml", model);
        }

        public ActionResult Save(HeadCountDetailModel model) {
            Table_Headcount updatedHeadcount = new Table_Headcount { 
                Headcount_ID=model.headcount.ID,
                Headcount_Code=model.headcount.Code,
                Headcount_Position=model.headcount.Position,
                Headcount_Number=model.headcount.Number,
                Headcount_CostCenter=model.headcount.CostCenter,
                Headcount_Department=model.headcount.Department,
                Headcount_InternalLevel=model.headcount.InternalLevel
            };
            this.HeadCountService.UpdateHeadCount(updatedHeadcount);

            return RedirectToAction("ShowDetail", "HeadCount", new { HeadcountCode = updatedHeadcount.Headcount_Code });
        }
        private dynamic GenerateDropDownList()
        {
            throw new NotImplementedException();
        }


       
    }
}
