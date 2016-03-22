using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Entity;
using NI.Apps.Hr.Entity.Models;

namespace NI.Apps.Hr.Service.Interface
{
    public interface IOfferService
    {
        int AddNewOffer(Table_Offer o,Table_PersonelInfo p,Table_ReportingInfo r,Table_SalaryInfo s,List<Table_BonusInfo> b);
        int GetValidOfferNumber(int? code);
        IEnumerable<Offer> FindOffersByHcCode(int code);
        IEnumerable<Offer> FindOffers(string Name,DateTime? FromDate,DateTime? ToDate,string Status);
        Table_Offer FindOfferByID(int ID);
        string GetOfferStatus(int OfferID);
        void RejectOffer(int offerID);
        void ApproveByMgr(int offerID);
        void ReviseOffer(int offerID);
        void SyncDomainToAddressBook(int OfferID,string DomainLogin,DateTime startDate);

        void UpdatePersonelInfo(Table_Offer newOffer, Table_PersonelInfo newPersonelInfo, List<Table_EducationInfo> newEducationInfo, List<Table_WorkExperienceInfo> newWorkExpInfo, List<Table_FamilyMembersInfo> newFamilyInfo, List<Table_ChildrenInfo> newChildrenInfo);

        void UpdateOfferFeedback(int offerID, Boolean? EmailITCompleted, Boolean? SyncDomainCompleted, Boolean? WelcomeCandidateCompleted, Boolean? WelcomeToMgrCompleted);
        void AcceptOffer(int offerID);

        void SaveOffer(Table_Offer offer, Table_PersonelInfo personelInfo, Table_ReportingInfo reportInfo, Table_SalaryInfo salaryInfo, List<Table_BonusInfo> bonusList);
    }
}
