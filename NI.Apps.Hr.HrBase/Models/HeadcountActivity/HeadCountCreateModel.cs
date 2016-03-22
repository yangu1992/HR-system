using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NI.Apps.Hr.HrBase.Models.HeadcountActivity
{
    [DataContract]
    public class HeadCountCreateModel
    {
        public int ID { get; set; }
        [DataMember(IsRequired = true)]
        [Required]
        public int Code { get; set; }
        public string Position { get; set; }
        public int Number { get; set; }
        public string CostCenter { get; set; }
        public string Department { get; set; }
        public string InternalLevel { get; set; }
        public string CreateBtn { get; set; }
    }
}