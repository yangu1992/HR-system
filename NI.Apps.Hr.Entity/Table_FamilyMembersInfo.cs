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
    
    public partial class Table_FamilyMembersInfo
    {
        public int FamilyMembersInfo_ID { get; set; }
        public int FamilyMembersInfo_PersonelInfoID { get; set; }
        public string FamilyMembersInfo_FullName { get; set; }
        public string FamilyMembersInfo_Relations { get; set; }
        public string FamilyMembersInfo_Employer { get; set; }
        public string FamilyMembersInfo_Department { get; set; }
        public string FamilyMembersInfo_Position { get; set; }
        public Nullable<System.DateTime> FamilyMembersInfo_CreatedAt { get; set; }
        public string FamilyMembersInfo_CreatedBy { get; set; }
        public Nullable<System.DateTime> FamilyMembersInfo_ModifiedAt { get; set; }
        public string FamilyMembersInfo_ModifiedBy { get; set; }
    }
}