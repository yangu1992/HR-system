using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NI.Application.HR.HRBase.Models.OfferActivity.PersonalInfoFormModels;
using NI.Application.HR.HRBase.Models.OfferActivity;
using NI.Application.HR.HRBase.Models;
using NI.Apps.Hr.Service;
using NI.Apps.Hr.Service.Interface;
using PagedList;
using NI.Apps.Hr.Entity;
using System.IO;
using NI.Apps.Hr.Repository;



namespace NI.Application.HR.HRBase.Controllers
{
    public class OfferController : Controller
    {
        //
        // GET: /Offer/
        private IOfferService _offerService;
        private IHeadCountService _headcountService;
        private IPersonelService _personelService;
        private IReportService _reportService;
        private ISalaryService _salaryService;
        private IEmployeeService _employeeService;
        public IOfferService OfferService
        {
            get { return _offerService ?? (_offerService = new OfferService()); }
        }

        public IHeadCountService HeadCountService
        {
            get { return _headcountService ?? (_headcountService = new HeadCountService()); }
        }

        public IPersonelService PersonelService
        {
            get { return _personelService ?? (_personelService = new PersonelService()); }
        }
        public IReportService ReportService
        {
            get { return _reportService ?? (_reportService = new ReportService()); }
        }
        public ISalaryService SalaryService
        {
            get { return _salaryService ?? (_salaryService = new SalaryService()); }
        }
        public IEmployeeService EmployeeService
        {
            get { return _employeeService ?? (_employeeService = new EmployeeService()); }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(OfferSearchModel model)
        {           
            int pageSize = 14;
            int pageNumber = (model.Page ?? 1);
            if (!string.IsNullOrEmpty(model.SearchButton) || model.Page.HasValue)
            {
                model.OfferList = this.OfferService.FindOffers(model.ChineseName,model.FromDate, model.ToDate, model.Status).ToPagedList(pageNumber, pageSize);

                if (model.OfferList == null || model.OfferList.Count == 0)
                {
                    TempData["AlertMessage"] = "No results matches !";
                }
            }          
            
            return View("/Views/OfferActivity/OfferSearch.cshtml", model);
        }

        public ActionResult Create(int? HeadcountCode)
        {
            SpinnerService service = new SpinnerService();
            IEnumerable<SelectListItem> list = service.GetEmployeeList().Select(
                b => new SelectListItem { Value = b, Text = b });
            ViewData["Report.ReportLine"] = list;
            ViewData["Report.DepartmentMgr"] = list;

            OfferCreateModel model = new OfferCreateModel();          

            if (HeadcountCode != null)
            {
                //in create offer process
                model.Offer = new OfferDetailModel();
                model.Offer.Status = "Draft";
                model.Offer.HeadcountCode = HeadcountCode??0;
            }
            else { 
                //in revise process
                model = (OfferCreateModel)TempData["model"];
                model.Offer.Status = this.OfferService.GetOfferStatus(model.Offer.ID);
            }
            
            model.Offer.ValidOfferNumber = this.OfferService.GetValidOfferNumber(model.Offer.HeadcountCode);
            model.Offer.TotalOfferNumber = this.HeadCountService.GetOfferQuota(model.Offer.HeadcountCode);           

            return View("/Views/OfferActivity/OfferCreate.cshtml", model);
        }

        [HttpPost]
        public ActionResult Create(OfferCreateModel model) {
            //save action
            //add offer related info to database
            model.Offer.ID = this.OfferService.AddNewOffer(
                getOfferEntity(model.Offer),
                getPersonelEntity(model.Personel),
                getReportEntity(model.Report),
                getSalaryEntity(model.Salary)
            );

            EmailControl emailControl = new EmailControl();
            emailControl.SendApprovalEmail(model); //send email for approve         

            model = getCurrentModelData(model.Offer.ID);
            return View("/Views/OfferActivity/OfferPendingApprove.cshtml", model);                                    
        }

        public ActionResult ShowInvalidOfferDetail() {
            OfferCreateModel model = new OfferCreateModel();
            model = (OfferCreateModel)TempData["model"];
                        
            return View("/Views/OfferActivity/InvalidOfferDetail.cshtml", model);
        }

        public ActionResult Reject(int offerID) {
            this.OfferService.RejectOffer(offerID);

            return RedirectToAction("Search","Offer");
        }

        public ActionResult ApproveByMgr(OfferCreateModel model) {
            //update offer status
            this.OfferService.ApproveByMgr(model.Offer.ID);

            //send offer email
            EmailControl emailControl = new EmailControl();
            emailControl.SendOfferEmail(model);

            OfferCreateModel model1 = getCurrentModelData(model.Offer.ID);
            return View("/Views/OfferActivity/OfferWaitingFeedback.cshtml", model1);
        }

        public ActionResult GoToNewestPage(int OfferID) {
            //recreat model
            OfferCreateModel model = getCurrentModelData(OfferID);
            TempData["model"] = model;

            if (model.Offer.Status == "Pending Candidate Onboarding") {
                return View("/Views/OfferActivity/OfferOnboarding.cshtml", model);
            }
            else if (model.Offer.Status == "Waiting Candidate Feedback") {
                return View("/Views/OfferActivity/OfferWaitingFeedback.cshtml", model);
            }
            else if (model.Offer.Status == "Pending Manager Approval")
            {
                return View("/Views/OfferActivity/OfferPendingApprove.cshtml", model);
            }

            return RedirectToAction("ShowInvalidOfferDetail", "Offer");
        }

        public PartialViewResult UploadPersonalInfoForm(int offerID)
        {
            HttpPostedFileBase fb = Request.Files[0];
 
            int fileName = offerID;  //get the name of the excel  xx.xls
            string path = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/" + offerID + "/");
            if (!Directory.Exists(path))//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(path);
            }
            string filePath = path + offerID + ".xls";
            fb.SaveAs(filePath);

            ExcelReadController excelReadController = new ExcelReadController();
            PersonalInfoFormModel formModel = excelReadController.getFormModel(fb, filePath);

            //add personel info to database and update offer info
            Table_Offer newOffer= new Table_Offer { 
                Offer_ID=offerID,
                Offer_OnboardingDate=DateTime.Parse(formModel.TentativeOnboardDate),
                Offer_SignedFile = filePath
            };
            Table_PersonelInfo newPersonelInfo = getNewPersonelInfo(offerID, formModel.PersonalInfo);
            List<Table_EducationInfo> newEducationInfo = getNewEducationInfo(formModel.EduInfo);
            List<Table_WorkExperienceInfo> newWorkExpInfo = getWorkExpInfo(formModel.WorkExperienceInfo);
            List<Table_FamilyMembersInfo> newFamilyInfo = getFamilyInfo(formModel.FamilyMemInfo);
            List<Table_ChildrenInfo> newChildrenInfo = getChildrenInfo(formModel.ChildsInfo);
            this.OfferService.UpdatePersonelInfo(newOffer,
                newPersonelInfo, newEducationInfo, newWorkExpInfo,
               newFamilyInfo, newChildrenInfo);

            return PartialView("/Views/Shared/PersonalFormDialogPartial.cshtml", formModel);
        }

        //need email to IT with candidate's info
        public ActionResult RequestDomain(int offerID)
        {
            OfferCreateModel model = getCurrentModelData(offerID);

            EmailControl emailControl = new EmailControl();
            string chineseName = model.Personel.FName + model.Personel.LName;
            string englishName = model.Personel.RomanFName + model.Personel.RomanLName;
            emailControl.SendDomainRequestEmail(chineseName+"/"+englishName,
                model.Offer.Postion,
                model.Report.ReportLine,
                model.Offer.OnBoardingDate);

            this.OfferService.UpdateOfferFeedback(offerID,true,null,null,null);
            model.Offer.EmailITCompleted = true;
            return View("/Views/OfferActivity/OfferWaitingFeedback.cshtml", model);
        }

        public ActionResult SyncDomain(int OfferID,string DomainLogin, DateTime ContractStartDate)
        {
            this.OfferService.SyncDomainToAddressBook(OfferID, DomainLogin, ContractStartDate);
            this.OfferService.UpdateOfferFeedback(OfferID, null, true, null, null);

            OfferCreateModel model = getCurrentModelData(OfferID);
            return View("/Views/OfferActivity/OfferWaitingFeedback.cshtml", model);
        }

        //send welcome email to candidate
        public ActionResult SendWelcomeToCandidate(int OfferID, string name, string candidateEmail, string MgrEmail, DateTime onboardingDate)
        {
            EmailControl emailControl = new EmailControl();
            emailControl.SendWelcomeEmail(name,candidateEmail,MgrEmail,onboardingDate);

            this.OfferService.UpdateOfferFeedback(OfferID, null, null, true, null);

            OfferCreateModel model = getCurrentModelData(OfferID);
            return View("/Views/OfferActivity/OfferWaitingFeedback.cshtml", model);
        }

        public ActionResult SendWelcomeToLineMgr(int OfferID, string LineMgrEmail)
        {
            EmailControl emailControl = new EmailControl();
            emailControl.SendWelcomeEmail(LineMgrEmail);

            this.OfferService.UpdateOfferFeedback(OfferID, null, null, null, true);

            OfferCreateModel model = getCurrentModelData(OfferID);
            return View("/Views/OfferActivity/OfferWaitingFeedback.cshtml", model);
        }

        //candidate accept in the process "waiting candidate feedback"
        public ActionResult Accept(int offerID) {
            this.OfferService.AcceptOffer(offerID);

            OfferCreateModel model = getCurrentModelData(offerID);
            return View("/Views/OfferActivity/OfferOnboarding.cshtml",model);
        }
        public ActionResult Revise(int offerID) {
            this.OfferService.ReviseOffer(offerID);

            OfferCreateModel model = getCurrentModelData(offerID);
            TempData["model"] = model;
            return RedirectToAction("Create", "Offer");
        }

        private List<Table_ChildrenInfo> getChildrenInfo(List<ChildrenInfoModel> list)
        {
            List<Table_ChildrenInfo> result = new List<Table_ChildrenInfo>();

            foreach (var model in list)
            {
                result.Add(new Table_ChildrenInfo
                {
                    ChildrenInfo_FullName = model.FullName,
                    ChildrenInfo_Gender = model.Gender,
                    ChildrenInfo_BirthDate = model.BirthDate
                });
            }
            return result;
        }

        private List<Table_WorkExperienceInfo> getWorkExpInfo(List<WorkExperienceModel> list)
        {
            List<Table_WorkExperienceInfo> result = new List<Table_WorkExperienceInfo>();

            foreach (var model in list)
            {
                result.Add(new Table_WorkExperienceInfo
                {
                    WorkExperienceInfo_StartDate = model.StartDate,
                    WorkExperienceInfo_EndDate = model.EndDate,
                    WorkExperienceInfo_Employer = model.Employer,
                    WorkExperienceInfo_Department = model.Department,
                    WorkExperienceInfo_Position = model.Position
                });
            }
            return result;
        }

        private List<Table_FamilyMembersInfo> getFamilyInfo(List<FamilyMemberModel> list)
        {
            List<Table_FamilyMembersInfo> result = new List<Table_FamilyMembersInfo>();

            foreach (var model in list)
            {
                result.Add(new Table_FamilyMembersInfo
                {
                    FamilyMembersInfo_FullName = model.FullName,
                    FamilyMembersInfo_Relations = model.Relations,
                    FamilyMembersInfo_Employer = model.Employer,
                    FamilyMembersInfo_Department = model.Department,
                    FamilyMembersInfo_Position = model.Position
                });
            }
            return result;
        }

        private List<Table_EducationInfo> getNewEducationInfo(List<EducationBackgroudModel> list)
        {
            List<Table_EducationInfo> result = new List<Table_EducationInfo>();

            foreach (var model in list)
            {
                result.Add(new Table_EducationInfo
                {
                    EducationInfo_StartDate = model.StartDate,
                    EducationInfo_EndDate = model.EndDate,
                    EducationInfo_School = model.School,
                    EducationInfo_Major = model.Major,
                    EducationInfo_Degree = model.Degree
                });
            }
            return result;
        }

        private Table_PersonelInfo getNewPersonelInfo(int offerID, PersonalInfoModel model)
        {
            Table_PersonelInfo entity = new Table_PersonelInfo
            {
                PersonelInfo_FName = model.ChineseFName,
                PersonelInfo_LName = model.ChineseGName,
                PersonelInfo_Gender = model.Gender,
                PersonelInfo_MartialStatus = model.MaritalStatus,
                PersonelInfo_RomanFName = model.EnglishGName,
                PersonelInfo_RomanLName = model.EnglishFName, //family name
                PersonelInfo_BirthDate = DateTime.Parse(model.BirthDate),
                PersonelInfo_IdentityID = model.ID,
                PersonelInfo_Nationality = model.Nationality,
                PersonelInfo_Hukou = model.Hukou,
                PersonelInfo_HukouType = model.HukouType,
                PersonelInfo_FileLocation = model.FileLocation,
                PersonelInfo_HomeAddress = model.HomeAddress,
                PersonelInfo_PostCode = model.PostCode,
                PersonelInfo_Phone = model.Phone,
                PersonelInfo_Email = model.Email,
                PersonelInfo_EmergencyContact = model.EmergencyContact,
                PersonelInfo_EmergencyContactPhone = model.EmergencyContactPhone
            };

            return entity;
        }

        private OfferCreateModel getCurrentModelData(int offerID)
        {
            Table_Offer offer = this.OfferService.FindOfferByID(offerID);
            Table_Headcount headcount = this.HeadCountService.FindHeadCountByID(offer.Offer_HCID);
            Table_PersonelInfo personel = this.PersonelService.FindPersonelByID(offer.Offer_PersonelInfoID);
            Table_ReportingInfo reportLine = this.ReportService.FindReportLineByID(offer.Offer_ReportingInfoID);
           // Table_Employee lineMgr = this.EmployeeService.FindEmployeeByID(reportLine.ReportingInfo_ReportLineEmpID);
           // Table_Employee manager = this.EmployeeService.FindEmployeeByID(reportLine.ReportingInfo_DeptMgrEmpID);
            Table_SalaryInfo salary = this.SalaryService.FindSalaryByID(offer.Offer_SalaryInfoID);

            OfferDetailModel offerDetail = new OfferDetailModel
            {
                ID=offer.Offer_ID,
                HeadcountCode=headcount.Headcount_Code,
                RecruitType=offer.Offer_RecruitType,
                Postion=offer.Offer_Position,
                Location=offer.Offer_Location,
                Channel=offer.Offer_RecruitChannel,
                OnBoardingDate=offer.Offer_OnboardingDate,
                ProbationDuration=offer.Offer_ProbationDuration,
                Status=offer.Offer_Status,
                UploadFormPath=offer.Offer_SignedFile,
                EmailITCompleted=offer.Offer_EmailITCompleted,
                SyncDomainCompleted=offer.Offer_SyncDomainCompleted,
                WelcomeCandidateCompleted=offer.Offer_WelcomeCandidateCompleted,
                WelcomeToMgrCompleted=offer.Offer_WelcomeToMgrCompleted
            };
            offerDetail.ValidOfferNumber = this.OfferService.GetValidOfferNumber(offerDetail.HeadcountCode);
            offerDetail.TotalOfferNumber = this.HeadCountService.GetOfferQuota(offerDetail.HeadcountCode);

            PersonelDetailModel personelDetail=new PersonelDetailModel{
                RomanFName=personel.PersonelInfo_RomanFName,
                RomanLName=personel.PersonelInfo_RomanLName,
                FName=personel.PersonelInfo_FName,
                LName=personel.PersonelInfo_LName,
                Phone=personel.PersonelInfo_Phone,
                Email=personel.PersonelInfo_Email
            };

            //how to show the name
            Table_Employee lineManager=this.EmployeeService.FindEmployeeByID(reportLine.ReportingInfo_ReportLineEmpID);
            Table_Employee depManager = this.EmployeeService.FindEmployeeByID(reportLine.ReportingInfo_DeptMgrEmpID);
            ReportDetailModel reportDetail=new ReportDetailModel{
                ReportLine=lineManager.Employee_FullName,
                ReportLineEmail=reportLine.ReportingInfo_ReportLineEmail,
                DepartmentMgr=depManager.Employee_FullName,
                DepartmentMgrEmail=reportLine.ReportingInfo_DeptMgrEmail
            };

            SalaryDetailModel salaryDetail = new SalaryDetailModel
            {
                Salary = salary.SalaryInfo_Salary,
                bonus = salary.SalaryInfo_Bonus
            };

            OfferCreateModel model = new OfferCreateModel
            {
                Offer=offerDetail,
                Personel=personelDetail,
                Report=reportDetail,
                Salary=salaryDetail
            };

            return model;
        }

        private Table_SalaryInfo getSalaryEntity(SalaryDetailModel salaryDetailModel)
        {
            return new Table_SalaryInfo
            {
                SalaryInfo_Salary=salaryDetailModel.Salary,
                SalaryInfo_Bonus=salaryDetailModel.bonus
            };           
        }

        private Table_ReportingInfo getReportEntity(ReportDetailModel reportDetailModel)
        {
            //int ReportLineEmpID=this.EmplyeeService.FindID

            EmployeeRepository repo = new EmployeeRepository();

            return new Table_ReportingInfo { 
                ReportingInfo_ReportLineEmpID=repo.GetID(reportDetailModel.ReportLine),
                ReportingInfo_ReportLineEmail=reportDetailModel.ReportLineEmail,
                ReportingInfo_DeptMgrEmpID=repo.GetID(reportDetailModel.DepartmentMgr),
                ReportingInfo_DeptMgrEmail=reportDetailModel.DepartmentMgrEmail
            };
        }

        private Table_PersonelInfo getPersonelEntity(PersonelDetailModel personelDetailModel)
        {
            return new Table_PersonelInfo
            {
                PersonelInfo_RomanFName=personelDetailModel.RomanFName,
                PersonelInfo_RomanLName=personelDetailModel.RomanLName,
                PersonelInfo_FName=personelDetailModel.FName,
                PersonelInfo_LName=personelDetailModel.LName,
                PersonelInfo_Email=personelDetailModel.Email,
                PersonelInfo_Phone=personelDetailModel.Phone
            };
        }

        private Table_Offer getOfferEntity(OfferDetailModel offerDetailModel)
        {
            int headcountID = this.HeadCountService.GetIDByCode(offerDetailModel.HeadcountCode);

            return new Table_Offer{
                Offer_HCID=headcountID,
                Offer_Status=offerDetailModel.Status,
                Offer_RecruitType=offerDetailModel.RecruitType,
                Offer_Position=offerDetailModel.Postion,
                Offer_Location=offerDetailModel.Location,
                Offer_OnboardingDate=offerDetailModel.OnBoardingDate,
                Offer_RecruitChannel=offerDetailModel.Channel,
                Offer_ProbationDuration=offerDetailModel.ProbationDuration
            };          
        }

        
    }
}
