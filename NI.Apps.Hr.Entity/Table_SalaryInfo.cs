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
    
    public partial class Table_SalaryInfo
    {
        public int SalaryInfo_ID { get; set; }
        public Nullable<decimal> SalaryInfo_Salary { get; set; }
        public Nullable<decimal> SalaryInfo_ReviewedSalary { get; set; }
        public Nullable<bool> SalaryInfo_Bonus { get; set; }
        public string SalaryInfo_BonusType { get; set; }
        public Nullable<decimal> SalaryInfo_BonusAmount { get; set; }
        public Nullable<System.DateTime> SalaryInfo_CreatedAt { get; set; }
        public string SalaryInfo_CreatedBy { get; set; }
        public Nullable<System.DateTime> SalaryInfo_ModifiedAt { get; set; }
        public string SalaryInfo_ModifiedBy { get; set; }
    }
}