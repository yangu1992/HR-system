using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NI.Application.HR.HRBase.Models.OfferActivity.PersonalInfoFormModels
{
    public class WorkExperienceModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Employer { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
    }
}