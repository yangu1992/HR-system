using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Service.Interface;
using NI.Apps.Hr.Repository;
using NI.Apps.Hr.Repository.Interface;
using NI.Apps.Hr.Entity.Models;
using NI.Apps.Hr.Entity;

namespace NI.Apps.Hr.Service
{
    public class OfferService : IOfferService
    {
        private IHeadCountRepository _headcountRepository;
        private IOfferRepository _offerRepository;
        private IPersonelRepository _personelRepository;
        private IReportRepository _reportRepository;
        private ISalaryRepository _salaryRepository;
        private IAddressBookRepository _addressBookRepository;
        private IEducationRepository _educationRepository;
        private IWorkExperienceRepository _workExperienceRepository;
        private IFamilyMembersRepository _familyMembersRepository;
        private IChildrenRepository _childrenRepository;

        public IHeadCountRepository HeadCountRepository
        {
            get { return _headcountRepository ?? (_headcountRepository = new HeadCountRepository()); }
        }
        public IOfferRepository OfferRepository
        {
            get { return _offerRepository ?? (_offerRepository = new OfferRepository()); }
        }
        public IPersonelRepository PersonelRepository
        {
            get { return _personelRepository ?? (_personelRepository = new PersonelRepository()); }
        }
        public IReportRepository ReportRepository
        {
            get { return _reportRepository ?? (_reportRepository = new ReportRepository()); }
        }
        public ISalaryRepository SalaryRepository
        {
            get { return _salaryRepository ?? (_salaryRepository = new SalaryRepository()); }
        }
        public IAddressBookRepository AddressBookRepository
        {
            get { return _addressBookRepository ?? (_addressBookRepository = new AddressBookRepository()); }
        }
        public IEducationRepository EducationRepository
        {
            get { return _educationRepository ?? (_educationRepository = new EducationRepository()); }
        }
        public IWorkExperienceRepository WorkExperienceRepository
        {
            get { return _workExperienceRepository ?? (_workExperienceRepository = new WorkExperienceRepository()); }
        }
        public IFamilyMembersRepository FamilyMembersRepository
        {
            get { return _familyMembersRepository ?? (_familyMembersRepository = new FamilyMembersRepository()); }
        }
        public IChildrenRepository ChildrenRepository
        {
            get { return _childrenRepository ?? (_childrenRepository = new ChildrenRepository()); }
        }

        public int AddNewOffer(Table_Offer o, Table_PersonelInfo p, Table_ReportingInfo r, Table_SalaryInfo s)
        {
            int personelID = this.PersonelRepository.Add(p);
            int reportID = this.ReportRepository.Add(r);
            int salaryID = this.SalaryRepository.Add(s);

            o.Offer_PersonelInfoID = personelID;
            o.Offer_ReportingInfoID = reportID;
            o.Offer_SalaryInfoID = salaryID;

            int offerID = this.OfferRepository.Add(o);

            return offerID;
        }

        public int GetValidOfferNumber(int? code) {
            return this.OfferRepository.FindValidOffer(code).Count();
        }

        public string GetOfferStatus(int OfferID) {
            return this.OfferRepository.GetStatus(OfferID);
        }

        public IEnumerable<Offer> FindOffersByHcCode(int code)
        {
            return this.OfferRepository.FindOfferListByHcCode(code);           
        }

        public IEnumerable<Offer> FindOffers(string Name, DateTime? FromDate, DateTime? ToDate, string Status)
        {
            return this.OfferRepository.FindOffers(Name,FromDate,ToDate,Status);
        }

        public Table_Offer FindOfferByID(int id) {
            return this.OfferRepository.FindOfferByID(id);
        }

        public void RejectOffer(int offerID) {
            this.OfferRepository.UpdateStatus(offerID, "Reject");
        }

        public void ApproveByMgr(int offerID) {
            this.OfferRepository.UpdateStatus(offerID,"Approve");
        }

        public void ReviseOffer(int offerID){
            this.OfferRepository.UpdateStatus(offerID,"Revise");
        }
        public void SyncDomainToAddressBook(int OfferID, string DomainLogin, DateTime startDate)
        {
            this.AddressBookRepository.Add(OfferID,DomainLogin,startDate);

        }

        public void UpdatePersonelInfo(Table_Offer newOffer, 
                                       Table_PersonelInfo newPersonelInfo, 
                                       List<Table_EducationInfo> newEducationInfo, 
                                       List<Table_WorkExperienceInfo> newWorkExpInfo, 
                                       List<Table_FamilyMembersInfo> newFamilyInfo, 
                                       List<Table_ChildrenInfo> newChildrenInfo) 
        {
            int personelID = this.OfferRepository.FindOfferByID(newOffer.Offer_ID).Offer_PersonelInfoID;
            newPersonelInfo.PersonelInfo_ID = personelID;

            this.OfferRepository.Update(newOffer);

            foreach (var entity in newEducationInfo) {
                entity.EducationInfo_PersonelInfoID = personelID;
                this.EducationRepository.Add(entity);
            }

            foreach (var entity in newWorkExpInfo)
            {
                entity.WorkExperienceInfo_PersonelInfoID = personelID;
                this.WorkExperienceRepository.Add(entity);
            }

            foreach (var entity in newFamilyInfo)
            {
                entity.FamilyMembersInfo_PersonelInfoID = personelID;
                this.FamilyMembersRepository.Add(entity);
            }

            foreach (var entity in newChildrenInfo)
            {
                entity.ChildrenInfo_PersonelInfoID = personelID;
                this.ChildrenRepository.Add(entity);
            }

            this.PersonelRepository.Update(newPersonelInfo);
        }
        public void UpdateOfferFeedback(int offerID, 
                                    Boolean? EmailITCompleted, 
                                    Boolean? SyncDomainCompleted, 
                                    Boolean? WelcomeCandidateCompleted, 
                                    Boolean? WelcomeToMgrCompleted){
            this.OfferRepository.UpdateFeedbackStatus(offerID,EmailITCompleted,SyncDomainCompleted,WelcomeCandidateCompleted,WelcomeToMgrCompleted);
        }
        public void AcceptOffer(int offerID) {
            this.OfferRepository.UpdateStatus(offerID,"Accept");
        }
    }
}
