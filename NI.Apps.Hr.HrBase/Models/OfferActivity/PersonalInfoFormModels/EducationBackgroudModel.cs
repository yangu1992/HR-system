using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NI.Application.HR.HRBase.Models.OfferActivity.PersonalInfoFormModels
{
    /*
     * this ViewModel is for Background Section in the PersonalInformationForm
     * **/
    public class EducationBackgroudModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string School { get; set; }
        public string Major { get; set; }
        public string Degree { get; set; }
    }
}