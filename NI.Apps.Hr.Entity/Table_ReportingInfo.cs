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
    
    public partial class Table_ReportingInfo
    {
        public int ReportingInfo_ID { get; set; }
        public Nullable<int> ReportingInfo_ReportLineEmpID { get; set; }
        public string ReportingInfo_ReportLineEmail { get; set; }
        public Nullable<int> ReportingInfo_DeptMgrEmpID { get; set; }
        public string ReportingInfo_DeptMgrEmail { get; set; }
        public Nullable<System.DateTime> ReportingInfo_CreatedAt { get; set; }
        public string ReportingInfo_CreatedBy { get; set; }
        public Nullable<System.DateTime> ReportingInfo_ModifiedAt { get; set; }
        public string ReportingInfo_ModifiedBy { get; set; }
    }
}
