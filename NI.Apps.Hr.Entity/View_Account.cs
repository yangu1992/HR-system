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
    
    public partial class View_Account
    {
        public int SID { get; set; }
        public string DomainLogin { get; set; }
        public string Name { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string JobTitle { get; set; }
        public Nullable<int> Dep_ID { get; set; }
        public int Status_ID { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public string Remark { get; set; }
        public Nullable<int> CostCenter_ID { get; set; }
        public Nullable<System.DateTime> ContractStartDate { get; set; }
        public Nullable<System.DateTime> ContractEndDate { get; set; }
        public byte[] SignCounts { get; set; }
        public string ContractMemo { get; set; }
        public byte[] ContractNo { get; set; }
        public Nullable<int> EmpType_ID { get; set; }
        public Nullable<int> Account_Supervisor { get; set; }
        public Nullable<System.DateTime> Contract_Start_Date { get; set; }
    }
}