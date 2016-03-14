using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NI.Application.HR.HRBase.Models.OfferActivity
{
    public class OfferDetailModel
    {
        public int ID { get; set; } //offer id
        public int HeadcountCode { get; set; }
        public int ValidOfferNumber { get; set; }
        public int TotalOfferNumber { get; set; }
        public string Status { get; set; }
        public string RecruitType { get; set; }
        public string Postion { get; set; }
        public string Location { get; set; }
        public string Channel { get; set; }
        public DateTime? OnBoardingDate { get; set; }
        public int? ProbationDuration { get; set; }
        public string UploadFormPath { get; set; }
        public bool? EmailITCompleted{get;set;}
        public bool? SyncDomainCompleted{get;set;}
        public bool? WelcomeCandidateCompleted{get;set;}
        public bool? WelcomeToMgrCompleted { get; set; }

    }
}