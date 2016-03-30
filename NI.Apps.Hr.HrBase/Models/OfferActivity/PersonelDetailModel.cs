using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NI.Application.HR.HRBase.Models.OfferActivity
{
    public class PersonelDetailModel
    {
        public string RomanFName { get; set; }
        public string RomanLName { get; set; }
        [Required]
        public string FName { get; set; }   //中国的名
        [Required]
        public string LName { get; set; }    //中国的姓，与英文名字顺序保持一致
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}