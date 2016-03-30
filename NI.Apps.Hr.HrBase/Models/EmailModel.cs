using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NI.Apps.Hr.HrBase.Models
{
    public class EmailModel
    {
        public String FromEmail { get; set; }
        public String ToEmail { get; set; }
        public String CCEmail { get; set; }
        public String Subject { get; set; }
        public string Body { get; set; }
        public string AttachmentsLocation { get; set; }
        public string NIOfferPath { get; set; }

    }
}