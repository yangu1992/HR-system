//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NI.Apps.Hr.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class Table_WorkExperienceInfo
    {
        public int WorkExperienceInfo_ID { get; set; }
        public int WorkExperienceInfo_PersonelInfoID { get; set; }
        public string WorkExperienceInfo_StartDate { get; set; }
        public string WorkExperienceInfo_EndDate { get; set; }
        public string WorkExperienceInfo_Employer { get; set; }
        public string WorkExperienceInfo_Department { get; set; }
        public string WorkExperienceInfo_Position { get; set; }
        public Nullable<System.DateTime> WorkExperienceInfo_CreatedAt { get; set; }
        public string WorkExperienceInfo_CreatedBy { get; set; }
        public Nullable<System.DateTime> WorkExperienceInfo_ModifiedAt { get; set; }
        public string WorkExperienceInfo_ModifiedBy { get; set; }
    }
}
