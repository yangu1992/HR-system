using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Entity.Models;
using NI.Apps.Hr.Entity;

namespace NI.Apps.Hr.Repository.Interface
{
    public interface IOfferRepository
    {
        int Add(Table_Offer newOffer);
        string GetStatus(int id);
        void UpdateStatus(int offerID, string process);
        IEnumerable<Table_Offer> FindValidOffer(int? code);
        IEnumerable<Offer> FindOfferListByHcCode(int code);
        Table_Offer FindOfferByID(int ID);
        IEnumerable<Offer> FindOffers(string Name,DateTime? FromDate, DateTime? ToDate, string Status);
        void Update(Table_Offer entity);

        void UpdateFeedbackStatus(int offerID, bool? EmailITCompleted, bool? SyncDomainCompleted, bool? WelcomeCandidateCompleted, bool? WelcomeToMgrCompleted);

        void SaveChanges(Table_Offer offer, Table_PersonelInfo personelInfo, Table_ReportingInfo reportInfo, Table_SalaryInfo salaryInfo, List<Table_BonusInfo> bonusList);
    }
}
