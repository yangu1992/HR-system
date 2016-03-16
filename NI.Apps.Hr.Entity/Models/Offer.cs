using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace NI.Apps.Hr.Entity.Models
{
    public class Offer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type{ get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? OnboardingDate { get; set; }
        public DateTime? ProbationDueDate { get; set; }
        public int? Offer_ProbationDuration { get; set; }
        public string Status { get; set; }

    }

}