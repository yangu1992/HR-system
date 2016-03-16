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
            var validStatus = new string[] { "Draft", "Pending Manager Approval", "Waiting Candidate Feedback", "Pending Welcome Letter", "Pending Candidate Onboarding" };

            using (var db = new HrDbContext()) { 
                var result=(from o in db.Table_Offer
                                join h in db.Table_Headcount 
                                on o.Offer_HCID equals h.Headcount_ID
                                where(code==null || h.Headcount_Code==code)
                                && validStatus.Contains(o.Offer_Status)
                                select o).ToList();
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
                                          Name = p.PersonelInfo_FName + p.PersonelInfo_LName,
                                          Type = o.Offer_RecruitType,
                                          Phone = p.PersonelInfo_Phone,
                                          Email = p.PersonelInfo_Email,
                                          OnboardingDate = o.Offer_OnboardingDate,
                                          Offer_ProbationDuration = o.Offer_ProbationDuration,
                                          Status = o.Offer_Status
                                      }).ToList();

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
                var result = (from o in db.Table_Offer
                              join h in db.Table_Headcount on o.Offer_HCID equals h.Headcount_ID
                              join p in db.Table_PersonelInfo on o.Offer_PersonelInfoID equals p.PersonelInfo_ID
                              where (string.IsNullOrEmpty(Name) || (p.PersonelInfo_FName+p.PersonelInfo_LName)==Name)
                              && (FromDate == null || o.Offer_CreatedAt >= FromDate)
                              && (ToDate == null || o.Offer_CreatedAt <=ToDate)
                              && (string.IsNullOrEmpty(Status) || o.Offer_Status == Status)
                            
                              select new Offer()
                              {
                                  ID=o.Offer_ID,
                                  Name = p.PersonelInfo_FName + p.PersonelInfo_LName,
                                  Type = o.Offer_RecruitType,
                                  Phone = p.PersonelInfo_Phone,
                                  Email = p.PersonelInfo_Email,
                                  OnboardingDate = o.Offer_OnboardingDate,
                                  Offer_ProbationDuration = o.Offer_ProbationDuration,
                                  Status = o.Offer_Status
                              }).ToList();

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

                //offer.Offer_OnboardingDate = entity.Offer_OnboardingDate ?? offer.Offer_OnboardingDate;
                offer.Offer_SignedFile = "D:\\VS2010\\old-version\\2016-3-3\\Demo1\\NI.Application.HR.HRBase\\App_Data\\3.xls";
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
    }
}
