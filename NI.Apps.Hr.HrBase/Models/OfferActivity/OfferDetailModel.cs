using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace NI.Apps.Hr.HrBase.Models.OfferActivity
{
    public class OfferDetailModel
    {
        public OfferDetailModel() {
            this.ContractDuration = 3;
            this.ProbationDuration = 6;
            this.OnBoardingDate = DateTime.Today;
        }
        public int ID { get; set; } //offer id
        public int HeadcountCode { get; set; }
        public int ValidOfferNumber { get; set; }
        public int TotalOfferNumber { get; set; }
        public string Status { get; set; }
        public string RecruitType { get; set; }
        public string Postion { get; set; }
        public string Location { get; set; }
        public string Channel { get; set; }
        //[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? OnBoardingDate { get; set; }
        [DefaultValue(3)]
        public int? ContractDuration { get; set; }
        [DefaultValue(6)]
        public int? ProbationDuration { get; set; }
        public string UploadFormPath { get; set; }
        public bool? EmailITCompleted{get;set;}
        public bool? SyncDomainCompleted{get;set;}
        public bool? WelcomeCandidateCompleted{get;set;}
        public bool? WelcomeToMgrCompleted { get; set; }

    }
}