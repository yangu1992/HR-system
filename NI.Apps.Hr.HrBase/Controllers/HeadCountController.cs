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

            
            return View("HeadCountSearch",model);
        }

        public ActionResult Create()
        {
            SpinnerService service = new SpinnerService();
           
            IEnumerable<SelectListItem> list1 = service.GetCostCenterList().Select(
                b => new SelectListItem { Value=b,Text=b});

            IEnumerable<SelectListItem> list2 = service.GetDepartmentList().Select(
                b => new SelectListItem { Value = b, Text = b });

            IEnumerable<SelectListItem> list3 = service.GetInternalLevelList().Select(
                b => new SelectListItem { Value = b, Text = b });

            ViewData["CostCenter"] = list1;
            ViewData["Department"] = list2;
            ViewData["InternalLevel"] = list3;

            return View("HeadCountCreate");
        }

        [HttpPost]
        public ActionResult Create(HeadCountCreateModel headcount)
        {
            Table_Headcount newHC = new Table_Headcount { 
                Headcount_ID=headcount.ID,
                Headcount_Code=headcount.Code,
                Headcount_Position=headcount.Position,
                Headcount_CostCenter=headcount.CostCenter,
                Headcount_Department=headcount.Department,
                Headcount_InternalLevel=headcount.InternalLevel
            };

            Table_Headcount tmp = this.HeadCountService.AddHeadCount(newHC);
            HeadCount hc = new HeadCount
            {
                ID = tmp.Headcount_ID,
                Code = tmp.Headcount_Code,
                Position = tmp.Headcount_Position,
                Number = tmp.Headcount_Number??0,
                CostCenter = tmp.Headcount_CostCenter,
                Department = tmp.Headcount_Department,
                InternalLevel = tmp.Headcount_InternalLevel
            };
            TempData["hc"] = hc;
            return RedirectToAction("ShowDetail", "HeadCount", new { CreateBtn =headcount.CreateBtn});
               // return View("test", result);      
        }

        public ActionResult ShowDetail(string CreateBtn, int? HeadcountCode, int? Page)
        {
            HeadCount hc = null;

            //in create new headcount process
            if (CreateBtn != null) 
            {
                Page = 1;
                hc = (HeadCount)TempData["hc"];      
            }

            //in search an existing headcount proccess
            if (HeadcountCode != null) {
                Table_Headcount searchResult = this.HeadCountService.FindHeadCountByCode(HeadcountCode);
                hc = new HeadCount
                {
                    ID=searchResult.Headcount_ID,
                    Code = searchResult.Headcount_Code,
                    Position = searchResult.Headcount_Position,
                    Number = searchResult.Headcount_Number??0,
                    CostCenter = searchResult.Headcount_CostCenter,
                    Department = searchResult.Headcount_Department,
                    InternalLevel = searchResult.Headcount_InternalLevel
                };
            }
          
            int pageNumber = (Page ?? 1);
            IEnumerable<Offer> offers = this.OfferService.FindOffersByHcCode(hc.Code);
            
            HeadCountDetailModel HcDetail = new HeadCountDetailModel { headcount = hc, offers = offers.ToPagedList(pageNumber,pageSize) };

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

            IEnumerable<SelectListItem> list1 = service.GetCostCenterList().Select(
                b => new SelectListItem { Value = b, Text = b });

            IEnumerable<SelectListItem> list2 = service.GetDepartmentList().Select(
                b => new SelectListItem { Value = b, Text = b });

            IEnumerable<SelectListItem> list3 = service.GetInternalLevelList().Select(
                b => new SelectListItem { Value = b, Text = b });

            ViewData["headcount.CostCenter"] = list1;
            ViewData["headcount.Department"] = list2;
            ViewData["headcount.InternalLevel"] = list3;

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
