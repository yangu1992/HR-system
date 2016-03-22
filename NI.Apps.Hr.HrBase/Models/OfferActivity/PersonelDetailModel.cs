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
        public string FName { get; set; }
        public string LName { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}