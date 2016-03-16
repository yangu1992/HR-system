using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NI.Application.HR.HRBase.Models.OfferActivity.PersonalInfoFormModels
{
    public class PersonalInfoModel
    {
        public string ChineseFName { get; set; }
        public string ChineseGName { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string EnglishFName { get; set; }
        public string EnglishGName { get; set; }
        public string BirthDate { get; set; }
        public string ID { get; set; }    //identity id
        public string Nationality { get; set; }
        public string Hukou { get; set; }
        public string HukouType { get; set; }
        public string FileLocation { get; set; }
        public string HomeAddress { get; set; }
        public string PostCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string EmergencyContact { get; set; }
        public string EmergencyContactPhone { get; set; }
    }
}