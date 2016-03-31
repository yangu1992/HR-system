using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using PagedList;
using NI.Apps.Hr.HrBase.Models;
using NI.Apps.Hr.HrBase.Models.OfferActivity;
using NI.Apps.Hr.HrBase.Models.OfferActivity.PersonalInfoFormModels;
using NI.Apps.Hr.Entity;
using NI.Apps.Hr.Service;
using NI.Apps.Hr.Service.Interface;
using NI.Apps.Hr.Repository;
using NI.Apps.Hr.Repository.Interface;
using NI.Apps.Hr.HrBase.Filters;
using NI.Apps.Hr.HrBase.BusinessRules;


namespace NI.Apps.Hr.HrBase.Controllers
{
    [CustomAuthorize(RoleName = "OfferSpecialist")]
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
        private IEmployeeRepository _employeeRepository;
      
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
        public IEmployeeRepository EmployeeRepository {
            get { return _employeeRepository ?? (_employeeRepository = new EmployeeRepository()); }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(OfferSearchModel model)
        {
            ViewBag.User = Utility.getCurrentUserName(Utility.CurrentUser);

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

            return View("OfferSearch", model);
        }

        public ActionResult Create(int? HeadcountCode)
        {
            ViewBag.User = Utility.getCurrentUserName(Utility.CurrentUser);
            ViewBag.UserEmail = Utility.getCurrentUserEmail(Utility.CurrentUser);

            OfferCreateModel model = new OfferCreateModel();          

            if (HeadcountCode != null)
            {
                //in create offer process
                Table_Headcount hc = this.HeadCountService.FindHeadCountByCode(HeadcountCode);

                model.Offer = new OfferDetailModel();
                model.Offer.Status = "Draft";
                model.Offer.HeadcountCode = HeadcountCode??0;
                model.Offer.Postion = hc.Headcount_Position;
            }
            
            model.Offer.ValidOfferNumber = this.OfferService.GetValidOfferNumber(model.Offer.HeadcountCode);
            model.Offer.TotalOfferNumber = this.HeadCountService.GetOfferQuota(model.Offer.HeadcountCode);

            return View("OfferCreate", model);
        }

        [HttpPost]
        public ActionResult Create(OfferCreateModel model) {
            //in revise process ,just need to update the offer
            if (model.Offer.ID > 0)
            {
                model.Offer.Status = "Pending Manager Approval";
                this.OfferService.SaveOffer(
                    getOfferEntity(model.Offer),
                    getPersonelEntity(model.Personel),
                    getReportEntity(model.Report),
                    getSalaryEntity(model.Salary),
                    getBonusEntities(model.Salary)
                );
            }
            else 
            {
                //save action
                //add new offer related info to database
                model.Offer.ID = this.OfferService.AddNewOffer(
                    getOfferEntity(model.Offer),
                    getPersonelEntity(model.Personel),
                    getReportEntity(model.Report),
                    getSalaryEntity(model.Salary),
                    getBonusEntities(model.Salary)
                );
            }          

            EmailControl emailControl = new EmailControl();
            emailControl.SendApprovalEmail(model); //send email for approve         

            return RedirectToAction("GoToNewestPage", "Offer", new { OfferID = model.Offer.ID });                                          
        }

        public ActionResult Reject(int offerID) {
            this.OfferService.RejectOffer(offerID);

            return RedirectToAction("GoToNewestPage", "Offer", new { OfferID = offerID });
        }

        //[CheckEmailFrom]
        public ActionResult ApproveByMgr(int OfferID, string FromEmail, string ToEmail, string CcEmail, string Subject)
        {
            //update offer status
            this.OfferService.ApproveByMgr(OfferID);

            OfferCreateModel model = getCurrentModelData(OfferID);
            //send offer email
            EmailModel email = new EmailModel
            {
                FromEmail = FromEmail,
                ToEmail = ToEmail,
                CCEmail = CcEmail,
                Subject = Subject
            };
            model.Email = email;
            EmailControl emailControl = new EmailControl();
            emailControl.SendOfferEmail(model);

            return Json(new { offer_id = OfferID }); 
        }

        public ActionResult GoToNewestPage(int OfferID) {
            ViewBag.User = Utility.getCurrentUserName(Utility.CurrentUser);
            ViewBag.UserEmail = Utility.getCurrentUserEmail(Utility.CurrentUser);
            //recreat model
            OfferCreateModel model = getCurrentModelData(OfferID);
            TempData["model"] = model;

            switch (model.Offer.Status)
            {
                case "Draft":
                    return View("OfferCreate",model);
                case "Pending Candidate Onboarding":
                    return View("OfferOnboarding", model);
                case "Waiting Candidate Feedback":
                    return View("OfferWaitingFeedback", model);
                case "Pending Manager Approval":
                    return View("OfferPendingApprove", model);
                case "Onboarded":
                    return View("InvalidOfferDetail", model);    //just for temp
            }
            return View("InvalidOfferDetail", model);
        }

        public ActionResult UploadPersonalInfoForm(int offerID)
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
            PersonalInfoFormModel formModel = excelReadController.getFormModel(filePath);

            //add personel info to database and update offer info
            try
            {
                Table_Offer newOffer = new Table_Offer
                {
                    Offer_ID = offerID,
                    Offer_OnboardingDate = (formModel.TentativeOnboardDate == null) ? (DateTime?)null : DateTime.Parse(formModel.TentativeOnboardDate),
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
            }catch(System.FormatException e){
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("failed"); 
            }
            
            return PartialView("~/Views/Shared/PersonalFormDialogPartial.cshtml", formModel);
        }

        //need email to IT with candidate's info
        public ActionResult RequestDomain(int OfferID,string FromEmail,string ToEmail,string CcEmail,string Subject)
        {
            OfferCreateModel model = getCurrentModelData(OfferID);

            EmailControl emailControl = new EmailControl();
            string chineseName = model.Personel.LName + model.Personel.FName;
            string englishName = model.Personel.RomanLName + ", " + model.Personel.RomanFName;
            EmailModel email=new EmailModel{
                FromEmail=FromEmail,
                ToEmail=ToEmail,
                CCEmail=CcEmail,
                Subject=Subject
            };
            emailControl.SendDomainRequestEmail(chineseName+"/"+englishName,
                model.Offer.Postion,
                model.Report.ReportLine,
                model.Offer.OnBoardingDate,
                email
                );

            this.OfferService.UpdateOfferFeedback(OfferID,true,null,null,null);

            return Json(new { offer_id = OfferID });
        }

        public ActionResult SyncDomain(int OfferID,string DomainLogin, DateTime ContractStartDate)
        {
            this.OfferService.SyncDomainToAddressBook(OfferID, DomainLogin, ContractStartDate);
            this.OfferService.UpdateOfferFeedback(OfferID, null, true, null, null);

            return RedirectToAction("GoToNewestPage", "Offer", new { OfferID = OfferID });  
        }

        //send welcome email to candidate
        public ActionResult SendWelcomeToCandidate(int OfferID ,string FromEmail,string ToEmail,string CcEmail,string Subject)
        {
            OfferCreateModel model = getCurrentModelData(OfferID);

            EmailControl emailControl = new EmailControl();
            EmailModel email = new EmailModel
            {
                FromEmail = FromEmail,
                ToEmail = ToEmail,
                CCEmail = CcEmail,
                Subject = Subject
            };

            string chineseName = model.Personel.LName + model.Personel.FName;
            emailControl.SendWelcomeEmail(chineseName, 
                email, 
                (model.Offer.OnBoardingDate ?? DateTime.Now));

            this.OfferService.UpdateOfferFeedback(OfferID, null, null, true, null);

            return Json(new { offer_id = OfferID });
        }

        public ActionResult SendWelcomeToLineMgr(int OfferID, string FromEmail, string ToEmail, string CcEmail, string Subject)
        {
            EmailControl emailControl = new EmailControl();
            EmailModel email = new EmailModel
            {
                FromEmail = FromEmail,
                ToEmail = ToEmail,
                CCEmail = CcEmail,
                Subject = Subject
            };
            emailControl.SendWelcomeEmail(email);

            this.OfferService.UpdateOfferFeedback(OfferID, null, null, null, true);

            return Json(new { offer_id=OfferID});
        }

        //candidate accept in the process "waiting candidate feedback"
        public ActionResult Accept(int offerID) {
            this.OfferService.AcceptOffer(offerID);

            return RedirectToAction("GoToNewestPage", "Offer", new { OfferID = offerID });  
        }

        //the candidate onboarded confirmed
        public ActionResult Confirm(int offerID) {
            this.OfferService.ConfirmOnboard(offerID);

            return RedirectToAction("GoToNewestPage", "Offer", new { OfferID = offerID });
        }
        public ActionResult Revise(int offerID) {
            this.OfferService.ReviseOffer(offerID);

            return RedirectToAction("GoToNewestPage", "Offer", new { OfferID = offerID });
        }

        public ActionResult Edit(int offerID) {
            ViewBag.User = Utility.getCurrentUserName(Utility.CurrentUser);

            OfferCreateModel model = getCurrentModelData(offerID);

            return View("OfferEdit",model);
        }

        public ActionResult SaveChanges(OfferCreateModel model)
        {
            this.OfferService.SaveOffer(
                getOfferEntity(model.Offer),
                getPersonelEntity(model.Personel),
                getReportEntity(model.Report),
                getSalaryEntity(model.Salary),
                getBonusEntities(model.Salary)
            );

            return RedirectToAction("GoToNewestPage", "Offer", new { OfferID = model.Offer.ID });
        }
        public JsonResult GetEmployeesEmails()
        {       
            var db = new HrDbContext();
            
            var result = from e in db.Table_Employee
                             select new { e.Employee_FullName, e.Employee_Email };
            
            return Json(result, JsonRequestBehavior.AllowGet);
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
            //string tmp=null;
            //tmp=model.ChineseFName;
            //tmp = model.ChineseGName;
            //tmp = model.Gender;
            //tmp = model.MaritalStatus;
            //tmp = model.EnglishGName;
            //tmp = model.EnglishFName; //family name
            //tmp = DateTime.Parse(model.BirthDate).ToShortDateString();
            //     tmp = model.ID;
            //     tmp = model.Nationality;
            //     tmp = model.Hukou;
            //      tmp = model.HukouType;
            //   tmp = model.FileLocation;
            //   tmp = model.HomeAddress;
            //   tmp = model.PostCode;
            //   tmp = model.Phone;
            //   tmp = model.Email;
            //   tmp = model.EmergencyContact;
            //   tmp = model.EmergencyContactPhone;

            Table_PersonelInfo entity = new Table_PersonelInfo
            {
                PersonelInfo_FName = model.ChineseGName,
                PersonelInfo_LName = model.ChineseFName,
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
            Table_SalaryInfo salary = this.SalaryService.FindSalaryByID(offer.Offer_SalaryInfoID);
            List<Table_BonusInfo> bonus = null;
            if (salary != null) {
                bonus = this.SalaryService.FindBonusBySalaryID(salary.SalaryInfo_ID);
            }

            OfferDetailModel offerDetail = new OfferDetailModel
            {
                ID=offer.Offer_ID,
                HeadcountCode=headcount.Headcount_Code,
                RecruitType=offer.Offer_RecruitType,
                Postion=offer.Offer_Position,
                Location=offer.Offer_Location,
                Channel=offer.Offer_RecruitChannel,
                OnBoardingDate=offer.Offer_OnboardingDate,
                ContractDuration=offer.Offer_ContractDuration,
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
                ReviewSalary=salary.SalaryInfo_ReviewedSalary
            };
            if (bonus != null) {
                salaryDetail.bonus = true;
                foreach (var b in bonus) { 
                    switch (b.BonusInfo_Type){
                        case "Sign-On":
                            salaryDetail.SignOn=true;
                            salaryDetail.SignOnPrice = b.BonusInfo_Amount;
                            continue;
                        case "Relocation":
                            salaryDetail.Relocation = true;
                            salaryDetail.RelocationPrice = b.BonusInfo_Amount;
                            continue;
                        case "Other":
                            salaryDetail.Other = true;
                            salaryDetail.OtherPrice = b.BonusInfo_Amount;
                            continue;
                    }                  
                }
            }

            OfferCreateModel model = new OfferCreateModel
            {
                Offer=offerDetail,
                Personel=personelDetail,
                Report=reportDetail,
                Salary=salaryDetail
            };

            return model;
        }

        private List<Table_BonusInfo> getBonusEntities(SalaryDetailModel model)
        {
            if (model == null) {
                return null;
            }

            List<Table_BonusInfo> list = new List<Table_BonusInfo>();
            if (model.SignOn) {
                list.Add(new Table_BonusInfo {
                    BonusInfo_Type="Sign-On",
                    BonusInfo_Amount=model.SignOnPrice??0
                });
            }
            if (model.Relocation)
            {
                list.Add(new Table_BonusInfo
                {
                    BonusInfo_Type = "Relocation",
                    BonusInfo_Amount = model.RelocationPrice ?? 0
                });
            }
            if (model.Other)
            {
                list.Add(new Table_BonusInfo
                {
                    BonusInfo_Type = "Other",
                    BonusInfo_Amount = model.OtherPrice ?? 0
                });
            }
            return list;
        }
        private Table_SalaryInfo getSalaryEntity(SalaryDetailModel salaryDetailModel)
        {
            if (salaryDetailModel == null)
                return null;

            return new Table_SalaryInfo
            {
                SalaryInfo_Salary=salaryDetailModel.Salary,
                SalaryInfo_ReviewedSalary=salaryDetailModel.ReviewSalary
            };           
        }

        private Table_ReportingInfo getReportEntity(ReportDetailModel reportDetailModel)
        {
            //int ReportLineEmpID=this.EmplyeeService.FindID
            if (reportDetailModel == null)
                return null;

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
                Offer_ID=offerDetailModel.ID,
                Offer_HCID=headcountID,
                Offer_Status=offerDetailModel.Status,
                Offer_RecruitType=offerDetailModel.RecruitType,
                Offer_Position=offerDetailModel.Postion,
                Offer_Location=offerDetailModel.Location,
                Offer_OnboardingDate=offerDetailModel.OnBoardingDate,
                Offer_RecruitChannel=offerDetailModel.Channel,
                Offer_ContractDuration=offerDetailModel.ContractDuration,
                Offer_ProbationDuration=offerDetailModel.ProbationDuration
            };          
        }

        
    }
}
