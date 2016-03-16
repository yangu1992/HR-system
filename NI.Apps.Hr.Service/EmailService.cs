using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Service.Interface;

namespace NI.Apps.Hr.Service
{
    public class EmailService:IEmailService
    {
        public void SendEmail(string From, string To, string CC, string Subject)
        {

            throw new NotImplementedException();
        }
    }
}
