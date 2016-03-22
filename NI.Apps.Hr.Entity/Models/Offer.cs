using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace NI.Apps.Hr.Entity.Models
{
    public class Offer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type{ get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? OnboardingDate { get; set; }
        public DateTime? ProbationDueDate { get; set; }
        public int? Offer_ProbationDuration { get; set; }
        public string Status { get; set; }

    }

}