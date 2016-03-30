using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NI.Apps.Hr.HrBase.Models.OfferActivity
{
    public class SalaryDetailModel
    {
        [DataType(DataType.Currency)]
        public decimal? Salary { get; set; }
        [DataType(DataType.Currency)]
        public decimal? ReviewSalary { get; set; }
        public Boolean bonus { get; set; }
        public Boolean SignOn { get; set; }
        [DataType(DataType.Currency)]
        public Decimal? SignOnPrice { get; set; }
        public Boolean Relocation { get; set; }
        [DataType(DataType.Currency)]
        public Decimal? RelocationPrice { get; set; }
        public Boolean Other { get; set; }
        [DataType(DataType.Currency)]
        public Decimal? OtherPrice { get; set; } 
    }
}