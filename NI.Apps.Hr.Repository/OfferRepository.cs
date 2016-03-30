using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using NI.Apps.Hr.Repository.Interface;
using NI.Apps.Hr.Entity;
using NI.Apps.Hr.Entity.Models;
using System.Data.Objects.SqlClient;
using System.Data.Objects;

namespace NI.Apps.Hr.Repository{
    public class OfferRepository : IOfferRepository
    {
        public int Add(Table_Offer entity) {
            using (var db = new HrDbContext())
            {
                entity.Offer_CreatedAt = DateTime.Now;
                entity.Offer_CreatedBy = "NIA Support";
                entity.Offer_Status = "Pending Manager Approval";

                db.Table_Offer.Add(entity);
                db.SaveChanges();

                return entity.Offer_ID;
            } 
        }

        public string GetStatus(int id) {
            using (var db = new HrDbContext())
            {
                var result = (from o in db.Table_Offer
                              where id==o.Offer_ID
                              select o.Offer_Status).First();
                return result;
            }
        }

        public void UpdateStatus(int offerID, string process)
        {
            using (var db = new HrDbContext()) {
                string startStatus = GetStatus(offerID);
                string endStatus = getNextStatus(startStatus,process);

                var query = (from o in db.Table_Offer
                             where o.Offer_ID == offerID
                             select o).SingleOrDefault();

                if (query != null) {
                    query.Offer_Status = endStatus;
                    query.Offer_ModifiedAt = DateTime.Now;
                    query.Offer_ModifiedBy = "NIA Support";
                    db.SaveChanges();
                }
            }
        }

        private string getNextStatus(string startStatus, string process)
        {
            string result = "";
            using (var db = new HrDbContext()) {
                result = (from c in db.Table_OfferCycle
                          where (c.OfferCycle_StartStatus == startStatus)
                          && (c.OfferCycle_ProcessName == process)
                          select c.OfferCycle_EndStatus).SingleOrDefault();
            }

            return result;
            
        }

        public IEnumerable<Table_Offer> FindValidOffer(int? code) {
            var validStatus = new string[] { "Draft", "Pending Manager Approval", "Waiting Candidate Feedback", "Pending Welcome Letter", "Pending Candidate Onboarding", "Onboarded" };

            using (var db = new HrDbContext()) { 
                var result=(from o in db.Table_Offer
                                join h in db.Table_Headcount 
                                on o.Offer_HCID equals h.Headcount_ID
                                where(code==null || h.Headcount_Code==code)
                                && validStatus.Contains(o.Offer_Status)
                                select o).OrderByDescending(o=>o.Offer_CreatedAt).ToList();
                return result;
            }
        }



        public IEnumerable<Offer> FindOfferListByHcCode(int code)
        {
            using (var db = new HrDbContext())
            {
                var result = (from o in db.Table_Offer
                                      join h in db.Table_Headcount on o.Offer_HCID equals h.Headcount_ID
                                      join p in db.Table_PersonelInfo on o.Offer_PersonelInfoID equals p.PersonelInfo_ID
                                      where h.Headcount_Code == code
                                      select new Offer()
                                      {
                                          ID=o.Offer_ID,
                                          Name = p.PersonelInfo_LName + p.PersonelInfo_FName,
                                          Type = o.Offer_RecruitType,
                                          Phone = p.PersonelInfo_Phone,
                                          Email = p.PersonelInfo_Email,
                                          OnboardingDate = o.Offer_OnboardingDate,
                                          Offer_ProbationDuration = o.Offer_ProbationDuration,
                                          Status = o.Offer_Status,
                                          CreatedAt=o.Offer_CreatedAt
                                      }).OrderByDescending(o=>o.CreatedAt).ToList();

                foreach(var item in result){
                    if(item.OnboardingDate.HasValue){
                        item.ProbationDueDate = (item.OnboardingDate).Value.AddMonths(item.Offer_ProbationDuration??0);
                    }                  
                }

                return result;
            }
        }
        public Table_Offer FindOfferByID(int ID) {
            using (var db = new HrDbContext())
            {
                var result=db.Table_Offer.Find(ID);
                              
                return result;
            }
        }
        public IEnumerable<Offer> FindOffers(string Name, DateTime? FromDate, DateTime? ToDate, string Status)
        {
            using (var db = new HrDbContext())
            {
                DateTime startDate = FromDate.Value.Date;
                DateTime endDate = ToDate.Value.AddDays(1).Date;

                var result = (from o in db.Table_Offer
                              join h in db.Table_Headcount on o.Offer_HCID equals h.Headcount_ID
                              join p in db.Table_PersonelInfo on o.Offer_PersonelInfoID equals p.PersonelInfo_ID
                              where (string.IsNullOrEmpty(Name) || (p.PersonelInfo_LName+p.PersonelInfo_FName).Contains(Name))
                              && (FromDate == null || o.Offer_CreatedAt >= startDate)
                              && (ToDate == null || o.Offer_CreatedAt < endDate)
                              && (string.IsNullOrEmpty(Status) || o.Offer_Status == Status)
                            
                              select new Offer()
                              {
                                  ID=o.Offer_ID,
                                  Name = p.PersonelInfo_LName + p.PersonelInfo_FName,
                                  Type = o.Offer_RecruitType,
                                  Phone = p.PersonelInfo_Phone,
                                  Email = p.PersonelInfo_Email,
                                  OnboardingDate = o.Offer_OnboardingDate,
                                  Offer_ProbationDuration = o.Offer_ProbationDuration,
                                  Status = o.Offer_Status,
                                  CreatedAt=o.Offer_CreatedAt
                              }).OrderByDescending(o => o.CreatedAt).ToList();

                foreach (var item in result)
                {
                    if (item.OnboardingDate.HasValue)
                    {
                        item.ProbationDueDate = (item.OnboardingDate).Value.AddMonths(item.Offer_ProbationDuration ?? 0);
                    }
                }

                return result;
            }
        }

        //update the whole entity
        public void Update(Table_Offer entity) {
            using (var db = new HrDbContext()) {
                Table_Offer offer = (from o in db.Table_Offer
                                      where o.Offer_ID == entity.Offer_ID
                                      select o).First();

                offer.Offer_OnboardingDate = entity.Offer_OnboardingDate ?? offer.Offer_OnboardingDate;
                offer.Offer_SignedFile = entity.Offer_SignedFile;
                offer.Offer_ModifiedAt = DateTime.Now;
                offer.Offer_ModifiedBy = "NIA Support";

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                                ve.PropertyName,
                                eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                                ve.ErrorMessage);
                        }
                    }
                    throw;
                }
                
            }
        }

        public void UpdateFeedbackStatus(int offerID, 
                                    bool? EmailITCompleted, 
                                    bool? SyncDomainCompleted, 
                                    bool? WelcomeCandidateCompleted, 
                                    bool? WelcomeToMgrCompleted) 
        {
            using (var db = new HrDbContext())
            {
                Table_Offer offer = (from o in db.Table_Offer
                                     where o.Offer_ID == offerID
                                     select o).First();

                offer.Offer_EmailITCompleted = EmailITCompleted??offer.Offer_EmailITCompleted;
                offer.Offer_SyncDomainCompleted = SyncDomainCompleted ?? offer.Offer_SyncDomainCompleted;
                offer.Offer_WelcomeCandidateCompleted = WelcomeCandidateCompleted ?? offer.Offer_WelcomeCandidateCompleted;
                offer.Offer_WelcomeToMgrCompleted = WelcomeToMgrCompleted ?? offer.Offer_WelcomeToMgrCompleted;   
                offer.Offer_ModifiedAt = DateTime.Now;
                offer.Offer_ModifiedBy = "NIA Support";

                db.SaveChanges();
            }
        }


        public void SaveChanges(Table_Offer offer, Table_PersonelInfo personelInfo, Table_ReportingInfo reportInfo, Table_SalaryInfo salaryInfo, List<Table_BonusInfo> bonusList)
        {
            using (var db = new HrDbContext())
            {
                Table_Offer entity = db.Table_Offer.Find(offer.Offer_ID);
                if (entity != null) {
                    entity.Offer_Status = offer.Offer_Status;
                    entity.Offer_RecruitType = offer.Offer_RecruitType;
                    entity.Offer_Position = offer.Offer_Position;
                    entity.Offer_Location=offer.Offer_Location;
                    entity.Offer_OnboardingDate=offer.Offer_OnboardingDate;
                    entity.Offer_RecruitChannel=offer.Offer_RecruitChannel;
                    entity.Offer_ContractDuration = offer.Offer_ContractDuration;
                    entity.Offer_ProbationDuration=offer.Offer_ProbationDuration;
                    entity.Offer_ModifiedAt=DateTime.Now;
                    entity.Offer_ModifiedBy = "NIA Support";       //should be the user name
                }

                Table_PersonelInfo person = db.Table_PersonelInfo.Find(entity.Offer_PersonelInfoID);
                if (person != null)
                {
                    person.PersonelInfo_RomanFName = personelInfo.PersonelInfo_RomanFName;
                    person.PersonelInfo_RomanLName = personelInfo.PersonelInfo_RomanLName;
                    person.PersonelInfo_FName = personelInfo.PersonelInfo_FName;
                    person.PersonelInfo_LName = personelInfo.PersonelInfo_LName;
                    person.PersonelInfo_Email = personelInfo.PersonelInfo_Email;
                    person.PersonelInfo_ModifiedAt = DateTime.Now;
                    person.PersonelInfo_ModifiedBy = "NIA Support";       //should be the user name
                }

                Table_ReportingInfo report=db.Table_ReportingInfo.Find(entity.Offer_ReportingInfoID);
                if(report!=null){
                    report.ReportingInfo_ReportLineEmpID=reportInfo.ReportingInfo_ReportLineEmpID;
                    report.ReportingInfo_ReportLineEmail=reportInfo.ReportingInfo_ReportLineEmail;
                    report.ReportingInfo_DeptMgrEmpID=reportInfo.ReportingInfo_DeptMgrEmpID;
                    report.ReportingInfo_DeptMgrEmail=reportInfo.ReportingInfo_DeptMgrEmail;
                    report.ReportingInfo_ModifiedAt=DateTime.Now;
                    report.ReportingInfo_ModifiedBy="NIA Support";
                }

                Table_SalaryInfo salary=db.Table_SalaryInfo.Find(entity.Offer_SalaryInfoID);
                if(salary!=null){
                    salary.SalaryInfo_Salary=salaryInfo.SalaryInfo_Salary;
                    salary.SalaryInfo_ReviewedSalary = salaryInfo.SalaryInfo_ReviewedSalary;
                    salary.SalaryInfo_ModifiedAt=DateTime.Now;
                    salary.SalaryInfo_ModifiedBy="NIA Support";
                }

                List<Table_BonusInfo> preBonus=(from b in db.Table_BonusInfo
                                                    where b.BonusInfo_SalaryInfoID==entity.Offer_SalaryInfoID
                                                    select b).ToList();
                if(bonusList==null){
                    //delete previous bonus
                    db.Table_BonusInfo.RemoveRange(preBonus);
                }else{
                    //update existing bonus
                    Table_BonusInfo updatedBonus=null;

                    foreach(var tmp in bonusList){
                        switch (tmp.BonusInfo_Type){
                            case "Sign-On":
                                updatedBonus = preBonus.Find(delegate(Table_BonusInfo b) { return b.BonusInfo_Type == "Sign-On"; });
                                preBonus.Remove(updatedBonus);
                                updatedBonus.BonusInfo_Amount = tmp.BonusInfo_Amount;
                                updatedBonus.BonusInfo_ModifiedAt=DateTime.Now;
                                updatedBonus.BonusInfo_ModifiedBy="NIA Support";
                                
                                continue;
                            case "Relocation":
                                updatedBonus = preBonus.Find(delegate(Table_BonusInfo b) { return b.BonusInfo_Type == "Relocation"; });
                                preBonus.Remove(updatedBonus);
                                updatedBonus.BonusInfo_Amount = tmp.BonusInfo_Amount;
                                updatedBonus.BonusInfo_ModifiedAt=DateTime.Now;
                                updatedBonus.BonusInfo_ModifiedBy="NIA Support";
                                continue;
                            case "Others":
                                updatedBonus = preBonus.Find(delegate(Table_BonusInfo b) { return b.BonusInfo_Type == "Others"; });
                                preBonus.Remove(updatedBonus);
                                updatedBonus.BonusInfo_Amount = tmp.BonusInfo_Amount;
                                updatedBonus.BonusInfo_ModifiedAt=DateTime.Now;
                                updatedBonus.BonusInfo_ModifiedBy="NIA Support";
                                continue;
                        }
                        
                    }
                }
                db.Table_BonusInfo.RemoveRange(preBonus);

                db.SaveChanges();

            }
        }
    }
}
