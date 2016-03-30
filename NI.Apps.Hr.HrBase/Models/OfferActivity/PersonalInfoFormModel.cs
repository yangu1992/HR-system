using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NI.Apps.Hr.HrBase.Models.OfferActivity.PersonalInfoFormModels;

namespace NI.Apps.Hr.HrBase.Models.OfferActivity
{
    public class PersonalInfoFormModel
    {
        public PersonalInfoModel PersonalInfo { get; set; }
        public List<EducationBackgroudModel> EduInfo { get; set; }
        public List<WorkExperienceModel> WorkExperienceInfo { get; set; }
        public List<FamilyMemberModel> FamilyMemInfo { get; set; }
        public List<ChildrenInfoModel> ChildsInfo { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string TentativeOnboardDate { get; set; }
    }
}